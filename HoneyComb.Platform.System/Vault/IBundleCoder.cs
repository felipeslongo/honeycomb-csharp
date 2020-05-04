namespace HoneyComb.Core.Vault
{
    /// <summary>
    ///     A mapping from String keys to various typed values.
    ///     An abstract class that serves as the basis for objects
    ///     that enable archiving and distribution of other objects.
    /// </summary>
    /// <remarks>
    ///     Abstraction for platform specific implementations like:
    ///         https://developer.apple.com/documentation/foundation/nscoder
    ///         https://developer.android.com/reference/android/os/Bundle
    /// </remarks>
    public interface IBundleCoder
    {
        /// <summary>
        ///     Encodes a Boolean value and associates it with the string key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Add(string key, bool value);

        /// <summary>
        ///     Encodes a Integer value and associates it with the string key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Add(string key, int value);

        /// <summary>
        ///     Encodes a Decimal value and associates it with the string key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Add(string key, decimal value);

        /// <summary>
        ///     Encodes a string value and associates it with the string key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void Add(string key, string value);

        /// <summary>
        ///     Returns a Boolean value that indicates whether an encoded value is available for a string.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool ContainsKey(string key);

        /// <summary>
        ///     Decodes and returns a boolean value that was previously encoded
        ///     and associated with the string key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        bool GetBoolean(string key);

        /// <summary>
        ///     Decodes and returns a decimal value that was previously encoded
        ///     and associated with the string key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        decimal GetDecimal(string key);

        /// <summary>
        ///     Decodes and returns a integer value that was previously encoded
        ///     and associated with the string key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        int GetInteger(string key);

        /// <summary>
        ///     Decodes and returns a string value that was previously encoded
        ///     and associated with the string key.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        string GetString(string key);
    }
}
