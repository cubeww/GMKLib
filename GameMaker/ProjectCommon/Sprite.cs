// Sprite.cs
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
    public class Sprite : Resource
    {
        #region Members

        #region Fields

        private List<Image> m_subImages = new List<Image>();
        private BoundingBox m_boundingBoxMode = BoundingBox.Auto;
        private Shape m_shapeMode = Shape.Rectangle;
        private DateTime m_lastChanged = DateTime.Now;
        private int m_width = 32;
        private int m_height = 32;
        private int m_originX = 0;
        private int m_originY = 0;
        private int m_boundingBoxLeft = 0;
        private int m_boundingBoxRight = 0;
        private int m_boundingBoxTop = 0;
        private int m_boundingBoxBottom = 0;
        private int m_alphaTolerance = 0;
        private bool m_transparent = true;
        private bool m_precise = true;
        private bool m_smoothEdges = false;
        private bool m_preload = true;
        private bool m_useVideoMemory = true;
        private bool m_loadOnlyOnUse = false;
        private bool m_useSeparateCollisionMasks = false;
        
        #endregion

        #region Properties

        public List<Image> SubImages
        {
            get { return m_subImages; }
            set { m_subImages = value; }
        }

        public BoundingBox BoundingBoxMode
        {
            get { return m_boundingBoxMode; }
            set { m_boundingBoxMode = value; }
        }

        public Shape ShapeMode
        {
            get { return m_shapeMode; }
            set { m_shapeMode = value; }
        }

        public DateTime LastChanged
        {
            get { return m_lastChanged; }
            set { m_lastChanged = value; }
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

        public int OriginX
        {
            get { return m_originX; }
            set { m_originX = value; }
        }

        public int OriginY
        {
            get { return m_originY; }
            set { m_originY = value; }
        }

        public int BoundingBoxLeft
        {
            get { return m_boundingBoxLeft; }
            set { m_boundingBoxLeft = value; }
        }

        public int BoundingBoxRight
        {
            get { return m_boundingBoxRight; }
            set { m_boundingBoxRight = value; }
        }

        public int BoundingBoxTop
        {
            get { return m_boundingBoxTop; }
            set { m_boundingBoxTop = value; }
        }

        public int BoundingBoxBottom
        {
            get { return m_boundingBoxBottom; }
            set { m_boundingBoxBottom = value; }
        }

        public int AlphaTolerance
        {
            get { return m_alphaTolerance; }
            set { m_alphaTolerance = value; }
        }

        public bool Transparent
        {
            get { return m_transparent; }
            set { m_transparent = value; }
        }

        public bool Precise
        {
            get { return m_precise; }
            set { m_precise = value; }
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

        public bool UseSeparateCollisionMasks
        {
            get { return m_useSeparateCollisionMasks; }
            set { m_useSeparateCollisionMasks = value; }
        }

        #endregion

        #endregion
    }
}
