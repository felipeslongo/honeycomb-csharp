using HoneyComb.Core.Vault;

namespace HoneyComb.UI
{
    /// <summary>
    ///     Represents the Id of this Intent.
    ///     Used to identify the purpose of intent in this UI component
    ///     in order to resume its workflow during a restoration process.
    /// </summary>
    public sealed class IntentId : IRestorableUIState
    {
        public IntentId(string id)
        {
            Value = id;
        }

        public static IntentId Empty { get; } = new IntentId(string.Empty);
        public string RestorationIdentifier { get; set; } = typeof(IntentId).FullName;
        public string Value { get; private set; } = string.Empty;

        public static implicit operator IntentId(string intentId) => new IntentId(intentId);

        public static implicit operator string(IntentId intentId) => intentId.ToString();

        public void OnPreservation(IBundleCoder savedInstanceState)
        {
            if (string.IsNullOrWhiteSpace(RestorationIdentifier))
                return;

            savedInstanceState.Add(RestorationIdentifier!, Value);
        }

        public void OnRestoration(IBundleCoder savedInstanceState)
        {
            if (string.IsNullOrWhiteSpace(RestorationIdentifier))
                return;

            Value = savedInstanceState.GetString(RestorationIdentifier!);
        }

        public override string ToString() => Value;
    }
}
