using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterCustomization : TemplateableEntity
    {
        private string head;

        [JsonProperty("Head")]
        public string Head
        {
            get => head;
            set => SetProperty(nameof(Head), ref head, value);
        }

        [JsonIgnore]
        public bool IsHeadChanged => changedValues.ContainsKey(nameof(Head));

        public override string TemplateEntityId => "Customization";

        public override string TemplateLocalizedName => "Customization";
    }
}