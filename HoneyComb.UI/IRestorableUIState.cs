using HoneyComb.Core.Vault;

namespace HoneyComb.UI
{
    /// <summary>
    ///     Type that is aware and conforms to the process of State Preservation and Restoration.
    ///     It can survive process death preserving its state before death and restoring it after.
    /// </summary>
    /// <remarks>
    ///     Based on both Android and iOS approach to recreating UI state upon process death.
    ///         Android's OnSaveInstanceState and OnRestoreInstanceState
    /// </remarks>
    public interface IRestorableUIState
    {
        /// <summary>
        ///     This property indicates whether the view controller and its contents should be preserved 
        ///     and is used to identify the view and it´s contents during the restoration process. 
        ///     
        ///     The value of this property is nil by default, which indicates that the view should not be saved. 
        ///     Assigning a string object to the property lets the system know that the view should be saved. 
        ///     In addition, the contents of the string are your way to identify the purpose of the view controller. 
        /// </summary>
        /// <remarks>
        ///     Based on iOS https://developer.apple.com/documentation/uikit/uiviewcontroller/1621499-restorationidentifier
        /// </remarks>
        string? RestorationIdentifier { get;}

        /// <summary>
        ///     Encodes UI state-related information for the view.
        ///     Call to give your view a chance to save state-related information.
        ///     
        ///     When deciding what data to save, write the smallest amount of data needed to 
        ///     restore the view controller to its current configuration. 
        ///     
        ///     The information you save should be data that you could not easily recreate, 
        ///     such as the user’s current selection.
        /// </summary>
        /// <param name="savedInstanceState">    The coder object to use to encode the state of the view.</param>
        void OnPreservation(IBundleCoder savedInstanceState);

        /// <summary>
        ///     Decodes and restores UI state-related information for the view.
        ///     
        ///     If your app supports state restoration, your implementation of this method should use 
        ///     any saved state information to restore the view controller to its previous configuration
        /// </summary>
        /// <param name="savedInstanceState">The coder object to use to decode the state of the view.</param>
        void OnRestoration(IBundleCoder savedInstanceState);
    }
}
