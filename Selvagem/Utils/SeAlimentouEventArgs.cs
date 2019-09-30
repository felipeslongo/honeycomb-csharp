using System;

namespace Selvagem.Utils
{
    public class SeAlimentouEventArgs : EventArgs
    {
        public SeAlimentouEventArgs(string alimento, string restoDoAlimento)
        {
            Alimento = alimento;
            RestoDoAlimento = restoDoAlimento;
        }
        
        public string Alimento { get; }
        public string RestoDoAlimento { get; }
    }
}
