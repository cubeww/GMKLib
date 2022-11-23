// Path.cs
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
    public class Path : Resource
    {
        #region Members

        #region Fields

        private List<Point> m_points;
        private ActionEnd m_actionEnd = ActionEnd.MoveToStart;
        private DateTime m_lastChanged = DateTime.Now;
        private int m_precision = 4;
        private int m_roomId = -1;
        private int m_snapX = 16;
        private int m_snapY = 16;
        private bool m_smooth;
        private bool m_closed = true;

        #endregion

        #region Properties

        public List<Point> Points
        {
            get { return m_points; }
            set { m_points = value; }
        }

        public ActionEnd ActionEnd
        {
            get { return m_actionEnd; }
            set { m_actionEnd = value; }
        }

        public DateTime LastChanged
        {
            get { return m_lastChanged; }
            set { m_lastChanged = value; }
        }

        public int Precision
        {
            get { return m_precision; }
            set { m_precision = value; }
        }

        public int RoomId
        {
            get {return m_roomId;}
            set {m_roomId = value;}
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

        public bool Smooth
        {
            get { return m_smooth; }
            set { m_smooth = value; }
        }

        public bool Closed
        {
            get { return m_closed; }
            set { m_closed = value; }
        }

        #endregion

        #endregion
    }

    public class Point
    {
        #region Members

        #region Fields

        private double m_x;
        private double m_y;
        private double m_speed;

        #endregion

        #region Properties

        public double X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        public double Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        public double Speed
        {
            get { return m_speed; }
            set { m_speed = value; }
        }

        #endregion

        #endregion
    }
}
