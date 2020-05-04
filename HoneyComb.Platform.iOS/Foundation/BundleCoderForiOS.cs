using Foundation;
using HoneyComb.Core.Vault;
using System;
using System.Collections.Generic;

namespace HoneyComb.Platform.iOS.Foundation
{
    /// <summary>
    ///     Implementation of <see cref="IBundleCoder"/> for iOS
    /// </summary>
    public sealed class BundleCoderForiOS : IBundleCoder
    {
        public BundleCoderForiOS(NSCoder coder)
        {
            Coder = coder;
        }

        public NSCoder Coder { get; }

        public void Add(string key, bool value) => Coder.Encode(value, key);

        public void Add(string key, int value) => Coder.Encode(value, key);

        public void Add(string key, decimal value) => Coder.Encode(Convert.ToDouble(value), key);

        public void Add(string key, string value) => Coder.Encode(new NSString(value), key);

        public bool ContainsKey(string key) => Coder.ContainsKey(key);

        public bool GetBoolean(string key) => GetValue(key, Coder.DecodeBool);

        public decimal GetDecimal(string key) => Convert.ToDecimal(GetValue(key, Coder.DecodeDouble));

        public int GetInteger(string key) => GetValue(key, Coder.DecodeInt);

        public string GetString(string key) => (NSString)GetValue(key, Coder.DecodeObject);

        private T GetValue<T>(string key, Func<string, T> decoder)
        {
            if (ContainsKey(key) is false)
                throw new KeyNotFoundException($"Key not found for {key}.");

            return decoder(key);
        }
    }
}
