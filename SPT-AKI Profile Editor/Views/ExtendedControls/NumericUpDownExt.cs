using MahApps.Metro.Controls;
using System.Linq;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    internal class NumericUpDownExt : NumericUpDown
    {
        protected override void OnPreviewTextInput(TextCompositionEventArgs e)
        {
            base.OnPreviewTextInput(e);
            e.Handled = e.Text.Any(x => char.IsLetter(x) || x == ',');
        }
    }
}