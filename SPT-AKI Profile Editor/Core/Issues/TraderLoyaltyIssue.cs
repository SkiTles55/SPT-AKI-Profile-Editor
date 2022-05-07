using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;

namespace SPT_AKI_Profile_Editor.Core.Issues
{
    public class TraderLoyaltyIssue : ProfileIssue
    {
        private readonly CharacterTraderStandingExtended _targetTrader;
        private readonly int _requiredLoyaltyLevel;

        public TraderLoyaltyIssue(CharacterQuest quest, CharacterTraderStandingExtended targetTrader, int requiredLoyaltyLevel) : base(quest.Qid)
        {
            _targetTrader = targetTrader;
            _requiredLoyaltyLevel = requiredLoyaltyLevel;
            Description = AppData.AppLocalization.GetLocalizedString("profile_issues_trader_loyalty_issue_quest",
                                                                     quest.LocalizedQuestName,
                                                                     quest.LocalizedTraderName,
                                                                     targetTrader.LocalizedName,
                                                                     requiredLoyaltyLevel.ToString());
        }

        public override Action FixAction => () => { _targetTrader.LoyaltyLevel = _requiredLoyaltyLevel; };

        public override string Description { get; }
    }
}