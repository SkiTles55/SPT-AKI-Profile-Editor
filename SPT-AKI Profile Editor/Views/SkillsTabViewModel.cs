using SPT_AKI_Profile_Editor.Helpers;
using System;

namespace SPT_AKI_Profile_Editor.Views
{
    public class SkillsTabViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly RelayCommand _reloadCommand;
        private readonly RelayCommand _faqCommand;
        private readonly IWorker _worker;
        private float setAllPmcSkillsValue;
        private float setAllScavSkillsValue;

        public SkillsTabViewModel(IDialogManager dialogManager,
                                  RelayCommand reloadCommand,
                                  RelayCommand faqCommand,
                                  IWorker worker)
        {
            _dialogManager = dialogManager;
            _reloadCommand = reloadCommand;
            _faqCommand = faqCommand;
            _worker = worker;
        }

        public virtual float MaxSkillsValue { get; }

        public float SetAllPmcSkillsValue
        {
            get => setAllPmcSkillsValue;
            set
            {
                setAllPmcSkillsValue = Math.Min(MaxSkillsValue, value);
                OnPropertyChanged("SetAllPmcSkillsValue");
            }
        }

        public float SetAllScavSkillsValue
        {
            get => setAllScavSkillsValue;
            set
            {
                setAllScavSkillsValue = Math.Min(MaxSkillsValue, value);
                OnPropertyChanged("SetAllScavSkillsValue");
            }
        }

        public virtual RelayCommand SetAllPmsSkillsCommand { get; }
        public virtual RelayCommand SetAllScavSkillsCommand { get; }

        public RelayCommand OpenSettingsCommand
            => new(async obj => await _dialogManager.ShowSettingsDialog(_reloadCommand,
                                                                        _faqCommand,
                                                                        _worker,
                                                                        1));
    }
}