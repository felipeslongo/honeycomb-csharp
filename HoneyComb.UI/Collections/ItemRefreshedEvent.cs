namespace HoneyComb.UI.Collections
{
    public sealed class ItemRefreshedEvent<T>
    {
        public ItemRefreshedEvent(T item, int index)
        {
            Item = item;
            Index = index;
        }

        public T Item { get; }
        public int Index { get; }
    }
}
