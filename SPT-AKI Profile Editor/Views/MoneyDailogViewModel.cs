using MahApps.Metro.IconPacks;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class MoneyDailogViewModel : BindableViewModel
    {
        private AddableItem moneys;

        public MoneyDailogViewModel(AddableItem money, RelayCommand addCommand, RelayCommand cancelCommand)
        {
            moneys = money;
            moneys.AddingQuantity = ((TarkovItem)moneys).Properties.StackMaxSize;
            AddMoneysCommand = addCommand;
            CancelCommand = cancelCommand;
        }

        public static AppSettings AppSettings => AppData.AppSettings;
        public static RelayCommand AddMoneysCommand { get; set; }
        public static RelayCommand CancelCommand { get; set; }
        public PackIconFontAwesomeKind Сurrency => GetIconKind();

        public AddableItem Moneys
        {
            get => moneys;
            set
            {
                moneys = value;
                OnPropertyChanged("Moneys");
            }
        }

        private PackIconFontAwesomeKind GetIconKind()
        {
            return moneys switch
            {
                _ when moneys.Id == AppSettings.MoneysRublesTpl => PackIconFontAwesomeKind.RubleSignSolid,
                _ when moneys.Id == AppSettings.MoneysDollarsTpl => PackIconFontAwesomeKind.DollarSignSolid,
                _ when moneys.Id == AppSettings.MoneysEurosTpl => PackIconFontAwesomeKind.EuroSignSolid,
                _ => PackIconFontAwesomeKind.RubleSignSolid,
            };
        }
    }
}