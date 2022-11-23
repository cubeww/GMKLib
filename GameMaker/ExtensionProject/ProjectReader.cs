// ProjectReader.cs
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

using GameMaker.ProjectCommon;
using GameMaker.IO;

namespace GameMaker.ExtensionProject
{
    public class ProjectReader : ReaderBase
    {
        Obfuscation m_obfuscation = new Obfuscation();

        public ProjectReader()
        {
        }

        public ProjectReader(string path)
        {
            Open(path);
        }

        public Project ReadProject()
        {
            Project p = new Project();

            ReadInt(); // version
            p.Editable = ReadBool();
            p.Name = ReadString();
            p.TempDirectory = ReadString();
            p.Version = ReadString();
            p.Author = ReadString();
            p.DateModified = ReadString();
            p.License = ReadString();
            p.Information = ReadString();
            p.HelpFile = ReadString();
            p.Hidden = ReadBool();
            int count = ReadInt();

            while ((count--) > 0)
                p.Uses.Add(ReadString());

            count = ReadInt();

            while ((count--) > 0)
            {
                IncludeFile file = new IncludeFile();
                ReadInt();
                file.Name = ReadString();
                file.Path = ReadString();
                file.FileType = (FileType)ReadInt();
                file.InitializeCode = ReadString();
                file.FinalizeCode = ReadString();
                int tmpCount = ReadInt();

                while ((tmpCount--) > 0)
                {
                    Function function = new Function();
                    ReadInt();
                    function.Name = ReadString();
                    function.ExternalName = ReadString();
                    function.CallConvention = (CallConvention)ReadInt();
                    function.Helpline = ReadString();
                    function.Hidden = ReadBool();

                    int argCount = ReadInt();

                    while ((argCount--) > 0)
                        function.Arguments.Add((ResultType)ReadInt());

                    if (function.Arguments.Count < 17)
                    {
                        int c = 17 - function.Arguments.Count;
                        while ((c--) > 0)
                            ReadInt();
                    }

                    function.ResultType = (ResultType)ReadInt();

                    file.Functions.Add(function);
                }

                tmpCount = ReadInt();

                while ((tmpCount--) > 0)
                {
                    Constant c = new Constant();
                    ReadInt();
                    c.Name = ReadString();
                    c.Value = ReadString();
                    c.Hidden = ReadBool();
                    file.Constants.Add(c);
                }

                p.Files.Add(file);
            }

            return p;
        }

        public Project LoadInstaller()
        {
            ReadInt(); // id;
            ReadInt(); // version;
            m_obfuscation.Seed = ReadInt(); // seed

            Project p = ReadProject();

            if (!string.IsNullOrEmpty(p.HelpFile))
            {
                byte[] b = ReadBytes(ReadInt());
                Inflater inf = new Inflater();
                inf.SetInput(b);

                using (BinaryWriter w = new BinaryWriter(new FileStream(System.IO.Path.GetFileName(p.HelpFile), FileMode.OpenOrCreate, FileAccess.ReadWrite)))
                {
                    while (!inf.IsFinished)
                    {
                        byte[] output = new byte[1000];
                        long size = inf.Inflate(output);
                        w.Write(output, 0, (int)size);
                    }
                }

            }
            
            // The data are stored in the order of the filenames
            foreach (IncludeFile f in p.Files)
            {
                if (!string.IsNullOrEmpty(f.Name))
                {
                    byte[] b = ReadBytes(ReadInt());
                    Inflater inf = new Inflater();
                    inf.SetInput(b);

                    using (BinaryWriter w = new BinaryWriter(new FileStream(f.Name , FileMode.OpenOrCreate, FileAccess.ReadWrite)))
                    {
                        while (!inf.IsFinished)
                        {
                            byte[] output = new byte[1000];
                            long size = inf.Inflate(output);
                            w.Write(output, 0, (int)size);
                        }
                    }

                }
            }

            return p;
        }

        protected override byte ReadByte()
        {
            if (m_obfuscation.SwapTable != null)
                return (byte)m_obfuscation.SwapTable[1][base.ReadByte()];

            return base.ReadByte();
        }
    }
}
