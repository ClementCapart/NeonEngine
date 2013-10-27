using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeonEngine
{
    public class NeonPool<T>
    {
        private List<T> _Items = new List<T>();
        private Queue<T> _AvailableItems = new Queue<T>();

        private Func<T> InitializeItem;

        public NeonPool(Func<T> InitializeItem)
        {
            this.InitializeItem = InitializeItem;
        }

        public void FlagAvailableItem(T item)
        {
            if(_Items.Contains(item))
                _AvailableItems.Enqueue(item);
        }

        public T GetAvailableItem()
        {
            if (_AvailableItems.Count == 0)
            {
                T item = InitializeItem();
                _Items.Add(item);

                return item;
            }

            return _AvailableItems.Dequeue();
        }

        public List<T> Items
        {
            get { return _Items; }
        }

        public void Clear()
        {
            _Items.Clear();
            _AvailableItems.Clear();
        }
    }
}
