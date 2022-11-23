// Library.cs
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

namespace GameMaker.ActionLibrary
{

    /// <summary>
    /// Defines basic information for libraries in GameMaker.
    /// </summary>
    public class Library
    {

        private FileVersion m_fileVersion;
        private string m_tabCaption;
        private int m_nId = (1000 + (new Random().Next() & 9000));
        private string m_author;
        private int m_version;
        private DateTime m_lastModified = DateTime.Now;
        private string m_info;
        private string m_initializationCode;
        private bool m_advanced;
        private List<Action> m_actions = new List<Action>();


        /// <summary>
        /// Gets or sets the <see cref="FileVersion"/>.
        /// </summary>
        public FileVersion FileVersion
        {
            get { return m_fileVersion; }
            set
            {
                m_fileVersion = value;

            }
        }

        /// <summary>
        /// Gets or sets the TabCaption.
        /// </summary>
        public string TabCaption
        {
            get { return m_tabCaption; }
            set
            {
                m_tabCaption = value;

            }
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public int Id
        {
            get { return m_nId; }
            set
            {
                m_nId = value;

            }
        }

        /// <summary>
        /// Gets or sets the Author
        /// </summary>
        public string Author
        {
            get { return m_author; }
            set
            {
                m_author = value;
            }
        }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public int Version
        {
            get { return m_version; }
            set
            {
                m_version = value;
            }
        }

        /// <summary>
        /// Gets or sets when the file was last modified.
        /// </summary>
        public DateTime LastModified
        {
            get { return m_lastModified; }
            set
            {
                m_lastModified = value;
            }
        }

        /// <summary>
        /// Gets or sets the Information.
        /// </summary>
        public string Info
        {
            get { return m_info; }
            set { m_info = value; }
        }

        /// <summary>
        /// Gets or sets the Initialization code.
        /// </summary>
        public string InitializationCode
        {
            get { return m_initializationCode; }
            set
            {
                m_initializationCode = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the Library only appears in Advanced Mode.
        /// </summary>
        public bool Advanced
        {
            get { return m_advanced; }
            set
            {
                m_advanced = value;
            }
        }

        /// <summary>
        /// Gets the current list of Actions.
        /// </summary>
        public List<Action> Actions
        {
            get { return m_actions; }
            internal set { m_actions = value; }
        }


        public Library()
        {
        }

        public void AddAction(Action a)
        {
            Actions.Add(a);
        }

        public void RemoveAction(int index)
        {
            Actions.RemoveAt(index);
        }

        public void RemoveAction(Action a)
        {
            Actions.Remove(a);
        }

        public void InsertAction(int index, Action a)
        {
            Actions.Insert(index, a);
        }

    }
}
