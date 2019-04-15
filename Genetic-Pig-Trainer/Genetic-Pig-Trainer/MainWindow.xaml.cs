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
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Genetic_Pig_Trainer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer();
        int count = 0;

        //NN.Generation aiGeneration = new NN.Generation(10, 3, 3, 5, 1);
        NN.Generation aiGeneration = new NN.Generation(40, 3, 4, 7, 1);

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
            CountLbl.Content = "Game: " + count;
            GenLbl.Content = "Gen: " + aiGeneration.currentGen;
            FitLbl.Content = "Max Fitness: " + aiGeneration.maxFitness;
            //do move
            for (int i = 0; i < 10000; i++)
                aiGeneration.PlayGame();


            timer.Start();
            //wait a ms or two or 5
        }


        private void ToggleStartBtn_Click(object sender, RoutedEventArgs e)
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
                toggleStartBtn.Content = "Run";
            }
            else
            {
                timer.Start();
                toggleStartBtn.Content = "Stop";
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            using (var stringwriter = new System.IO.StringWriter())
            {
                Type[] types = new Type[3];
                types[0] = typeof(NN.NeuralNet);
                types[1] = typeof(NN.Neuron);
                types[2] = typeof(NN.Layer);
                var serializer = new XmlSerializer(aiGeneration.Players[0].GetType(), types);
                serializer.Serialize(stringwriter, aiGeneration.Players[0]);

                File.WriteAllText("bestPlayer.dat", stringwriter.ToString());
            }
        }

        private void BaselineBtn_Click(object sender, RoutedEventArgs e)
        {
            Pig.Player humanPlayer = new Pig.Player();
            XmlSerializer deserializer = new XmlSerializer(typeof(Pig.Player));
            Pig.Player aiPlayer = new Pig.Player();


            FileStream openFile = new FileStream("basePlayer.dat", FileMode.Open);


            //creation of two players
            Pig.Player tempPlayer = (Pig.Player)deserializer.Deserialize(openFile);

            aiPlayer = new Pig.Player(tempPlayer);
            Pig.Player bestCurrentPlayer = new Pig.Player(aiGeneration.Players[0]);

            for(int i = 0; i < 100000; i++)
            {
                //get two players
                Pig.Pig game = new Pig.Pig(aiPlayer, bestCurrentPlayer);

                while (!game.hasEnded && game.turnCount < game.maxTurns)
                {
                    //breaks here
                    game.PlayRound();

                }

                aiPlayer.gameCount++;
                bestCurrentPlayer.gameCount++;
                game.CalculateFitness();
            }

            baseLbl.Content = bestCurrentPlayer.totalFitness;
        }
    }
}
