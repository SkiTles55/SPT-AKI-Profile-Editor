using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;

namespace SPT_AKI_Profile_Editor.Core.Issues
{
    public class QuestStatusIssue : ProfileIssue
    {
        private readonly CharacterQuest _targetQuest;
        private readonly QuestStatus _requiredQuestStatus;

        public QuestStatusIssue(CharacterQuest quest, CharacterQuest targetQuest, QuestStatus requiredStatus) : base(quest.Qid)
        {
            _targetQuest = targetQuest;
            _requiredQuestStatus = requiredStatus;
            Description = AppData.AppLocalization.GetLocalizedString("profile_issues_quest_status_issue_quest",
                                                                     quest.LocalizedQuestName,
                                                                     quest.LocalizedTraderName,
                                                                     targetQuest.LocalizedQuestName,
                                                                     targetQuest.LocalizedTraderName,
                                                                     _requiredQuestStatus.ToString());
        }

        public override Action FixAction => () => { _targetQuest.Status = _requiredQuestStatus; };

        public override string Description { get; }
    }
}