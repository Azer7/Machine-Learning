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
using System.Threading;

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
        NN.Generation aiGeneration;
        Random rndGen;


        public MainWindow()
        {
            InitializeComponent();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);


            rndGen = new Random();
            Pig.RndGen.rnd = rndGen;
            NN.RndGen.rnd = rndGen;

            aiGeneration = new NN.Generation(20, 10, 3, 1, 1, 1);
        }

        public void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            count++;
            CountLbl.Content = "Game: " + count;
            GenLbl.Content = "Gen: " + aiGeneration.currentGen;
            FitLbl.Content = "Max Fitness: " + aiGeneration.maxFitness;
            //do move

            //aiGeneration.playGen();

            for(int i = 0; i < 5000; i++)
            {
                aiGeneration.PlayGame();                
            }

            if (aiGeneration.currentGen % 50 == 0)
            {
                CalcBaseline();
            }

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

                string writeString = stringwriter.ToString();
                writeString = writeString.Replace("encoding=\"utf-16\"", "");

                File.WriteAllText("bestPlayer.dat", writeString);
            }
        }

        private void CalcBaseline()
        {
            Pig.Player humanPlayer = new Pig.Player();
            XmlSerializer deserializer = new XmlSerializer(typeof(Pig.Player));
            Pig.Player aiPlayer = new Pig.Player();


            FileStream openFile = new FileStream("basePlayer.dat", FileMode.Open);

            Pig.Player tempPlayer;
            try
            {
                tempPlayer = (Pig.Player)deserializer.Deserialize(openFile);
            }
            catch (Exception err)
            {
                System.Windows.Forms.MessageBox.Show(err.Message);
                throw;
            }
            //creation of two players

            openFile.Close();

            aiPlayer = new Pig.Player(tempPlayer);
            Pig.Player bestCurrentPlayer = new Pig.Player(aiGeneration.Players[0]);

            for (int i = 0; i < 10000; i++)
            {
                //get two players
                Pig.Pig game;
                if (rndGen.NextDouble() > 0.5) // to change who is starting player
                    game = new Pig.Pig(aiPlayer, bestCurrentPlayer);
                else
                    game = new Pig.Pig(bestCurrentPlayer, aiPlayer);

                while (!game.hasEnded && game.turnCount < game.maxTurns)
                {
                    //breaks here
                    game.PlayRound();

                }

                aiPlayer.gameCount++;
                bestCurrentPlayer.gameCount++;
                game.CalculateFitness();
            }

            //ratio of fitness of bestplayer to otherplayer
            baseLbl.Content = bestCurrentPlayer.totalFitness / aiPlayer.totalFitness;
        }

        private void BaselineBtn_Click(object sender, RoutedEventArgs e)
        {
            CalcBaseline();
        }
    }
}
