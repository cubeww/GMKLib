// Project.cs
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

using System;
using System.Collections.Generic;

namespace GameMaker.ProjectCommon
{
    public class Project
    {
        #region Members

        #region Constants

        public const int InstanceIdMin = 100000;
        public const int TileIdMin = 10000000;

        #endregion

        #region Fields

        private int m_lastInstanceId = InstanceIdMin;
        private int m_lastTileId = TileIdMin;

        private FileVersion m_fileVersion = FileVersion.GameMaker50;
        private ProjectNode m_projectTree;
        private GlobalSettings m_globalSettings = null;
        private List<Trigger> m_triggers = new List<Trigger>();
        private ResourceCollection<Sprite> m_sprites = new ResourceCollection<Sprite>();
        private ResourceCollection<Sound> m_sounds = new ResourceCollection<Sound>();
        private ResourceCollection<Background> m_backgrounds = new ResourceCollection<Background>();
        private ResourceCollection<Path> m_paths = new ResourceCollection<Path>();
        private ResourceCollection<Script> m_scripts = new ResourceCollection<Script>();
        private ResourceCollection<Font> m_fonts = new ResourceCollection<Font>();
        private ResourceCollection<Timeline> m_timelines = new ResourceCollection<Timeline>();
        private ResourceCollection<Object> m_objects = new ResourceCollection<Object>();
        private ResourceCollection<Room> m_rooms = new ResourceCollection<Room>();
        private List<Include> m_includes = new List<Include>();
        private List<Extension> m_extensions = new List<Extension>();
        private GameInformation m_gameInformation;
        private List<Library> m_libraries = new List<Library>();
        private List<int> m_roomOrder = new List<int>();

        #endregion

        #region Properties

        public int LastInstanceId
        {
            get { return m_lastInstanceId; }
            set { m_lastInstanceId = value; }
        }

        public int LastTileId
        {
            get { return m_lastTileId; }
            set { m_lastTileId = value; }
        }

        public FileVersion FileVersion
        {
            get { return m_fileVersion; }
            set { m_fileVersion = value; }
        }

        public ProjectNode ProjectTree
        {
            get { return m_projectTree; }
            set { m_projectTree = value; }
        }

        public GlobalSettings Settings
        {
            get { return m_globalSettings; }
            set { m_globalSettings = value; }
        }

        public List<Trigger> Triggers
        {
            get { return m_triggers; }
            set { m_triggers = value; }
        }

        public ResourceCollection<Sprite> Sprites
        {
            get { return m_sprites; }
            set { m_sprites = value; }
        }

        public ResourceCollection<Sound> Sounds
        {
            get { return m_sounds; }
            set { m_sounds = value; }
        }

        public ResourceCollection<Background> Backgrounds
        {
            get { return m_backgrounds; }
            set { m_backgrounds = value; }
        }

        public ResourceCollection<Path> Paths
        {
            get { return m_paths; }
            set { m_paths = value; }
        }

        public ResourceCollection<Script> Scripts
        {
            get { return m_scripts; }
            set { m_scripts = value; }
        }

        public ResourceCollection<Font> Fonts
        {
            get { return m_fonts; }
            set { m_fonts = value; }
        }

        public ResourceCollection<Timeline> Timelines
        {
            get { return m_timelines; }
            set { m_timelines = value; }
        }

        public ResourceCollection<Object> Objects
        {
            get { return m_objects; }
            set { m_objects = value; }
        }

        public ResourceCollection<Room> Rooms
        {
            get { return m_rooms; }
            set { m_rooms = value; }
        }

        public List<Include> Includes
        {
            get { return m_includes; }
            set { m_includes = value; }
        }

        public List<Extension> Extensions
        {
            get { return m_extensions; }
            set { m_extensions = value; }
        }

        public GameInformation GameInformation
        {
            get { return m_gameInformation; }
            set { m_gameInformation = value; }
        }

        public List<Library> Libraries
        {
            get { return m_libraries; }
            set { m_libraries = value; }
        }

        public List<int> RoomOrder
        {
            get { return m_roomOrder; }
            set { m_roomOrder = value; }
        }

        #endregion

        #endregion

        #region Methods

        #region RefactorIds

        public void RefactorIds()
        {

            RefactorInstanceIds();
            RefactorTileIds();

            m_sprites.ResetIds();
            m_sounds.ResetIds();
            m_backgrounds.ResetIds();
            m_paths.ResetIds();
            m_scripts.ResetIds();
            m_timelines.ResetIds();
            m_objects.ResetIds();
            m_rooms.ResetIds();
            for (int i = 0; i < 9; i++)
            {
                RefactorIdsRecursive(m_projectTree.Nodes[i]);
            }
        }

        private void RefactorIdsRecursive(ProjectNode parent)
        {
            switch (parent.ResourceType)
            {
                case ResourceType.Sprites:
                    foreach (ProjectNode node in parent.Nodes)
                    {
                        if (node.NodeType == ProjectNodeType.Group)
                        {
                            RefactorIdsRecursive(node);
                            continue;
                        }
                        m_sprites.LastId++;

                        Sprite sprite = m_sprites.Find(delegate(Sprite s) { return s.Name == node.Name; });
                        if (sprite != null)
                        {
                            foreach (Object obj in m_objects)
                            {
                                if (obj.SpriteId == node.Id)
                                    obj.SpriteId = m_sprites.LastId;
                                if (obj.Mask == node.Id)
                                    obj.Mask = m_sprites.LastId;
                            }

                            sprite.Id = m_sprites.LastId;
                            node.Id = m_sprites.LastId;
                        }
                    }
                    break;
                case ResourceType.Sounds:
                    foreach (ProjectNode node in parent.Nodes)
                    {
                        if (node.NodeType == ProjectNodeType.Group)
                        {
                            RefactorIdsRecursive(node);
                            continue;
                        }
                        m_sounds.LastId++;

                        Sound sound = m_sounds.Find(delegate(Sound s) { return s.Name == node.Name; });

                        if (sound != null)
                        {
                            sound.Id = m_sounds.LastId;
                            node.Id = m_sounds.LastId;
                        }
                    }
                    break;
                case ResourceType.Backgrounds:
                    foreach (ProjectNode node in parent.Nodes)
                    {
                        if (node.NodeType == ProjectNodeType.Parent)
                        {
                            RefactorIdsRecursive(node);
                            continue;
                        }
                        m_backgrounds.LastId++;

                        Background background = m_backgrounds.Find(delegate(Background b) { return b.Name== node.Name; });

                        if (background != null)
                        {
                            foreach (Room room in m_rooms)
                            {
                                foreach (Parallax parallax in room.Parallaxs)
                                    if (parallax.BackgroundId == node.Id)
                                        parallax.BackgroundId = m_backgrounds.LastId;
                            }

                            background.Id = m_backgrounds.LastId;
                            node.Id = m_backgrounds.LastId;
                        }
                    }
                    break;
                case ResourceType.Paths:
                    foreach (ProjectNode node in parent.Nodes)
                    {
                        if (node.NodeType == ProjectNodeType.Group)
                        {
                            RefactorIdsRecursive(node);
                            continue;
                        }

                        m_paths.LastId++;

                        Path path = m_paths.Find(delegate(Path p) { return p.Name == node.Name; });

                        if (path != null)
                        {
                            path.Id = m_paths.LastId;
                            node.Id = m_paths.LastId;
                        }
                    }
                    break;
                case ResourceType.Scripts:
                    foreach (ProjectNode node in parent.Nodes)
                    {
                        if (node.NodeType == ProjectNodeType.Group)
                        {
                            RefactorIdsRecursive(node);
                            continue;
                        }

                        m_scripts.LastId++;

                        Script script = m_scripts.Find(delegate(Script s) { return s.Name == node.Name; });

                        if (script != null)
                        {
                            script.Id = m_scripts.LastId;
                            node.Id = m_scripts.LastId;
                        }
                    }
                    break;
                case ResourceType.Timelines:
                    foreach (ProjectNode node in parent.Nodes)
                    {
                        if (node.NodeType == ProjectNodeType.Group)
                        {
                            RefactorIdsRecursive(node);
                            continue;
                        }

                        m_timelines.LastId++;

                        Timeline timeline = m_timelines.Find(delegate(Timeline t) { return t.Name == node.Name; });

                        if (timeline != null)
                        {
                            timeline.Id = m_timelines.LastId;
                            node.Id = m_timelines.LastId;
                        }
                    }
                    break;
                case ResourceType.Objects:
                    foreach (ProjectNode node in parent.Nodes)
                    {
                        if (node.NodeType == ProjectNodeType.Group)
                        {
                            RefactorIdsRecursive(node);
                            continue;
                        }

                        m_objects.LastId++;

                        Object obj = m_objects.Find(delegate(Object o) { return o.Name == node.Name; });

                        if (obj != null)
                        {
                            foreach (Room room in m_rooms)
                            {
                                foreach (Instance instance in room.Instances)
                                    if (instance.ObjectId == node.Id)
                                        instance.ObjectId = m_objects.LastId;
                            }

                            obj.Id = m_objects.LastId;
                            node.Id = m_objects.LastId;
                        }
                    }
                    break;
                case ResourceType.Rooms:
                    foreach (ProjectNode node in parent.Nodes)
                    {
                        if (node.NodeType == ProjectNodeType.Group)
                        {
                            RefactorIdsRecursive(node);
                            continue;
                        }

                        m_rooms.LastId++;

                        Room room = m_rooms.Find(delegate(Room r) { return r.Name == node.Name; });
                        if (room != null)
                        {
                            room.Id = m_rooms.LastId;
                            node.Id = m_rooms.LastId;
                        }
                    }
                    break;
            }
        }

        #endregion

        public void RefactorInstanceIds()
        {
            // Reset the instance id.
            m_lastInstanceId = InstanceIdMin;

            foreach (Room room in m_rooms)
            {
                foreach (Instance instance in room.Instances)
                {
                    instance.Id = m_lastInstanceId;

                    m_lastInstanceId++;
                }
            }
        }

        public void RefactorTileIds()
        {
            // Reset the tile id.
            m_lastTileId = TileIdMin;

            foreach (Room room in m_rooms)
            {
                foreach (Tile tile in room.Tiles)
                {
                    tile.Id = m_lastTileId;

                    m_lastTileId++;
                }
            }
        }


        public int GenerateProjectId()
        {
            Random rand = new Random();

            return rand.Next() % 100000000;
        }

        #endregion
    }
}
