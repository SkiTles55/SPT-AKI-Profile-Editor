using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Core.ProgressTransfer
{
    public class SettingsModel : BindableEntity
    {
        private bool merchants = true;
        private bool quests = true;
        private bool hideout = true;
        private bool crafts = true;
        private bool examinedItems = true;
        private bool clothing = true;

        public SettingsModel()
        {
            Info.GroupStateChanged = new(_ => NotifySelectableUpdate());
            Skills.GroupStateChanged = new(_ => NotifySelectableUpdate());
            Masterings.GroupStateChanged = new(_ => NotifySelectableUpdate());
            Builds.GroupStateChanged = new(_ => NotifySelectableUpdate());
        }

        public Info Info { get; } = new();

        public bool Merchants
        {
            get => merchants;
            set
            {
                merchants = value;
                OnPropertyChanged(nameof(Merchants));
                NotifySelectableUpdate();
            }
        }

        public bool Quests
        {
            get => quests;
            set
            {
                quests = value;
                OnPropertyChanged(nameof(Quests));
                NotifySelectableUpdate();
            }
        }

        public bool Hideout
        {
            get => hideout;
            set
            {
                hideout = value;
                OnPropertyChanged(nameof(Hideout));
                NotifySelectableUpdate();
            }
        }

        public bool Crafts
        {
            get => crafts;
            set
            {
                crafts = value;
                OnPropertyChanged(nameof(Crafts));
                NotifySelectableUpdate();
            }
        }

        public bool ExaminedItems
        {
            get => examinedItems;
            set
            {
                examinedItems = value;
                OnPropertyChanged(nameof(ExaminedItems));
                NotifySelectableUpdate();
            }
        }

        public bool Clothing
        {
            get => clothing;
            set
            {
                clothing = value;
                OnPropertyChanged(nameof(Clothing));
                NotifySelectableUpdate();
            }
        }

        public bool CanSelectAll
            => Info.GroupState != true
            || !merchants
            || !quests
            || !hideout
            || !crafts
            || !examinedItems
            || !clothing
            || Skills.GroupState != true
            || Masterings.GroupState != true
            || Builds.GroupState != true;

        public bool CanDeselectAny
            => Info.GroupState != false
            || merchants
            || quests
            || hideout
            || crafts
            || examinedItems
            || clothing
            || Skills.GroupState != false
            || Masterings.GroupState != false
            || Builds.GroupState != false;

        public SkillGroup Skills { get; } = new();

        public SkillGroup Masterings { get; } = new();

        public Builds Builds { get; } = new();

        internal void ChangeAll(bool value)
        {
            Info.GroupState = value;
            Merchants = value;
            Quests = value;
            Hideout = value;
            Crafts = value;
            ExaminedItems = value;
            Clothing = value;
            Skills.GroupState = value;
            Masterings.GroupState = value;
            Builds.GroupState = value;
        }

        private void NotifySelectableUpdate()
        {
            OnPropertyChanged(nameof(CanSelectAll));
            OnPropertyChanged(nameof(CanDeselectAny));
        }
    }
}