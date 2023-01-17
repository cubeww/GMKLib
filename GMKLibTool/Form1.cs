using GameMaker.ProjectCommon;
using GameMaker.GM8Project;
using System.Diagnostics;
using Object = GameMaker.ProjectCommon.Object;
using System.Linq;
using System.Text;
using GameMaker.GML;

namespace GMKLibTool
{
    public partial class Form1 : Form
    {
        Project Project { get; set; }
        public Form1()
        {
            InitializeComponent();

            statusLabel.Text = "Done";
        }
        void UpdateTree()
        {
            assetTree.Nodes.Clear();

            var rooms = assetTree.Nodes.Add("rooms");
            foreach (var room in Project.Rooms)
            {
                rooms.Nodes.Add(room.Name);
            }
        }
        private void MenuOpen_Click(object sender, EventArgs e)
        {
            if (openProjectDialog.ShowDialog() == DialogResult.OK)
            {
                statusLabel.Text = "Loading...";
                Task.Run(() =>
                {
                    var reader = new ProjectReader(openProjectDialog.FileName);
                    Project = reader.ReadProject();
                    
                    Invoke(()=> {
                        statusLabel.Text = "Done";
                        UpdateTree();
                    });
                });

            }
        }

        private void assetTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (assetTree.SelectedNode.Name != "rooms")
            {
                // Search...
                var blocks = new List<string>() {
                    "world",
                    "player",
                };
                var objs = new List<Object>();
                var sprs = new List<Sprite>();
                var bgs = new List<Background>();
                var scrs = new List<Script>();

                var room = Project.Rooms.Find(x => x.Name == assetTree.SelectedNode.Text);

                // Search 1: Simple Search
                // Objects

                void CheckForObject(Object obj)
                {
                    if (blocks.Contains(obj.Name))
                    {
                        return;
                    }

                    if (!objs.Contains(obj))
                    {
                        objs.Add(obj);

                        // DEEP Search
                        foreach (var ev in obj.Events)
                        {
                            foreach (var ev1 in ev)
                            {
                                foreach (var a in ev1.Actions)
                                {
                                    if (a.ActionKind == GameMaker.ActionKind.Code)
                                    {
                                        // Parse script
                                        var ast = Parser.Parse(Project, obj.Name, a.Arguments[0].Value);

                                        // Search AST
                                        void SearchAST(AST node)
                                        {
                                            if (node.Token == Token.Constant && node.Value.Kind == Kind.Number && !double.TryParse(node.Text, out _))
                                            {
                                                // Get Extra Asset
                                                // Object
                                                var obj = Project.Objects.Find(x => x.Name == node.Text);
                                                if (obj != null)
                                                {
                                                    CheckForObject(obj);
                                                }
                                                // Sprite
                                                var spr = Project.Sprites.Find(x => x.Name == node.Text);
                                                if (spr != null)
                                                {
                                                    if (!sprs.Contains(spr))
                                                        sprs.Add(spr);
                                                }
                                            }

                                            SearchChildren(node);
                                        }
                                        void SearchChildren(AST node)
                                        {
                                            foreach (var child in node.Children)
                                            {
                                                SearchAST(child);
                                            }
                                        }
                                        SearchAST(ast);
                                    }
                                }
                            }
                        }
                    }
                }

                foreach (var i in room.Instances)
                {
                    var obj = Project.Objects.Find(x => x.Id == i.ObjectId);
                    if (obj == null)
                        continue;

                    CheckForObject(obj);
                }

                // Bg
                foreach (var i in room.Tiles)
                {
                    var bg = Project.Backgrounds.Find(x => x.Id == i.BackgroundId);
                    if (bg == null)
                        continue;
                    if (!bgs.Contains(bg))
                        bgs.Add(bg);
                }
                foreach (var i in room.Parallaxs)
                {
                    var bg = Project.Backgrounds.Find(x => x.Id == i.BackgroundId);
                    if (bg == null)
                        continue;
                    if (!bgs.Contains(bg))
                        bgs.Add(bg);
                }

                // Sprite
                foreach (var obj in objs)
                {
                    var spr = Project.Sprites.Find(x => x.Id == obj.SpriteId);
                    if (spr == null)
                        continue;
                    if (!sprs.Contains(spr))
                        sprs.Add(spr);
                }

                // ---------- Output -----------
                var text = "";

                text += "Sprites\n";
                sprs.ForEach(x => text += "- " + x.Name + "\n");
                text += "\n";

                text += "Backgrounds\n";
                bgs.ForEach(x => text += "- " + x.Name + "\n");
                text += "\n";

                text += "Objects\n";
                objs.ForEach(x => text += "- " + x.Name + "\n");
                text += "\n";

                lblInfo.Text = text;
            }
        }
    }
}