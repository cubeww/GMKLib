// ReaderBase.cs
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

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.Drawing;
using System.IO;
using System.IO.Compression;

namespace GameMaker.IO
{
    public class ReaderBase
    {
        private string m_fileName;
        public MemoryStream m_reader = null;

        public string FileName
        {
            get { return m_fileName; }
            set { m_fileName = value; }
        }

        public long Length
        {
            get { return m_reader.Length; }
        }

        public long Position
        {
            get { return m_reader.Position; }
            set { m_reader.Position = value; }
        }

        public ReaderBase() { }

        public virtual long Seek(long offset, SeekOrigin origin)
        {
            Position = offset;
            return Position;
        }


        public virtual byte ReadByte()
        {
            return (byte)m_reader.ReadByte();
        }

        public virtual byte[] ReadBytes(int count)
        {
            var b = new byte[count];
            m_reader.Read(b, 0, count);

            return b;
        }

        public virtual char ReadChar()
        {
            return (char)ReadByte();
        }

        public virtual char[] ReadChars(int count)
        {
            char[] c = new char[count];
            for (int i = 0; i < count; i++)
                c[i] = ReadChar();

            return c;
        }

        public virtual string ReadString()
        {
            int len = ReadInt();
            return new string(ReadChars(len));
        }

        public virtual int ReadInt()
        {
            return BitConverter.ToInt32(ReadBytes(4), 0);
        }

        public virtual bool ReadBool()
        {
            return BitConverter.ToBoolean(ReadBytes(4), 0);
        }

        public virtual double ReadDouble()
        {
            return BitConverter.ToDouble(ReadBytes(8), 0);
        }

        public virtual ReaderBase ReadChunk()
        {
            int count = ReadInt();

            var newReader = new ReaderBase();
            newReader.m_reader = new MemoryStream(ReadBytes(count));
            newReader.Position = 0;
            return newReader;
        }

        public virtual void Decompress()
        {
            ReadBytes(2);

            MemoryStream compressed = new MemoryStream(ReadBytes((int)m_reader.Length - 2));
            DeflateStream deflateStream = new DeflateStream(compressed, CompressionMode.Decompress);
            m_reader = new MemoryStream();

            deflateStream.CopyTo(m_reader);
            Position = 0;
        }

        public void Open(string path)
        {
            Close(); // Just in case

            m_fileName = path;
            m_reader = new MemoryStream(File.ReadAllBytes(path));
            Position = 0;
        }

        public void Close()
        {
            if (m_reader == null)
                return;

            m_reader = null;
        }

        /// <summary>
        /// Returns the files GM version. 
        /// </summary>
        /// <param name="path">
        /// A <see cref="System.String"/>
        /// </param>
        /// <returns>
        /// A <see cref="FileVersion"/>
        /// </returns>
        public static FileVersion GetVersion(string path)
        {
            FileVersion ret;

            using (BinaryReader reader = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.ReadWrite)))
            {
                reader.BaseStream.Position = 4;
                ret = (FileVersion)reader.ReadInt32();
            }
            return ret;
        }


    }
}
