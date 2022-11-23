// Background.cs
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
    public class Background : Resource
    {
        #region Members

        #region Fields

        private DateTime m_lastChanged = DateTime.Now;
        private Image m_image = null;
        private int m_width;
        private int m_height;
        private int m_tileWidth = 16;
        private int m_tileHeight = 16;
        private int m_horizontalOffset;
        private int m_verticalOffset;
        private int m_horizontalSeparation;
        private int m_verticalSeparation;
        private bool m_transparent;
        private bool m_smoothEdges;
        private bool m_preload;
        private bool m_useAsTileSet;
        private bool m_useVideoMemory = true;
        private bool m_loadOnlyOnUse = true;

        #endregion

        #region Properties

        public DateTime LastChanged
        {
            get { return m_lastChanged; }
            set { m_lastChanged = value; }
        }

        public Image Image
        {
            get { return m_image; }
            set { m_image = value; }
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

        public int TileWidth
        {
            get { return m_tileWidth; }
            set { m_tileWidth = value; }
        }
        public int TileHeight
        {
            get { return m_tileHeight; }
            set { m_tileWidth = value; }
        }
        public int HorizontalOffset
        {
            get { return m_horizontalOffset; }
            set { m_horizontalOffset = value; }
        }
        public int VerticalOffset
        {
            get { return m_verticalOffset; }
            set { m_verticalOffset = value; }
        }

        public int HorizontalSeparation
        {
            get { return m_horizontalSeparation; }
            set { m_horizontalSeparation = value; }
        }

        public int VerticalSeparation
        {
            get { return m_verticalSeparation; }
            set { m_verticalSeparation = value; }
        }

        public bool Transparent
        {
            get { return m_transparent; }
            set { m_transparent = value; }
        }

        public bool SmoothEdges
        {
            get { return m_smoothEdges; }
            set { m_smoothEdges = value; }
        }

        public bool Preload
        {
            get { return m_preload; }
            set { m_preload = value; }
        }

        public bool UseAsTileSet
        {
            get { return m_useAsTileSet; }
            set { m_useAsTileSet = value; }
        }

        public bool UseVideoMemory
        {
            get { return m_useVideoMemory; }
            set { m_useVideoMemory = value; }
        }

        public bool LoadOnlyOnUse
        {
            get { return m_loadOnlyOnUse; }
            set { m_loadOnlyOnUse = value; }
        }

        #endregion

        #endregion
    }
}
