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

namespace Genetic_Neural_Pig
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int p1Score = 0, p2Score = 0;
        int p1RoundScore = 0, p2RoundScore = 0;

        private void P1Roll_Click(object sender, RoutedEventArgs e)
        {
            int roll = new Random().Next(1, 6);
            p1DieLabel.Content = roll;

            if (roll == 1)
            {
                p1RoundScore = 0;
                p1EndTurn();
            }
            else
                p1RoundScore += roll;

            p1RoundScoreLabel.Content = p1RoundScore;
        }

        private bool p1EndTurn()
        {
            p1Panel.IsEnabled = false;
            p2Panel.IsEnabled = true;
            p1Score += p1RoundScore;

            p1ScoreLabel.Content = p1Score;
            return p1Score > 100;
        }

        private void P1Hold_Click(object sender, RoutedEventArgs e)
        {
            if(p1EndTurn())
            {
                winLabel.Visibility = Visibility.Visible;
            }
        }



        private void P2Roll_Click(object sender, RoutedEventArgs e)
        {
            int roll = new Random().Next(1, 6);
            p2DieLabel.Content = roll;
            
            if (roll == 1)
            {
                p2RoundScore = 0;
                p2EndTurn();
            }
            else
                p2RoundScore += roll;
            
            p2RoundScoreLabel.Content = p2RoundScore;
        }

        private bool p2EndTurn()
        {
            p1Panel.IsEnabled = true;
            p2Panel.IsEnabled = false;
            p2Score += p2RoundScore;

            p2ScoreLabel.Content = p2Score;
            return p2Score > 100;
        }

        private void P2Hold_Click(object sender, RoutedEventArgs e)
        {
            if (p2EndTurn())
            {
                winLabel.Visibility = Visibility.Visible;
            }
        }


        public MainWindow()
        {
            InitializeComponent();
            p2Panel.Background = (LinearGradientBrush)this.Resources["waitBrush"];
        }


    }
}
