// Obfuscation.cs
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

namespace GameMaker.IO
{
    public class Obfuscation
    {
        private int[][] m_swapTable = null;
        private int m_seed;

        public Obfuscation()
        {
        }

        public int[][] SwapTable
        {
            get { return m_swapTable; }
        }

        public int Seed
        {
            get { return m_seed; }
            set
            {
                if (value < 0)
                {
                    m_swapTable = null;
                    m_seed = 0;
                    return;
                }
                m_seed = value;
                GenerateSwaptable();
            }
        }

        private void GenerateSwaptable()
        {
            m_swapTable = new int[2][];
            m_swapTable[0] = new int[256];
            m_swapTable[1] = new int[256];
            int a, b, i, j, t;
            a = (m_seed % 250) + 6;
            b = m_seed / 250;

            for (i = 0; i < 256; i++)
                m_swapTable[0][i] = i;

            for (i = 1; i < 10001; i++)
            {
                j = 1 + ((i * a + b) % 254);
                t = m_swapTable[0][j];
                m_swapTable[0][j] = m_swapTable[0][j + 1];
                m_swapTable[0][j + 1] = t;
            }

            for (i = 1; i < 256; i++)
            {
                m_swapTable[1][m_swapTable[0][i]] = i;
            }
        }
    }
}
