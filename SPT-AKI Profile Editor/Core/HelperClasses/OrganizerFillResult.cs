namespace SPT_AKI_Profile_Editor.Core.HelperClasses
{
    /// <param name="AddedItems">Candidates that were successfully placed.</param>
    /// <param name="AddedContainers">New organizers created in the stash.</param>
    /// <param name="SkippedItems">Candidates that could not be placed, e.g. the stash had no room
    /// for another organizer. Reported so that a partial fill is never presented as a complete one.</param>
    public record OrganizerFillResult(int AddedItems, int AddedContainers, int SkippedItems = 0);
}