using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;

namespace SPT_AKI_Profile_Editor.Core.Issues
{
    public class PMCLevelIssue : ProfileIssue
    {
        public PMCLevelIssue(CharacterTraderStandingExtended trader) : base(trader.Id)
        {
            RequiredLevel = trader.TraderBase.LoyaltyLevels[trader.LoyaltyLevel - 1].MinLevel;
            Description = AppData.AppLocalization.GetLocalizedString("profile_issues_pmc_level_issue_trader",
                                                                     trader.LocalizedName,
                                                                     RequiredLevel.ToString());
        }

        public PMCLevelIssue(CharacterQuest quest, int requiredLevel) : base(quest.Qid)
        {
            RequiredLevel = requiredLevel;
            Description = AppData.AppLocalization.GetLocalizedString("profile_issues_pmc_level_issue_quest",
                                                                     quest.LocalizedQuestName,
                                                                     quest.LocalizedTraderName,
                                                                     RequiredLevel.ToString());
        }

        public int RequiredLevel { get; }

        public override Action FixAction => () => { AppData.Profile.Characters.Pmc.Info.Level = RequiredLevel; };

        public override string Description { get; }
    }
}