namespace SPT_AKI_Profile_Editor.Core.HelperClasses
{
    public class GridFilters : BindableEntity
    {
        private SkillsTab skillsTab;

        private MasteringTab masteringTab;

        private StashTab stashTab;

        private CleaningFromModsTab cleaningFromModsTab;

        public GridFilters()
        {
            SkillsTab = new();
            MasteringTab = new();
            StashTab = new();
            CleaningFromModsTab = new();
        }

        public SkillsTab SkillsTab
        {
            get => skillsTab;
            set
            {
                skillsTab = value;
                OnPropertyChanged(nameof(SkillsTab));
            }
        }

        public MasteringTab MasteringTab
        {
            get => masteringTab;
            set
            {
                masteringTab = value;
                OnPropertyChanged(nameof(MasteringTab));
            }
        }

        public StashTab StashTab
        {
            get => stashTab;
            set
            {
                stashTab = value;
                OnPropertyChanged(nameof(StashTab));
            }
        }

        public CleaningFromModsTab CleaningFromModsTab
        {
            get => cleaningFromModsTab;
            set
            {
                cleaningFromModsTab = value;
                OnPropertyChanged(nameof(CleaningFromModsTab));
            }
        }

        public void Clear()
        {
            SkillsTab = new();
            MasteringTab = new();
            StashTab = new();
            CleaningFromModsTab = new();
        }
    }
}