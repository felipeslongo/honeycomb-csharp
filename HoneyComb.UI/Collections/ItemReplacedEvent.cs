namespace HoneyComb.UI.Collections
{
    public sealed class ItemReplacedEvent<T>
    {
        public ItemReplacedEvent(T oldItem, T item, int index)
        {
            Item = item;
            Index = index;
            OldItem = oldItem;
        }

        public T Item { get; }
        public int Index { get; }
        public T OldItem { get; }
    }
}
