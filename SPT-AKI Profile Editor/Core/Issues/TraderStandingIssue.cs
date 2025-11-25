using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;

namespace SPT_AKI_Profile_Editor.Core.Issues
{
    public class TraderStandingIssue(CharacterQuest quest, CharacterTraderStandingExtended targetTrader, float requiredStanding) : ProfileIssue(quest.Qid)
    {
        public override Action FixAction => () => { targetTrader.TraderStanding.Standing = requiredStanding; };

        public override string Description { get; } = AppData.AppLocalization.GetLocalizedString("profile_issues_trader_standing_issue_quest",
            quest.LocalizedQuestName,
            quest.LocalizedTraderName,
            targetTrader.LocalizedName,
            requiredStanding.ToString("N2"));
    }
}