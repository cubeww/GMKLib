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
using System.Reflection.PortableExecutable;

namespace GameMaker.GM8Project
{
    public class ProjectReader : ReaderBase
    {
        public ProjectReader()
        { }

        public ProjectReader(string path)
        {
            Open(path);
        }

        List<Task> Tasks { get; set; }

        public Project ReadProject()
        {
            Tasks = new List<Task>();

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

            Task.WaitAll(Tasks.ToArray());

            return project;
        }


        private GlobalSettings ReadSettings()
        {
            GlobalSettings settings = new GlobalSettings();
            settings.GameId = ReadInt();
            settings.Guid = ReadBytes(16);
            ReadInt();

            var reader = ReadChunk();
            reader.Decompress();
            settings.Fullscreen = reader.ReadBool();
            settings.Interpolate = reader.ReadBool();
            settings.NoBorders = reader.ReadBool();
            settings.DisplayCursor = reader.ReadBool();
            settings.Scaling = reader.ReadInt();
            settings.AllowResizing = reader.ReadBool();
            settings.StayOnTop = reader.ReadBool();
            settings.OutsideColor = reader.ReadInt();
            settings.SetResolution = reader.ReadBool();
            settings.ColorDepth2 = (ColorDepth2)reader.ReadInt();
            settings.Resolution2 = (Resolution2)reader.ReadInt();
            settings.Frequency2 = (Frequency2)reader.ReadInt();
            settings.NoButtons = reader.ReadBool();
            settings.UseVSync = reader.ReadBool();
            settings.DisableScreensavers = reader.ReadBool();
            settings.F4ChangeScreenModes = reader.ReadBool();
            settings.F1ShowInformation = reader.ReadBool();
            settings.EscapeEndGame = reader.ReadBool();
            settings.F5SaveF6Load = reader.ReadBool();
            settings.F9Screenshot = reader.ReadBool();
            settings.CloseButtonAsEsc = reader.ReadBool();
            settings.Priority = (Priority)reader.ReadInt();
            settings.FreezeOnLoseFocus = reader.ReadBool();

            ProgressBar pbar = new ProgressBar();
            {
                pbar.ProgressBarType = (ProgressBarType)reader.ReadInt();

                if (pbar.ProgressBarType == ProgressBarType.UserDefined)
                {
                    if (reader.ReadBool())
                        pbar.BackImage = reader.ReadBytes(reader.ReadInt());
                    if (reader.ReadBool())
                        pbar.FrontImage = reader.ReadBytes(reader.ReadInt());
                }

                pbar.OwnSplashImage = reader.ReadBool();
                if (pbar.OwnSplashImage)
                {
                    if (reader.ReadBool())
                        pbar.SplashImage = reader.ReadBytes(reader.ReadInt());
                }

                pbar.MakePartiallyTransparent = reader.ReadBool();
                pbar.Alpha = reader.ReadInt();
                pbar.ScaleProgressBar = reader.ReadBool();
                settings.ProgressBar = pbar;
            }
            settings.Icon = reader.ReadBytes(reader.ReadInt());
            settings.DisplayErrors = reader.ReadBool();
            settings.WriteLog = reader.ReadBool();
            settings.AbortOnErrors = reader.ReadBool();
            settings.TreatUninitializedAsZero = reader.ReadBool();
            settings.Author = reader.ReadString();
            settings.Version = reader.ReadString();
            settings.SettingsLastModified = System.DateTime.FromOADate(reader.ReadDouble());
            settings.Information = reader.ReadString();
            settings.Major = reader.ReadInt();
            settings.Minor = reader.ReadInt();
            settings.Release = reader.ReadInt();
            settings.Build = reader.ReadInt();
            settings.Company = reader.ReadString();
            settings.Product = reader.ReadString();
            settings.Copyright = reader.ReadString();
            settings.Description = reader.ReadString();
            settings.BuildLastModified = System.DateTime.FromOADate(reader.ReadDouble());

            return settings;
        }



        private List<Trigger> ReadTriggers()
        {
            ReadInt();

            List<Trigger> triggers = new List<Trigger>();
            int count = ReadInt();

            for (int i = 0; i < count; i++)
            {
                var reader = ReadChunk();
                reader.Decompress();
                if (reader.ReadBool())
                {
                    reader.ReadInt();
                    Trigger t = new Trigger();

                    t.Name = reader.ReadString();
                    t.Condition = reader.ReadString();
                    t.Moment = (MomentType)reader.ReadInt();
                    t.Constant = reader.ReadString();
                    triggers.Add(t);
                }
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
                var reader = ReadChunk();
                Tasks.Add(Task.Factory.StartNew(() =>
                {
                    reader.Decompress();

                    if (reader.ReadBool() == false)
                    {
                        sounds.LastId++;
                        return;
                    }

                    Sound sound = new Sound();

                    sound.Id = i;

                    sound.Name = reader.ReadString();
                    sound.LastChanged = System.DateTime.FromOADate(reader.ReadDouble());

                    reader.ReadInt();

                    sound.SoundType = (SoundType)reader.ReadInt();

                    sound.FileType = reader.ReadString();
                    sound.FileName = reader.ReadString();

                    if (reader.ReadBool())
                        sound.Data = reader.ReadBytes(reader.ReadInt());

                    sound.Effects = reader.ReadInt();
                    sound.Volume = reader.ReadDouble();
                    sound.Pan = reader.ReadDouble();
                    sound.Preload = reader.ReadBool();

                    sounds.Add(sound);
                }));
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
                var reader = ReadChunk();
                Tasks.Add(Task.Factory.StartNew(() => {
                    reader.Decompress();
                    if (!reader.ReadBool())
                    {
                        sprites.LastId++;
                        return;
                    }

                    Sprite sprite = new Sprite();
                    sprite.Id = i;
                    sprite.Name = reader.ReadString();

                    sprite.LastChanged = System.DateTime.FromOADate(reader.ReadDouble());

                    reader.ReadInt(); // no need for version

                    sprite.OriginX = reader.ReadInt();
                    sprite.OriginY = reader.ReadInt();

                    int imgCount = reader.ReadInt();

                    for (int j = 0; j < imgCount; j++)
                    {
                        reader.ReadInt();

                        int width = reader.ReadInt();
                        int height = reader.ReadInt();

                        if (width != 0 && height != 0)
                        {
                            Image image = new Image();
                            image.Width = width;
                            image.Height = height;

                            image.Data = reader.ReadBytes(reader.ReadInt());
                            sprite.SubImages.Add(image);
                        }
                    }

                    sprite.ShapeMode = (Shape)reader.ReadInt();
                    sprite.AlphaTolerance = reader.ReadInt();
                    sprite.UseSeparateCollisionMasks = reader.ReadBool();
                    sprite.BoundingBoxMode = (BoundingBox)reader.ReadInt();
                    sprite.BoundingBoxLeft = reader.ReadInt();
                    sprite.BoundingBoxRight = reader.ReadInt();
                    sprite.BoundingBoxBottom = reader.ReadInt();
                    sprite.BoundingBoxTop = reader.ReadInt();

                    sprites.Add(sprite);
                }));       
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
                var reader = ReadChunk();
                Tasks.Add(Task.Factory.StartNew(() =>
                {
                    reader.Decompress();

                    if (!reader.ReadBool())
                    {
                        backgrounds.LastId++;
                        return;
                    }

                    Background background = new Background();

                    background.Id = i;

                    background.Name = reader.ReadString();
                    background.LastChanged = System.DateTime.FromOADate(reader.ReadDouble());

                    reader.ReadInt();

                    background.UseAsTileSet = reader.ReadBool();
                    background.TileWidth = reader.ReadInt();
                    background.TileHeight = reader.ReadInt();
                    background.HorizontalOffset = reader.ReadInt();
                    background.VerticalOffset = reader.ReadInt();
                    background.HorizontalSeparation = reader.ReadInt();
                    background.VerticalSeparation = reader.ReadInt();

                    reader.ReadInt();

                    int width = reader.ReadInt();
                    int height = reader.ReadInt();

                    if (width != 0 && height != 0)
                    {
                        Image image = new Image();

                        image.Width = width;
                        image.Height = height;
                        image.Data = reader.ReadBytes(reader.ReadInt());

                        background.Image = image;
                    }

                    backgrounds.Add(background);
                }));
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
                var reader = ReadChunk();
                Tasks.Add(Task.Factory.StartNew(() =>
                {
                    reader.Decompress();

                    if (!reader.ReadBool())
                    {
                        paths.LastId++;
                        return;
                    }

                    GMPath path = new GMPath();
                    path.Id = i;

                    path.Name = reader.ReadString();
                    path.LastChanged = System.DateTime.FromOADate(reader.ReadDouble());

                    reader.ReadInt();
                    path.Smooth = reader.ReadBool();
                    path.Closed = reader.ReadBool();
                    path.Precision = reader.ReadInt();
                    path.RoomId = reader.ReadInt();
                    path.SnapX = reader.ReadInt();
                    path.SnapY = reader.ReadInt();

                    path.Points = new List<Point>();

                    int pointCount = reader.ReadInt();

                    for (int j = 0; j < pointCount; j++)
                    {
                        Point point = new Point();
                        point.X = reader.ReadDouble();
                        point.Y = reader.ReadDouble();
                        point.Speed = reader.ReadDouble();

                        path.Points.Add(point);
                    }

                    paths.Add(path);
                }));
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
                var reader = ReadChunk();
                Tasks.Add(Task.Factory.StartNew(() =>
                {
                    reader.Decompress();

                    if (!reader.ReadBool())
                    {
                        scripts.LastId++;
                        return;
                    }

                    Script script = new Script();

                    script.Id = i;

                    script.Name = reader.ReadString();

                    script.LastChanged = System.DateTime.FromOADate(reader.ReadDouble());

                    reader.ReadInt();

                    script.Code = reader.ReadString();

                    scripts.Add(script);
                }));
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
                var reader = ReadChunk();
                Tasks.Add(Task.Factory.StartNew(() =>
                {
                    reader.Decompress();

                    if (!reader.ReadBool())
                    {
                        fonts.LastId++;
                        return;
                    }

                    Font font = new Font();

                    font.Id = i;

                    font.Name = reader.ReadString();

                    font.LastChanged = System.DateTime.FromOADate(reader.ReadDouble());

                    reader.ReadInt(); // Version

                    font.FontName = reader.ReadString();
                    font.Size = reader.ReadInt();
                    font.Bold = reader.ReadBool();
                    font.Italic = reader.ReadBool();
                    font.CharacterRangeMin = reader.ReadInt();
                    font.CharacterRangeMax = reader.ReadInt();

                    fonts.Add(font);
                }));
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
                var reader = ReadChunk();
                Tasks.Add(Task.Factory.StartNew(() =>
                {
                    reader.Decompress();

                    if (!reader.ReadBool())
                    {
                        timelines.LastId++;
                        return;
                    }

                    Timeline timeline = new Timeline();

                    timeline.Id = i;
                    timeline.Name = reader.ReadString();
                    timeline.LastChanged = System.DateTime.FromOADate(reader.ReadDouble());

                    reader.ReadInt();

                    timeline.Moments = new List<Moment>();

                    int momentCount = reader.ReadInt();

                    for (int j = 0; j < momentCount; j++)
                    {
                        Moment moment = new Moment();

                        moment.StepIndex = reader.ReadInt();

                        moment.Actions = ReadActions(reader);

                        timeline.Moments.Add(moment);
                    }

                    timelines.Add(timeline);
                }));
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
                var reader = ReadChunk();
                Tasks.Add(Task.Factory.StartNew(() =>
                {
                    reader.Decompress();

                    if (!reader.ReadBool())
                    {
                        objects.LastId++;
                        return;
                    }

                    Object obj = new Object();

                    obj.Id = i;
                    obj.Name = reader.ReadString();

                    obj.LastChanged = System.DateTime.FromOADate(reader.ReadDouble());

                    reader.ReadInt();

                    obj.SpriteId = reader.ReadInt();
                    obj.Solid = reader.ReadBool();
                    obj.Visible = reader.ReadBool();
                    obj.Depth = reader.ReadInt();
                    obj.Persistent = reader.ReadBool();
                    obj.Parent = reader.ReadInt();
                    obj.Mask = reader.ReadInt();

                    reader.ReadInt();

                    for (int j = 0; j < 12; j++)
                    {
                        bool done = false;
                        while (!done)
                        {
                            int eventNum = reader.ReadInt();

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
                                ev.Actions = ReadActions(reader);
                                obj.Events[j].Add(ev);
                            }
                            else
                                done = true;
                        }
                    }

                    objects.Add(obj);
                }));
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
                var reader = ReadChunk();
                Tasks.Add(Task.Factory.StartNew(() =>
                {
                    reader.Decompress();

                    if (!reader.ReadBool())
                    {
                        rooms.LastId++;
                        return;
                    }

                    Room room = new Room();

                    room.Id = i;
                    room.Name = reader.ReadString();

                    room.LastChanged = System.DateTime.FromOADate(reader.ReadDouble());

                    reader.ReadInt();

                    room.Caption = reader.ReadString();
                    room.Width = reader.ReadInt();
                    room.Height = reader.ReadInt();
                    room.SnapY = reader.ReadInt();
                    room.SnapX = reader.ReadInt();
                    room.IsometricGrid = reader.ReadBool();

                    room.Speed = reader.ReadInt();
                    room.Persistent = reader.ReadBool();
                    room.BackgroundColor = reader.ReadInt();
                    room.DrawBackgroundColor = reader.ReadBool();
                    room.CreationCode = reader.ReadString();

                    room.Parallaxs = new Parallax[reader.ReadInt()];

                    for (int j = 0; j < room.Parallaxs.Length; j++)
                    {
                        room.Parallaxs[j] = new Parallax();

                        room.Parallaxs[j].Visible = reader.ReadBool();
                        room.Parallaxs[j].Foreground = reader.ReadBool();
                        room.Parallaxs[j].BackgroundId = reader.ReadInt();
                        room.Parallaxs[j].X = reader.ReadInt();
                        room.Parallaxs[j].Y = reader.ReadInt();
                        room.Parallaxs[j].TileHorizontally = reader.ReadBool();
                        room.Parallaxs[j].TileVertically = reader.ReadBool();
                        room.Parallaxs[j].HorizontalSpeed = reader.ReadInt();
                        room.Parallaxs[j].VerticalSpeed = reader.ReadInt();

                        room.Parallaxs[j].Stretch = reader.ReadBool();
                    }

                    room.EnableViews = reader.ReadBool();

                    room.Views = new View[reader.ReadInt()];

                    for (int j = 0; j < room.Views.Length; j++)
                    {
                        room.Views[j] = new View();

                        room.Views[j].Visible = reader.ReadBool();
                        room.Views[j].ViewX = reader.ReadInt();
                        room.Views[j].ViewY = reader.ReadInt();
                        room.Views[j].ViewWidth = reader.ReadInt();
                        room.Views[j].ViewHeight = reader.ReadInt();
                        room.Views[j].PortX = reader.ReadInt();
                        room.Views[j].PortY = reader.ReadInt();
                        room.Views[j].PortWidth = reader.ReadInt();
                        room.Views[j].PortHeight = reader.ReadInt();

                        room.Views[j].HorizontalBorder = reader.ReadInt();
                        room.Views[j].VerticalBorder = reader.ReadInt();
                        room.Views[j].HorizontalSpeed = reader.ReadInt();
                        room.Views[j].VerticalSpeed = reader.ReadInt();
                        room.Views[j].FollowObject = reader.ReadInt();
                    }


                    int instanceCount = reader.ReadInt();
                    for (int j = 0; j < instanceCount; j++)
                    {
                        Instance instance = new Instance();

                        instance.X = reader.ReadInt();
                        instance.Y = reader.ReadInt();
                        instance.ObjectId = reader.ReadInt();
                        instance.Id = reader.ReadInt();

                        instance.CreationCode = reader.ReadString();
                        instance.Locked = reader.ReadBool();

                        Object obj = objects.Find(delegate (Object o) { return o.Id == instance.ObjectId; });

                        if (obj != null)
                        {
                            instance.Name = obj.Name;
                            instance.Depth = obj.Depth;
                        }

                        room.Instances.Add(instance);
                    }

                    int tileCount = reader.ReadInt();
                    for (int j = 0; j < tileCount; j++)
                    {
                        Tile tile = new Tile();

                        tile.X = reader.ReadInt();
                        tile.Y = reader.ReadInt();
                        tile.BackgroundId = reader.ReadInt();
                        tile.BackgroundX = reader.ReadInt();
                        tile.BackgroundY = reader.ReadInt();
                        tile.Width = reader.ReadInt();
                        tile.Height = reader.ReadInt();
                        tile.Depth = reader.ReadInt();
                        tile.Locked = reader.ReadBool();

                        room.Tiles.Add(tile);
                    }

                    room.RememberWindowSize = reader.ReadBool();
                    room.EditorWidth = reader.ReadInt();
                    room.EditorHeight = reader.ReadInt();
                    room.ShowGrid = reader.ReadBool();
                    room.ShowObjects = reader.ReadBool();
                    room.ShowTiles = reader.ReadBool();
                    room.ShowBackgrounds = reader.ReadBool();
                    room.ShowForegrounds = reader.ReadBool();
                    room.ShowViews = reader.ReadBool();
                    room.DeleteUnderlyingObjects = reader.ReadBool();
                    room.DeleteUnderlyingTiles = reader.ReadBool();
                    room.CurrentTab = (TabSetting)reader.ReadInt();
                    room.ScrollBarX = reader.ReadInt();
                    room.ScrollBarY = reader.ReadInt();

                    rooms.Add(room);
                }));
            }

            return rooms;
        }



        private List<Action> ReadActions(ReaderBase reader)
        {
            reader.ReadInt();

            List<Action> actions = new List<Action>();

            int count = reader.ReadInt();
            for (int i = 0; i < count; i++)
            {
                Action action = new Action();

                reader.ReadInt();

                action.LibraryId = reader.ReadInt();
                action.ActionId = reader.ReadInt();
                action.ActionKind = (ActionKind)reader.ReadInt();
                action.AllowRelative = reader.ReadBool();
                action.Question = reader.ReadBool();
                action.CanApplyTo = reader.ReadBool();
                action.ExecutionType = (ExecutionType)reader.ReadInt();

                if (action.ExecutionType == ExecutionType.Function)
                    action.ExecuteCode = reader.ReadString();
                else
                    reader.ReadBytes(reader.ReadInt());

                if (action.ExecutionType == ExecutionType.Code)
                    action.ExecuteCode = reader.ReadString();
                else
                    reader.ReadBytes(reader.ReadInt());

                action.Arguments = new Argument[reader.ReadInt()];

                int[] argTypes = new int[reader.ReadInt()];

                for (int j = 0; j < argTypes.Length; j++)
                    argTypes[j] = reader.ReadInt();

                action.AppliesTo = reader.ReadInt();
                action.Relative = reader.ReadBool();

                int argCount = reader.ReadInt();

                for (int j = 0; j < argCount; j++)
                {
                    if (j >= action.Arguments.Length)
                    {
                        reader.ReadBytes(reader.ReadInt());
                        continue;
                    }

                    action.Arguments[j] = new Argument();

                    action.Arguments[j].Type = (ArgumentType)argTypes[j];

                    action.Arguments[j].Value = reader.ReadString();
                }

                action.Not = reader.ReadBool();

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
                var reader = ReadChunk();
                Tasks.Add(Task.Factory.StartNew(() =>
                {
                    reader.Decompress();

                    Include include = new Include();

                    include.LastChanged = System.DateTime.FromOADate(reader.ReadDouble()); // This is a compressed read.

                    reader.ReadInt();

                    include.Filename = reader.ReadString();
                    include.FilePath = reader.ReadString();
                    include.OriginalFile = reader.ReadBool();
                    include.OriginalFileSize = reader.ReadInt();
                    include.StoreInEditable = reader.ReadBool();

                    if (include.StoreInEditable)
                        include.FileData = reader.ReadBytes(reader.ReadInt());

                    include.ExportMode = (Export)(reader.ReadInt());
                    include.ExportFolder = reader.ReadString();
                    include.Overwrite = reader.ReadBool();
                    include.FreeMemory = reader.ReadBool();
                    include.RemoveAtEnd = reader.ReadBool();

                    includes.Add(include);
                }));
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

            var reader = ReadChunk();
            reader.Decompress();

            GameInformation gameInfo = new GameInformation();

            gameInfo.BackgroundColor = reader.ReadInt();
            gameInfo.MimicGameWindow = reader.ReadBool();

            gameInfo.FormCaption = reader.ReadString();
            gameInfo.X = reader.ReadInt();
            gameInfo.Y = reader.ReadInt();
            gameInfo.Width = reader.ReadInt();
            gameInfo.Height = reader.ReadInt();
            gameInfo.ShowBorder = reader.ReadBool();
            gameInfo.AllowResize = reader.ReadBool();
            gameInfo.AlwaysOnTop = reader.ReadBool();
            gameInfo.PauseGame = reader.ReadBool();

            gameInfo.LastChanged = System.DateTime.FromOADate(reader.ReadDouble());

            gameInfo.Information = reader.ReadString();

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
    }
}
