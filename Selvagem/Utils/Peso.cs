using System;
using System.Collections.Generic;
using System.Text;

namespace Selvagem.Utils
{
    public struct Peso
    {
        public decimal Kilos { get; }

        private Peso(decimal kilos)
        {
            Kilos = kilos;
        }

        public static Peso FromKilos(decimal kilos) => new Peso(kilos);
        public static Peso FromGramas(decimal gramas) => new Peso(gramas / 1000);
        public static Peso FromToneladas(decimal toneladas) => new Peso(toneladas * 1000);
    }
}
