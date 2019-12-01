using System.Windows;

namespace Pytaniozadawacz
{
    /// <summary>
    /// Interaction logic for ErrorHandlingWindow.xaml
    /// </summary>
    public partial class ErrorHandlingWindow : Window
    {
        public ErrorHandlingWindow(string errorDescription)
        {
            InitializeComponent();
            this.errorDescriptionLabel.Content = string.Format(@"An Error occured during the execution of this program:
                
{0}
                
Please press 'OK' button, to close the program",errorDescription);
        }

        private void ConfirmationButton_Click(object sender, RoutedEventArgs e)
        {
            for (int intCounter = App.Current.Windows.Count - 1; intCounter >= 0; intCounter--)
                App.Current.Windows[intCounter].Close();
        }
    }
}
