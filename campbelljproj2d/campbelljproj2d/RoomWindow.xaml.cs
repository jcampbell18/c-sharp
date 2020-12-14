using System.Windows;

namespace campbelljproj2d
{

    /// <summary>
    /// Interaction logic for RoomWindow.xaml
    /// Window that holds the question, user's input (answer), and will show the correct answer
    /// </summary>
    public partial class RoomWindow : Window
    {
        private TriviaQuestion triviaQuestion;

        /// <summary>
        /// Sets up the question in the window
        /// </summary>
        /// <param name="triviaQuestion">trivia question from the database</param>
        internal RoomWindow(TriviaQuestion triviaQuestion)
        {
            InitializeComponent();
            this.triviaQuestion = triviaQuestion;
            this.Answer = false;

            SetUpQuestion();
        }


        public bool Answer { get; private set; }

        /// <summary>
        /// Determines if the question is multiple choice or a True/False question
        /// </summary>
        private void SetUpQuestion()
        {
            this.tbQuestion.Text = triviaQuestion.Question;
            this.rbChoice1.Content = triviaQuestion.Choice1;
            this.rbChoice2.Content = triviaQuestion.Choice2;

            if (triviaQuestion.Choice3.Length > 1)
            {
                this.rbChoice3.Content = triviaQuestion.Choice3;
            }
            else 
            {
                this.rbChoice3.IsEnabled = false;
            }
        }

        /// <summary>
        /// Feedback for the user to show if the answer is correct or wrong
        /// </summary>
        /// <param name="answerIs"></param>
        private void ReportAnswer(bool answerIs)
        {
            btnSubmit.IsEnabled = false;
            btnClose.IsEnabled = true;

            if (answerIs)
            {
                this.tbAnswer.Text = "CORRECT!";

                if (triviaQuestion.AnswerInfo.Length > 1)
                {
                    this.tbAnswerInfo.Text = triviaQuestion.AnswerInfo;
                }

                this.Answer = true;
            }
            else
            {
                this.tbAnswer.Text = "SORRY!";
                string msg = "The correct is: " + triviaQuestion.Answer;

                if (triviaQuestion.AnswerInfo.Length > 1)
                {
                    msg += " - " + triviaQuestion.AnswerInfo;
                }

                this.tbAnswerInfo.Text = msg;
            }
        }

        /// <summary>
        /// Submits users answer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnSubmit(object sender, RoutedEventArgs e)
        {
            if (this.rbChoice1.IsChecked == true && triviaQuestion.Choice1.Equals(triviaQuestion.Answer))
            {
                ReportAnswer(true);
            }
            else if (this.rbChoice2.IsChecked == true && triviaQuestion.Choice2.Equals(triviaQuestion.Answer))
            {
                ReportAnswer(true);
            }
            else if (this.rbChoice3.IsChecked == true && triviaQuestion.Choice3.Equals(triviaQuestion.Answer))
            {
                ReportAnswer(true);
            }
            else
            {
                ReportAnswer(false);
            }
        }

        /// <summary>
        /// Warning for the user if they try to exit out of question...forfeiting that door
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCancel(object sender, RoutedEventArgs e)
        {
            string msg = "By exiting this window, you will be forfeiting your turn and the door will be permanently locked";
            string titleCaption = "WARNING";
            MessageBoxButton btn = MessageBoxButton.OKCancel;
            MessageBoxImage img = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(msg, titleCaption, btn, img);

            if (result == MessageBoxResult.OK)
            {
                this.Close();
            }
            
        }

        /// <summary>
        /// After the answer is revealed, the user can either X out, and click the close button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
