﻿using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
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
                                  IHelperModManager helperModManager)
        {
            _dialogManager = dialogManager;
            _reloadCommand = reloadCommand;
            _faqCommand = faqCommand;
            this.worker = worker;
            _helperModManager = helperModManager;
        }

        public static QuestStatus SetAllValue { get; set; } = QuestStatus.Success;

        public static RelayCommand SetAllCommand
            => new(obj => Profile?.Characters?.Pmc?.SetAllQuests(SetAllValue));

        public static RelayCommand AddMissingQuests => new(_ => AddMissingQuest(false));

        public static RelayCommand AddMissingEventQuests => new(_ => AddMissingQuest(true));

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

            HasMissingQuests = Profile.Characters.Pmc.MissingQuests(false).Any();
            HasMissingEventQuests = Profile.Characters.Pmc.MissingQuests(true).Any();

            QuestsCollection = filteredQuests;
        }

        private static void AddMissingQuest(bool isEvent)
            => Profile.Characters.Pmc.AddAllMisingQuests(isEvent);

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