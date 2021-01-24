namespace HoneyComb.UI.Collections
{
    public sealed class ItemAddedEvent<T>
    {
        public ItemAddedEvent(T item, int index)
        {
            Item = item;
            Index = index;
        }

        public T Item { get; }
        public int Index { get; }
    }
}