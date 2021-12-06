using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Views
{
    class QuestsTabViewModel : BindableViewModel
    {
        public static string SetAllValue { get; set; } = "Success";
        public string TraderFilter { get; set; }
        public static RelayCommand SetAllCommand => new(obj =>
        {
            if (Profile.Characters?.Pmc?.Quests == null)
                return;
            Profile.Characters.Pmc.SetAllQuests(SetAllValue);
        });
        public RelayCommand OpenSettingsCommand => new(async obj =>
        {
            await Dialogs.ShowSettingsDialog(this, 1);
        });
        public static List<string> QuestStatuses => new() { "Locked", "AvailableForStart", "Started", "Fail", "AvailableForFinish", "Success" };
    }
}