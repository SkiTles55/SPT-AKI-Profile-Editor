using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Views
{
    class QuestsTabViewModel : INotifyPropertyChanged
    {
        public static AppLocalization AppLocalization => AppData.AppLocalization;
        public static Profile Profile => AppData.Profile;
        public static ServerDatabase ServerDatabase => AppData.ServerDatabase;
        public static string SetAllValue { get; set; } = "Success";
        public string TraderFilter { get; set; }
        public static RelayCommand SetAllCommand => new(obj =>
        {
            if (Profile.Characters?.Pmc?.Quests == null)
                return;
            foreach (var quest in Profile.Characters.Pmc.Quests)
                quest.Status = SetAllValue;
        });
        public static List<string> QuestStatuses => new() { "Locked", "AvailableForStart", "Started", "Fail", "AvailableForFinish", "Success" };

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}