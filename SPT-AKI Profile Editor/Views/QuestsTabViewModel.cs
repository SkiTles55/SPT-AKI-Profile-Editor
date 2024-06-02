using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Views
{
    public class QuestsTabViewModel : PmcBindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly RelayCommand _reloadCommand;
        private readonly RelayCommand _faqCommand;
        private readonly IWorker worker;
        private readonly IHelperModManager _helperModManager;
        private readonly ServerConfigs serverConfigs;

        private readonly Dictionary<string, bool> questTypesExpander = new();
        private ObservableCollection<CharacterQuest> questsCollection = new();
        private string traderNameFilter;
        private string questNameFilter;
        private string questStatusFilter;
        private bool hasMissingQuests;
        private bool hasMissingEventQuests;

        public QuestsTabViewModel(IDialogManager dialogManager,
                                  RelayCommand reloadCommand,
                                  RelayCommand faqCommand,
                                  IWorker worker,
                                  IHelperModManager helperModManager,
                                  ServerConfigs serverConfigs)
        {
            _dialogManager = dialogManager;
            _reloadCommand = reloadCommand;
            _faqCommand = faqCommand;
            this.worker = worker;
            _helperModManager = helperModManager;
            this.serverConfigs = serverConfigs;
        }

        public static QuestStatus SetAllValue { get; set; } = QuestStatus.Success;

        public static RelayCommand SetAllCommand
            => new(obj => Profile?.Characters?.Pmc?.SetAllQuests(SetAllValue));

        public string TraderNameFilter
        {
            get => traderNameFilter;
            set
            {
                traderNameFilter = value;
                OnPropertyChanged(nameof(TraderNameFilter));
                ApplyFilter();
            }
        }

        public string QuestNameFilter
        {
            get => questNameFilter;
            set
            {
                questNameFilter = value;
                OnPropertyChanged(nameof(QuestNameFilter));
                ApplyFilter();
            }
        }

        public string QuestStatusFilter
        {
            get => questStatusFilter;
            set
            {
                questStatusFilter = value;
                OnPropertyChanged(nameof(QuestStatusFilter));
                ApplyFilter();
            }
        }

        public RelayCommand ExpanderStateChanged => new(obj => UpdateExpanderGroups(obj as RoutedEventArgs));

        public RelayCommand ExpanderLoaded => new(obj => UpdateLoadedExpander(obj as RoutedEventArgs));

        public ObservableCollection<CharacterQuest> QuestsCollection
        {
            get => questsCollection;
            set
            {
                questsCollection = value;
                OnPropertyChanged(nameof(QuestsCollection));
            }
        }

        public RelayCommand RemoveQuestCommand => new(obj =>
        {
            if (obj is string qid)
                RemoveQuest(qid);
        });

        public RelayCommand OpenSettingsCommand
            => new(async obj => await _dialogManager.ShowSettingsDialog(_reloadCommand,
                                                                        _faqCommand,
                                                                        worker,
                                                                        _helperModManager,
                                                                        1));

        public bool HasMissingQuests
        {
            get => hasMissingQuests;
            set
            {
                hasMissingQuests = value;
                OnPropertyChanged(nameof(HasMissingQuests));
            }
        }

        public bool HasMissingEventQuests
        {
            get => hasMissingEventQuests;
            set
            {
                hasMissingEventQuests = value;
                OnPropertyChanged(nameof(HasMissingEventQuests));
            }
        }

        public override void ApplyFilter()
        {
            ObservableCollection<CharacterQuest> filteredQuests;

            if (Profile?.Characters?.Pmc?.Quests == null || !Profile.Characters.Pmc.Quests.Any())
                filteredQuests = new();
            else if (FiltersIsEmpty())
                filteredQuests = new(Profile.Characters.Pmc.Quests);
            else
                filteredQuests = new(Profile.Characters.Pmc.Quests.Where(x => CanShow(x)));

            //var dbQuestQids = ServerDatabase.QuestsData.Select(x => x.Key);
            //var dbQuestsCount = dbQuestQids.Sum(x => GetIntForQuestSum(x, false));
            //var dbEventQuestsCount = dbQuestQids.Sum(x => GetIntForQuestSum(x, true));

            //var questsCount = Profile.Characters.Pmc.Quests?.Sum(x => GetIntForQuestSum(x.Qid, false)) ?? 0;
            //var eventQuestsCount = Profile.Characters.Pmc.Quests?.Sum(x => GetIntForQuestSum(x.Qid, true)) ?? 0;

            //HasMissingQuests = questsCount < dbQuestsCount;
            //HasMissingEventQuests = eventQuestsCount < dbEventQuestsCount;

            QuestsCollection = filteredQuests;
        }

        private int GetIntForQuestSum(string qid, bool asEvent)
            => serverConfigs.Quest.EventQuests.ContainsKey(qid) ? (asEvent ? 0 : 1) : (asEvent ? 1 : 0);

        private async void RemoveQuest(string qid)
        {
            if (await _dialogManager.YesNoDialog("remove_quest_title", "remove_quest_caption"))
                Profile?.Characters?.Pmc?.RemoveQuests(new string[] { qid });
        }

        private void UpdateExpanderGroups(RoutedEventArgs e)
        {
            if (e.Source is not Expander expander || expander.DataContext is not CollectionViewGroup group)
                return;
            var groupName = group.Name.ToString();
            if (!questTypesExpander.ContainsKey(groupName))
                questTypesExpander.Add(groupName, expander.IsExpanded);
            else
                questTypesExpander[groupName] = expander.IsExpanded;
        }

        private void UpdateLoadedExpander(RoutedEventArgs e)
        {
            if (e.Source is not Expander expander || expander.DataContext is not CollectionViewGroup group)
                return;
            var groupName = group.Name.ToString();
            if (!questTypesExpander.ContainsKey(groupName))
                questTypesExpander.Add(groupName, true);
            expander.IsExpanded = questTypesExpander[groupName];
        }

        private bool FiltersIsEmpty() => string.IsNullOrEmpty(QuestNameFilter)
            && string.IsNullOrEmpty(QuestStatusFilter)
            && string.IsNullOrEmpty(TraderNameFilter);

        private bool CanShow(CharacterQuest quest)
        {
            return QuestNameContainsFilterText()
                && QuestTraderNameContainsFilterText()
                && QuestStatusContainsFilterText();

            bool QuestNameContainsFilterText()
            {
                return string.IsNullOrEmpty(QuestNameFilter)
                || quest.LocalizedQuestName.ToUpper().Contains(QuestNameFilter.ToUpper());
            }

            bool QuestTraderNameContainsFilterText()
            {
                return string.IsNullOrEmpty(TraderNameFilter)
                || quest.LocalizedTraderName.ToUpper().Contains(TraderNameFilter.ToUpper());
            }

            bool QuestStatusContainsFilterText()
            {
                return string.IsNullOrEmpty(QuestStatusFilter)
                || quest.Status.ToString().ToUpper().Contains(QuestStatusFilter.ToUpper());
            }
        }
    }
}