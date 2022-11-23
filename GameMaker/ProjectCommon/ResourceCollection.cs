// ResourceCollection.cs
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
using System.Collections;
using System.Collections.Generic;

namespace GameMaker.ProjectCommon
{
    public class ResourceCollection<T> : IList<T> where T : Resource
    {



        private const int lastInstanceId = 100000;
        private const int lastTileId = 1000000;
        private List<T> m_items = new List<T>();
        private int m_lastId = -1;



        public int LastId
        {
            get { return m_lastId; }
            set { m_lastId = value; }
        }

        public T this[int index]
        {
            get { return m_items[index]; }
            set { m_items[index] = value; }
        }

        public int Count
        {
            get { return m_items.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }




        public void ResetIds()
        {
            m_lastId = -1;
        }

        public T Find(Predicate<T> match)
        {
            return m_items.Find(match);
        }

        public int IndexOf(T item)
        {
            return m_items.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            m_items.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            m_items.RemoveAt(index);
        }

        public void Add(T item)
        {
            if (item is Resource)
            {
                m_lastId++;
                (item as Resource).Id = m_lastId;
            }

            m_items.Add(item);
        }

        public void Clear()
        {
            m_items.Clear();
        }

        public bool Contains(T item)
        {
            return m_items.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            m_items.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            return m_items.Remove(item);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_items.GetEnumerator();
        }

        public T[] ToArray()
        {
            return m_items.ToArray();
        }


    }
}
