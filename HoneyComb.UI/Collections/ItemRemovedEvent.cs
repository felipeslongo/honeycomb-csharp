namespace HoneyComb.UI.Collections
{
    public sealed class ItemRemovedEvent<T>
    {
        public ItemRemovedEvent(T item, int oldIndex)
        {
            Item = item;
            OldIndex = oldIndex;
        }

        public T Item { get; }
        public int OldIndex { get; }
    }
}
