using SPT_AKI_Profile_Editor.Core.Issues;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core
{
    public class IssuesService : BindableEntity
    {
        private ObservableCollection<ProfileIssue> profileIssues = new();

        public ObservableCollection<ProfileIssue> ProfileIssues { get => profileIssues; set => profileIssues = value; }

        public bool HasIssues => ProfileIssues != null && ProfileIssues.Count > 0;

        public void GetIssues()
        {
            profileIssues.Clear();
            GetDuplicateItemsIDIssues(AppData.Profile?.Characters?.Pmc?.Inventory);
            GetDuplicateItemsIDIssues(AppData.Profile?.Characters?.Scav?.Inventory);
            GetTraderIssues(AppData.Profile?.Characters?.Pmc);
            UpdateIssues();
        }

        private void GetDuplicateItemsIDIssues(CharacterInventory inventory)
        {
            if (inventory?.InventoryHaveDuplicatedItems ?? false)
                profileIssues.Add(new DuplicateItemsIDIssue(inventory));
        }

        public void UpdateIssues()
        {
            OnPropertyChanged("ProfileIssues");
            OnPropertyChanged("HasIssues");
        }

        public void FixAllIssues()
        {
            while (profileIssues.Count > 0)
            {
                profileIssues[0].FixAction();
                GetIssues();
            }
        }

        private void GetTraderIssues(Character character)
        {
            var teadersWithIssues = character?.TraderStandingsExt?.Where(x => x.TraderBase?.LoyaltyLevels[x.LoyaltyLevel - 1].MinLevel > character?.Info?.Level);
            foreach (var trader in teadersWithIssues)
                profileIssues.Add(new PMCLevelIssue(trader));
        }
    }
}