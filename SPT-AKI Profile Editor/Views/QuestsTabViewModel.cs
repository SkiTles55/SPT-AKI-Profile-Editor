using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class QuestsTabViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly RelayCommand _reloadCommand;
        private readonly RelayCommand _faqCommand;
        private readonly IWorker worker;
        private readonly IHelperModManager _helperModManager;

        public QuestsTabViewModel(IDialogManager dialogManager,
                                  RelayCommand reloadCommand,
                                  RelayCommand faqCommand,
                                  IWorker worker,
                                  IHelperModManager helperModManager)
        {
            _dialogManager = dialogManager;
            _reloadCommand = reloadCommand;
            _faqCommand = faqCommand;
            this.worker = worker;
            _helperModManager = helperModManager;
        }

        public static QuestStatus SetAllValue { get; set; } = QuestStatus.Success;

        public RelayCommand RemoveQuestCommand => new(obj =>
        {
            if (obj is string qid)
                RemoveQuest(qid);
        });

        public static RelayCommand SetAllCommand
            => new(obj => Profile?.Characters?.Pmc?.SetAllQuests(SetAllValue));

        public RelayCommand OpenSettingsCommand
            => new(async obj => await _dialogManager.ShowSettingsDialog(_reloadCommand,
                                                                        _faqCommand,
                                                                        worker,
                                                                        _helperModManager,
                                                                        1));

        private async void RemoveQuest(string qid)
        {
            if (await _dialogManager.YesNoDialog("remove_quest_title", "remove_quest_caption"))
                Profile?.Characters?.Pmc?.RemoveQuests(new string[] { qid });
        }
    }
}