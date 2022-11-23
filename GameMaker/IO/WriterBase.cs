// WriterBase.cs
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

namespace GameMaker.IO
{
    public class WriterBase
    {


        private string m_fileName;
        protected MemoryStream m_writer = null;



        public string FileName
        {
            get { return m_fileName; }
            set { m_fileName = value; }
        }

        public long Position
        {
            get { return m_writer.Position; }
            set { m_writer.Position = value; }
        }




        protected virtual void WriteByte(byte b)
        {
            m_writer.WriteByte(b);
        }

        protected virtual void WriteBytes(byte[] b)
        {
            WriteBytes(b, 0);
        }

        protected virtual void WriteBytes(byte[] b, int startIndex)
        {
            WriteBytes(b, startIndex, b.Length);
        }

        protected virtual void WriteBytes(byte[] b, int startIndex, int count)
        {
            if (b == null)
                throw new ArgumentNullException("b");
            if (startIndex > count)
                throw new ArgumentOutOfRangeException("startIndex");
            if (count > b.Length)
                throw new ArgumentOutOfRangeException("count");

            for (int i = startIndex; i < count; i++)
                WriteByte(b[i]);
        }

        protected virtual void WriteChar(char c)
        {
            WriteByte((byte)c);
        }

        protected virtual void WriteChars(char[] c)
        {
            WriteChars(c, 0);
        }

        protected virtual void WriteChars(char[] c, int startIndex)
        {
            WriteChars(c, startIndex, c.Length);
        }

        protected virtual void WriteChars(char[] c, int startIndex, int endIndex)
        {
            if (c == null)
                throw new ArgumentNullException("c");
            if (startIndex > endIndex)
                throw new ArgumentOutOfRangeException("startIndex");
            if (endIndex > c.Length)
                throw new ArgumentOutOfRangeException("endIndex");

            for (int i = startIndex; i < endIndex; i++)
                WriteChar(c[i]);
        }

        protected virtual void WriteString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                WriteInt(0);
                return;
            }

            WriteInt(str.Length);
            WriteChars(str.ToCharArray());
        }

        protected virtual void WriteInt(int val)
        {
            WriteBytes(BitConverter.GetBytes(val));
        }

        protected virtual void WriteBool(bool val)
        {
            WriteInt(Convert.ToInt32(val));
        }

        protected virtual void WriteDouble(double val)
        {
            WriteBytes(BitConverter.GetBytes(val));
        }

        public void Open(string filename)
        {
            Close(); // Just in case
            m_fileName = filename;
            m_writer = new MemoryStream();
        }

        public void Close()
        {
            if (m_writer == null)
                return;
						
			using (BinaryWriter writer = new BinaryWriter(new FileStream(m_fileName, FileMode.Create, FileAccess.ReadWrite)))
			{
				
				writer.Write(m_writer.ToArray());
			}
			
            m_writer.Close();
            m_writer = null;
        }

    }
}
