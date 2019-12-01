using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.IO;
using System.Windows.Threading;

namespace Pytaniozadawacz
{
    /// <summary>
    /// Interaction logic for QuestonWindow.xaml
    /// </summary>
    public partial class QuestonWindow : Window
    {
        private Dictionary<int, Questions> QuestionsDict;
        //private Dictionary<int, List<int>> UserAnswers = new Dictionary<int, List<int>>();
        //private List<int> QuestionAnswers = new List<int>();
        private DispatcherTimer dt;
        private byte secondsOnQuestion;
        private int minutesOnQuestion;
        private int currentQuestNo;
        private byte percentage;
        public string userName;
        private bool testDone;
        private Button answerBut;
        private Viewbox answerVb;
        private TextBlock answerTb;
        private RowDefinition newRow;

        public QuestonWindow()
        {
            QuestionsDict = new Dictionary<int, Questions>();
            dt = new DispatcherTimer();
            testDone = false;
        }
        public QuestonWindow(string userName, byte percentage)
            :this()
        {
            InitializeComponent();
            this.percentage = percentage;
            this.userName = userName;
        }

        static public string SubstringAfter(string wholeString, string prefix)
        {
            int ix = wholeString.IndexOf(prefix);
            if (ix != -1)
                return wholeString.Substring(ix + prefix.Length);
            else
                return "error";
        }

        private void LoadFile(string[] filePaths)
        {
            if(filePaths.Length == 0)
            {
                throw new ArgumentException("File was not found in proper directory. Please check if you have put your files in correct folder.");
            }
            foreach (string path in filePaths)
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string question = "";
                    string currentLine = "";
                    int questionNumber = 0;
                    int answerCounter = 0;
                    List<string> Answers = new List<string>();
                    List<int> correctAnswer = new List<int>();
                    while (!reader.EndOfStream)
                    {
                        currentLine = reader.ReadLine();
                        if (currentLine.Length > 0)
                        {
                            //Console.WriteLine(line[0] + "\n");
                            if (char.IsDigit(currentLine[0]))
                            {
                                if (SubstringAfter(currentLine, ".") != "error")
                                {
                                    if (question.Length > 0)
                                    {
                                        QuestionsDict.Add(questionNumber, new Questions(question, Answers, correctAnswer));
                                        questionNumber++;
                                    }
                                    Answers.Clear();
                                    correctAnswer.Clear();
                                    answerCounter = 0;
                                    question = SubstringAfter(currentLine, ".").Trim(' ');
                                    // Console.WriteLine(question);
                                }
                            }
                            else if (currentLine.StartsWith("-"))
                            {
                                if (SubstringAfter(currentLine, "-") != "error")
                                {
                                    Answers.Add(SubstringAfter(currentLine, "-").Trim(' '));
                                    //Console.WriteLine(SubstringAfter(line, "-"));
                                    answerCounter++;
                                }
                            }
                            else if (currentLine.StartsWith("*"))
                            {
                                if (SubstringAfter(currentLine, "*") != "error")
                                {
                                    correctAnswer.Add(answerCounter);
                                    Answers.Add(SubstringAfter(currentLine, "*").Trim(' '));
                                    // Console.WriteLine(SubstringAfter(line, "*"));
                                    answerCounter++;
                                }
                            }
                        }
                        currentLine = "";
                    }
                    if (Answers.Count == 0)
                        throw new Exception("You have provided an empty file!");
                    else
                        QuestionsDict.Add(questionNumber, new Questions(question, Answers, correctAnswer));
                }
            }
        }

        

        private void PrepareQuestionsButtons()
        {
            RowDefinition row;
            ColumnDefinition column;
            Button butt;
            foreach (int i in QuestionsDict.Keys)
            {
                row = new RowDefinition();
                column = new ColumnDefinition();
                butt = new Button();
                answerBut.Click += (s, ev) => { Button button = s as Button; PrepareQuestionContent("next"); };
                WindowManager.FillDynamicGrid(questionGrid, row, column, butt, i, 1, 1);
            }
        }

        

        private void ChooseAnswer(Button chosenBut, KeyValuePair<int, string> answer)
        {
            if (!QuestionsDict[currentQuestNo].CheckIfAnswerWasChoosen(answer.Key))
            {
                chosenBut.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 129, 129, 129)); ;
                chosenBut.BorderThickness = new Thickness(3, 3, 3, 3);
                QuestionsDict[currentQuestNo].AddUserAnswer(answer.Key);
                //QuestionAnswers.Add(answer.Key);
            }
            else
            {
                chosenBut.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 221, 221, 221)); ;
                chosenBut.BorderThickness = new Thickness(1, 1, 1, 1);
                QuestionsDict[currentQuestNo].RemoveUserAnswer(answer.Key);
                //QuestionAnswers.Remove(answer.Key);
            }
        }

        private int CorrectQuestionsCounter()
        {
            int incorrectQuestions = 0;
            foreach (KeyValuePair<int,Questions> question in QuestionsDict)
            {
                if (question.Value.UserAnswers.Count != question.Value.GetNumberOfCorrectAnswers()) { incorrectQuestions++; }
                else
                    foreach (int answer in question.Value.UserAnswers)
                    {
                        if (!question.Value.CheckIfAnswerIsCorrect(answer))
                        {
                            incorrectQuestions++;
                            break;
                        }
                    }
            }
            return QuestionsDict.Count - incorrectQuestions;
        }

        private void PrepareQuestionContent(string whichQuestion)
        {
           /* actualTimeTB.Text = "00:00";
            dt.Stop();
            dt.Start();*/
            if (whichQuestion == "next")
            {
                currentQuestNo += 1;
                this.prevQuestBt.Visibility = Visibility.Visible;
                if(!QuestionsDict.ContainsKey(currentQuestNo + 1))
                    this.nextQuestBt.Visibility = Visibility.Hidden;
            } else
            {
                currentQuestNo -= 1;
                this.nextQuestBt.Visibility = Visibility.Visible;
                if(!QuestionsDict.ContainsKey(currentQuestNo - 1))
                    this.prevQuestBt.Visibility = Visibility.Hidden;
            }
            this.questNoTB.Text = "Pytanie: " + (currentQuestNo + 1).ToString() + "/" + QuestionsDict.Count;
            this.questionTB.Text = QuestionsDict[currentQuestNo].QuestionContent;
            //secondsOnQuestion = 0;
            //minutesOnQuestion = 0;

            /*if (QuestionsDict[currentQuestNo].IsQuestionAnswered())
                testButton.Visibility = Visibility.Hidden;
            else
                testButton.Visibility = Visibility.Visible;*/
            WindowManager.ClearGrid(answersGrid);

            foreach (KeyValuePair<int, string> answer in QuestionsDict[currentQuestNo].Answers)
            {
                newRow = new RowDefinition();
                answerBut = new Button();
                answerBut.Click += (s, ev) => { Button button = s as Button; ChooseAnswer(button, answer); };
                answerVb = new Viewbox();
                answerTb = new TextBlock();
                WindowManager.FillDynamicGrid(answersGrid, newRow, answerBut, answerVb, answerTb, answer);

                if (testDone == true)
                {
                    answerBut.IsHitTestVisible = false;
                    if (QuestionsDict[currentQuestNo].CheckIfAnswerWasChoosen(answer.Key) && QuestionsDict[currentQuestNo].CheckIfAnswerIsCorrect(answer.Key))
                    {
                        answerBut.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 60, 212, 36)); ;
                        answerBut.BorderThickness = new Thickness(3, 3, 3, 3);
                        answerBut.IsHitTestVisible = false;
                        //MessageBox.Show("Przycisk poprawny");
                    }
                    else if (QuestionsDict[currentQuestNo].CheckIfAnswerWasChoosen(answer.Key) && !QuestionsDict[currentQuestNo].CheckIfAnswerIsCorrect(answer.Key))
                    {
                        answerBut.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 230, 15, 15)); ;
                        answerBut.BorderThickness = new Thickness(3, 3, 3, 3);
                        answerBut.IsHitTestVisible = false;
                        // MessageBox.Show("Przycisk niepoprawny");
                    }
                    else if (!QuestionsDict[currentQuestNo].CheckIfAnswerWasChoosen(answer.Key) && QuestionsDict[currentQuestNo].CheckIfAnswerIsCorrect(answer.Key))
                    {
                        //butt.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 60, 212, 36));
                        answerBut.Background = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
                        answerBut.BorderThickness = new Thickness(3, 3, 3, 3);
                        answerBut.IsHitTestVisible = false;
                        //MessageBox.Show("Poprawny");
                    }
                    else
                    {
                        answerBut.IsHitTestVisible = false;
                        //MessageBox.Show("Przycisk");
                    }
                }
                else
                {
                    if (QuestionsDict[currentQuestNo].CheckIfAnswerWasChoosen(answer.Key))
                    {
                        answerBut.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 129, 129, 129)); ;
                        answerBut.BorderThickness = new Thickness(3, 3, 3, 3);
                    }
                    else
                    {
                        answerBut.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 221, 221, 221)); ;
                        answerBut.BorderThickness = new Thickness(1, 1, 1, 1);
                    }
                }
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            QuestionsDict.Clear();
            string rootPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(AppDomain.CurrentDomain.BaseDirectory), @"Data\");
            byte minNumberOfQuestions = 1;

            try
            {
                string[] allFilePaths = Directory.GetFiles(rootPath);
                LoadFile(allFilePaths);
            }
            catch (Exception exception)
            {
                ErrorHandlingWindow errorWindow = new ErrorHandlingWindow(exception.Message);
                errorWindow.Show();
                return;
            }
            
            dt.Interval = TimeSpan.FromSeconds(1);
            dt.Tick += OnTimedEvent;
            dt.Start();


            string correctAnswers = "";
            foreach (int ans in QuestionsDict[currentQuestNo].GetCorrectAnswers())
            {
                correctAnswers += (ans.ToString() + " ");
            }


            this.questNoTB.Text = "Pytanie: " + (currentQuestNo + 1).ToString() + "/" + QuestionsDict.Count;
            this.questionTB.Text = QuestionsDict[currentQuestNo].QuestionContent;

            if(QuestionsDict.Count > minNumberOfQuestions)
                this.nextQuestBt.Visibility = Visibility.Visible;
            //this.questionTB.Text = correctAnswers;

            //int answerNo = 0;
            foreach (KeyValuePair<int, string> answer in QuestionsDict[currentQuestNo].Answers)
            {
                newRow = new RowDefinition();
                answerBut = new Button();
                //isAnswerChoosen = QuestionsDict[currentQuestNo].CheckIfAnswerWasChoosen(answer.Key);
                answerBut.Click += (s, ev) => { Button button = s as Button; ChooseAnswer(button, answer); };
                    //new RoutedEventHandler(checkAnswer_Click);
                answerVb = new Viewbox();
                answerTb = new TextBlock();
                WindowManager.FillDynamicGrid(answersGrid, newRow, answerBut, answerVb, answerTb, answer);
            }
            /* textBlock.Text = "";
             foreach (KeyValuePair<int, Questions> entry in QuestionsDict)
             {
                 textBlock.Text += "Pytanie numer " + entry.Key + "\n";
                 textBlock.Text += entry.Value.QuestionContent;
                 textBlock.Text += "\nOpdowiedzi:\n";
                 foreach (KeyValuePair<int, string> answer in entry.Value.Answers)
                 {
                     textBlock.Text += "- " + entry.Value.ReturnAnswer(answer.Key) + "\n";
                 }
                 textBlock.Text += "\n";
             }
             textBlock.Text += "\n KUNIEC";*/
        }

        private void OnTimedEvent(object sender, EventArgs e)
        {
            string varActualTime;
            secondsOnQuestion++;

            if (secondsOnQuestion > 59)
            {
                secondsOnQuestion = 0;
                minutesOnQuestion++;
            }
            if (minutesOnQuestion < 10)
            {
                if (secondsOnQuestion < 10)
                {
                    varActualTime = string.Format("0{0}:0{1}", minutesOnQuestion.ToString(), secondsOnQuestion.ToString());
                }
                else
                    varActualTime = string.Format("0{0}:{1}", minutesOnQuestion.ToString(), secondsOnQuestion.ToString());
            }
            else
            {
                if (secondsOnQuestion < 10)
                {
                    varActualTime = string.Format("{0}:0{1}", minutesOnQuestion.ToString(), secondsOnQuestion.ToString());
                }
                else
                    varActualTime = string.Format("{0}:{1}",minutesOnQuestion.ToString(), secondsOnQuestion.ToString());
            }
            timeTB.Text = string.Format("Czas: {0}", varActualTime);
        }

        private void nextQuestBt_Click(object sender, RoutedEventArgs e)
        {
            PrepareQuestionContent("next");
        }

        private void prevQuestBt_Click(object sender, RoutedEventArgs e)
        {
            PrepareQuestionContent("previous");
        }

        private void checkAnswer_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            //button.IsPressed = true;
        }

        private void testButton_Click(object sender, RoutedEventArgs e)
        {
            int currentButtonNo = 0;
            /*string correctAnswers = "";
            string yourAnswers = "";
            
            foreach (int ans in CorrectAnswers)
            {
                correctAnswers += (ans.ToString() + " ");
            }
            foreach (int yans in QuestionAnswers)
            {
                yourAnswers += (yans.ToString() + " ");
            }
            if (QuestionAnswers.Count() != CorrectAnswers.Count())
            {
                MessageBox.Show("Niepoprawna ilosc odpowiedzi\n" + "Poprawne: " + correctAnswers + "\nTwoje: " + yourAnswers);
            }
            else
            {
                if (!QuestionAnswers.SequenceEqual(CorrectAnswers))
                {
                    MessageBox.Show("Niepoprawne odpowiedzi\n" + "Poprawne: " + correctAnswers + "\nTwoje: " + yourAnswers);
                }
                else
                    MessageBox.Show("Poprawne odpowiedzi\n" + "Poprawne: " + correctAnswers + "\nTwoje: " + yourAnswers);
            }*/
            foreach(Control butt in this.answersGrid.Children)
            {
                if (butt is Button)
                {
                    if (QuestionsDict[currentQuestNo].CheckIfAnswerWasChoosen(currentButtonNo) && QuestionsDict[currentQuestNo].CheckIfAnswerIsCorrect(currentButtonNo))
                    {
                        butt.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 60, 212, 36));
                        //butt.BorderThickness = new Thickness(3, 3, 3, 3);
                        butt.IsHitTestVisible = false;
                        //MessageBox.Show("Przycisk poprawny");
                    }
                    else if (QuestionsDict[currentQuestNo].CheckIfAnswerWasChoosen(currentButtonNo) && !QuestionsDict[currentQuestNo].CheckIfAnswerIsCorrect(currentButtonNo))
                    {
                        butt.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 230, 15, 15));
                        //butt.BorderThickness = new Thickness(3, 3, 3, 3);
                        butt.IsHitTestVisible = false;
                        //MessageBox.Show("Przycisk niepoprawny");
                    }
                    else if (!QuestionsDict[currentQuestNo].CheckIfAnswerWasChoosen(currentButtonNo) && QuestionsDict[currentQuestNo].CheckIfAnswerIsCorrect(currentButtonNo))
                    {
                        //butt.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 60, 212, 36));
                        butt.Background = new SolidColorBrush(Color.FromArgb(255, 0, 255, 0));
                        //butt.BorderThickness = new Thickness(3, 3, 3, 3);
                        butt.IsHitTestVisible = false;
                        //MessageBox.Show("Poprawny");
                    }
                    else
                    {
                        butt.IsHitTestVisible = false;
                        //MessageBox.Show("Przycisk");
                    }
                    currentButtonNo++;
                }
            }
            testButton.Visibility = Visibility.Hidden;
            finishButton.Visibility = Visibility.Visible;
            testDone = true;
            //QuestionsDict[currentQuestNo].QuestionWasAnswered();
        }

        private void finishButton_Click(object sender, RoutedEventArgs e)
        {
            ConfirmationWindow confirm = new ConfirmationWindow(this, CorrectQuestionsCounter(), QuestionsDict.Count, secondsOnQuestion, minutesOnQuestion, userName, percentage);
            confirm.Show();
        }
    }
}
