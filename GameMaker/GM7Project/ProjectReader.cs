// ProjectReader.cs
//
// Copyright 2011 Phillip Stephens
//
// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
//
// Linking this library statically or dynamically with other modules is
// making a combined work based on this library.  Thus, the terms and
// conditions of the GNU General Public License cover the whole
// combination.
// 
// As a special exception, the copyright holders of this library give you
// permission to link this library with independent modules to produce an
// executable, regardless of the license terms of these independent
// modules, and to copy and distribute the resulting executable under
// terms of your choice, provided that you also meet, for each linked
// independent module, the terms and conditions of the license of that
// module.  An independent module is a module which is not derived from
// or based on this library.  If you modify this library, you may extend
// this exception to your version of the library, but you are not
// obligated to do so.  If you do not wish to do so, delete this
// exception statement from your version.
/*
using System.Collections.Generic;
using System.IO;
using GameMaker.ProjectCommon;
using GameMaker.IO;
using ICSharpCode.SharpZipLib.Zip.Compression;
using Action = GameMaker.ProjectCommon.Action;
using Object = GameMaker.ProjectCommon.Object;

namespace GameMaker.GM7Project
{
    public class ProjectReader : ReaderBase
    {


        private Obfuscation m_obfuscation = new Obfuscation();
       


        internal Obfuscation Obfuscation
        {
            get { return m_obfuscation; }
        }

        public int[][] SwapTable
        {
            get { return m_obfuscation.SwapTable; }
        }

        public int Seed
        {
            get { return m_obfuscation.Seed; }
            set { m_obfuscation.Seed = value; }
        }




        public ProjectReader()
        { }

        public ProjectReader(string path)
        {
            Open(path);
        }

        public Project ReadProject()
        {
            if (ReadInt() != 1234321)
                throw new System.Exception("Not a valid GameMaker File");

            int version = ReadInt();
            
            if ((FileVersion)version != FileVersion.GameMaker70)
                throw new System.Exception("Not a GameMaker 7.0 Project");

            int bill = ReadInt();
            int fred = ReadInt();

            ReadBytes(bill * 4);
            int seed = ReadInt();
            ReadBytes(fred * 4);
            Seed = seed;

            Project project = new Project();
            project.FileVersion = (FileVersion)version;
            project.Settings = ReadSettings();
            project.Sounds = ReadSounds();
            project.Sprites = ReadSprites();
            project.Backgrounds = ReadBackgrounds();
            project.Paths = ReadPaths();
            project.Scripts = ReadScripts();
            project.Fonts = ReadFonts();
            project.Timelines = ReadTimelines();
            project.Objects = ReadObjects();
            project.Rooms = ReadRooms(project.Objects);

            project.LastInstanceId = ReadInt();
            project.LastTileId = ReadInt();

            project.Includes = ReadIncludes();
            project.Extensions = ReadExtensions();
            project.GameInformation = ReadGameInformation();

            ReadInt();

            int libCount = ReadInt();

            for (int i = 0; i < libCount; i++)
            {
                Library library = new Library();
                library.Code = ReadString();

                project.Libraries.Add(library);
            }

            project.ProjectTree = ReadTree(System.IO.Path.GetFileName(FileName), project);
            Close();

            return project;
        }


        private GlobalSettings ReadSettings()
        {
            GlobalSettings s = new GlobalSettings();
            s.GameId = ReadGameId();
            s.Guid = ReadBytes(16);

            ReadInt();

            s.Fullscreen = ReadBool();
            s.Interpolate = ReadBool();
            s.NoBorders = ReadBool();
            s.DisplayCursor = ReadBool();
            s.Scaling = ReadInt();
            s.AllowResizing = ReadBool();
            s.StayOnTop = ReadBool();
            s.OutsideColor = ReadInt();
            s.SetResolution = ReadBool();
            s.ColorDepth2 = (ColorDepth2)ReadInt();
            s.Resolution2 = (Resolution2)ReadInt();
            s.Frequency2 = (Frequency2)ReadInt();
            s.NoButtons = ReadBool();
            s.UseVSync = ReadBool();
            s.F4ChangeScreenModes = ReadBool();
            s.F1ShowInformation = ReadBool();
            s.EscapeEndGame = ReadBool();
            s.F5SaveF6Load = ReadBool();
            s.F9Screenshot = ReadBool();
            s.CloseButtonAsEsc = ReadBool();
            s.Priority = (Priority)ReadInt();
            s.FreezeOnLoseFocus = ReadBool();
            ProgressBar pb = new ProgressBar();
            pb.ProgressBarType = (ProgressBarType)ReadInt();
            if (pb.ProgressBarType == ProgressBarType.UserDefined)
            {
                if (ReadInt() != -1)
                    pb.BackImage = ReadBytes(ReadInt());
                if (ReadInt() != -1)
                    pb.FrontImage = ReadBytes(ReadInt());
            }

            pb.OwnSplashImage = ReadBool();
            if (pb.OwnSplashImage && ReadInt() != -1)
                pb.SplashImage = ReadBytes(ReadInt());
            
            pb.MakePartiallyTransparent = ReadBool();
            pb.Alpha = ReadInt();
            pb.ScaleProgressBar = ReadBool();
            s.ProgressBar = pb;
            
            s.Icon = ReadBytes(ReadInt());
            s.DisplayErrors = ReadBool();
            s.WriteLog = ReadBool();
            s.AbortOnErrors = ReadBool();
            s.TreatUninitializedAsZero = ReadBool();
            s.Author = ReadString();
            s.Version = ReadString();
            s.SettingsLastModified = System.DateTime.FromOADate(ReadDouble());
            s.Information = ReadString();

            int constCount = ReadInt();
            while ((constCount--) > 0)
            {
                Constant c = new Constant();
                c.Name = ReadString();
                c.Value = ReadString();
                s.Constants.Add(c);
            }

            s.Major = ReadInt();
            s.Minor = ReadInt();
            s.Release = ReadInt();
            s.Build = ReadInt();
            s.Company = ReadString();
            s.Product = ReadString();
            s.Copyright = ReadString();
            s.Description = ReadString();

            return s;
        }



        private ResourceCollection<Sound> ReadSounds()
        {
            ReadInt();

            ResourceCollection<Sound> sounds = new ResourceCollection<Sound>();
            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {
                if (ReadBool() == false)
                {
                    sounds.LastId++;
                    continue;
                }

                Sound sound = new Sound();

                sound.Id = i;

                sound.Name = ReadString();
                ReadInt();

                sound.SoundType = (SoundType)ReadInt();

                sound.FileType = ReadString();
                sound.FileName = ReadString();

                if (ReadBool())
                    sound.Data = ReadBytes(ReadInt());

                // if the data exists decompress it
                if (sound.Data.Length > 0)
                    sound.Data = DecompressData(sound.Data);

                sound.Effects = ReadInt();
                sound.Volume = ReadDouble();
                sound.Pan = ReadDouble();
                sound.Preload = ReadBool();

                sounds.Add(sound);
            }

            return sounds;
        }



        private ResourceCollection<Sprite> ReadSprites()
        {
            ReadInt();

            ResourceCollection<Sprite> sprites = new ResourceCollection<Sprite>();

            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {
                if (!ReadBool())
                {
                    sprites.LastId++;
                    continue;
                }

                Sprite sprite = new Sprite();
                sprite.Id = i;
                sprite.Name = ReadString();

                ReadInt(); // no need for version

                sprite.Width = ReadInt();
                sprite.Height = ReadInt();

                sprite.BoundingBoxLeft = ReadInt();
                sprite.BoundingBoxRight = ReadInt();
                sprite.BoundingBoxBottom = ReadInt();
                sprite.BoundingBoxTop = ReadInt();
                sprite.Transparent = ReadBool();
                sprite.SmoothEdges = ReadBool();
                sprite.Preload = ReadBool();
                sprite.BoundingBoxMode = (BoundingBox)ReadInt();
                sprite.Precise = ReadBool();
                sprite.OriginX = ReadInt();
                sprite.OriginY = ReadInt();

                int imgCount = ReadInt();

                for (int j = 0; j < imgCount; j++)
                {
                    if (ReadInt() != -1)
                    {
                        Image image = new Image();
                        image.Width = sprite.Width;
                        image.Height = sprite.Height;
                        image.Data = DecompressData(ReadBytes(ReadInt()));
                        sprite.SubImages.Add(image);
                    }
                }

                sprites.Add(sprite);
            }

            return sprites;
        }



        private ResourceCollection<Background> ReadBackgrounds()
        {
            ReadInt();

            ResourceCollection<Background> backgrounds = new ResourceCollection<Background>();

            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {

                if (!ReadBool())
                {
                    backgrounds.LastId++;
                    continue;
                }

                Background background = new Background();

                background.Id = i;

                background.Name = ReadString();
                ReadInt();

                background.Width = ReadInt();
                background.Height = ReadInt();
                background.Transparent = ReadBool();
                background.SmoothEdges = ReadBool();
                background.Preload = ReadBool();
                background.UseAsTileSet = ReadBool();
                background.TileWidth = ReadInt();
                background.TileHeight = ReadInt();
                background.HorizontalOffset = ReadInt();
                background.VerticalOffset = ReadInt();
                background.HorizontalSeparation = ReadInt();
                background.VerticalSeparation = ReadInt();

                ReadInt();

                if (ReadBool())
                {
                    background.Image = new Image();
                    background.Image.Width = background.Width;
                    background.Image.Height = background.Height;
                    
                    if (ReadInt() != -1)
                        background.Image.Data = DecompressData(ReadBytes(ReadInt()));
                }

                backgrounds.Add(background);
            }

            return backgrounds;
        }



        private ResourceCollection<ProjectCommon.Path> ReadPaths()
        {
            ReadInt();

            ResourceCollection<ProjectCommon.Path> paths = new ResourceCollection<ProjectCommon.Path>();

            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {
                if (!ReadBool())
                {
                    paths.LastId++;
                    continue;
                }

                ProjectCommon.Path path = new ProjectCommon.Path();
                path.Id = i;

                path.Name = ReadString();
                ReadInt();
                path.Smooth = ReadBool();
                path.Closed = ReadBool();
                path.Precision = ReadInt();
                path.RoomId = ReadInt();
                path.SnapX = ReadInt();
                path.SnapY = ReadInt();

                path.Points = new List<Point>();

                int pointCount = ReadInt();

                for (int j = 0; j < pointCount; j++)
                {
                    Point point = new Point();
                    point.X = ReadDouble();
                    point.Y = ReadDouble();
                    point.Speed = ReadDouble();

                    path.Points.Add(point);
                }

                paths.Add(path);
            }

            return paths;
        }



        private ResourceCollection<Script> ReadScripts()
        {
            ReadInt();

            ResourceCollection<Script> scripts = new ResourceCollection<Script>();

            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {

                if (!ReadBool())
                {
                    scripts.LastId++;
                    continue;
                }

                Script script = new Script();

                script.Id = i;
                script.Name = ReadString();

                ReadInt();

                script.Code = ReadString();
                scripts.Add(script);
            }

            return scripts;
        }



        private ResourceCollection<Font> ReadFonts()
        {
            ReadInt();

            ResourceCollection<Font> fonts = new ResourceCollection<Font>();

            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {
                if (!ReadBool())
                {
                    fonts.LastId++;
                    continue;
                }

                Font font = new Font();

                font.Id = i;

                font.Name = ReadString();

                ReadInt(); // Version

                font.FontName = ReadString();
                font.Size = ReadInt();
                font.Bold = ReadBool();
                font.Italic = ReadBool();
                font.CharacterRangeMin = ReadInt();
                font.CharacterRangeMax = ReadInt();

                fonts.Add(font);
            }

            return fonts;
        }


        private ResourceCollection<Timeline> ReadTimelines()
        {
            ReadInt();

            ResourceCollection<Timeline> timelines = new ResourceCollection<Timeline>();

            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {

                if (!ReadBool())
                {
                    timelines.LastId++;
                    continue;
                }

                Timeline timeline = new Timeline();

                timeline.Id = i;
                timeline.Name = ReadString();

                ReadInt();

                timeline.Moments = new List<Moment>();

                int momentCount = ReadInt();

                for (int j = 0; j < momentCount; j++)
                {
                    Moment moment = new Moment();

                    moment.StepIndex = ReadInt();

                    moment.Actions = ReadActions();

                    timeline.Moments.Add(moment);
                }

                timelines.Add(timeline);
            }
            return timelines;
        }



        private ResourceCollection<Object> ReadObjects()
        {
            ReadInt();

            ResourceCollection<Object> objects = new ResourceCollection<Object>();

            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {

                if (!ReadBool())
                {
                    objects.LastId++;
                    continue;
                }

                Object obj = new Object();

                obj.Id = i;
                obj.Name = ReadString();

                ReadInt();

                obj.SpriteId = ReadInt();
                obj.Solid = ReadBool();
                obj.Visible = ReadBool();
                obj.Depth = ReadInt();
                obj.Persistent = ReadBool();
                obj.Parent = ReadInt();
                obj.Mask = ReadInt();

                ReadInt();

                for (int j = 0; j < 11; j++)
                {
                    bool done = false;
                    while (!done)
                    {
                        int eventNum = ReadInt();

                        // If the event exists
                        if (eventNum != -1)
                        {
                            // Create new event
                            Event ev = new Event();

                            // Set type of event
                            ev.MainType = (EventType)(j);

                            // If a collision type of event set other object id.
                            if (ev.MainType == EventType.Collision)
                                ev.OtherId = eventNum;
                            else
                                ev.Subtype = eventNum;

                            // ReadEventActions
                            ev.Actions = ReadActions();
                            obj.Events[j].Add(ev);
                        }
                        else
                            done = true;
                    }
                }

                objects.Add(obj);
            }

            return objects;
        }



        private ResourceCollection<Room> ReadRooms(ResourceCollection<Object> objects)
        {
            ReadInt();

            ResourceCollection<Room> rooms = new ResourceCollection<Room>();

            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {
                if (!ReadBool())
                {
                    rooms.LastId++;
                    continue;
                }

                Room room = new Room();

                room.Id = i;
                room.Name = ReadString();

                ReadInt();

                room.Caption = ReadString();
                room.Width = ReadInt();
                room.Height = ReadInt();
                room.SnapY = ReadInt();
                room.SnapX = ReadInt();
                room.IsometricGrid = ReadBool();

                room.Speed = ReadInt();
                room.Persistent = ReadBool();
                room.BackgroundColor = ReadInt();
                room.DrawBackgroundColor = ReadBool();
                room.CreationCode = ReadString();

                room.Parallaxs = new Parallax[ReadInt()];

                for (int j = 0; j < room.Parallaxs.Length; j++)
                {
                    room.Parallaxs[j] = new Parallax();

                    room.Parallaxs[j].Visible = ReadBool();
                    room.Parallaxs[j].Foreground = ReadBool();
                    room.Parallaxs[j].BackgroundId = ReadInt();
                    room.Parallaxs[j].X = ReadInt();
                    room.Parallaxs[j].Y = ReadInt();
                    room.Parallaxs[j].TileHorizontally = ReadBool();
                    room.Parallaxs[j].TileVertically = ReadBool();
                    room.Parallaxs[j].HorizontalSpeed = ReadInt();
                    room.Parallaxs[j].VerticalSpeed = ReadInt();

                    room.Parallaxs[j].Stretch = ReadBool();
                }

                room.EnableViews = ReadBool();

                room.Views = new View[ReadInt()];

                for (int j = 0; j < room.Views.Length; j++)
                {
                    room.Views[j] = new View();

                    room.Views[j].Visible = ReadBool();
                    room.Views[j].ViewX = ReadInt();
                    room.Views[j].ViewY = ReadInt();
                    room.Views[j].ViewWidth = ReadInt();
                    room.Views[j].ViewHeight = ReadInt();
                    room.Views[j].PortX = ReadInt();
                    room.Views[j].PortY = ReadInt();
                    room.Views[j].PortWidth = ReadInt();
                    room.Views[j].PortHeight = ReadInt();

                    room.Views[j].HorizontalBorder = ReadInt();
                    room.Views[j].VerticalBorder = ReadInt();
                    room.Views[j].HorizontalSpeed = ReadInt();
                    room.Views[j].VerticalSpeed = ReadInt();
                    room.Views[j].FollowObject = ReadInt();
                }


                int instanceCount = ReadInt();
                for (int j = 0; j < instanceCount; j++)
                {
                    Instance instance = new Instance();

                    instance.X = ReadInt();
                    instance.Y = ReadInt();
                    instance.ObjectId = ReadInt();
                    instance.Id = ReadInt();

                    instance.CreationCode = ReadString();
                    instance.Locked = ReadBool();

                    Object obj = objects.Find(delegate(Object o) { return o.Id == instance.ObjectId; });

                    if (obj != null)
                    {
                        instance.Name = obj.Name;
                        instance.Depth = obj.Depth;
                    }

                    room.Instances.Add(instance);
                }

                int tileCount = ReadInt();
                for (int j = 0; j < tileCount; j++)
                {
                    Tile tile = new Tile();

                    tile.X = ReadInt();
                    tile.Y = ReadInt();
                    tile.BackgroundId = ReadInt();
                    tile.BackgroundX = ReadInt();
                    tile.BackgroundY = ReadInt();
                    tile.Width = ReadInt();
                    tile.Height = ReadInt();
                    tile.Depth = ReadInt();
                    tile.Locked = ReadBool();

                    room.Tiles.Add(tile);
                }

                room.RememberWindowSize = ReadBool();
                room.EditorWidth = ReadInt();
                room.EditorHeight = ReadInt();
                room.ShowGrid = ReadBool();
                room.ShowObjects = ReadBool();
                room.ShowTiles = ReadBool();
                room.ShowBackgrounds = ReadBool();
                room.ShowForegrounds = ReadBool();
                room.ShowViews = ReadBool();
                room.DeleteUnderlyingObjects = ReadBool();
                room.DeleteUnderlyingTiles = ReadBool();
                room.CurrentTab = (TabSetting)ReadInt();
                room.ScrollBarX = ReadInt();
                room.ScrollBarY = ReadInt();

                rooms.Add(room);
            }

            return rooms;
        }



        private List<Action> ReadActions()
        {
            ReadInt();

            List<Action> actions = new List<Action>();

            int count = ReadInt();
            for (int i = 0; i < count; i++)
            {
                Action action = new Action();

                ReadInt();

                action.LibraryId = ReadInt();
                action.ActionId = ReadInt();
                action.ActionKind = (ActionKind)ReadInt();
                action.AllowRelative = ReadBool();
                action.Question = ReadBool();
                action.CanApplyTo = ReadBool();
                action.ExecutionType = (ExecutionType)ReadInt();

                if (action.ExecutionType == ExecutionType.Function)
                    action.ExecuteCode = ReadString();
                else
                    ReadBytes(ReadInt());

                if (action.ExecutionType == ExecutionType.Code)
                    action.ExecuteCode = ReadString();
                else
                    ReadBytes(ReadInt());

                action.Arguments = new Argument[ReadInt()];

                int[] argTypes = new int[ReadInt()];

                for (int j = 0; j < argTypes.Length; j++)
                    argTypes[j] = ReadInt();

                action.AppliesTo = ReadInt();
                action.Relative = ReadBool();

                int argCount = ReadInt();

                for (int j = 0; j < argCount; j++)
                {
                    if (j >= action.Arguments.Length)
                    {
                        ReadBytes(ReadInt());
                        continue;
                    }

                    action.Arguments[j] = new Argument();

                    action.Arguments[j].Type = (ArgumentType)argTypes[j];

                    action.Arguments[j].Value = ReadString();
                }

                action.Not = ReadBool();

                actions.Add(action);
            }

            return actions;
        }



        private List<Include> ReadIncludes()
        {
            ReadInt(); // This is a raw read.

            List<Include> includes = new List<Include>();

            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {
                Include include = new Include();

                ReadInt();

                include.Filename = ReadString();
                include.FilePath = ReadString();
                include.OriginalFile = ReadBool();
                include.OriginalFileSize = ReadInt();
                include.StoreInEditable = ReadBool();

                if (include.StoreInEditable)
                    include.FileData = ReadBytes(ReadInt());

                include.ExportMode = (Export)(ReadInt());
                include.ExportFolder = ReadString();
                include.Overwrite = ReadBool();
                include.FreeMemory = ReadBool();
                include.RemoveAtEnd = ReadBool();

                includes.Add(include);
            }

            return includes;

        }



        private List<Extension> ReadExtensions()
        {
            ReadInt();

            List<Extension> extensions = new List<Extension>();

            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {
                Extension extension = new Extension();

                extension.Name = ReadString();

                extensions.Add(extension);
            }

            return extensions;
        }



        private GameInformation ReadGameInformation()
        {
            ReadInt();

            GameInformation gameInfo = new GameInformation();

            gameInfo.BackgroundColor = ReadInt();
            gameInfo.MimicGameWindow = ReadBool();

            gameInfo.FormCaption = ReadString();
            gameInfo.X = ReadInt();
            gameInfo.Y = ReadInt();
            gameInfo.Width = ReadInt();
            gameInfo.Height = ReadInt();
            gameInfo.ShowBorder = ReadBool();
            gameInfo.AllowResize = ReadBool();
            gameInfo.AlwaysOnTop = ReadBool();
            gameInfo.PauseGame = ReadBool();

            gameInfo.Information = ReadString();

            return gameInfo;
        }



        private ProjectNode ReadTree(string name, Project project)
        {
            ReadInt();

            int orderCount = ReadInt();

            for (int i = 0; i < orderCount; i++)
                project.RoomOrder.Add(ReadInt());

            ProjectNode projectTree = new ProjectNode();
            projectTree.Nodes = new List<ProjectNode>();
            projectTree.Name = name;
            projectTree.NodeType = ProjectNodeType.Parent;
            projectTree.Children = 12;

            for (int i = 0; i < 12; i++)
            {
                ProjectNode node = new ProjectNode();

                node.NodeType = (ProjectNodeType)ReadInt();
                node.ResourceType = (ResourceType)ReadInt();

                node.Id = ReadInt();
                node.Name = ReadString();
                node.Children = ReadInt();

                if (node.Children > 0)
                {
                    node.Nodes = new List<ProjectNode>();

                    for (int j = 0; j < node.Children; j++)
                        node.Nodes.Add(new ProjectNode());

                    ReadNodeRecursive(node);
                }

                projectTree.Nodes.Add(node);
            }

            return projectTree;
        }

        private void ReadNodeRecursive(ProjectNode parent)
        {
            foreach (ProjectNode node in parent.Nodes)
            {
                node.NodeType = (ProjectNodeType)ReadInt();
                node.ResourceType = (ResourceType)ReadInt();
                node.Id = ReadInt();
                node.Name = ReadString();
                node.Children = ReadInt();

                if (node.Children > 0)
                {
                    node.Nodes = new List<ProjectNode>();

                    for (int i = 0; i < node.Children; i++)
                        node.Nodes.Add(new ProjectNode());

                    ReadNodeRecursive(node);
                }
            }
        }



        protected override byte ReadByte()
        {
            byte t = base.ReadByte();
            if (m_obfuscation.SwapTable != null)
                return (byte)((m_obfuscation.SwapTable[1][t] - m_reader.Position + 1) & 255);
            else
                return t;
        }

        private byte[] DecompressData(byte[] input)
        {
            if (input == null)
                throw new System.ArgumentNullException("input");

            byte[] output;
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Inflater inf = new Inflater();
                    inf.SetInput(input);

                    while (!inf.IsFinished)
                    {
                        byte[] buf = new byte[1000];
                        int size = (int)inf.Inflate(buf);
                        ms.Write(buf, 0, size);
                    }

                    output = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                }
            }
            catch (System.Exception)
            {
                return input;
            }

            return output;
        }

        public int ReadGameId()
        {
            byte rawByte = base.ReadByte(); // the first byte of the game id is not obfuscated, so read it as a raw byte.
            m_reader.Seek(-1, SeekOrigin.Current);
            byte[] b = ReadBytes(4);
            b[0] = rawByte;
            return System.BitConverter.ToInt32(b, 0);
        }



    }
}
*/