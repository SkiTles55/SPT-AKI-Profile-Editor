using MahApps.Metro.IconPacks;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class MoneyDailogViewModelTests
    {
        [OneTimeSetUp]
        public void Setup() => TestHelpers.LoadDatabase();

        [Test]
        public void CanInitializeForRoubles()
            => InitializeAndCheck(AppData.AppSettings.MoneysRublesTpl,
                                  PackIconFontAwesomeKind.RubleSignSolid);

        [Test]
        public void CanInitializeForDollars()
            => InitializeAndCheck(AppData.AppSettings.MoneysDollarsTpl,
                                  PackIconFontAwesomeKind.DollarSignSolid);

        [Test]
        public void CanInitializeForEuros()
            => InitializeAndCheck(AppData.AppSettings.MoneysEurosTpl,
                                  PackIconFontAwesomeKind.EuroSignSolid);

        [Test]
        public void CanInitializeForWrongItem()
            => InitializeAndCheck(AppData.ServerDatabase.ItemsDB.Keys.First(),
                                  PackIconFontAwesomeKind.TriangleExclamationSolid,
                                  PackIconFontAwesomeKind.EuroSignSolid);

        [Test]
        public void HasNeededData() => Assert.That(MoneyDailogViewModel.AppSettings, Is.Not.Null);

        [Test]
        public void CanExecuteAddMoneysCommand()
        {
            var addMoneysCommandExecuted = false;
            var roubles = AppData.ServerDatabase.ItemsDB[AppData.AppSettings.MoneysRublesTpl];
            MoneyDailogViewModel viewModel = new(roubles, new((_) => addMoneysCommandExecuted = true), null);
            viewModel.AddMoneysCommand.Execute(null);
            Assert.That(addMoneysCommandExecuted, Is.True);
        }

        private static void InitializeAndCheck(string moneyTpl,
                                               PackIconFontAwesomeKind icon,
                                               PackIconFontAwesomeKind wrongIcon = PackIconFontAwesomeKind.TriangleExclamationSolid)
        {
            var money = AppData.ServerDatabase.ItemsDB[moneyTpl];
            MoneyDailogViewModel viewModel = new(money, null, null);
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.AddMoneysCommand, Is.Not.Null);
            Assert.That(viewModel.Сurrency, Is.EqualTo(icon));
            Assert.That(viewModel.Сurrency, Is.Not.EqualTo(wrongIcon));
            Assert.That(viewModel.Moneys, Is.Not.Null);
            Assert.That(viewModel.Moneys.Id, Is.EqualTo(moneyTpl));
            Assert.That(viewModel.AddingInterval, Is.EqualTo(money.Properties.StackMaxSize));
        }
    }
}