// ProjectWriter.cs
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
using System.IO;
using GameMaker;
using GameMaker.IO;

namespace GameMaker.GM7Project
{
    public class ProjectWriter : WriterBase
    {
        #region Members

        #region Fields

        private Obfuscation m_obfuscation = new Obfuscation();

        #endregion

        #region Properties


        internal Obfuscation Obfuscation
        {
            get { return m_obfuscation; }
        }

        public int[][] SwapTable
        {
            get { return m_obfuscation.SwapTable; }
        }

        public int Seed
        {
            get { return m_obfuscation.Seed; }
            set { m_obfuscation.Seed = value; }
        }

        #endregion

        #endregion

        #region Methods

        public ProjectWriter()
        {
        }

        public ProjectWriter(string path)
        {
            Open(path);
        }

        public void WriteGameId(int id)
        {
            byte[] array = BitConverter.GetBytes(id);
            m_writer.WriteByte(array[0]);
            WriteBytes(array, 1);
        }

        #region WriterBase

        protected override void WriteByte(byte b)
        {
            if (m_obfuscation.SwapTable != null)
                base.WriteByte((byte)m_obfuscation.SwapTable[0][(b + m_writer.Position) & 255]);
            else
                base.WriteByte(b);
        }


        #endregion

        #endregion
    }
}
