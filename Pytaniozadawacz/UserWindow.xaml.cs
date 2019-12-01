using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Text.RegularExpressions;

namespace Pytaniozadawacz
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
            
        }

        private void DownButt_Click(object sender, RoutedEventArgs e)
        {
            int i = Convert.ToInt16(PercentageTb.Text);
            if (i>0)
                PercentageTb.Text = (--i).ToString();
        }

        private void UpButt_Click(object sender, RoutedEventArgs e)
        {
            int i = Convert.ToInt16(PercentageTb.Text);
            if (i < 100)
                PercentageTb.Text = (++i).ToString();
        }

        private bool CheckPercentage()
        {
            
            Regex regPercentage = new Regex("^([0-9])+$");
            
            if (regPercentage.IsMatch(PercentageTb.Text) ){
                int percentage = Convert.ToInt32(PercentageTb.Text);
                if (percentage <= 100)
                    return true;
                else
                    return false;
            }
            else
                return false;
            
        }

        private bool CheckName()
        {
            if (UserTb.Text.Length == 0)
                return false;
            else
                return true;
        }

        private bool ValidateUserInput()
        {
            if (!CheckPercentage() && !CheckName())
            {
                WindowManager.ChangeText(UserLabel, Colors.Red, "Wprowadź nazwę użytkownika!");
                WindowManager.ChangeText(PercentageLabel, Colors.Red, "Wprowadź poprawną liczbę (0-100)!");
                return false;
            }
            else if ((!CheckPercentage()) && CheckName())
            {
                WindowManager.ChangeText(UserLabel, Colors.Black, "Nazwa użytkownika");
                WindowManager.ChangeText(PercentageLabel, Colors.Red, "Wprowadź poprawną liczbę (0-100)!");
                return false;
            }
            else if (CheckPercentage() && !CheckName())
            {
                WindowManager.ChangeText(UserLabel, Colors.Red, "Wprowadź nazwę użytkownika!");
                WindowManager.ChangeText(PercentageLabel, Colors.Black, "Ile potrzeba procent, żeby zaliczyć?");
                return false;
            }
            else
            {
                WindowManager.ChangeText(UserLabel, Colors.Black, "Nazwa użytkownika");
                WindowManager.ChangeText(PercentageLabel, Colors.Black, "Ile potrzeba procent, żeby zaliczyć?");
                return true;
            }
        }

        private void normalTestBut_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateUserInput())
            {
                QuestonWindow win = new QuestonWindow(UserTb.Text, Convert.ToByte(PercentageTb.Text));
                win.Show();
                this.Close();
            }
        }

        private void PercentageTb_KeyUp(object sender, KeyEventArgs e)
        {
            if (CheckPercentage() && PercentageTb.Text.Trim().StartsWith("0") && PercentageTb.Text != "0")
            {
                string percentage = PercentageTb.Text;
                percentage = percentage.TrimStart(new Char[] { '0' });
                PercentageTb.Text = percentage;
            }

        }
    }
}
