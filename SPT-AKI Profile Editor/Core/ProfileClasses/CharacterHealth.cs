using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterHealth : BindableEntity
    {
        private CharacterMetric hydration;
        private CharacterMetric energy;
        private CharacterBodyParts bodyParts;

        public CharacterMetric Hydration
        {
            get => hydration;
            set
            {
                hydration = value;
                OnPropertyChanged(nameof(Hydration));
            }
        }

        public CharacterMetric Energy
        {
            get => energy;
            set
            {
                energy = value;
                OnPropertyChanged(nameof(Energy));
            }
        }

        public CharacterBodyParts BodyParts
        {
            get => bodyParts;
            set
            {
                bodyParts = value;
                OnPropertyChanged(nameof(BodyParts));
            }
        }
    }

    public class CharacterBodyParts : BindableEntity
    {
        private WrappedCharacterMetric head;
        private WrappedCharacterMetric chest;
        private WrappedCharacterMetric stomach;
        private WrappedCharacterMetric leftArm;
        private WrappedCharacterMetric rightArm;
        private WrappedCharacterMetric leftLeg;
        private WrappedCharacterMetric rightLeg;

        public WrappedCharacterMetric Head
        {
            get => head;
            set
            {
                head = value;
                OnPropertyChanged(nameof(Head));
            }
        }

        public WrappedCharacterMetric Chest
        {
            get => chest;
            set
            {
                chest = value;
                OnPropertyChanged(nameof(Chest));
            }
        }

        public WrappedCharacterMetric Stomach
        {
            get => stomach;
            set
            {
                stomach = value;
                OnPropertyChanged(nameof(Stomach));
            }
        }

        public WrappedCharacterMetric LeftArm
        {
            get => leftArm;
            set
            {
                leftArm = value;
                OnPropertyChanged(nameof(LeftArm));
            }
        }

        public WrappedCharacterMetric RightArm
        {
            get => rightArm;
            set
            {
                rightArm = value;
                OnPropertyChanged(nameof(RightArm));
            }
        }

        public WrappedCharacterMetric LeftLeg
        {
            get => leftLeg;
            set
            {
                leftLeg = value;
                OnPropertyChanged(nameof(LeftLeg));
            }
        }

        public WrappedCharacterMetric RightLeg
        {
            get => rightLeg;
            set
            {
                rightLeg = value;
                OnPropertyChanged(nameof(RightLeg));
            }
        }

        public class WrappedCharacterMetric : BindableEntity
        {
            private CharacterMetric health;

            public CharacterMetric Health
            {
                get => health;
                set
                {
                    health = value;
                    OnPropertyChanged(nameof(Health));
                }
            }
        }
    }
}