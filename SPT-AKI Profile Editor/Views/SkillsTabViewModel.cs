using SPT_AKI_Profile_Editor.Helpers;
using System;

namespace SPT_AKI_Profile_Editor.Views
{
    public class SkillsTabViewModel(IDialogManager dialogManager,
        RelayCommand reloadCommand,
        RelayCommand faqCommand,
        IWorker worker,
        IHelperModManager helperModManager) : BindableViewModel
    {
        private float setAllPmcSkillsValue;
        private float setAllScavSkillsValue;

        public virtual float MaxSkillsValue { get; }

        public float SetAllPmcSkillsValue
        {
            get => setAllPmcSkillsValue;
            set
            {
                setAllPmcSkillsValue = Math.Min(MaxSkillsValue, value);
                OnPropertyChanged(nameof(SetAllPmcSkillsValue));
            }
        }

        public float SetAllScavSkillsValue
        {
            get => setAllScavSkillsValue;
            set
            {
                setAllScavSkillsValue = Math.Min(MaxSkillsValue, value);
                OnPropertyChanged(nameof(SetAllScavSkillsValue));
            }
        }

        public virtual RelayCommand SetAllPmsSkillsCommand { get; }
        public virtual RelayCommand SetAllScavSkillsCommand { get; }

        public RelayCommand OpenSettingsCommand
            => new(async obj => await dialogManager.ShowSettingsDialog(reloadCommand,
                                                                       faqCommand,
                                                                       worker,
                                                                       helperModManager,
                                                                       1));
    }
}