using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterHealth : BindableEntity
    {
        private CharacterMetric hydration;
        private CharacterMetric energy;
        private CharacterBodyParts bodyParts;

        [JsonProperty("Hydration")]
        public CharacterMetric Hydration
        {
            get => hydration;
            set
            {
                hydration = value;
                OnPropertyChanged("Hydration");
            }
        }

        [JsonProperty("Energy")]
        public CharacterMetric Energy
        {
            get => energy;
            set
            {
                energy = value;
                OnPropertyChanged("Energy");
            }
        }

        [JsonProperty("BodyParts")]
        public CharacterBodyParts BodyParts
        {
            get => bodyParts;
            set
            {
                bodyParts = value;
                OnPropertyChanged("BodyParts");
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

        [JsonProperty("Head")]
        public WrappedCharacterMetric Head
        {
            get => head;
            set
            {
                head = value;
                OnPropertyChanged("Head");
            }
        }

        [JsonProperty("Chest")]
        public WrappedCharacterMetric Chest
        {
            get => chest;
            set
            {
                chest = value;
                OnPropertyChanged("Chest");
            }
        }

        [JsonProperty("Stomach")]
        public WrappedCharacterMetric Stomach
        {
            get => stomach;
            set
            {
                stomach = value;
                OnPropertyChanged("Stomach");
            }
        }

        [JsonProperty("LeftArm")]
        public WrappedCharacterMetric LeftArm
        {
            get => leftArm;
            set
            {
                leftArm = value;
                OnPropertyChanged("LeftArm");
            }
        }

        [JsonProperty("RightArm")]
        public WrappedCharacterMetric RightArm
        {
            get => rightArm;
            set
            {
                rightArm = value;
                OnPropertyChanged("RightArm");
            }
        }

        [JsonProperty("LeftLeg")]
        public WrappedCharacterMetric LeftLeg
        {
            get => leftLeg;
            set
            {
                leftLeg = value;
                OnPropertyChanged("LeftLeg");
            }
        }

        [JsonProperty("RightLeg")]
        public WrappedCharacterMetric RightLeg
        {
            get => rightLeg;
            set
            {
                rightLeg = value;
                OnPropertyChanged("RightLeg");
            }
        }

        public class WrappedCharacterMetric : BindableEntity
        {
            private CharacterMetric health;

            [JsonProperty("Health")]
            public CharacterMetric Health
            {
                get => health;
                set
                {
                    health = value;
                    OnPropertyChanged("Health");
                }
            }
        }
    }
}