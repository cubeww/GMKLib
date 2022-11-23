// GameInformation.cs
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
    public class GameInformation
    {
        #region Fields

        private string m_information = @"{\rtf1\ansi\ansicpg1252\deff0\deflang1033{\fonttbl{\f0\fnil Arial;}}{\colortbl ;\red0\green0\blue0;}\viewkind4\uc1\pard\cf1\f0\fs24}";
        private string m_formCaption = "Game Information";
        private DateTime m_lastChanged;
        private int m_x = -1;
        private int m_y = -1;
        private int m_width = 600;
        private int m_height = 400;
        private int m_backgroundColor = -16777192;
        private bool m_mimicGameWindow;
        private bool m_showBorder = true;
        private bool m_allowResize = true;
        private bool m_alwaysOnTop;
        private bool m_pauseGame = true;

        #endregion

        #region Properties

        public string Information
        {
            get { return m_information; }
            set { m_information = value; }
        }

        public string FormCaption
        {
            get { return m_formCaption; }
            set { m_formCaption = value; }
        }

        public DateTime LastChanged
        {
            get { return m_lastChanged; }
            set { m_lastChanged = value; }
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

        public int BackgroundColor
        {
            get { return m_backgroundColor; }
            set { m_backgroundColor = value; }
        }

        public bool MimicGameWindow
        {
            get { return m_mimicGameWindow; }
            set { m_mimicGameWindow = value; }
        }

        public bool ShowBorder
        {
            get { return m_showBorder; }
            set { m_showBorder = value; }
        }

        public bool AllowResize
        {
            get { return m_allowResize; }
            set { m_allowResize = value; }
        }

        public bool AlwaysOnTop
        {
            get { return m_alwaysOnTop; }
            set { m_alwaysOnTop = value; }
        }

        public bool PauseGame
        {
            get { return m_pauseGame; }
            set { m_pauseGame = value; }
        }

        #endregion
    }
}
