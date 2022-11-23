// ProjectWriter.cs
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

using System.Collections.Generic;
using System.IO;
using GameMaker.IO;
using GameMaker.ProjectCommon;
using GMPath = GameMaker.ProjectCommon.Path;
using ICSharpCode.SharpZipLib.Zip.Compression;
using Action = GameMaker.ProjectCommon.Action;
using Object = GameMaker.ProjectCommon.Object;

namespace GameMaker.GM8Project
{
    public class ProjectWriter : WriterBase
    {
        private List<byte> m_zlibBuffer = null;
        private Project m_project = null;

        public ProjectWriter()
            : this(string.Empty, null)
        {}

        public ProjectWriter(string path, Project project)
        {
            m_project = project;
            Open(path);
        }

        public void WriteProject()
        {
            WriteInt(1234321);
            WriteInt(800);            
            WriteSettings(m_project.Settings);
            WriteTriggers(m_project.Triggers);
            WriteContants(m_project.Settings.Constants);
            WriteSounds(m_project.Sounds);
            WriteSprites(m_project.Sprites);
            WriteBackgrounds(m_project.Backgrounds);
            WritePaths(m_project.Paths);
            WriteScripts(m_project.Scripts);
            WriteFonts(m_project.Fonts);
            WriteTimelines(m_project.Timelines);
            WriteObjects(m_project.Objects);
            WriteRooms(m_project.Rooms);

            WriteInt(m_project.LastInstanceId);
            WriteInt(m_project.LastTileId);

            WriteIncludes(m_project.Includes);
            WriteExtensions(m_project.Extensions);
            WriteGameInformation(m_project.GameInformation);

            WriteInt(500);

            WriteInt(m_project.Libraries.Count);

            foreach (Library library in m_project.Libraries)
            {
                WriteString(library.Code);
            }

            WriteTree(m_project.ProjectTree);

            Close(); // Close the stream and write the data to a file.
        }


        #region WriteSettings

        private void WriteSettings(GlobalSettings settings)
        {
            WriteInt(settings.GameId);
            WriteBytes(settings.Guid);
            WriteInt(800);
            BeginCompression();
            {
                WriteBool(settings.Fullscreen);
                WriteBool(settings.Interpolate);
                WriteBool(settings.NoBorders);
                WriteBool(settings.DisplayCursor);
                WriteInt(settings.Scaling);
                WriteBool(settings.AllowResizing);
                WriteBool(settings.StayOnTop);
                WriteInt(settings.OutsideColor);
                WriteBool(settings.SetResolution);
                WriteInt((int)settings.ColorDepth2);
                WriteInt((int)settings.Resolution2);
                WriteInt((int)settings.Frequency2);
                WriteBool(settings.NoButtons);
                WriteBool(settings.UseVSync);
                WriteBool(settings.DisableScreensavers);
                WriteBool(settings.F4ChangeScreenModes);
                WriteBool(settings.F1ShowInformation);
                WriteBool(settings.EscapeEndGame);
                WriteBool(settings.F5SaveF6Load);
                WriteBool(settings.F9Screenshot);
                WriteBool(settings.CloseButtonAsEsc);
                WriteInt((int)settings.Priority);
                WriteBool(settings.FreezeOnLoseFocus);
                ProgressBar pbar = settings.ProgressBar;
                {
                    WriteInt((int)pbar.ProgressBarType);
                    if (pbar.ProgressBarType == ProgressBarType.UserDefined)
                    {
                        if (pbar.BackImage != null)
                        {
                            WriteBool(true);
                            WriteInt(pbar.BackImage.Length);
                            WriteBytes(pbar.BackImage);
                        }
                        else
                            WriteBool(false);

                        if (pbar.FrontImage != null)
                        {
                            WriteBool(true);
                            WriteInt(pbar.FrontImage.Length);
                            WriteBytes(pbar.FrontImage);
                        }
                        else
                            WriteBool(false);
                    }

                    WriteBool(pbar.OwnSplashImage);
                    if (pbar.OwnSplashImage)
                    {
                        if (pbar.SplashImage != null)
                        {
                            WriteBool(true);
                            WriteInt(pbar.SplashImage.Length);
                            WriteBytes(pbar.SplashImage);
                        }
                        else
                            WriteBool(false);
                    }
                    WriteBool(pbar.MakePartiallyTransparent);
                    WriteInt(pbar.Alpha);
                    WriteBool(pbar.ScaleProgressBar);
                }

                WriteInt(settings.Icon.Length);
                WriteBytes(settings.Icon);
                WriteBool(settings.DisplayErrors);
                WriteBool(settings.WriteLog);
                WriteBool(settings.AbortOnErrors);
                WriteBool(settings.TreatUninitializedAsZero);
                WriteString(settings.Author);
                WriteString(settings.Version);
                WriteDouble(settings.SettingsLastModified.ToOADate());
                WriteString(settings.Information);
                WriteInt(settings.Major);
                WriteInt(settings.Minor);
                WriteInt(settings.Release);
                WriteInt(settings.Build);
                WriteString(settings.Company);
                WriteString(settings.Product);
                WriteString(settings.Copyright);
                WriteString(settings.Description);
                WriteDouble(settings.BuildLastModified.ToOADate());
            }
            EndCompression();
        }

        #endregion

        #region WriteTriggers

        private void WriteTriggers(List<Trigger> triggers)
        {
            WriteInt(800);

            // Write trigger ammount
            WriteInt(triggers.Count);

            foreach (Trigger trigger in triggers)
            {
                BeginCompression();
                
                // This trigger exists!
                WriteBool(true);
                WriteInt(800);

                // Write trigger data.
                WriteString(trigger.Name);
                WriteString(trigger.Condition);
                WriteInt((int)trigger.Moment);
                WriteString(trigger.Constant);


                EndCompression();
            }

            WriteDouble(Trigger.LastChanged.ToOADate());
        }

        #endregion

        #region WriteConstants

        private void WriteContants(List<Constant> consants)
        {
            WriteInt(800);
            WriteInt(consants.Count);

            foreach (Constant constant in consants)
            {
                WriteString(constant.Name);
                WriteString(constant.Value);
            }

            WriteDouble(Constant.LastChanged.ToOADate());
        }

        #endregion

        #region WriteSounds

        private void WriteSounds(ResourceCollection<Sound> sounds)
        {
            WriteInt(800);
            
            // Write number of sound ids.
            WriteInt(sounds.LastId + 1);

            // Iterate through sound ids.
            for (int i = 0; i < sounds.LastId + 1; i++)
            {
                BeginCompression();

                Sound sound = sounds.Find(delegate(Sound s) { return s.Id == i; });

                WriteBool(sound != null);

                if (sound == null)
                {
                    EndCompression();
                    continue;
                }

                WriteString(sound.Name);
                WriteDouble(sound.LastChanged.ToOADate());

                WriteInt(800);

                WriteInt((int)sound.SoundType);

                WriteString(sound.FileType);
                WriteString(sound.FileName);

                WriteBool((sound.Data != null));
                if (sound.Data != null)
                {
                    WriteInt(sound.Data.Length);
                    WriteBytes(sound.Data);
                }

                WriteInt(sound.Effects);
                WriteDouble(sound.Volume);
                WriteDouble(sound.Pan);
                WriteBool(sound.Preload);

                EndCompression();
            }
        }

        #endregion

        #region WriteSprites

        private void WriteSprites(ResourceCollection<Sprite> sprites)
        {
            WriteInt(800);

            // Write number of sprite ids.
            WriteInt(sprites.LastId + 1);

            // Iterate through sprites.
            for (int i = 0; i < sprites.LastId + 1; i++)
            {
                BeginCompression();
                
                Sprite sprite = sprites.Find(delegate(Sprite s) { return s.Id == i; });
                WriteBool(sprite != null);

                if (sprite == null)
                {
                    EndCompression();
                    continue;
                }

                WriteString(sprite.Name);

                WriteDouble(sprite.LastChanged.ToOADate());

                WriteInt(800);

                WriteInt(sprite.OriginX);
                WriteInt(sprite.OriginY);

                WriteInt(sprite.SubImages.Count);

                foreach (Image image in sprite.SubImages)
                {
                    WriteInt(800);

                    WriteInt(image.Width);
                    WriteInt(image.Height);
                    if (image.Width != 0 && image.Height != 0)
                    {
                        WriteInt(image.Data.Length);
                        WriteBytes(image.Data);
                    }
                }

                WriteInt((int)sprite.ShapeMode);
                WriteInt(sprite.AlphaTolerance);
                WriteBool(sprite.UseSeparateCollisionMasks);
                WriteInt((int)sprite.BoundingBoxMode);
                WriteInt(sprite.BoundingBoxLeft);
                WriteInt(sprite.BoundingBoxRight);
                WriteInt(sprite.BoundingBoxBottom);
                WriteInt(sprite.BoundingBoxTop);

                EndCompression();
            }
        }
        
        #endregion

        #region WriteBackgrounds

        private void WriteBackgrounds(ResourceCollection<Background> backgrounds)
        {
            WriteInt(800);

            // Amount of background ids.
            WriteInt(backgrounds.LastId + 1);

            // Iterate through backgrounds.
            for (int i = 0; i < backgrounds.LastId + 1; i++)
            {
                BeginCompression();
                Background background = backgrounds.Find(delegate(Background b) { return b.Id == i; });
                WriteBool(background != null);

                if (background == null)
                {
                    EndCompression();
                    continue;
                }

                WriteString(background.Name);
                WriteDouble(background.LastChanged.ToOADate());

                WriteInt(710);

                WriteBool(background.UseAsTileSet);
                WriteInt(background.TileWidth);
                WriteInt(background.TileHeight);
                WriteInt(background.HorizontalOffset);
                WriteInt(background.VerticalOffset);
                WriteInt(background.HorizontalSeparation);
                WriteInt(background.VerticalSeparation);

                WriteInt(800);

                WriteInt(background.Image.Width);
                WriteInt(background.Image.Height);

                if (background.Image.Width != 0 && background.Image.Height != 0)
                {
                    WriteInt(background.Image.Data.Length);
                    WriteBytes(background.Image.Data);
                }

                EndCompression();
            }
        }

        #endregion

        #region WritePaths

        private void WritePaths(ResourceCollection<GMPath> paths)
        {
            WriteInt(800);

            // Amount of path ids.
            WriteInt(paths.LastId + 1);

            // Iterate through paths.
            for (int i = 0; i < paths.LastId + 1; i++)
            {
                BeginCompression();
                GMPath path = paths.Find(delegate(GMPath p) { return p.Id == i; });
                
                WriteBool(path != null);

                if (path == null)
                {
                    EndCompression();
                    continue;
                }

                WriteString(path.Name);
                WriteDouble(path.LastChanged.ToOADate());

                WriteInt(530);

                WriteBool(path.Smooth);
                WriteBool(path.Closed);
                WriteInt(path.Precision);
                WriteInt(path.RoomId);
                WriteInt(path.SnapX);
                WriteInt(path.SnapY);

                WriteInt(path.Points.Count);

                foreach (Point point in path.Points)
                {
                    WriteDouble(point.X);
                    WriteDouble(point.Y);
                    WriteDouble(point.Speed);
                }

                EndCompression();
            }
        }

        #endregion

        #region WriteScripts

        private void WriteScripts(ResourceCollection<Script> scripts)
        {
            WriteInt(800);

            WriteInt(scripts.LastId + 1);

            for (int i = 0; i < scripts.LastId + 1; i++)
            {
                BeginCompression();

                Script script = scripts.Find(delegate(Script s) { return s.Id == i; });

                WriteBool(script != null);

                if (script == null)
                {
                    EndCompression();
                    continue;
                }

                WriteString(script.Name);
                WriteDouble(script.LastChanged.ToOADate());

                WriteInt(800);

                WriteString(script.Code);

                EndCompression();
            }
        }

        #endregion

        #region WriteFonts

        private void WriteFonts(ResourceCollection<Font> fonts)
        {
            WriteInt(800);

            WriteInt(fonts.LastId + 1);

            for (int i = 0; i < fonts.LastId + 1; i++)
            {
                BeginCompression();

                Font font = fonts.Find(delegate(Font f) { return f.Id == i; });
                
                WriteBool(font != null);
                if (font == null)
                {
                    EndCompression();
                    continue;
                }

                // Node data
                WriteString(font.Name);
                WriteDouble(font.LastChanged.ToOADate());

                WriteInt(800);

                // Font data
                WriteString(font.FontName);
                WriteInt(font.Size);
                WriteBool(font.Bold);
                WriteBool(font.Italic);
                WriteInt(font.CharacterRangeMin);
                WriteInt(font.CharacterRangeMax);

                EndCompression();
            }
        }

        #endregion

        #region WriteTimelines

        private void WriteTimelines(ResourceCollection<Timeline> timelines)
        {
            WriteInt(800);

            WriteInt(timelines.LastId + 1);

            for(int i = 0; i < timelines.LastId + 1; i++)
            {
                Timeline timeline = timelines.Find(delegate(Timeline t) { return t.Id == i;});

                BeginCompression();
                WriteBool((timeline != null));

                if (timeline == null)
                {                
                    EndCompression();
                    continue;
                }

                WriteString(timeline.Name);
                WriteDouble(timeline.LastChanged.ToOADate());

                WriteInt(500);

                WriteInt(timeline.Moments.Count);

                foreach (Moment moment in timeline.Moments)
                {
                    WriteInt(moment.StepIndex);
                    WriteActions(moment.Actions);
                }

                EndCompression();
            }
        }

        #endregion

        #region WriteObjects

        private void WriteObjects(ResourceCollection<Object> objects)
        {
            WriteInt(800);

            WriteInt(objects.LastId + 1);

            for (int i = 0; i < objects.LastId + 1; i++)
            {
                Object obj = objects.Find(delegate(Object o) { return o.Id == i; });

                BeginCompression();
                WriteBool(obj != null);

                if (obj == null)
                {
                    EndCompression();
                    continue;
                }

                WriteString(obj.Name);
                WriteDouble(obj.LastChanged.ToOADate());

                WriteInt(430);

                // Object data
                WriteInt(obj.SpriteId);
                WriteBool(obj.Solid);
                WriteBool(obj.Visible);
                WriteInt(obj.Depth);
                WriteBool(obj.Persistent);
                WriteInt(obj.Parent);
                WriteInt(obj.Mask);

                WriteInt(11);

                for (int j = 0; j < 12; j++)
                {
                    foreach (Event ev in obj.Events[j])
                    {
                        if (ev.Actions != null)
                        {
                            if (ev.MainType == EventType.Collision)
                                WriteInt(ev.OtherId);
                            else
                                WriteInt(ev.Subtype);

                            // Write the event actions
                            WriteActions(ev.Actions);
                        }
                    }

                    WriteInt(-1);
                }

                EndCompression();
            }
        }

        #endregion

        #region WriteRooms

        private void WriteRooms(ResourceCollection<Room> rooms)
        {
            WriteInt(800);

            WriteInt(rooms.LastId + 1);

            for (int i = 0; i < rooms.LastId + 1; i++)
            {
                BeginCompression();
                Room room = rooms.Find(delegate(Room r) { return r.Id == i; });

                WriteBool(room != null);

                if (room == null)
                {
                    EndCompression();
                    continue;
                }

                WriteString(room.Name);
                WriteDouble(room.LastChanged.ToOADate());

                WriteInt(541);

                // Write room data.
                WriteString(room.Caption);
                WriteInt(room.Width);
                WriteInt(room.Height);
                WriteInt(room.SnapY);
                WriteInt(room.SnapX);
                WriteBool(room.IsometricGrid);

                WriteInt(room.Speed);
                WriteBool(room.Persistent);
                WriteInt(room.BackgroundColor);
                WriteBool(room.DrawBackgroundColor);
                WriteString(room.CreationCode);

                // Write the amount of room parallaxes.
                WriteInt(8);

                // Iterate through parallaxs.
                for (int j = 0; j < room.Parallaxs.Length; j++)
                {
                    // Write room parallax data.
                    WriteBool(room.Parallaxs[j].Visible);
                    WriteBool(room.Parallaxs[j].Foreground);
                    WriteInt(room.Parallaxs[j].BackgroundId);
                    WriteInt(room.Parallaxs[j].X);
                    WriteInt(room.Parallaxs[j].Y);
                    WriteBool(room.Parallaxs[j].TileHorizontally);
                    WriteBool(room.Parallaxs[j].TileVertically);
                    WriteInt(room.Parallaxs[j].HorizontalSpeed);
                    WriteInt(room.Parallaxs[j].VerticalSpeed);

                    WriteBool(room.Parallaxs[j].Stretch);
                }

                // Write room data.
                WriteBool(room.EnableViews);

                // Write the amount of room views.
                WriteInt(8);

                // Iterate through views
                for (int j = 0; j < room.Views.Length; j++)
                {
                    // Write room view data.
                    WriteBool(room.Views[j].Visible);
                    WriteInt(room.Views[j].ViewX);
                    WriteInt(room.Views[j].ViewY);
                    WriteInt(room.Views[j].ViewWidth);
                    WriteInt(room.Views[j].ViewHeight);
                    WriteInt(room.Views[j].PortX);
                    WriteInt(room.Views[j].PortY);

                    // Write room view data.
                    WriteInt(room.Views[j].PortWidth);
                    WriteInt(room.Views[j].PortHeight);

                    // Write room view data.
                    WriteInt(room.Views[j].HorizontalBorder);
                    WriteInt(room.Views[j].VerticalBorder);
                    WriteInt(room.Views[j].HorizontalSpeed);
                    WriteInt(room.Views[j].VerticalSpeed);
                    WriteInt(room.Views[j].FollowObject);
                }

                // If there are instances to write.
                if (room.Instances.Count > 0)
                {
                    // Write the amount of instances.
                    WriteInt(room.Instances.Count);

                    // Iterate through room instances.
                    for (int j = 0; j < room.Instances.Count; j++)
                    {
                        // Write room instance data.
                        WriteInt(room.Instances[j].X);
                        WriteInt(room.Instances[j].Y);
                        WriteInt(room.Instances[j].ObjectId);
                        WriteInt(room.Instances[j].Id);

                             WriteString(room.Instances[j].CreationCode);
                            WriteBool(room.Instances[j].Locked);
                    }
                }
                else  // Write no instances.
                    WriteInt(0);

                // If there are tiles to write.
                if (room.Tiles.Count > 0)
                {
                    // Write the amount of room tiles.
                    WriteInt(room.Tiles.Count);

                    // Iterate through room tiles.
                    for (int j = 0; j < room.Tiles.Count; j++)
                    {
                        // Write room tile data.
                        WriteInt(room.Tiles[j].X);
                        WriteInt(room.Tiles[j].Y);
                        WriteInt(room.Tiles[j].BackgroundId);
                        WriteInt(room.Tiles[j].BackgroundX);
                        WriteInt(room.Tiles[j].BackgroundY);
                        WriteInt(room.Tiles[j].Width);
                        WriteInt(room.Tiles[j].Height);
                        WriteInt(room.Tiles[j].Depth);
                        WriteInt(room.Tiles[j].Id);

                        WriteBool(room.Tiles[j].Locked);
                    }
                }
                else  // Write no tiles.
                    WriteInt(0);

                // Write room data.
                WriteBool(room.RememberWindowSize);
                WriteInt(room.EditorWidth);
                WriteInt(room.EditorHeight);
                WriteBool(room.ShowGrid);
                WriteBool(room.ShowObjects);
                WriteBool(room.ShowTiles);
                WriteBool(room.ShowBackgrounds);
                WriteBool(room.ShowForegrounds);
                WriteBool(room.ShowViews);
                WriteBool(room.DeleteUnderlyingObjects);
                WriteBool(room.DeleteUnderlyingTiles);

                // Write room data.
                WriteInt((int)room.CurrentTab);
                WriteInt(room.ScrollBarX);
                WriteInt(room.ScrollBarY);

                EndCompression();
            }
        }

        #endregion

        #region WriteActions

        private void WriteActions(List<Action> actions)
        {
            WriteInt(400);

            WriteInt(actions.Count);

            foreach (Action action in actions)
            {
                WriteInt(440);

                WriteInt(action.LibraryId);
                WriteInt(action.ActionId);
                WriteInt((int)action.ActionKind);
                WriteBool(action.AllowRelative);
                WriteBool(action.Question);
                WriteBool(action.CanApplyTo);
                WriteInt((int)action.ExecutionType);

                if (action.ExecutionType == ExecutionType.Function)
                    WriteString(action.ExecuteCode);
                else
                    WriteInt(0);

                if (action.ExecutionType == ExecutionType.Code)
                    WriteString(action.ExecuteCode);
                else
                    WriteInt(0);

                WriteInt(action.Arguments.Length);

                WriteInt(8);

                for (int j = 0; j < 8; j++)
                {
                    if (j < action.Arguments.Length)
                        WriteInt((int)action.Arguments[j].Type);
                    else
                        WriteInt(0);
                }

                WriteInt(action.AppliesTo);
                WriteBool(action.Relative);

                WriteInt(8);

                for (int j = 0; j < 8; j++)
                {
                    if (j >= action.Arguments.Length)
                    {
                        WriteInt(1);
                        WriteByte(0);
                        continue;
                    }

                    WriteString(action.Arguments[j].Value);
                }

                WriteBool(action.Not);
            }
        }

        #endregion

        #region WriteIncludes

        private void WriteIncludes(List<Include> includes)
        {
            WriteInt(800);

            WriteInt(includes.Count);

            foreach (Include include in includes)
            {
                BeginCompression();

                WriteDouble(include.LastChanged.ToOADate());

                WriteInt(800);

                WriteString(include.Filename);
                WriteString(include.FilePath);
                WriteBool(include.OriginalFile);
                WriteInt(include.OriginalFileSize);
                WriteBool(include.StoreInEditable);

                if (include.StoreInEditable)
                {
                    WriteInt(include.FileData.Length);
                    WriteBytes(include.FileData);
                }

                WriteInt((int)include.ExportMode);
                WriteString(include.ExportFolder);
                WriteBool(include.Overwrite);
                WriteBool(include.FreeMemory);
                WriteBool(include.RemoveAtEnd);

                EndCompression();
            }
        }

        #endregion

        #region WriteExtensions

        private void WriteExtensions(List<Extension> extensions)
        {
            WriteInt(700);

            WriteInt(extensions.Count);

            foreach (Extension extension in extensions)
            {
                WriteString(extension.Name);
            }
        }

        #endregion

        #region WriteGameInformation

        private void WriteGameInformation(GameInformation information)
        {
            WriteInt(800);

            BeginCompression();

            WriteInt(information.BackgroundColor);
            WriteBool(information.MimicGameWindow);

            WriteString(information.FormCaption);
            WriteInt(information.X);
            WriteInt(information.Y);
            WriteInt(information.Width);
            WriteInt(information.Height);
            WriteBool(information.ShowBorder);
            WriteBool(information.AllowResize);
            WriteBool(information.AlwaysOnTop);
            WriteBool(information.PauseGame);

            WriteDouble(information.LastChanged.ToOADate());

            WriteString(information.Information);

            EndCompression();
        }

        #endregion

        #region WriteTree

        private void WriteTree(ProjectNode rootNode)
        {
            WriteInt(700);

            WriteInt(m_project.RoomOrder.Count);

            foreach (int room in m_project.RoomOrder)
                WriteInt(room);

            for (int i = 0; i < 12; i++)
            {
                WriteNodeRecursive(rootNode.Nodes[i]);
            }
        }

        private void WriteNodeRecursive(ProjectNode parent)
        {
            WriteInt((int)parent.NodeType);
            WriteInt((int)parent.ResourceType);
            WriteInt(parent.Id);
            WriteString(parent.Name);
            WriteInt(parent.Children);

            if (parent.Nodes.Count > 0)
            {
                for (int i = 0; i < parent.Children; i++)
                {
                    WriteNodeRecursive(parent.Nodes[i]);
                }
            }
        }

        #endregion

        #region Write Methods

        protected override void WriteByte(byte b)
        {
            if (m_zlibBuffer != null)
                m_zlibBuffer.Add(b);
            else
                base.WriteByte(b);
        }

        #endregion

        #region Compression Methods

        private void BeginCompression()
        {
            if (m_zlibBuffer != null)
                throw new System.InvalidOperationException("Compression already in progress.");

            m_zlibBuffer = new List<byte>();
        }

        public void EndCompression()
        {
            if (m_zlibBuffer == null)
                throw new System.InvalidOperationException("EndCompression cannot be called before BeginCompression.");

            // Input buffer.
            byte[] input = m_zlibBuffer.ToArray();

            // Dump buffer
            m_zlibBuffer.Clear();
            m_zlibBuffer = null;


            using (MemoryStream ms = new MemoryStream())
            {
                Deflater compress = new Deflater();
                compress.SetInput(input);
                compress.Finish(); // Why do I need to call this?

                // the result buffer
                byte[] result = new byte[input.Length];

                // Loop it just incase (shouldn't happen BUT, zlib's been known to cause a size increase)
                while (!compress.IsFinished)
                {
                    int length = compress.Deflate(result);
                    ms.Write(result, 0, length);
                }

                WriteInt(ms.ToArray().Length);
                WriteBytes(ms.ToArray());
            }
        }

        #endregion
    }
}
