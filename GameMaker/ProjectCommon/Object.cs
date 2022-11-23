// Object.cs
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
    public class Object : Resource
    {
        #region Members

        #region Fields

        private List<Event>[] m_events = new List<Event>[12];
        private DateTime m_lastChanged = DateTime.Now;
        private int m_spriteId = -1;
        private int m_mask = -1;
        private int m_parent = -1;
        private int m_depth;
        private bool m_solid;
        private bool m_visible;
        private bool m_persistent;

        #endregion

        #region Properties
        public List<Event>[] Events
        {
            get { return m_events; }
            set { m_events = value; }
        }

        public DateTime LastChanged
        {
            get { return m_lastChanged; }
            set { m_lastChanged = value; }
        }

        public int SpriteId
        {
            get { return m_spriteId; }
            set { m_spriteId = value; }
        }

        public int Mask
        {
            get { return m_mask; }
            set { m_mask = value; }
        }

        public int Parent
        {
            get { return m_parent; }
            set { m_parent = value; }
        }

        public int Depth
        {
            get { return m_depth; }
            set { m_depth = value; }
        }

        public bool Solid
        {
            get { return m_solid; }
            set { m_solid = value; }
        }

        public bool Visible
        {
            get { return m_visible; }
            set { m_visible = value; }
        }

        public bool Persistent
        {
            get { return m_persistent; }
            set { m_persistent = value; }
        }

        #endregion

        #endregion

        /// <summary>
        /// Constructs a new Game Maker object.
        /// </summary>
        public Object()
        {
            // Iterate through events.
            for (int i = 0; i < m_events.Length; i++)
                m_events[i] = new List<Event>();
        }
    }
}
