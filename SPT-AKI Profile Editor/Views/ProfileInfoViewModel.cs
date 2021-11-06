using SPT_AKI_Profile_Editor.Core;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Views
{
    class ProfileInfoViewModel : INotifyPropertyChanged
    {
        public static AppLocalization AppLocalization => App.appLocalization;

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}