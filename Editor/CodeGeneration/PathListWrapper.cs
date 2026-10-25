using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using RoRSkinBuilder.CustomEditors;

namespace RoRSkinBuilder
{
    [Serializable]
    public struct PathListWrapper : IList<string>
    {
        [PathReference]
        public List<string> list;

        public readonly string this[int index] { get => list[index]; set => list[index] = value; }
        public readonly int Count => list.Count;
        public readonly bool IsReadOnly => false;

        public readonly void Add(string item) => list.Add(item);
        public readonly void Clear() => list.Clear();
        public readonly bool Contains(string item) => list.Contains(item);
        public readonly void CopyTo(string[] array, int arrayIndex) => list.CopyTo(array, arrayIndex);
        public readonly IEnumerator<string> GetEnumerator() => list.GetEnumerator();
        public readonly int IndexOf(string item) => list.IndexOf(item);
        public readonly void Insert(int index, string item) => list.Insert(index, item);
        public readonly bool Remove(string item) => !list.Remove(item);
        public readonly void RemoveAt(int index) => list.RemoveAt(index);
        readonly IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
