namespace SPT_AKI_Profile_Editor.Core
{
    public class GridFilters : BindableEntity
    {
        private QuestsTab questsTab;

        private HideoutTab hideoutTab;

        private SkillsTab skillsTab;

        private MasteringTab masteringTab;

        private string examinedItemsFilter;

        private string clothingNameFilter;

        private StashTab stashTab;

        public GridFilters()
        {
            QuestsTab = new();
            HideoutTab = new();
            SkillsTab = new();
            MasteringTab = new();
            StashTab = new();
        }

        public QuestsTab QuestsTab
        {
            get => questsTab;
            set
            {
                questsTab = value;
                OnPropertyChanged("QuestsTab");
            }
        }

        public HideoutTab HideoutTab
        {
            get => hideoutTab;
            set
            {
                hideoutTab = value;
                OnPropertyChanged("HideoutTab");
            }
        }

        public SkillsTab SkillsTab
        {
            get => skillsTab;
            set
            {
                skillsTab = value;
                OnPropertyChanged("SkillsTab");
            }
        }

        public MasteringTab MasteringTab
        {
            get => masteringTab;
            set
            {
                masteringTab = value;
                OnPropertyChanged("MasteringTab");
            }
        }

        public string ExaminedItemsFilter
        {
            get => examinedItemsFilter;
            set
            {
                examinedItemsFilter = value;
                OnPropertyChanged("ExaminedItemsFilter");
            }
        }

        public string ClothingNameFilter
        {
            get => clothingNameFilter;
            set
            {
                clothingNameFilter = value;
                OnPropertyChanged("ClothingNameFilter");
            }
        }

        public StashTab StashTab
        {
            get => stashTab;
            set
            {
                stashTab = value;
                OnPropertyChanged("StashTab");
            }
        }

        public void Clear()
        {
            QuestsTab = new();
            HideoutTab = new();
            SkillsTab = new();
            MasteringTab = new();
            StashTab = new();
            ExaminedItemsFilter = string.Empty;
            ClothingNameFilter = string.Empty;
        }
    }
}