using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;

namespace SPT_AKI_Profile_Editor.Core.Issues
{
    public class QuestStatusIssue : ProfileIssue
    {
        private readonly CharacterQuest _targetQuest;

        public QuestStatusIssue(CharacterQuest quest, CharacterQuest targetQuest, QuestStatus requiredStatus) : base(quest.Qid)
        {
            _targetQuest = targetQuest;
            RequiredQuestStatus = requiredStatus;
            Description = AppData.AppLocalization.GetLocalizedString("profile_issues_quest_level_issue_quest",
                                                                     quest.LocalizedQuestName,
                                                                     quest.LocalizedTraderName,
                                                                     targetQuest.LocalizedQuestName,
                                                                     targetQuest.LocalizedTraderName,
                                                                     RequiredQuestStatus.ToString());
        }

        public QuestStatus RequiredQuestStatus { get; }

        public override Action FixAction => () => { _targetQuest.Status = RequiredQuestStatus; };

        public override string Description { get; }
    }
}