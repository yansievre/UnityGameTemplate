using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Utility
{
    public class ExpandingArray<T> : ICollection<T>, IList<T>
    {
        private T[] _array;

        private ExpandingArray()
        {
            
        }
        
        public ExpandingArray(int initCapacity = 0)
        {
            _array = new T[initCapacity];
        }

        private int NextSize => _array.Length > 0 ? _array.Length * 2 : 2;

        public int IndexOf(T item)
        {
            return Array.IndexOf(_array, item);
        }

        public void Insert(int index, T item)
        {
            throw new NotSupportedException("NotSupported_FixedSizeCollection");
        }

        public void RemoveAt(int index)
        {
            throw new NotSupportedException("NotSupported_FixedSizeCollection");
        }

        public T this[int index]
        {
            get
            {
                if (index >= _array.Length)
                {
                    Array.Resize(ref _array, NextSize);
                }

                return _array[index];
            }
            set
            {
                if (index >= _array.Length)
                {
                    Array.Resize(ref _array, NextSize);
                }

                _array[index] = value;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(this);
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(this);
        }

        public void Add(T item)
        {
            throw new NotSupportedException("NotSupported_FixedSizeCollection");
        }

        public void Clear()
        {
            Array.Clear(_array, _array.GetLowerBound(0), _array.Length);
        }

        public bool Contains(T item)
        {
            return Array.IndexOf(_array, item) >= _array.GetLowerBound(0);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            _array.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item)
        {
            throw new NotSupportedException("NotSupported_FixedSizeCollection");
        }

        public int Count => _array.Length;
        public int Length => _array.Length;

        public bool IsReadOnly => _array.IsReadOnly;
        
        public struct Enumerator : IEnumerator<T>, IEnumerator, IDisposable
        {
            private ExpandingArray<T> m_Array;
            private int m_Index;
            private T value;

            public Enumerator(ExpandingArray<T> array)
            {
                this.m_Array = array;
                this.m_Index = -1;
                this.value = default (T);
            }

            public void Dispose()
            {
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                ++this.m_Index;
                if (this.m_Index < this.m_Array.Length)
                {
                    this.value = m_Array[m_Index];
                    return true;
                }
                this.value = default (T);
                return false;
            }

            public void Reset() => this.m_Index = -1;

            public T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)] get => this.value;
            }

            object IEnumerator.Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)] get => (object) this.Current;
            }
        }
    }
}