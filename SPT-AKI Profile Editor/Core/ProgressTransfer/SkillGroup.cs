namespace SPT_AKI_Profile_Editor.Core.ProgressTransfer
{
    public class SkillGroup : Group
    {
        private bool pmc = true;
        private bool scav = true;

        public override bool? GroupState
        {
            get => FullState ? true : NotFullState;
            set
            {
                var updatedValue = value ?? false;
                Pmc = updatedValue;
                Scav = updatedValue;
                NotifyGroupStateChanged();
            }
        }

        public bool Pmc
        {
            get => pmc;
            set
            {
                pmc = value;
                OnPropertyChanged(nameof(Pmc));
                NotifyGroupStateChanged();
            }
        }

        public bool Scav
        {
            get => scav;
            set
            {
                scav = value;
                OnPropertyChanged(nameof(Scav));
                NotifyGroupStateChanged();
            }
        }

        internal override bool FullState => pmc && scav;

        internal override bool? NotFullState => pmc || scav ? null : false;
    }
}