using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;

namespace SPT_AKI_Profile_Editor.Core.Issues
{
    public class TraderStandingIssue : ProfileIssue
    {
        private readonly CharacterTraderStandingExtended _targetTrader;
        private readonly float _requiredStanding;

        public TraderStandingIssue(CharacterQuest quest, CharacterTraderStandingExtended targetTrader, float requiredStanding) : base(quest.Qid)
        {
            _targetTrader = targetTrader;
            _requiredStanding = requiredStanding;
            Description = AppData.AppLocalization.GetLocalizedString("profile_issues_trader_standing_issue_quest",
                                                                     quest.LocalizedQuestName,
                                                                     quest.LocalizedTraderName,
                                                                     targetTrader.LocalizedName,
                                                                     requiredStanding.ToString("N2"));
        }

        public override Action FixAction => () => { _targetTrader.TraderStanding.Standing = _requiredStanding; };

        public override string Description { get; }
    }
}