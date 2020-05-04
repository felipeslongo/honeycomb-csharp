using Android.OS;
using HoneyComb.Core.Vault;
using System;
using System.Collections.Generic;

namespace HoneyComb.Platform.Android.OS
{
    /// <summary>
    ///     Implementation of <see cref="IBundleCoder"/> for Android
    /// </summary>
    public sealed class BundleCoderForAndroid : IBundleCoder
    {
        public BundleCoderForAndroid(Bundle bundle)
        {
            Bundle = bundle;
        }

        public Bundle Bundle { get; }

        public void Add(string key, bool value) => Bundle.PutBoolean(key, value);

        public void Add(string key, int value) => Bundle.PutInt(key, value);

        public void Add(string key, decimal value) => Bundle.PutDouble(key, Convert.ToDouble(value));

        public void Add(string key, string value) => Bundle.PutString(key, value);

        public bool ContainsKey(string key) => Bundle.ContainsKey(key);

        public bool GetBoolean(string key) => GetValue(key, Bundle.GetBoolean);

        public decimal GetDecimal(string key) => Convert.ToDecimal(GetValue(key, Bundle.GetDouble));

        public int GetInteger(string key) => GetValue(key, Bundle.GetInt);

        public string GetString(string key) => GetValue(key, Bundle.GetString);

        private T GetValue<T>(string key, Func<string, T> bundleGetter)
        {
            if (ContainsKey(key) is false)
                throw new KeyNotFoundException($"Key not found for {key}.");

            return bundleGetter(key);
        }
    }
}
