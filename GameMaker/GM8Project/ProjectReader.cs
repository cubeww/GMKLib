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

using System.IO;
using SysPath = System.IO.Path;
using GameMaker.IO;
using System.Collections.Generic;
using GameMaker.ProjectCommon;
using GMPath = GameMaker.ProjectCommon.Path;
using ICSharpCode.SharpZipLib.Zip.Compression;
using Action = GameMaker.ProjectCommon.Action;
using Object = GameMaker.ProjectCommon.Object;
using System.IO.Compression;

namespace GameMaker.GM8Project
{
    public class ProjectReader : ReaderBase
    {


        private MemoryStream m_zlibStream = null;

        public ProjectReader()
        { }

        public ProjectReader(string path)
        {
            Open(path);
        }


        public Project ReadProject()
        {
            if (ReadInt() != 1234321)
                throw new System.IO.IOException("Not a valid Game Maker Project");

            int version = ReadInt();
            if (version != 800 && version != 810)
                throw new System.IO.IOException("Not a Game Maker 8 Project");

            Project project = new Project();
            project.FileVersion = (FileVersion)version;
            project.Settings = ReadSettings();
            project.Triggers = ReadTriggers();
            project.Settings.Constants = ReadConstants();
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

            project.ProjectTree = ReadTree(SysPath.GetFileName(FileName), project);
            Close();

            return project;
        }


        private GlobalSettings ReadSettings()
        {
            GlobalSettings settings = new GlobalSettings();
            settings.GameId = ReadInt();
            settings.Guid = ReadBytes(16);
            ReadInt();
            BeginDecompression();
            {
                settings.Fullscreen = ReadBool();
                settings.Interpolate = ReadBool();
                settings.NoBorders = ReadBool();
                settings.DisplayCursor = ReadBool();
                settings.Scaling = ReadInt();
                settings.AllowResizing = ReadBool();
                settings.StayOnTop = ReadBool();
                settings.OutsideColor = ReadInt();
                settings.SetResolution = ReadBool();
                settings.ColorDepth2 = (ColorDepth2)ReadInt();
                settings.Resolution2 = (Resolution2)ReadInt();
                settings.Frequency2 = (Frequency2)ReadInt();
                settings.NoButtons = ReadBool();
                settings.UseVSync = ReadBool();
                settings.DisableScreensavers = ReadBool();
                settings.F4ChangeScreenModes = ReadBool();
                settings.F1ShowInformation = ReadBool();
                settings.EscapeEndGame = ReadBool();
                settings.F5SaveF6Load = ReadBool();
                settings.F9Screenshot = ReadBool();
                settings.CloseButtonAsEsc = ReadBool();
                settings.Priority = (Priority)ReadInt();
                settings.FreezeOnLoseFocus = ReadBool();

                ProgressBar pbar = new ProgressBar();
                {
                    pbar.ProgressBarType = (ProgressBarType)ReadInt();

                    if (pbar.ProgressBarType == ProgressBarType.UserDefined)
                    {
                        if (ReadBool())
                            pbar.BackImage = ReadBytes(ReadInt());
                        if (ReadBool())
                            pbar.FrontImage = ReadBytes(ReadInt());
                    }

                    pbar.OwnSplashImage = ReadBool();
                    if (pbar.OwnSplashImage)
                    {
                        if (ReadBool())
                            pbar.SplashImage = ReadBytes(ReadInt());
                    }

                    pbar.MakePartiallyTransparent = ReadBool();
                    pbar.Alpha = ReadInt();
                    pbar.ScaleProgressBar = ReadBool();
                    settings.ProgressBar = pbar;
                }
                settings.Icon = ReadBytes(ReadInt());
                settings.DisplayErrors = ReadBool();
                settings.WriteLog = ReadBool();
                settings.AbortOnErrors = ReadBool();
                settings.TreatUninitializedAsZero = ReadBool();
                settings.Author = ReadString();
                settings.Version = ReadString();
                settings.SettingsLastModified = System.DateTime.FromOADate(ReadDouble());
                settings.Information = ReadString();
                settings.Major = ReadInt();
                settings.Minor = ReadInt();
                settings.Release = ReadInt();
                settings.Build = ReadInt();
                settings.Company = ReadString();
                settings.Product = ReadString();
                settings.Copyright = ReadString();
                settings.Description = ReadString();
                settings.BuildLastModified = System.DateTime.FromOADate(ReadDouble());
            }
            EndDecompression();

            return settings;
        }



        private List<Trigger> ReadTriggers()
        {
            ReadInt();

            List<Trigger> triggers = new List<Trigger>();
            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {
                BeginDecompression();
                if (ReadBool())
                {
                    ReadInt();
                    Trigger t = new Trigger();

                    t.Name = ReadString();
                    t.Condition = ReadString();
                    t.Moment = (MomentType)ReadInt();
                    t.Constant = ReadString();
                    triggers.Add(t);
                }
                EndDecompression();
            }

            Trigger.LastChanged = System.DateTime.FromOADate(ReadDouble());

            return triggers;
        }



        private List<Constant> ReadConstants()
        {
            ReadInt();

            List<Constant> constants = new List<Constant>();
            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {
                // Create a new constant
                Constant constant = new Constant();

                constant.Name = ReadString();
                constant.Value = ReadString();

                constants.Add(constant);
            }

            Constant.LastChanged = System.DateTime.FromOADate(ReadDouble());

            return constants;
        }



        private ResourceCollection<Sound> ReadSounds()
        {
            ReadInt();

            ResourceCollection<Sound> sounds = new ResourceCollection<Sound>();
            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {
                BeginDecompression();

                if (ReadBool() == false)
                {
                    sounds.LastId++;
                    EndDecompression();
                    continue;
                }

                Sound sound = new Sound();

                sound.Id = i;

                sound.Name = ReadString();
                sound.LastChanged = System.DateTime.FromOADate(ReadDouble());

                ReadInt();

                sound.SoundType = (SoundType)ReadInt();

                sound.FileType = ReadString();
                sound.FileName = ReadString();

                if (ReadBool())
                    sound.Data = ReadBytes(ReadInt());

                sound.Effects = ReadInt();
                sound.Volume = ReadDouble();
                sound.Pan = ReadDouble();
                sound.Preload = ReadBool();

                EndDecompression();

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
                BeginDecompression();

                if (!ReadBool())
                {
                    sprites.LastId++;
                    EndDecompression();
                    continue;
                }

                Sprite sprite = new Sprite();
                sprite.Id = i;
                sprite.Name = ReadString();

                sprite.LastChanged = System.DateTime.FromOADate(ReadDouble());

                ReadInt(); // no need for version

                sprite.OriginX = ReadInt();
                sprite.OriginY = ReadInt();

                int imgCount = ReadInt();

                for (int j = 0; j < imgCount; j++)
                {
                    ReadInt();

                    int width = ReadInt();
                    int height = ReadInt();

                    if (width != 0 && height != 0)
                    {
                        Image image = new Image();
                        image.Width = width;
                        image.Height = height;

                        image.Data = ReadBytes(ReadInt());
                        sprite.SubImages.Add(image);
                    }
                }

                sprite.ShapeMode = (Shape)ReadInt();
                sprite.AlphaTolerance = ReadInt();
                sprite.UseSeparateCollisionMasks = ReadBool();
                sprite.BoundingBoxMode = (BoundingBox)ReadInt();
                sprite.BoundingBoxLeft = ReadInt();
                sprite.BoundingBoxRight = ReadInt();
                sprite.BoundingBoxBottom = ReadInt();
                sprite.BoundingBoxTop = ReadInt();

                EndDecompression();

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
                BeginDecompression();

                if (!ReadBool())
                {
                    backgrounds.LastId++;
                    EndDecompression();
                    continue;
                }

                Background background = new Background();

                background.Id = i;

                background.Name = ReadString();
                background.LastChanged = System.DateTime.FromOADate(ReadDouble());

                ReadInt();

                background.UseAsTileSet = ReadBool();
                background.TileWidth = ReadInt();
                background.TileHeight = ReadInt();
                background.HorizontalOffset = ReadInt();
                background.VerticalOffset = ReadInt();
                background.HorizontalSeparation = ReadInt();
                background.VerticalSeparation = ReadInt();

                ReadInt();

                int width = ReadInt();
                int height = ReadInt();

                if (width != 0 && height != 0)
                {
                    Image image = new Image();

                    image.Width = width;
                    image.Height = height;
                    image.Data = ReadBytes(ReadInt());

                    background.Image = image;
                }
                EndDecompression();

                backgrounds.Add(background);
            }

            return backgrounds;
        }



        private ResourceCollection<GMPath> ReadPaths()
        {
            ReadInt();

            ResourceCollection<GMPath> paths = new ResourceCollection<GMPath>();

            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {
                BeginDecompression();

                if (!ReadBool())
                {
                    paths.LastId++;
                    EndDecompression();
                    continue;
                }

                GMPath path = new GMPath();
                path.Id = i;

                path.Name = ReadString();
                path.LastChanged = System.DateTime.FromOADate(ReadDouble());

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

                EndDecompression();
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
                BeginDecompression();

                if (!ReadBool())
                {
                    scripts.LastId++;
                    EndDecompression();
                    continue;
                }

                Script script = new Script();

                script.Id = i;

                script.Name = ReadString();

                script.LastChanged = System.DateTime.FromOADate(ReadDouble());

                ReadInt();

                script.Code = ReadString();

                EndDecompression();

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
                BeginDecompression();

                if (!ReadBool())
                {
                    fonts.LastId++;
                    EndDecompression();
                    continue;
                }

                Font font = new Font();

                font.Id = i;

                font.Name = ReadString();

                font.LastChanged = System.DateTime.FromOADate(ReadDouble());

                ReadInt(); // Version

                font.FontName = ReadString();
                font.Size = ReadInt();
                font.Bold = ReadBool();
                font.Italic = ReadBool();
                font.CharacterRangeMin = ReadInt();
                font.CharacterRangeMax = ReadInt();

                EndDecompression();
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
                BeginDecompression();

                if (!ReadBool())
                {
                    timelines.LastId++;
                    EndDecompression();
                    continue;
                }

                Timeline timeline = new Timeline();

                timeline.Id = i;
                timeline.Name = ReadString();
                timeline.LastChanged = System.DateTime.FromOADate(ReadDouble());

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

                EndDecompression();
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
                BeginDecompression();

                if (!ReadBool())
                {
                    objects.LastId++;
                    EndDecompression();
                    continue;
                }

                Object obj = new Object();

                obj.Id = i;
                obj.Name = ReadString();

                obj.LastChanged = System.DateTime.FromOADate(ReadDouble());

                ReadInt();

                obj.SpriteId = ReadInt();
                obj.Solid = ReadBool();
                obj.Visible = ReadBool();
                obj.Depth = ReadInt();
                obj.Persistent = ReadBool();
                obj.Parent = ReadInt();
                obj.Mask = ReadInt();

                ReadInt();

                for (int j = 0; j < 12; j++)
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

                EndDecompression();
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
                BeginDecompression();

                if (!ReadBool())
                {
                    rooms.LastId++;
                    EndDecompression();
                    continue;
                }

                Room room = new Room();

                room.Id = i;
                room.Name = ReadString();

                room.LastChanged = System.DateTime.FromOADate(ReadDouble());

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

                    Object obj = objects.Find(delegate (Object o) { return o.Id == instance.ObjectId; });

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

                EndDecompression();
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
                BeginDecompression();

                Include include = new Include();

                include.LastChanged = System.DateTime.FromOADate(ReadDouble()); // This is a compressed read.

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

                EndDecompression();
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

            BeginDecompression();

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

            gameInfo.LastChanged = System.DateTime.FromOADate(ReadDouble());

            gameInfo.Information = ReadString();

            EndDecompression();

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
            if (m_zlibStream != null)
                return (byte)m_zlibStream.ReadByte();
            else
                return base.ReadByte();
        }

        protected override byte[] ReadBytes(int count)
        {
            if (m_zlibStream != null)
            {
                byte[] b = new byte[count];
                m_zlibStream.Read(b, 0, count);

                return b;
            }
            else
            {
                return base.ReadBytes(count);
            }
        }

        private void BeginDecompression()
        {
            // It would be bad if we started writing over our own data, so prevent it by checking for null (This is probably overkill, but I REALLY want to emphasize this).
            if (m_zlibStream != null)
                throw new System.InvalidOperationException("Decompression() already in progress."); // Woops, we can't do that!
            
            int size = ReadInt();
            ReadBytes(2);

            MemoryStream compressed = new MemoryStream(ReadBytes(size - 2));

            DeflateStream deflateStream = new DeflateStream(compressed, CompressionMode.Decompress); // 注意： 这里第一个参数同样是填写压缩的数据，但是这次是作为输入的数据

            m_zlibStream = new MemoryStream();
            deflateStream.CopyTo(m_zlibStream);

            m_zlibStream.Position = 0;
        }

        private void EndDecompression()
        {
            // Overkill FTW!!!
            if (m_zlibStream == null)
                throw new System.InvalidOperationException("EndDecompression() cannot be called before BeginDecompression.");

            m_zlibStream.Close();
            m_zlibStream = null;
        }


    }
}
