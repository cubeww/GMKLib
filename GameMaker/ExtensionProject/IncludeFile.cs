// IncludeFile.cs
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
using GameMaker.ProjectCommon;

namespace GameMaker.ExtensionProject
{
    public class IncludeFile
    {
        #region Members

        #region Fields

        private string m_name;
        private string m_path;
        private FileType m_fileType;
        private string m_initializeCode;
        private string m_finalizeCode;
        private List<Function> m_functions = new List<Function>();
        private List<Constant> m_constants = new List<Constant>();

        #endregion

        #region Properties

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string Path
        {
            get { return m_path; }
            set { m_path = value; }
        }

        public FileType FileType
        {
            get { return m_fileType; }
            set { m_fileType = value; }
        }

        public string InitializeCode
        {
            get { return m_initializeCode; }
            set { m_initializeCode = value; }
        }

        public string FinalizeCode
        {
            get { return m_finalizeCode; }
            set { m_finalizeCode = value; }
        }

        public List<Function> Functions
        {
            get { return m_functions; }
        }

        public List<Constant> Constants
        {
            get { return m_constants; }
        }

        #endregion

        #endregion

        #region Methods

        public IncludeFile()
        { }

        public IncludeFile(string name, string path, FileType type, string initializeCode, string finalizeCode)
        {
            m_name = name;
            m_path = path;
            m_fileType = type;
            m_initializeCode = initializeCode;
            m_finalizeCode = finalizeCode;
        }

        #endregion
    }
}
