using System;
using System.Collections.Generic;
using System.Text;

namespace Selvagem.Utils
{
    public struct Peso : IEquatable<Peso>, IComparable<Peso>, IComparable
    {
        public decimal Kilos { get; }

        private Peso(decimal kilos)
        {
            Kilos = kilos;
        }
        

        public static Peso FromKilos(decimal kilos) => new Peso(kilos);
        public static Peso FromGramas(decimal gramas) => new Peso(gramas / 1000);
        public static Peso FromToneladas(decimal toneladas) => new Peso(toneladas * 1000);

        public override string ToString() => Kilos.ToString() + " Kg";
        
        public bool Equals(Peso other)
        {
            return Kilos == other.Kilos;
        }

        public override bool Equals(object obj)
        {
            return obj is Peso other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Kilos.GetHashCode();
        }

        public static bool operator ==(Peso left, Peso right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Peso left, Peso right)
        {
            return !left.Equals(right);
        }

        public int CompareTo(Peso other)
        {
            return Kilos.CompareTo(other.Kilos);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            return obj is Peso other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Peso)}");
        }

        public static bool operator <(Peso left, Peso right)
        {
            return left.CompareTo(right) < 0;
        }

        public static bool operator >(Peso left, Peso right)
        {
            return left.CompareTo(right) > 0;
        }

        public static bool operator <=(Peso left, Peso right)
        {
            return left.CompareTo(right) <= 0;
        }

        public static bool operator >=(Peso left, Peso right)
        {
            return left.CompareTo(right) >= 0;
        }
        
        public static implicit operator decimal(Peso peso) => peso.Kilos;
        public static explicit operator Peso(decimal pesoEmKilos) => FromKilos(pesoEmKilos);
    }
}
