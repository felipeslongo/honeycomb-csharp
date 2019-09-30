using Selvagem.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Selvagem
{
    public class Kiwi : Ave
    {
        public const int VidaUtilEmAnos = 25;

        public Guid Id { get; } = Guid.NewGuid();
        public TimeSpan VidaUtil { get; } = TimeSpanFactory.FromYears(VidaUtilEmAnos);
        public decimal Peso { get; } = 5;
    }
}
