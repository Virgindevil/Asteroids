using System;
using System.Collections.Generic;

namespace Game.Core
{
    public class ObjectPool<T> where T : class
    {
        private readonly Stack<T> _pool = new();
        private readonly Func<T> _factory;
        private readonly Action<T> _onGet;
        private readonly Action<T> _onRelease;

        public int CountActive { get; private set; }

        public ObjectPool(Func<T> factory, Action<T> onGet = null, Action<T> onRelease = null, int initialSize = 0)
        {
            _factory = factory;
            _onGet = onGet;
            _onRelease = onRelease;
            for (int i = 0; i < initialSize; i++)
                _pool.Push(_factory());
        }

        public T Get()
        {
            var item = _pool.Count > 0 ? _pool.Pop() : _factory();
            _onGet?.Invoke(item);
            CountActive++;
            return item;
        }

        public void Release(T item)
        {
            _onRelease?.Invoke(item);
            _pool.Push(item);
            CountActive--;
        }

        public void Clear()
        {
            _pool.Clear();
            CountActive = 0;
        }
    }
}