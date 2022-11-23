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

using ICSharpCode.SharpZipLib.Zip.Compression;

using GameMaker.IO;
using GameMaker.ProjectCommon;

namespace GameMaker.ExtensionProject
{
    public class ProjectWriter : WriterBase
    {

        Obfuscation m_obfuscation = new Obfuscation();

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

        public ProjectWriter()
        {
        }

        public ProjectWriter(string path)
        {
            Open(path);
        }

        public void WriteProject(Project proj)
        {
            if (proj == null)
                throw new ArgumentNullException("proj");

            WriteInt(700);
            WriteBool(proj.Editable);
            WriteString(proj.Name);
            WriteString(proj.TempDirectory);
            WriteString(proj.Version);
            WriteString(proj.Author);
            WriteString(proj.DateModified);
            WriteString(proj.License);
            WriteString(proj.Information);
            WriteString(proj.HelpFile);
            WriteBool(proj.Hidden);
            WriteInt(proj.Uses.Count);
            foreach (string s in proj.Uses)
                WriteString(s);

            WriteInt(proj.Files.Count);
            foreach (IncludeFile f in proj.Files)
            {
                WriteInt(700);
                WriteString(f.Name);
                WriteString(f.Path);
                WriteInt((int)f.FileType);
                WriteString(f.InitializeCode);
                WriteString(f.FinalizeCode);
                WriteInt(f.Functions.Count);
                foreach (Function func in f.Functions)
                {
                    WriteInt(700);
                    WriteString(func.Name);
                    WriteString(func.ExternalName);
                    WriteInt((int)func.CallConvention);
                    WriteString(func.Helpline);
                    WriteBool(func.Hidden);
                    WriteInt(func.Arguments.Count);
                    foreach (ResultType t in func.Arguments)
                        WriteInt((int)t);

                    if (func.Arguments.Count < 17)
                    {
                        for (int i = 0; i < 17 - func.Arguments.Count; i++)
                            WriteInt(1);
                    }

                    WriteInt((int)func.ResultType);
                }

                WriteInt(f.Constants.Count);
                foreach (Constant c in f.Constants)
                {
                    WriteInt(700);
                    WriteString(c.Name);
                    WriteString(c.Value);
                    WriteBool(c.Hidden);
                }
            }
        }

        public void CreateInstaller(Project proj)
        {
            WriteInt(1234321);
            WriteInt(701);
            Random random = new Random();
            int seed = (random.Next() % 25600) + 3000;
            WriteInt(seed);
            Seed = seed;

            proj.Editable = false;

            WriteProject(proj);

            if (!string.IsNullOrEmpty(proj.HelpFile))
                CompressFile(proj.HelpFile);

            foreach (IncludeFile f in proj.Files)
                CompressFile(f.Path);
        }

        private void CompressFile(string path)
        {
            MemoryStream stm = new MemoryStream();

            // Open it.
            using (BinaryReader r = new BinaryReader(new FileStream(path, FileMode.Open, FileAccess.ReadWrite)))
            {
                // Get the data
                byte[] data = r.ReadBytes((int)r.BaseStream.Length);
                Deflater def = new Deflater();
                def.SetInput(data);
                def.Finish();

                // Compress it.
                while (!def.IsFinished)
                {
                    byte[] buf = new byte[1000];
                    int size = def.Deflate(buf);
                    stm.Write(buf, 0, size);
                }
            }

            // Write it to the installer
            WriteInt((int)stm.Length);
            WriteBytes(stm.ToArray());
            stm.Close();
        }

        protected override void WriteByte(byte b)
        {
            if (m_obfuscation.SwapTable != null)
                base.WriteByte((byte)m_obfuscation.SwapTable[0][b]);
            else
                base.WriteByte(b);
        }
    }
}
