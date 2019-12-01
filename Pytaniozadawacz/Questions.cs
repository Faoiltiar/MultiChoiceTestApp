using System.Collections.Generic;

namespace Pytaniozadawacz
{
    class Questions
    {
        public string QuestionContent { get; set; }
        //private bool questionAnswered;
        public readonly Dictionary<int, string> Answers;
        private readonly List<int> CorrectAnswer;
        public List<int> UserAnswers;

        public Questions()
        {
            Answers = new Dictionary<int, string>();
            CorrectAnswer = new List<int>();
            UserAnswers = new List<int>();
        }
        public Questions(string mQuestionContent, List<string> mAnswers, List<int> mCorrectAnswer)
            :this()
        {
            this.QuestionContent = mQuestionContent;
            int answerNumber = 0;
            foreach (string answer in mAnswers)
            {
                this.addToDictionary(answerNumber, answer);
                answerNumber++;
            }
            this.CorrectAnswer.AddRange(mCorrectAnswer);
            // this.questionAnswered = false;
        }
        public void addToDictionary(int key, string content)
        {
            Answers.Add(key, content);
        }
        public string ReturnAnswer(int mQuestionNumber)
        {
            if (Answers.ContainsKey(mQuestionNumber))
                return Answers[mQuestionNumber];
            else
                return "error";
        }
        
        public List<int> GetCorrectAnswers()
        {
            return CorrectAnswer;
        }
        public int GetNumberOfCorrectAnswers()
        {
            return CorrectAnswer.Count;
        }
        /*public void QuestionWasAnswered()
        {
            this.questionAnswered = true;
        }
        public bool IsQuestionAnswered()
        {
            return questionAnswered;
        }*/
        public void AddUserAnswer(int answerNo)
        {
            UserAnswers.Add(answerNo);
        }
        public void RemoveUserAnswer(int answerNo)
        {
            UserAnswers.Remove(answerNo);
        }
        public bool CheckIfAnswerWasChoosen(int answerNo)
        {
            bool mAnswerChoosen = false;
            if (UserAnswers.Contains(answerNo))
                mAnswerChoosen = true;
            return mAnswerChoosen;
        }
        public bool CheckIfAnswerIsCorrect(int answerNo)
        {
            bool mAnswerChoosen = false;
            if (CorrectAnswer.Contains(answerNo))
                mAnswerChoosen = true;
            return mAnswerChoosen;
        }
    }
}
