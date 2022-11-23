// Room.cs
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

namespace GameMaker.ProjectCommon
{
    public class Room : Resource
    {


        private ResourceCollection<Instance> m_instances = new ResourceCollection<Instance>();
        private ResourceCollection<Tile> m_tiles = new ResourceCollection<Tile>();
        private TabSetting m_currentTab = TabSetting.Objects;
        private Parallax[] m_parallaxs = new Parallax[8];
        private View[] m_views = new View[8];
        private DateTime m_lastChanged = DateTime.Now;
        private string m_caption = string.Empty;
        private string m_creationCode = string.Empty;
        private int m_width = 640;
        private int m_height = 480;
        private int m_speed = 30;
        private int m_editorWidth = 200;
        private int m_editorHeight = 200;
        private int m_snapX = 16;
        private int m_snapY = 16;
        private int m_tileWidth = 16;
        private int m_tileHeight = 16;
        private int m_tileHorizontalSeparation = 1;
        private int m_tileVerticalSeparation = 1;
        private int m_tileHorizontalOffset;
        private int m_tileVerticalOffset;
        private int m_backgroundColor;
        private int m_scrollBarX;
        private int m_scrollBarY;
        private bool m_persistent;
        private bool m_drawBackgroundColor = true;
        private bool m_enableViews;
        private bool m_rememberWindowSize = true;
        private bool m_showGrid = true;
        private bool m_isometricGrid;
        private bool m_showObjects = true;
        private bool m_showTiles = true;
        private bool m_showBackgrounds = true;
        private bool m_showForegrounds = true;
        private bool m_showViews;
        private bool m_deleteUnderlyingObjects = true;
        private bool m_deleteUnderlyingTiles = true;



        public ResourceCollection<Instance> Instances
        {
            get { return m_instances; }
            set { m_instances = value; }
        }

        public ResourceCollection<Tile> Tiles
        {
            get { return m_tiles; }
            set { m_tiles = value; }
        }

        public TabSetting CurrentTab
        {
            get { return m_currentTab; }
            set { m_currentTab = value; }
        }

        public Parallax[] Parallaxs
        {
            get { return m_parallaxs; }
            set { m_parallaxs = value; }
        }

        public View[] Views
        {
            get { return m_views; }
            set { m_views = value; }
        }

        public DateTime LastChanged
        {
            get { return m_lastChanged; }
            set { m_lastChanged = value; }
        }

        public string Caption
        {
            get { return m_caption; }
            set { m_caption = value; }
        }

        public string CreationCode
        {
            get { return m_creationCode; }
            set { m_creationCode = value; }
        }

        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        public int Speed
        {
            get { return m_speed; }
            set { m_speed = value; }
        }

        public int EditorWidth
        {
            get { return m_editorWidth; }
            set { m_editorWidth = value; }
        }

        public int EditorHeight
        {
            get { return m_editorHeight; }
            set { m_editorHeight = value; }
        }

        public int SnapX
        {
            get { return m_snapX; }
            set { m_snapX = value; }
        }

        public int SnapY
        {
            get { return m_snapY; }
            set { m_snapY = value; }
        }

        public int TileWidth
        {
            get { return m_tileWidth; }
            set { m_tileWidth = value; }
        }

        public int TileHeight
        {
            get { return m_tileHeight; }
            set { m_tileHeight = value; }
        }

        public int TileHorizontalSeparation
        {
            get { return m_tileHorizontalSeparation; }
            set { m_tileHorizontalSeparation = value; }
        }

        public int TileVerticalSeparation
        {
            get { return m_tileVerticalSeparation; }
            set { m_tileVerticalSeparation = value; }
        }

        public int TileHorizontalOffset
        {
            get { return m_tileHorizontalOffset; }
            set { m_tileHorizontalOffset = value; }
        }

        public int TileVerticalOffset
        {
            get { return m_tileVerticalOffset; }
            set { m_tileVerticalOffset = value; }
        }

        public int BackgroundColor
        {
            get { return m_backgroundColor; }
            set { m_backgroundColor = value; }
        }

        public int ScrollBarX
        {
            get { return m_scrollBarX; }
            set { m_scrollBarX = value; }
        }

        public int ScrollBarY
        {
            get { return m_scrollBarY; }
            set { m_scrollBarY = value; }
        }

        public bool Persistent
        {
            get { return m_persistent; }
            set { m_persistent = value; }
        }

        public bool DrawBackgroundColor
        {
            get { return m_drawBackgroundColor; }
            set { m_drawBackgroundColor = value; }
        }

        public bool EnableViews
        {
            get { return m_enableViews; }
            set { m_enableViews = value; }
        }

        public bool RememberWindowSize
        {
            get { return m_rememberWindowSize; }
            set { m_rememberWindowSize = value; }
        }

        public bool ShowGrid
        {
            get { return m_showGrid; }
            set { m_showGrid = value; }
        }

        public bool IsometricGrid
        {
            get { return m_isometricGrid; }
            set { m_isometricGrid = value; }
        }

        public bool ShowObjects
        {
            get { return m_showObjects; }
            set { m_showObjects = value; }
        }

        public bool ShowTiles
        {
            get { return m_showTiles; }
            set { m_showTiles = value; }
        }

        public bool ShowBackgrounds
        {
            get { return m_showBackgrounds; }
            set { m_showBackgrounds = value; }
        }

        public bool ShowForegrounds
        {
            get { return m_showForegrounds; }
            set { m_showForegrounds = value; }
        }

        public bool ShowViews
        {
            get { return m_showViews; }
            set { m_showViews = value; }
        }

        public bool DeleteUnderlyingObjects
        {
            get { return m_deleteUnderlyingObjects; }
            set { m_deleteUnderlyingObjects = value; }
        }

        public bool DeleteUnderlyingTiles
        {
            get { return m_deleteUnderlyingTiles; }
            set { m_deleteUnderlyingTiles = value; }
        }




        /// <summary>
        /// Construct a new Game Maker room.
        /// </summary>
        public Room()
        {
            // Create a new array of parallaxs.
            for (int i = 0; i < m_parallaxs.Length; i++)
                m_parallaxs[i] = new Parallax();

            // Create a new array of views.
            for (int i = 0; i < m_views.Length; i++)
                m_views[i] = new View();
        }

    }

    public class Parallax
    {


        private int m_backgroundId = -1;
        private int m_x;
        private int m_y;
        private int m_horizontalSpeed;
        private int m_verticalSpeed;
        private bool m_visible;
        private bool m_foreground;
        private bool m_tileHorizontally = true;
        private bool m_tileVertically = true;
        private bool m_stretch;



        public int BackgroundId
        {
            get { return m_backgroundId; }
            set { m_backgroundId = value; }
        }

        public int X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        public int Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        public int HorizontalSpeed
        {
            get { return m_horizontalSpeed; }
            set { m_horizontalSpeed = value; }
        }

        public int VerticalSpeed
        {
            get { return m_verticalSpeed; }
            set { m_verticalSpeed = value; }
        }

        
        public bool Visible
        {
            get { return m_visible; }
            set { m_visible = value; }
        }

        public bool Foreground
        {
            get { return m_foreground; }
            set { m_foreground = value; }
        }

        public bool TileHorizontally
        {
            get { return m_tileHorizontally; }
            set { m_tileHorizontally = value; }
        }

        public bool TileVertically
        {
            get { return m_tileVertically; }
            set { m_tileVertically = value; }
        }

        public bool Stretch
        {
            get { return m_stretch; }
            set { m_stretch = value; }
        }


    }

    public class View
    {

        private int m_viewX;
        private int m_viewY;
        private int m_viewWidth = 640;
        private int m_viewHeight = 480;
        private int m_portX;
        private int m_portY;
        private int m_portWidth = 640;
        private int m_portHeight = 480;
        private int m_horizontalBorder = 32;
        private int m_verticalBorder = 32;
        private int m_horizontalSpeed = -1;
        private int m_verticalSpeed = -1;
        private int m_followObject = -1;
        private bool m_visible;



        public int ViewX
        {
            get { return m_viewX; }
            set { m_viewX = value; }
        }

        public int ViewY
        {
            get { return m_viewY; }
            set { m_viewY = value; }
        }

        public int ViewWidth
        {
            get { return m_viewWidth; }
            set { m_viewWidth = value; }
        }

        public int ViewHeight
        {
            get { return m_viewHeight; }
            set { m_viewHeight = value; }
        }

        public int PortX
        {
            get { return m_portX; }
            set { m_portX = value; }
        }

        public int PortY
        {
            get { return m_portY; }
            set { m_portY = value; }
        }

        public int PortWidth
        {
            get { return m_portWidth; }
            set { m_portWidth = value; }
        }

        public int PortHeight
        {
            get { return m_portHeight; }
            set { m_portHeight = value; }
        }

        public int HorizontalBorder
        {
            get { return m_horizontalBorder; }
            set { m_horizontalBorder = value; }
        }

        public int VerticalBorder
        {
            get { return m_verticalBorder; }
            set { m_verticalBorder = value; }
        }

        public int HorizontalSpeed
        {
            get { return m_horizontalSpeed; }
            set { m_horizontalSpeed = value; }
        }

        public int VerticalSpeed
        {
            get { return m_verticalSpeed; }
            set { m_verticalSpeed = value; }
        }

        public int FollowObject
        {
            get { return m_followObject; }
            set { m_followObject = value; }
        }

        public bool Visible
        {
            get { return m_visible; }
            set { m_visible = value; }
        }


    }

    public class Instance : Resource
    {


        private string m_creationCode = string.Empty;
        private int m_depth;
        private int m_x;
        private int m_y;
        private int m_objectId = -1;
        private bool m_locked;



        public string CreationCode
        {
            get { return m_creationCode; }
            set { m_creationCode = value; }
        }

        public int Depth
        {
            get { return m_depth; }
            set { m_depth = value; }
        }

        public int X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        public int Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        public int ObjectId
        {
            get { return m_objectId; }
            set { m_objectId = value; }
        }

        public bool Locked
        {
            get { return m_locked; }
            set { m_locked = value; }
        }


    }

    public class Tile : Resource
    {


        private int m_x;
        private int m_y;
        private int m_width = 16;
        private int m_height = 16;
        private int m_backgroundId = -1;
        private int m_backgroundX;
        private int m_backgroundY;
        private int m_depth;
        private bool m_locked;



        public int X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        public int Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        public int Width
        {
            get { return m_width; }
            set { m_width = value; }
        }

        public int Height
        {
            get { return m_height; }
            set { m_height = value; }
        }

        public int BackgroundId
        {
            get { return m_backgroundId; }
            set { m_backgroundId = value; }
        }

        public int BackgroundX
        {
            get { return m_backgroundX; }
            set { m_backgroundX = value; }
        }

        public int BackgroundY
        {
            get { return m_backgroundY; }
            set { m_backgroundY = value; }
        }

        public int Depth
        {
            get { return m_depth; }
            set { m_depth = value; }
        }

        public bool Locked
        {
            get { return m_locked; }
            set { m_locked = value; }
        }


    }
}
