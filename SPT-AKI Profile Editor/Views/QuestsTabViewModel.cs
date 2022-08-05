using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    internal class QuestsTabViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;

        public QuestsTabViewModel(IDialogManager dialogManager) => _dialogManager = dialogManager;

        public static QuestStatus SetAllValue { get; set; } = QuestStatus.Success;

        public static RelayCommand SetAllCommand => new(obj =>
        {
            if (Profile.Characters?.Pmc?.Quests == null)
                return;
            Profile.Characters.Pmc.SetAllQuests(SetAllValue);
        });

        public RelayCommand OpenSettingsCommand => new(async obj => await _dialogManager.ShowSettingsDialog(this, 1));
    }
}