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

namespace GameMaker.ExtensionProject
{
    public class Project
    {

        
        private bool m_editable;
        private string m_name = "NewLibrary";
        private string m_tempDirectory = "temp";
        private string m_version = "1.0";
        private string m_author = "temp";
        private string m_dateModified = DateTime.Now.ToString();
        private string m_license = "Blah";
        private string m_information = "Test";
        private string m_help = string.Empty; // If this is not empty then we have a help file so include one.
        private bool m_hidden;
        private List<string> m_uses = new List<string>(); // Extension packages required by this extension
        private List<IncludeFile> m_files = new List<IncludeFile>();
        


        public bool Editable
        {
            get { return m_editable; }
            set { m_editable = value; }
        }

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string TempDirectory
        {
            get { return m_tempDirectory; }
            set { m_tempDirectory = value; }
        }

        public string Version
        {
            get { return m_version; }
            set { m_version = value; }
        }

        public string Author
        {
            get { return m_author; }
            set { m_author = value; }
        }

        public string DateModified
        {
            get { return m_dateModified; }
            set { m_dateModified = value; }
        }

        public string License
        {
            get { return m_license; }
            set { m_license = value; }
        }

        public string Information
        {
            get { return m_information; }
            set { m_information = value; }
        }

        public string HelpFile
        {
            get { return m_help; }
            set { m_help = value; }
        }

        public bool Hidden
        {
            get { return m_hidden; }
            set { m_hidden = value; }
        }

        public List<string> Uses
        {
            get { return m_uses; }
            set { m_uses = value; }
        }

        public List<IncludeFile> Files
        {
            get { return m_files; }
        }



    }
}
