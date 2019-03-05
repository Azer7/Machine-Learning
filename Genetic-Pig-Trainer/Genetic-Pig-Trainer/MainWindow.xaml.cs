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
using System.Windows.Threading;

namespace Genetic_Pig_Trainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();       
        int count = 0;

        NN.Generation aiGeneration = new NN.Generation(10, 4, 3, 5, 1);

        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);            
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            count++;
            CountLbl.Content = count;
            //do move
            aiGeneration.PlayGame();

            timer.Start();
            //wait a ms or two or 5
        }


        private void ToggleStartBtn_Click(object sender, RoutedEventArgs e)
        {
            if(timer.IsEnabled)
            {
                timer.Stop();
                toggleStartBtn.Content = "Run";
            } else
            {
                timer.Start();
                toggleStartBtn.Content = "Stop";
            }
        }
    }
}
