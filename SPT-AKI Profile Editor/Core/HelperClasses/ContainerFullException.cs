using System;

namespace SPT_AKI_Profile_Editor.Core.HelperClasses
{
    /// <summary>
    /// Thrown when a container has no free slots left for the item being added.
    /// The message is localized because <c>Worker</c> surfaces it to the user verbatim.
    /// Callers that need to react to "container is full" must catch this type instead of
    /// comparing <see cref="Exception.Message"/> against a localized string, which breaks as
    /// soon as the app language or the wording changes.
    /// </summary>
    public class ContainerFullException : Exception
    {
        public ContainerFullException()
            : base(AppData.AppLocalization.GetLocalizedString("tab_stash_no_slots"))
        {
        }
    }
}
