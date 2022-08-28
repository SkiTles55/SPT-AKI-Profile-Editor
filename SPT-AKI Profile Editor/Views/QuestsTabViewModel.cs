using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class QuestsTabViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly RelayCommand _reloadCommand;
        private readonly RelayCommand _faqCommand;

        public QuestsTabViewModel(IDialogManager dialogManager, RelayCommand reloadCommand, RelayCommand faqCommand)
        {
            _dialogManager = dialogManager;
            _reloadCommand = reloadCommand;
            _faqCommand = faqCommand;
        }

        public static QuestStatus SetAllValue { get; set; } = QuestStatus.Success;

        public static RelayCommand SetAllCommand => new(obj => Profile.Characters?.Pmc?.SetAllQuests(SetAllValue));

        public RelayCommand OpenSettingsCommand => new(async obj => await _dialogManager.ShowSettingsDialog(_reloadCommand, _faqCommand, 1));
    }
}