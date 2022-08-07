using MahApps.Metro.IconPacks;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class MoneyDailogViewModelTests
    {
        [OneTimeSetUp]
        public void Setup() => TestConstants.LoadDatabase();

        [Test]
        public void CanInitializeForRoubles()
        {
            var money = AppData.ServerDatabase.ItemsDB[AppData.AppSettings.MoneysRublesTpl];
            MoneyDailogViewModel viewModel = new(money, null);
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.AddMoneysCommand, Is.Null);
            Assert.That(viewModel.Сurrency, Is.EqualTo(PackIconFontAwesomeKind.RubleSignSolid));
            Assert.That(viewModel.Сurrency, Is.Not.EqualTo(PackIconFontAwesomeKind.ExclamationTriangleSolid));
            Assert.That(viewModel.Moneys, Is.Not.Null);
            Assert.That(viewModel.Moneys.Id, Is.EqualTo(AppData.AppSettings.MoneysRublesTpl));
        }

        [Test]
        public void CanInitializeForDollars()
        {
            var money = AppData.ServerDatabase.ItemsDB[AppData.AppSettings.MoneysDollarsTpl];
            MoneyDailogViewModel viewModel = new(money, null);
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.AddMoneysCommand, Is.Null);
            Assert.That(viewModel.Сurrency, Is.EqualTo(PackIconFontAwesomeKind.DollarSignSolid));
            Assert.That(viewModel.Сurrency, Is.Not.EqualTo(PackIconFontAwesomeKind.ExclamationTriangleSolid));
            Assert.That(viewModel.Moneys, Is.Not.Null);
            Assert.That(viewModel.Moneys.Id, Is.EqualTo(AppData.AppSettings.MoneysDollarsTpl));
        }

        [Test]
        public void CanInitializeForEuros()
        {
            var money = AppData.ServerDatabase.ItemsDB[AppData.AppSettings.MoneysEurosTpl];
            MoneyDailogViewModel viewModel = new(money, null);
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.AddMoneysCommand, Is.Null);
            Assert.That(viewModel.Сurrency, Is.EqualTo(PackIconFontAwesomeKind.EuroSignSolid));
            Assert.That(viewModel.Сurrency, Is.Not.EqualTo(PackIconFontAwesomeKind.ExclamationTriangleSolid));
            Assert.That(viewModel.Moneys, Is.Not.Null);
            Assert.That(viewModel.Moneys.Id, Is.EqualTo(AppData.AppSettings.MoneysEurosTpl));
        }

        [Test]
        public void HasNeededData() => Assert.That(MoneyDailogViewModel.AppSettings, Is.Not.Null);

        [Test]
        public void CanExecuteAddMoneysCommand()
        {
            var addMoneysCommandExecuted = false;
            var roubles = AppData.ServerDatabase.ItemsDB[AppData.AppSettings.MoneysRublesTpl];
            MoneyDailogViewModel viewModel = new(roubles, new((_) => addMoneysCommandExecuted = true));
            viewModel.AddMoneysCommand.Execute(null);
            Assert.That(addMoneysCommandExecuted, Is.True);
        }
    }
}