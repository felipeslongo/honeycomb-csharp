using System.Collections.Generic;
using System.Linq;

namespace HoneyComb.UI.Collections
{
    public sealed class ItensRemovedEvent<T>
    {
        public ItensRemovedEvent(IEnumerable<T> itens, int index)
        {
            Itens = itens;
            Index = index;
            Count = Itens.Count();
        }

        public int Count { get; }
        public IEnumerable<T> Itens { get; }
        public int Index { get; }
    }
}