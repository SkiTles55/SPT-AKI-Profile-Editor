using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;

namespace SPT_AKI_Profile_Editor.Core.Issues
{
    public class TraderLoyaltyIssue(CharacterQuest quest, CharacterTraderStandingExtended targetTrader, int requiredLoyaltyLevel) : ProfileIssue(quest.Qid)
    {
        public override Action FixAction => () => { targetTrader.LoyaltyLevel = requiredLoyaltyLevel; };

        public override string Description { get; } = AppData.AppLocalization.GetLocalizedString("profile_issues_trader_loyalty_issue_quest",
            quest.LocalizedQuestName,
            quest.LocalizedTraderName,
            targetTrader.LocalizedName,
            requiredLoyaltyLevel.ToString());
    }
}