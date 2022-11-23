// Action.cs
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

using GameMaker;

namespace GameMaker.ProjectCommon
{
    public class Action
    {
        #region Members

        #region Fields

        private bool m_relative;
        private bool m_not;
        private bool m_allowRelative;
        private bool m_question;
        private bool m_canApplyTo;
        private int m_libraryId = 1;
        private int m_actionId = 101;
        private int m_appliesTo = -1;
        private string m_executeCode = string.Empty;
        private ActionKind m_actionKind = ActionKind.Normal;
        private ExecutionType m_executionType = ExecutionType.Function;
        private Argument[] m_arguments = null;

        #endregion

        #region Properties

        public bool Question
        {
            get { return m_question; }
            set { m_question = value; }
        }

        public bool Relative
        {
            get { return m_relative; }
            set { m_relative = value; }
        }

        public bool Not
        {
            get { return m_not; }
            set { m_not = value; }
        }

        public bool AllowRelative
        {
            get { return m_allowRelative; }
            set { m_allowRelative = value; }
        }

        public bool CanApplyTo
        {
            get { return m_canApplyTo; }
            set { m_canApplyTo = value; }
        }

        public int LibraryId
        {
            get { return m_libraryId; }
            set { m_libraryId = value; }
        }

        public int ActionId
        {
            get { return m_actionId; }
            set { m_actionId = value; }
        }
        
        public int AppliesTo
        {
            get {return m_appliesTo;}
            set {m_appliesTo = value;}
        }

        public string ExecuteCode
        {
            get { return m_executeCode; }
            set { m_executeCode = value; }
        }

        public ActionKind ActionKind
        {
            get { return m_actionKind; }
            set { m_actionKind = value; }
        }

        public ExecutionType ExecutionType
        {
            get { return m_executionType; }
            set { m_executionType = value; }
        }

        public Argument[] Arguments
        {
            get { return m_arguments; }
            set { m_arguments = value; }
        }

        #endregion

        #endregion
    }

    public class Argument
    {
        #region Members

        #region Fields

        private int m_resource = -1;
        private string m_value = string.Empty;
        private ArgumentType m_type = ArgumentType.Expression;

        #endregion

        #region Properties

        public int Resource
        {
            get { return m_resource; }
            set { m_resource = value; }
        }

        public string Value
        {
            get { return m_value; }
            set { m_value = value; }
        }

        public ArgumentType Type
        {
            get { return m_type; }
            set { m_type = value; }
        }

        #endregion

        #endregion
    }
}
