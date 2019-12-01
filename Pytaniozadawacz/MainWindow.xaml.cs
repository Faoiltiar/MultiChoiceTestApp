using System.Windows;

namespace Pytaniozadawacz
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        
        private void loadFileButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void showFileContentButton_Click(object sender, RoutedEventArgs e)
        {
            UserWindow win = new UserWindow();
            win.Show();
            this.Close();
            
            
            /*QuestonWindow win = new QuestonWindow();
            win.Show();
            this.Close();


            */
            //string content = "dddd";
            //content += QuestionsDict[0].QuestionContent;
            /*textBox.Text = "";
            foreach (KeyValuePair<int, Questions> entry in QuestionsDict)
            {
                textBox.Text += "Pytanie numer " + entry.Key + "\n";
                textBox.Text += entry.Value.QuestionContent;
                textBox.Text += "\nOpdowiedzi:\n";
                foreach (KeyValuePair<int, string> answer in entry.Value.Answers)
                {
                    textBox.Text += "- " + entry.Value.ReturnAnswer(answer.Key) + "\n";
                }
                textBox.Text += "\n";
            }
            textBox.Text += "\n KUNIEC";*/
           // textBox.Text = content;
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
