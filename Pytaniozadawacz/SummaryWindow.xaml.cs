using System.Windows;
using System;

namespace Pytaniozadawacz
{
    /// <summary>
    /// Interaction logic for SummaryWindow.xaml
    /// </summary>
    public partial class SummaryWindow : Window
    {
        public SummaryWindow(int numberOfCorrectQuestions, int numberOfQuestions, int seconds, int minutes, string userName, int percentage)
        {
            InitializeComponent();
            NameText.Text = string.Format("{0} {1}", NameText.Text, userName);
            Time.Text = string.Format("{0}:{1}", minutes.ToString(), seconds.ToString());
            Correct.Text = numberOfCorrectQuestions.ToString();
            Wrong.Text = (numberOfQuestions - numberOfCorrectQuestions).ToString();
            double userPercentage = Math.Round((((double)numberOfCorrectQuestions / numberOfQuestions) *100), 2);
            Percentage.Text = string.Format("{0}%", (userPercentage).ToString());
            if((double)percentage <= userPercentage)
                Result.Text = "Zdane!";
            else
                Result.Text = "Doucz się!";
        }

        private void FinishTest_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
