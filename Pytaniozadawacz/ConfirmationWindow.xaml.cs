using System.Windows;

namespace Pytaniozadawacz
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        private int numberOfCorrectQuestions;
        private int numberOfQuestions;
        private byte seconds;
        private int minutes;
        private byte percentage;
        private string userName;
        private QuestonWindow questWin;
        public ConfirmationWindow(QuestonWindow win, int numberOfCorrectQuestions, int numberOfQuestions, byte seconds, int minutes, string userName, byte percentage)
        {
            InitializeComponent();
            questWin = win;
            this.numberOfCorrectQuestions = numberOfCorrectQuestions;
            this.numberOfQuestions = numberOfQuestions;
            this.seconds = seconds;
            this.minutes = minutes;
            this.userName = userName;
            this.percentage = percentage;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Information.Text = string.Format("{0} czy chcesz zakończyć test?", userName);
        }

        private void Reject_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            questWin.Close();
            SummaryWindow sumWin = new SummaryWindow(numberOfCorrectQuestions, numberOfQuestions, seconds, minutes, userName, percentage);
            sumWin.Show();
            this.Close();
        }
    }
}
