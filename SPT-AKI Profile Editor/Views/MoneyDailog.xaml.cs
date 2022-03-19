using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для MoneyDailog.xaml
    /// </summary>
    public partial class MoneyDailog : UserControl
    {
        public MoneyDailog()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "1";
                textBox.CaretIndex = 1;
                return;
            }
            if (int.TryParse(textBox.Text.Replace(",", ""), out int money))
            {
                if (money < 1)
                {
                    textBox.Text = "1";
                    textBox.CaretIndex = 1;
                }
            }
            else
            {
                textBox.Text = int.MaxValue.ToString();
                textBox.CaretIndex = textBox.Text.Length;
            }
        }
    }
}