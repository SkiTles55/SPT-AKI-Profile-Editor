using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;

namespace SPT_AKI_Profile_Editor.Core.Issues
{
    public class PMCLevelIssue : ProfileIssue
    {
        private readonly int _requiredLevel;

        public PMCLevelIssue(CharacterTraderStandingExtended trader) : base(trader.Id)
        {
            _requiredLevel = trader.TraderBase.LoyaltyLevels[trader.LoyaltyLevel - 1].MinLevel;
            Description = AppData.AppLocalization.GetLocalizedString("profile_issues_pmc_level_issue_trader",
                                                                     trader.LocalizedName,
                                                                     _requiredLevel.ToString());
        }

        public PMCLevelIssue(CharacterQuest quest, int requiredLevel) : base(quest.Qid)
        {
            _requiredLevel = requiredLevel;
            Description = AppData.AppLocalization.GetLocalizedString("profile_issues_pmc_level_issue_quest",
                                                                     quest.LocalizedQuestName,
                                                                     quest.LocalizedTraderName,
                                                                     _requiredLevel.ToString());
        }

        public override Action FixAction => () => { AppData.Profile.Characters.Pmc.Info.Level = _requiredLevel; };

        public override string Description { get; }
    }
}