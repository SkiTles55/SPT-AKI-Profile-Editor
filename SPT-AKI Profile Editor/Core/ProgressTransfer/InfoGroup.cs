namespace SPT_AKI_Profile_Editor.Core.ProgressTransfer
{
    public class InfoGroup : Group
    {
        private bool nickname = true;
        private bool side = true;
        private bool voice = true;
        private bool experience = true;
        private bool head = true;
        private bool pockets = true;
        private bool health = true;

        public InfoGroup(bool sideEnabled)
        {
            SideEnabled = sideEnabled;
            if (!sideEnabled)
                Side = false;
        }

        public bool SideEnabled { get; }

        public override bool? GroupState
        {
            get => FullState ? true : NotFullState;
            set
            {
                var updatedValue = value ?? false;
                Nickname = updatedValue;
                if (SideEnabled)
                    Side = updatedValue;
                Voice = updatedValue;
                Experience = updatedValue;
                Head = updatedValue;
                Pockets = updatedValue;
                Health = updatedValue;
                NotifyGroupStateChanged();
            }
        }

        public bool Nickname
        {
            get => nickname;
            set
            {
                nickname = value;
                OnPropertyChanged(nameof(Nickname));
                NotifyGroupStateChanged();
            }
        }

        public bool Side
        {
            get => side;
            set
            {
                side = value;
                OnPropertyChanged(nameof(Side));
                NotifyGroupStateChanged();
            }
        }

        public bool Voice
        {
            get => voice;
            set
            {
                voice = value;
                OnPropertyChanged(nameof(Voice));
                NotifyGroupStateChanged();
            }
        }

        public bool Experience
        {
            get => experience;
            set
            {
                experience = value;
                OnPropertyChanged(nameof(Experience));
                NotifyGroupStateChanged();
            }
        }

        public bool Head
        {
            get => head;
            set
            {
                head = value;
                OnPropertyChanged(nameof(Head));
                NotifyGroupStateChanged();
            }
        }

        public bool Pockets
        {
            get => pockets;
            set
            {
                pockets = value;
                OnPropertyChanged(nameof(Pockets));
                NotifyGroupStateChanged();
            }
        }

        public bool Health
        {
            get => health;
            set
            {
                health = value;
                OnPropertyChanged(nameof(Health));
                NotifyGroupStateChanged();
            }
        }

        internal override bool FullState
            => nickname && (!SideEnabled || side) && voice && experience && head && pockets && health;

        internal override bool? NotFullState
            => nickname || (SideEnabled && side) || voice || experience || head || pockets || health ? null : false;
    }
}