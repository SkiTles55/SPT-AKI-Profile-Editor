using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterCustomization : BindableEntity
    {
        private string head;
        private string voice;

        public string Head
        {
            get => head;
            set
            {
                head = value;
                OnPropertyChanged(nameof(Head));
            }
        }

        public string Voice
        {
            get => voice;
            set
            {
                voice = value;
                OnPropertyChanged(nameof(Voice));
            }
        }
    }
}