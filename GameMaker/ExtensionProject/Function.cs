// Function.cs
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
    public class Function
    {
        #region Members
        #region Fields
        private string m_name;
        private string m_externalName;
        private CallConvention m_callConvention;
        private string m_helpline;
        private bool m_hidden;
        private List<ResultType> m_arguments = new List<ResultType>();
        private ResultType m_resultType;
        #endregion
        #region properties

        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        public string ExternalName
        {
            get { return m_externalName; }
            set { m_externalName = value; }
        }

        public CallConvention CallConvention
        {
            get { return m_callConvention; }
            set { m_callConvention = value; }
        }

        public string Helpline
        {
            get { return m_helpline; }
            set { m_helpline = value; }
        }

        public bool Hidden
        {
            get { return m_hidden; }
            set { m_hidden = value; }
        }

        public List<ResultType> Arguments
        {
            get { return m_arguments; }
        }

        public ResultType ResultType
        {
            get { return m_resultType; }
            set { m_resultType = value; }
        }
        #endregion
        #endregion

        #region Methods
        public Function()
        {}

        public Function(string name, string externalName, string helpline, bool hidden, ResultType[] arguments, ResultType resultType)
        {
            m_name = name;
            m_externalName = externalName;
            m_helpline = helpline;
            m_hidden = hidden;
            m_resultType = resultType;

            if (arguments != null)
            {
                foreach (ResultType t in arguments)
                    m_arguments.Add(t);
            }
        }


        #endregion
    }
}
