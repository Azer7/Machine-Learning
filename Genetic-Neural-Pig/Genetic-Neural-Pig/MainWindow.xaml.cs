using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Xml.Serialization;

namespace Genetic_Neural_Pig
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Windows.Threading.DispatcherTimer aiTimer = new System.Windows.Threading.DispatcherTimer();

        Pig.Player humanPlayer = new Pig.Player();
        XmlSerializer deserializer = new XmlSerializer(typeof(Pig.Player));
        Pig.Player aiPlayer = new Pig.Player();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {            
            FileStream openFile = new FileStream("bestPlayer.dat", FileMode.Open);

            Pig.Player tempPlayer = (Pig.Player)deserializer.Deserialize(openFile);
            aiPlayer = new Pig.Player(tempPlayer.net);

            aiTimer.Tick += aiTimer_Tick;
            aiTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);



        }

        private void aiTimer_Tick(object sender, EventArgs e)
        {

            if(aiPlayer.RollDecision(humanPlayer._score)[0] > 0) //roll
            {
                P2Roll_Click();
            } else
            {
                P2Hold_Click();
            }
            // code goes here
        }

        private void P1Roll_Click(object sender, RoutedEventArgs e)
        {
            int roll = new Random().Next(1, 6);
            p1DieLabel.Content = roll;

            if (roll == 1)
            {
                humanPlayer._roundScore = 0;
                p1EndTurn();
            }
            else
                humanPlayer._roundScore += roll;

            p1RoundScoreLabel.Content = humanPlayer._roundScore;
        }

        private void P1Hold_Click(object sender, RoutedEventArgs e)
        {
            if (p1EndTurn())
            {
                winLabel.Visibility = Visibility.Visible;
            }
        }


        private bool p1EndTurn()
        {
            p1Panel.IsEnabled = false;
            p2Panel.IsEnabled = true;
            humanPlayer._score += humanPlayer._roundScore;
            humanPlayer._roundScore = 0;

            p1ScoreLabel.Content = humanPlayer._score;
            if (humanPlayer._score >= 100)
                return true;            

            aiTimer.Start();
            return false;
        }


        private void P2Roll_Click()
        {
            int roll = new Random().Next(1, 6);
            p2DieLabel.Content = roll;

            if (roll == 1)
            {
                aiPlayer._roundScore = 0;
                p2EndTurn();
            }
            else
                aiPlayer._roundScore += roll;

            p2RoundScoreLabel.Content = aiPlayer._roundScore;
        }


        private void P2Hold_Click()
        {
            if (p2EndTurn())
            {
                winLabel.Visibility = Visibility.Visible;
            }
        }
        private bool p2EndTurn()
        {
            p1Panel.IsEnabled = true;
            p2Panel.IsEnabled = false;
            aiPlayer._score += aiPlayer._roundScore;
            aiPlayer._roundScore = 0;

            p2ScoreLabel.Content = aiPlayer._score;

            aiTimer.Stop();
            return aiPlayer._score >= 100;
        }

        public MainWindow()
        {
            InitializeComponent();
            p2Panel.Background = (LinearGradientBrush)this.Resources["waitBrush"];
        }

    }
}
