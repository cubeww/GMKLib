// Sound.cs
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

namespace GameMaker.ProjectCommon
{
    public class Sound : Resource
    {


        private DateTime m_lastChanged = DateTime.Now;
        private SoundType m_soundType = SoundType.Normal;
        private SoundFormat m_soundFormat = SoundFormat.None;
        private double m_volume = 1.0d;
        private double m_pan;
        private string m_fileType;
        private string m_filename;
        private int m_effects;
        private int m_buffers = 1;
        private bool m_allowSoundEffects;
        private bool m_loadOnlyOnUse;
        private bool m_preload = true;
        private byte[] m_data = null;



        public DateTime LastChanged
        {
            get { return m_lastChanged; }
            set { m_lastChanged = value; }
        }

        public SoundType SoundType
        {
            get { return m_soundType; }
            set { m_soundType = value; }
        }

        public SoundFormat SoundFormat
        {
            get { return m_soundFormat; }
            set { m_soundFormat = value; }
        }

        public double Volume
        {
            get { return m_volume; }
            set { m_volume = value; }
        }

        public double Pan
        {
            get { return m_pan; }
            set { m_pan = value; }
        }

        public string FileType
        {
            get { return m_fileType; }
            set { m_fileType = value; }
        }

        public string FileName
        {
            get { return m_filename; }
            set { m_filename = value; }
        }

        public int Effects
        {
            get { return m_effects; }
            set { m_effects = value; }
        }

        public int Buffers
        {
            get { return m_buffers; }
            set { m_buffers = value; }
        }

        public bool AllowSoundEffects
        {
            get { return m_allowSoundEffects; }
            set { m_allowSoundEffects = value; }
        }

        public bool LoadOnlyOnUse
        {
            get { return m_loadOnlyOnUse; }
            set { m_loadOnlyOnUse = value; }
        }

        public bool Preload
        {
            get { return m_preload; }
            set { m_preload = value; }
        }

        public byte[] Data
        {
            get { return m_data; }
            set { m_data = value; }
        }


    }
}
