using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class RagfairInfo : BindableEntity
    {
        private float rating;

        [JsonProperty("rating")]
        public float Rating
        {
            get => rating;
            set
            {
                rating = value;
                OnPropertyChanged("Rating");
            }
        }
    }
}