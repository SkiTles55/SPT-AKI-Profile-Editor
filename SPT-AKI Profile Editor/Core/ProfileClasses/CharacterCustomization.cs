using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterCustomization : BindableEntity
    {
        private string head;

        public string Head
        {
            get => head;
            set
            {
                head = value;
                OnPropertyChanged("Head");
            }
        }
    }
}