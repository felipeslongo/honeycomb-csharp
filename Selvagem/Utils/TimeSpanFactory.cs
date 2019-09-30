using System;
using System.Collections.Generic;
using System.Text;

namespace Selvagem.Utils
{
    internal static class TimeSpanFactory
    {
        #region TimeSpanDeUmAnoWithLazy
        private static Lazy<TimeSpan> _timeSpanDeUmAnoWithLazy = new Lazy<TimeSpan>(() =>
        {
            var now = DateTime.Now;
            var span = now.AddYears(1) - now;
            return span;
        });

        public static TimeSpan TimeSpanDeUmAnoWithLazy => _timeSpanDeUmAnoWithLazy.Value;
        #endregion

        private static TimeSpan? _timeSpanDeUmAno;

        public static TimeSpan TimeSpanDeUmAno
        {
            get
            {
                //https://stackoverflow.com/questions/8355399/how-to-get-a-timespan-of-1-year
                if (_timeSpanDeUmAno == null)
                {
                    var now = DateTime.Now;
                    _timeSpanDeUmAno = now.AddYears(1) - now;
                }

                return _timeSpanDeUmAno.Value;
            }
        }

        public static TimeSpan FromYears(int years) => TimeSpanDeUmAno * years;
    }
}
