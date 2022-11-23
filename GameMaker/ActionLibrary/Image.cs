// Image.cs
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
using System.Drawing;
using System.Drawing.Imaging;

namespace GameMaker.ActionLibrary
{
    /// <summary>
    /// A helper class to ease bitmap conversion.
    /// </summary>
    public class Image
    {

        private byte[] m_data;



        /// <summary>
        /// Returns the image data as a <see cref="System.Byte[]"/>.
        /// </summary>
        public byte[] Data
        {
            get { return m_data; }
            set { m_data = value; } // Is this really necessary?
        }

        public int Size
        {
            get { return m_data.Length; }
        }




        /// <summary>
        /// Creates an instance of <see cref="Image"/>.
        /// </summary>
        public Image() { }

        /// <summary>
        /// Creates an instance using the <see cref="System.Byte[]"/> provided.
        /// </summary>
        /// <param name="image">
        /// A <see cref="System.Byte[]"/>
        /// </param>
        public Image(byte[] image)
        {
            m_data = image;
        }

        /// <summary>
        /// Creates an instance of <see cref="Image"/> converting a <see cref="Bitmap"/> to a <see cref="System.Byte[]"/>.
        /// </summary>
        /// <param name="btmp">
        /// A <see cref="Bitmap"/>
        /// </param>
        public Image(Bitmap btmp)
        {
            FromBitmap(btmp);
        }

        /// <summary>
        /// Converts the image data to a <see cref="Bitmap"/> for use with a <see cref="PictureBox"/> or to save
        /// to the hard drive.
        /// </summary>
        /// <returns>
        /// A <see cref="Bitmap"/>
        /// </returns>
        public Bitmap ToBitmap()
        {
            MemoryStream stm = new MemoryStream(m_data);
            Bitmap btmp = new Bitmap(stm);
            stm.Close();
            return btmp;
        }

        /// <summary>
        /// Takes a <see cref="Bitmap"/> and converts to a <see cref="System.Byte[]"/>.
        /// </summary>
        /// <param name="btmp">
        /// A <see cref="Bitmap"/>
        /// </param>
        public void FromBitmap(Bitmap btmp)
        {
            MemoryStream stm = new MemoryStream();
            btmp.Save(stm, ImageFormat.Bmp);
            m_data = stm.ToArray();
            stm.Close();
        }

    }
}
