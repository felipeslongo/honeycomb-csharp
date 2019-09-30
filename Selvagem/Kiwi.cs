﻿using Selvagem.Utils;
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
        public DateTimeOffset Nascimento { get; } = DateTimeOffset.UtcNow;
        public DateTimeOffset? Falescimento { get; }
        public TimeSpan Idade => (Falescimento ?? DateTimeOffset.Now) - Nascimento;
        public bool TaVivo => !Falescimento.HasValue;
        public Peso Peso { get; } = Peso.FromKilos(5);

        public string Alimentar(string alimento)
        {
            var restoDoAlimento = String.Empty;
            switch (alimento.ToLower())
            {
                case "fruta":
                    restoDoAlimento = "sementes";
                    break;
                case "inseto":
                    restoDoAlimento = string.Empty;
                    break;
                default:
                    restoDoAlimento = alimento;
                    break;
            }

            return restoDoAlimento;
        }
    }
}
