/*
 * Author: Jason Campbell
 * Assignment 4: Animcation with WPF
 * 
 * The goal of this game is that the user can move the paddle to intercept the ball
 *      and the ball will bounce away.  Each time the paddle hits the ball, the user 
 *      gets a point. The score is displayed in the bottom left, the level is on the
 *      bottom right. If the ball is able to move beyond the paddle, the game stops.
 * 
 * Complete documentation found below
*/

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace campbelljcscd371hw4
{
    /// <summary>
    /// The Main Window holds the menu items, status bar, and game area.
    /// It  also holds all the windows event triggers and animation
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private HighscoresWindow highscoresWindow;
        private HighscorePlacementWindow highscorePlacementWindow;
        private readonly DatabaseConnection databaseConnection;
        private int ballX = 1;
        private int ballY = 1;
        private readonly int ballResetSize = 50;
        private readonly int paddleResetSize = 125;
        private int paddleSpeed = 2;
        private bool paddleUp = false;
        private bool paddleDown = false;
        private int score = 0;
        private int level = 1;
        private readonly int highestscore;
        private readonly int lowestscore;

        /// <summary>
        /// initializes the database connection, and sets up the game along
        /// with initially prompting the user with instructions
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            this.sbCurrentScore.Content = score;
            this.databaseConnection = new DatabaseConnection();
            int[] highlow = this.databaseConnection.GetHighAndLowScores();
            this.highestscore = highlow[0];
            this.lowestscore = highlow[1];
            GetInstructions();
            ResetGame();
        }

        /// <summary>
        /// Sets up the timer, defaulting to 10ms
        /// </summary>
        private void SetupTimer()
        {
            int milliseconds = 10;

            this.timer = new DispatcherTimer
            {
                Interval = new TimeSpan(0, 0, 0, 0, milliseconds)
            };

            this.timer.Tick += Timer_Tick;
        }

        /// <summary>
        /// The animation that controls the ball and paddle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            double ballTop = Canvas.GetTop(ball);
            double ballLeft = Canvas.GetLeft(ball);
            double canvasWidth = myCanvas.Width;
            double canvasHeight = myCanvas.Height;
            double paddleTop = Canvas.GetTop(paddle);
            double paddleBottom = paddleTop + paddle.Height;
            

            //paddle movement
            if (paddleUp && paddleTop > 0)
            {
                Canvas.SetTop(paddle, paddleTop - paddleSpeed);
            }
            if (paddleDown && paddleBottom < canvasHeight)
            {
                Canvas.SetTop(paddle, paddleTop + paddleSpeed);
            }

            //ball movement
            if (ballLeft <= Canvas.GetLeft(paddle) + (paddle.Width))
            {
                if (PaddleCollision())
                {
                    ballX *= -1;
                    ballLeft = 70;
                }             
            }
            if (ballTop <= 0)
            {
                ballY *= -1;
            }
            if (ballLeft + ball.Width + (ball.Width/5) >= canvasWidth)
            {
                ballX *= -1;
            }
            if (ballTop + ball.Height >= canvasHeight)
            {
                ballY *= -1;
            }

            if (ballLeft <= Canvas.GetLeft(paddle))
            {
                timer.Stop();
                CheckForHighscore();
                ResetGame();
            }
            else
            {
                Canvas.SetTop(ball, ballTop + ballY);
                Canvas.SetLeft(ball, ballLeft + ballX);
            }
        }

        /// <summary>
        /// detects if the ball intersects with the paddle
        /// </summary>
        /// <returns></returns>
        private bool PaddleCollision()
        {
            double paddleTop = Canvas.GetTop(paddle);
            double paddleBottom = paddleTop + paddle.Height;
            double paddleRight = Canvas.GetLeft(paddle) + paddle.Width;
            double ballTop = Canvas.GetTop(ball);
            double ballLeft = Canvas.GetLeft(ball);

            if (ballLeft <= paddleRight && ballTop + ball.Height/2 >= paddleTop && ballTop + ball.Height / 2 <= paddleBottom)
            {
                IncreaseScore();
                IncreaseLevel();
                return true;
            }

            return false;
        }

        /// <summary>
        /// increases the score by 1 point
        /// </summary>
        private void IncreaseScore()
        {
            score += 1;
            this.sbCurrentScore.Content = score;
        }

        /// <summary>
        /// increases the level with different variations (paddle
        ///     size or ball size/speed)
        /// </summary>
        /// <remarks>
        /// Every 3rd level, unless it is divisible by 5, decreases the
        ///     paddle size.
        /// Every 5th level, the ball size decreases and the ball speed
        ///     increases
        /// </remarks>
        private void IncreaseLevel()
        {
            if (score % 3 == 0 && score % 5 != 0)
            {
                level++;
                this.sbLevelNumber.Content = level + "";
                DecreasePaddleSize();
                ChangePaddleColor();
            }

            if (score % 5 == 0)
            {
                level++;
                this.sbLevelNumber.Content = level + "";
                DecreaseBallSize();
                IncreaseBallSpeed();
                ChangePaddleColor();
            }
        }

        /// <summary>
        /// Resets the game (and pieces), selecting a random area for the ball
        /// </summary>
        private void ResetGame()
        {
            score = 0;
            ballX = 1;
            ballY = 1;
            ball.Height = ballResetSize;
            ball.Width = ballResetSize;
            paddle.Height = paddleResetSize;
            this.sbCurrentScore.Content = "0";
            this.menuItemPause.Header = "Un_pause Game";
            this.menuItemSettings.IsEnabled = true;
            this.menuItemScore.IsEnabled = true;
            this.menuItemStart.IsEnabled = true;
            this.menuItemHelp.IsEnabled = true;
            this.sbLevelNumber.Content = "1";

            if (ballX < 0)
            {
                ballX *= -1;
            }

            ballY = RandomInt(-4, 2);
            Canvas.SetTop(ball, RandomInt(1, (int) (myCanvas.Height - paddle.Height * 2)));
            Canvas.SetLeft(ball, RandomInt((int)(Canvas.GetLeft(paddle) + paddle.Width), (int) (myCanvas.Width - paddle.Width * 2)));
            
            SetupTimer();
        }

        /// <summary>
        /// Pauses or resumes the game depending on the current status
        /// </summary>
        private void PauseGame()
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                this.menuItemPause.Header = "Un_pause Game";
                this.menuItemSettings.IsEnabled = true;
                this.menuItemHelp.IsEnabled = true;
                this.menuItemScore.IsEnabled = true;
                this.menuItemStart.IsEnabled = true;
            }
            else
            {
                timer.Start();
                this.menuItemPause.Header = "_Pause Game";
                this.menuItemSettings.IsEnabled = false;
                this.menuItemHelp.IsEnabled = false;
                this.menuItemScore.IsEnabled = false;
                this.menuItemStart.IsEnabled = false;
            }
        }

        /// <summary>
        /// If the user does not get a high score, a window will appear
        /// letting them know the game is over, and a another one has
        /// been set up.
        /// </summary>
        private void GameOver()
        {
            string msg = "";
            msg += "Nice try! A new game has been\n";           
            msg += "set up, if you want to try again.\n\n";
            string titleBar = "Game Over";
            MessageBoxButton messageBoxButtons = MessageBoxButton.OK;
            MessageBox.Show(msg, titleBar, messageBoxButtons);
        }

        private void ChangePaddleColor()
        {
            SolidColorBrush[] colors =
            {
                Brushes.Cyan,       // levels 2, 22, 42, 62, 82
                Brushes.DarkCyan,     
                Brushes.DarkBlue,
                Brushes.MidnightBlue,
                Brushes.Indigo,
                Brushes.DarkMagenta,
                Brushes.Brown,
                Brushes.Firebrick,
                Brushes.DarkRed,
                Brushes.Maroon,
                Brushes.DarkOliveGreen,
                Brushes.DarkGreen,
                Brushes.Green,
                Brushes.GreenYellow,
                Brushes.LawnGreen,
                Brushes.SpringGreen,
                Brushes.SeaGreen,
                Brushes.RoyalBlue,
                Brushes.SteelBlue,
                Brushes.Blue
            };

            paddle.Fill = colors[(level % 20) - 2];
        }

        /// <summary>
        /// Basic instructions on how to play the game
        /// </summary>
        private void GetInstructions()
        {
            string msg = "";
            msg += "Control the paddle with the keyboard Up and Down arrows\n\n";
            msg += "To acquire points, the ball must bounce off the paddle\n\n";
            msg += "The Spacebar is used for Pausing and Resuming the Game\n\n";
            msg += "As the points increase, the harder it gets\n\n";
            msg += "Each level may:\n\n";
            msg += "\t1. the Paddle size decreases\n\n";
            msg += "\t2. the Ball size decreases\n\n";
            msg += "\t3. the Ball speed increases\n\n";
            msg += "Instructions can be found in the Help section\n\n";
            MessageBox.Show(msg, "Instructions", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Compares the new score to the high and low scores from the database
        /// </summary>
        private void CheckForHighscore()
        {
            if (highestscore == 0 || score > lowestscore)
            {
                this.highscorePlacementWindow = new HighscorePlacementWindow();
                while (this.highscorePlacementWindow.ShowDialog() == true);
                this.databaseConnection.WriteToDatabase(new Highscore
                {
                    Name = this.highscorePlacementWindow.PlayerName,
                    Score = score,
                    Timestamp = GetTimeStamp()
                });

                ViewHighScores();
            } else
            {
                GameOver();
            }
        }

        /// <summary>
        /// Calls a new window that displays the current high scores
        /// </summary>
        private void ViewHighScores()
        {
            this.highscoresWindow = new HighscoresWindow(this.databaseConnection.QueryDatabase());
            while (this.highscoresWindow.ShowDialog() == true) ;

            if (this.highscoresWindow.EmptyDatabase)
            {
                this.databaseConnection.EmptyDatabase();
                this.menuItemScore.IsEnabled = true;
                ResetHighscoresConfirmation();
            }
        }

        /// <summary>
        /// Message box that confirms the user wants to dump contents of the database
        /// </summary>
        private void ResetHighscoresConfirmation()
        {
            MessageBox.Show("Highscores have been deleted", "Reset Highscores", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Determines a random in with the parameters being the boundaries
        /// </summary>
        /// <param name="min">the minimum number for the random generator</param>
        /// <param name="max">the maximum number for the random generator</param>
        /// <returns>a random number</returns>
        private int RandomInt(int min, int max)
        {
            Random random = new Random();
            int num;

            do
            {
                num = random.Next(min, max);
            }
            while (num == 0);

            if (num < 0)
            {
                return -1;
            }

            return num;
        }

        /// <summary>
        /// gets the current data and time
        /// </summary>
        /// <returns>timestamp string</returns>
        private static string GetTimeStamp()
        {
            DateTime timestamp = DateTime.Now;
            return timestamp.ToString("g");
        }

        /// <summary>
        /// detects if a key is pressed such as the up & down arrows for
        /// the paddle, and spacebar for pause/resume
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                paddleDown = true;
            }
            if (e.Key == Key.Up)
            {
                paddleUp = true;
            }
            if (e.Key == Key.Space)
            {
                PauseGame();
            }

        }

        /// <summary>
        /// detects if a key is released, such as the up and down arrow
        /// for the paddle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void KeyReleased(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                paddleDown = false;
            }
            if (e.Key == Key.Up)
            {
                paddleUp = false;
            }

        }

        /// <summary>
        /// Decreases the current paddle size by 5, unless it is at it's
        /// minimum height
        /// </summary>
        private void DecreasePaddleSize()
        {
            int increment = 5;
            int min = 25;

            if (paddle.Height <= min || paddle.Height - increment <= min)
            {
                paddle.Height = min;
            }
            else
            {
                paddle.Height -= increment;
            }
        }

        /// <summary>
        /// Decreases the ball size by 5, unless it is at it's minimum size of 10
        /// </summary>
        private void DecreaseBallSize()
        {
            int increment = 5;
            int min = 10;

            if (ball.Height <= min || ball.Height - increment <= min)
            {
                ball.Height = min;
                ball.Width = min;
            }
            else
            {
                ball.Height -= increment;
                ball.Width -= increment;
            }
        }

        /// <summary>
        /// Increases the ball speed by 1, unless it is at it's maximum speed of 10
        /// </summary>
        private void IncreaseBallSpeed()
        {
            int increment = 1;
            int max = 10;

            if (ballX >= max || ballX + increment >= max)
            {
                ballX = max;
                ballY = ballX;
            }
            else
            {
                ballX -= increment;
                ballY = ballX;
            }
        }

        /// <summary>
        /// Event trigger for the Menuitem under File, which calls the Reset Game method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnStartNewGame(object sender, RoutedEventArgs e)
        {
            ResetGame();
        }

        /// <summary>
        /// Event trigger for the Menuitem under File, which calls the Pause Game method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPauseGame(object sender, RoutedEventArgs e)
        {
            PauseGame();
        }

        /// <summary>
        /// Event trigger for the Menuitem under Settings, which increases paddle speed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPaddleIncreaseSpeed(object sender, RoutedEventArgs e)
        {
            if (paddleSpeed >= 20 || paddleSpeed + 5 >= 20)
            {
                paddleSpeed = 20;
                menuItemPaddleIncreaseSpeed.IsEnabled = false;
            }
            else
            {
                paddleSpeed += 5;
                menuItemPaddleDecreaseSpeed.IsEnabled = true;
            }
        }

        /// <summary>
        /// Event trigger for the Menuitem under Settings, which decreases paddle speed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPaddleDecreaseSpeed(object sender, RoutedEventArgs e)
        {
            if (paddleSpeed <= 1 || paddleSpeed - 5 <= 1)
            {
                paddleSpeed = 1;
                menuItemPaddleDecreaseSpeed.IsEnabled = false;
            }
            else
            {
                paddleSpeed -= 5;
                menuItemPaddleIncreaseSpeed.IsEnabled = true;
            }
        }

        /// <summary>
        /// Event trigger for the Menuitem under Settings, which resets the paddle speed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPaddleResetSpeed(object sender, RoutedEventArgs e)
        {
            paddleSpeed = 10;
            menuItemPaddleIncreaseSpeed.IsEnabled = true;
            menuItemPaddleDecreaseSpeed.IsEnabled = true;
        }

        /// <summary>
        /// Event trigger for the Menuitem under Highschores, which calls the View Highscores method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHighscoreList(object sender, RoutedEventArgs e)
        {
            ViewHighScores();
        }

        /// <summary>
        /// Event trigger for the Menuitem under Highscores, which calls a confirmation window to 
        /// verifiy that the user wants to delete the highscores
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnHighscoreReset(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                this.databaseConnection.EmptyDatabase();
                ResetHighscoresConfirmation();
            }
        }

        /// <summary>
        /// Event trigger for the Menuitem under Help, which produces a window containing
        /// information about the Game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAbout(object sender, RoutedEventArgs e)
        {
            string str = "";
            str += "Author: Jason Campbell\n\n";
            str += "Project Name: Assignment 4 - Animation with WPF\n\n";
            str += "Project Description: The goal of this game is that the user\n";
            str += "\tcan move the paddle to intercept the ball and\n";
            str += "\tthe ball will bounce away.Each time the paddle\n";
            str += "\thits the ball, the user gets a point. \n";
            str += "\tDisplay the user score anywhere you'd like.\n";
            str += "\tIf the ball is able to move beyond the paddle,\n";
            str += "\tthe game stops.\n\n";
            str += "Version: 1.0\n";
            MessageBox.Show(str, "About", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Event trigger for the Menuitem under File, which produces a confirmation window, and if the
        /// user wants to exit will check if the current score is in the top 10
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExit(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
            }

            string msg = "Are you sure?";
            string titleBar = "Confirm Exit";
            MessageBoxButton messageBoxButtons = MessageBoxButton.YesNo;
            MessageBoxImage messageBoxImage = MessageBoxImage.Question;
            MessageBoxResult messageBoxResult = MessageBox.Show(msg, titleBar, messageBoxButtons, messageBoxImage);

            if (messageBoxResult == MessageBoxResult.Yes)
            {
                CheckForHighscore();
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Event trigger for the Menuitem under Help, which calls the Get instructions method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnInstructions(object sender, RoutedEventArgs e)
        {
            GetInstructions();
        }
    }
}
