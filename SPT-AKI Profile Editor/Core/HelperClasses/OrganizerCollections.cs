using SPT_AKI_Profile_Editor.Core.ServerClasses;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.HelperClasses
{
    public static class OrganizerCollections
    {
        public const string KeyOrganizerTpl = "59fafd4b86f7745ca07e1232";
        public const string KeycardOrganizerTpl = "619cbf9e0a7c3a1a2731940a";

        public static readonly List<string> BlockedKeys =
        [
            "5a043f2c86f7741aa57b5145",
            "5751916f24597720a27126df",
            "57518f7724597720a31c09ab",
            "57518fd424597720c85dbaaa",
            "590de4a286f77423d9312a32"
        ];

        public static readonly IReadOnlyDictionary<string, OrganizerCollectionInfo> Collections =
            new Dictionary<string, OrganizerCollectionInfo>
            {
                [KeyOrganizerTpl] = new(KeyOrganizerTpl, BlockedKeys, "container_add_all_keys"),
                [KeycardOrganizerTpl] = new(KeycardOrganizerTpl, BlockedKeys, "container_add_all_keycards")
            };

        public static OrganizerCollectionInfo GetByOrganizerTpl(string organizerTpl)
            => Collections.TryGetValue(organizerTpl, out OrganizerCollectionInfo info) ? info : null;
    }

    public class OrganizerCollectionInfo
    {
        public OrganizerCollectionInfo(string organizerTpl, IEnumerable<string> blockedItems, string buttonTextKey)
        {
            OrganizerTpl = organizerTpl;
            BlockedItems = blockedItems;
            ButtonTextKey = buttonTextKey;
        }

        public string OrganizerTpl { get; }

        public IEnumerable<string> BlockedItems { get; }

        public string ButtonTextKey { get; }

        public IEnumerable<TarkovItem> GetCandidates()
        {
            if (!AppData.ServerDatabase.ItemsDB.TryGetValue(OrganizerTpl, out TarkovItem containerTemplate))
                return [];
            return AppData.ServerDatabase.ItemsDB.Values
                .Where(x => x.Type == "Item"
                    && x.CanBeAddedToContainer(containerTemplate)
                    && !BlockedItems.Contains(x.Id))
                .OrderBy(x => x.Id);
        }
    }
}