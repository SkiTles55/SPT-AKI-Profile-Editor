using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterMetric : BindableEntity
    {
        private float current;

        private float maximum;

        public float Current
        {
            get => current;
            set
            {
                current = value;
                OnPropertyChanged(nameof(Current));
                if (value > Maximum)
                    Maximum = value;
            }
        }

        public float Maximum
        {
            get => maximum;
            set
            {
                maximum = value;
                OnPropertyChanged(nameof(Maximum));
            }
        }
    }
}