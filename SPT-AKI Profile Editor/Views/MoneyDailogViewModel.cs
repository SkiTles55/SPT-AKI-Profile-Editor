using MahApps.Metro.IconPacks;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Views
{
    public class MoneyDailogViewModel : INotifyPropertyChanged
    {
        public MoneyDailogViewModel(string tpl, RelayCommand addCommand, RelayCommand cancelCommand)
        {
            moneysTpl = tpl;
            AddMoneysCommand = addCommand;
            CancelCommand = cancelCommand;
        }
        public static AppLocalization AppLocalization => AppData.AppLocalization;
        public static AppSettings AppSettings => AppData.AppSettings;
        public static RelayCommand AddMoneysCommand { get; set; }
        public static RelayCommand CancelCommand { get; set; }
        public PackIconFontAwesomeKind Сurrency => GetIconKind();
        public Tuple<int, bool> Result => new(Count, Fir);
        public int Count
        {
            get => count;
            set
            {
                count = value;
                OnPropertyChanged("Count");
                OnPropertyChanged("Result");
            }
        }
        public bool Fir
        {
            get => fir;
            set
            {
                fir = value;
                OnPropertyChanged("Fir");
                OnPropertyChanged("Result");
            }
        }

        private readonly string moneysTpl;
        private int count = 500000;
        private bool fir = false;

        private PackIconFontAwesomeKind GetIconKind()
        {
            return moneysTpl switch
            {
                _ when moneysTpl == AppSettings.MoneysRublesTpl => PackIconFontAwesomeKind.RubleSignSolid,
                _ when moneysTpl == AppSettings.MoneysDollarsTpl => PackIconFontAwesomeKind.DollarSignSolid,
                _ when moneysTpl == AppSettings.MoneysEurosTpl => PackIconFontAwesomeKind.EuroSignSolid,
                _ => PackIconFontAwesomeKind.RubleSignSolid,
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}
