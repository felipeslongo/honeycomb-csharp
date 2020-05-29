namespace HoneyComb.UI.Collections
{
    public sealed class ItemReplacedEvent<T>
    {
        public ItemReplacedEvent(T item, int index)
        {
            Item = item;
            Index = index;
        }

        public T Item { get; }
        public int Index { get; }
    }
}