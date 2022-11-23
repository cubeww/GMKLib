// Event.cs
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

namespace GameMaker.ProjectCommon
{
    public class Event
    {


        private EventType m_mainType = EventType.None;
        private List<Action> m_actions = null;
        private int m_subtype;
        private int m_otherId;



        public EventType MainType
        {
            get { return m_mainType; }
            set { m_mainType = value; }
        }

        public List<Action> Actions
        {
            get { return m_actions; }
            set { m_actions = value; }
        }

        public int Subtype
        {
            get { return m_subtype; }
            set { m_subtype = value; }
        }

        public int OtherId
        {
            get { return m_otherId; }
            set { m_otherId = value; }
        }


    }
}
