﻿// Font.cs
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
    public class Font : Resource
    {


        private string m_fontName = "Arial";
        private DateTime m_lastChanged = DateTime.Now;
        private int m_characterRangeMin = 32;
        private int m_characterRangeMax = 127;
        private int m_size = 12;
        private bool m_bold;
        private bool m_italic;



        public string FontName
        {
            get { return m_fontName; }
            set { m_fontName = value; }
        }

        public DateTime LastChanged
        {
            get { return m_lastChanged; }
            set { m_lastChanged = value; }
        }

        public int CharacterRangeMin
        {
            get { return m_characterRangeMin; }
            set { m_characterRangeMin = value; }
        }

        public int CharacterRangeMax
        {
            get { return m_characterRangeMax; }
            set { m_characterRangeMax = value; }
        }

        public int Size
        {
            get { return m_size; }
            set { m_size = value; }
        }

        public bool Bold
        {
            get { return m_bold; }
            set { m_bold = value; }
        }

        public bool Italic
        {
            get { return m_italic; }
            set { m_italic = value; }
        }


    }
}