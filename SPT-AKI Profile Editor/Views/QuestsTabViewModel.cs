using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Views
{
    class QuestsTabViewModel : INotifyPropertyChanged
    {
        public static AppLocalization AppLocalization => AppData.AppLocalization;
        public static Profile Profile => AppData.Profile;
        public static GridFilters GridFilters => AppData.GridFilters;
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}