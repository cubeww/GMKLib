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

using System;
using System.Collections.Generic;

namespace GameMaker.ActionLibrary
{

    /// <summary>
    /// Defines basic information for actions in GameMaker.
    /// </summary>
    public class Action
    {

        private string m_name;
        private int m_id;
        private Image m_image;
        private bool m_hidden;
        private bool m_advanced;
        private bool m_registered; // GM 5.3 and up
        private string m_description;
        private string m_list;
        private string m_hint;
        private ActionKind m_kind;
        private InterfaceKind m_interfaceKind;
        private bool m_question;
        private bool m_applyTo;
        private bool m_relative;
        private List<Argument> m_arguments = new List<Argument>();
        private ExecutionType m_type;
        private string m_function;
        private string m_code;


        /// <summary>
        /// Gets or sets the name of the action
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
            }
        }


        /// <summary>
        /// Gets or sets the Id of the action.
        /// </summary>
        public int Id
        {
            get { return m_id; }
            set
            {
                m_id = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Image"/> of the action.
        /// </summary>
        public Image Image
        {
            get { return m_image; }
            set
            {
                m_image = value;
            }
        }

        /// <summary>
        /// Gets or sets whether action is hidden in GameMaker
        /// </summary>
        public bool Hidden
        {
            get { return m_hidden; }
            set
            {
                m_hidden = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the action should only show in advanced mode.
        /// </summary>
        public bool Advanced
        {
            get { return m_advanced; }
            set { m_advanced = value; }
        }

        /// <summary>
        /// Gets or sets whether the action is Registered only.
        /// </summary>
        public bool Registered
        {
            get { return m_registered; }
            set
            {
                m_registered = value;
            }
        }

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        public string Description
        {
            get { return m_description; }
            set
            {
                m_description = value;
            }
        }

        /// <summary>
        /// Gets or sets the List.
        /// </summary>
        public string List
        {
            get { return m_list; }
            set
            {
                m_list = value;
            }
        }

        /// <summary>
        /// Gets or sets the Hint
        /// </summary>
        public string Hint
        {
            get { return m_hint; }
            set
            {
                m_hint = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="Kind"/> of action.
        /// </summary>
        public ActionKind Kind
        {
            get { return m_kind; }
            set
            {
                m_kind = value;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="InterfaceKind"/> of the action.
        /// </summary>
        public InterfaceKind InterfaceKind
        {
            get { return m_interfaceKind; }
            set
            {
                m_interfaceKind = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to show the "Question" check box in GameMaker.
        /// </summary>
        public bool Question
        {
            get { return m_question; }
            set
            {
                m_question = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to show the "ApplyTo" check box in GameMaker.
        /// </summary>
        public bool ApplyTo
        {
            get { return m_applyTo; }
            set
            {
                m_applyTo = value;
            }
        }

        /// <summary>
        /// Gets or sets whether the action is Relative.
        /// </summary>
        public bool Relative
        {
            get { return m_relative; }
            set
            {
                m_relative = value;
            }
        }
        
        /// <summary>
        /// Gets the current list of arguments.
        /// </summary>
        public List<Argument> Arguments
        {
            get { return m_arguments; }
        }

        /// <summary>
        /// Gets or sets the <see cref="ExecutionType"/> of the action.
        /// </summary>
        public ExecutionType ExecutionType
        {
            get { return m_type; }
            set
            {
                m_type = value;
            }
        }

        /// <summary>
        /// Gets or sets the Function of the action.
        /// </summary>
        public string Function
        {
            get { return m_function; }
            set
            {
                m_function = value;
            }
        }

        /// <summary>
        /// Gets or sets the Code of the action.
        /// </summary>
        public string Code
        {
            get { return m_code; }
            set
            {
                m_code = value;
            }
        }


        /// <summary>
        /// Instantiates an insance of <see cref="Action"/>
        /// </summary>
        public Action()
        {}

        /// <summary>
        /// Adds an Argument to the list.
        /// </summary>
        /// <param name="a">The argument to add</param>
        public void AddArgument(Argument a)
        {
            Arguments.Add(a);
        }

        /// <summary>
        /// Removes an argument at a specific index from the list.
        /// </summary>
        /// <param name="index">Index to argument</param>
        public void RemoveArgument(int index)
        {
            Arguments.RemoveAt(index);
        }

        /// <summary>
        /// Removes a given argument from the list.
        /// </summary>
        /// <param name="a">Argument to add</param>
        public void RemoveArgument(Argument a)
        {
            Arguments.Remove(a);
        }

        /// <summary>
        /// Inserts a given argument at a given index.
        /// </summary>
        /// <param name="index">The index to insert at</param>
        /// <param name="a">The argument to insert</param>
        public void InsertArgument(int index, Argument a)
        {
            Arguments.Insert(index, a);
        }

        public override string ToString()
        {
            return m_name;
        }
    }
}
