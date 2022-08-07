using MahApps.Metro.IconPacks;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class MoneyDailogViewModel : ClosableDialogViewModel
    {
        public MoneyDailogViewModel(AddableItem money, RelayCommand addCommand)
        {
            Moneys = money;
            Moneys.AddingQuantity = ((TarkovItem)Moneys).Properties.StackMaxSize;
            AddMoneysCommand = addCommand;
        }

        public static AppSettings AppSettings => AppData.AppSettings;
        public RelayCommand AddMoneysCommand { get; }
        public PackIconFontAwesomeKind Сurrency => GetIconKind();

        public AddableItem Moneys { get; }

        private PackIconFontAwesomeKind GetIconKind()
        {
            return Moneys switch
            {
                _ when Moneys.Id == AppSettings.MoneysRublesTpl => PackIconFontAwesomeKind.RubleSignSolid,
                _ when Moneys.Id == AppSettings.MoneysDollarsTpl => PackIconFontAwesomeKind.DollarSignSolid,
                _ when Moneys.Id == AppSettings.MoneysEurosTpl => PackIconFontAwesomeKind.EuroSignSolid,
                _ => PackIconFontAwesomeKind.ExclamationTriangleSolid,
            };
        }
    }
}