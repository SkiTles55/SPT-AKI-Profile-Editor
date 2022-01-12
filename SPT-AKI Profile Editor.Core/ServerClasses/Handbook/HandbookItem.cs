using System.Linq;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class HandbookItem
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("ParentId")]
        public string ParentId { get; set; }

        [JsonIgnore]
        public TarkovItem Item =>
            AppData.ServerDatabase?.ItemsDB?
            .Where(x => x.Key == Id).FirstOrDefault()
            .Value;
    }
}