using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterCustomization : BindableEntity
    {
        private string head;

        [JsonProperty("Head")]
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