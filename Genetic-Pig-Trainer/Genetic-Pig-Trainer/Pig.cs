using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
namespace Pig
{
    public static class RndGen
    {
        //public static Random rnd = new Random();
        public static Random rnd;
    }



    public class Pig
    {
        public int maxTurns = 200;
        public bool hasEnded = false;
        public int turnCount = 0;
        public Player _player1;
        public Player _player2;
        public Player _currentPlayer;
        public Player _otherPlayer;
        public Player _winnerPlayer;

        public Pig(Player player1, Player player2)
        {
            _player1 = player1;
            _player2 = player2;

            _currentPlayer = player1;
            _otherPlayer = player2;
        }

        public void PlayRound()
        {
            turnCount++;
            double toRoll = _currentPlayer.RollDecision(_otherPlayer._score)[0];

            if (toRoll > 0)
            {
                RollRound(_currentPlayer, _otherPlayer);
            }
            else
            {
                HoldRound(_currentPlayer, _otherPlayer);
            }
        }

        public void RollRound(Player currentPlayer, Player otherPlayer)
        {
            Random dieGen = RndGen.rnd;
            //Random dieGen = new Random(1);
            int roll = dieGen.Next(1, 7);

            if (roll == 1)
            {
                //switch turn
                SwitchTurn(currentPlayer, otherPlayer);
            }
            else
            {
                currentPlayer._roundScore += roll;
            }
        }

        public void HoldRound(Player currentPlayer, Player otherPlayer)
        {
            currentPlayer._score += currentPlayer._roundScore;
            if (currentPlayer._score >= 100)
            {
                hasEnded = true;
                currentPlayer._score = 100;
                _winnerPlayer = currentPlayer;
            }
            else
                SwitchTurn(currentPlayer, otherPlayer);
        }

        public void CalculateFitness()
        {
            _player1.gameFitness = 0;
            _player2.gameFitness = 0;

            _player1.gameFitness += _player1._score;
            _player2.gameFitness += _player2._score;


            if (hasEnded)
            {
                //add fitness to greater score difference
                if (_winnerPlayer == _player1)
                    _winnerPlayer.gameFitness += _winnerPlayer._score - _player2._score;
                else
                    _winnerPlayer.gameFitness += _winnerPlayer._score - _player1._score;


                _winnerPlayer.gameFitness += 50;
                if (this.turnCount != 200)
                    hasEnded = true;
                _winnerPlayer.gameFitness += 2 * (this.maxTurns - this.turnCount);
            }

            _player1.totalFitness += _player1.gameFitness;
            _player2.totalFitness += _player2.gameFitness;



            _player1._roundScore = 0;
            _player2._roundScore = 0;
            _player1._score = 0;
            _player2._score = 0;
        }

        public void SwitchTurn(Player currentPlayer, Player otherPlayer)
        {
            currentPlayer._roundScore = 0;
            //reset score

            currentPlayer.IsTurn = false;
            otherPlayer.IsTurn = true;

            _currentPlayer = otherPlayer;
            _otherPlayer = currentPlayer;
        }
    }

    [Serializable]
    public class Player
    {
        public int _score = 0;
        public int _roundScore = 0;
        public bool IsTurn = false;
        public double totalFitness = 100;
        public double averageFitness = 0;
        public double gameFitness = 0;
        public double gameCount = 0;

        public NN.NeuralNet net;

        public Player()
        {

        }

        public Player(NN.NeuralNet Net)
        {
            net = new NN.NeuralNet(Net);
        }

        public Player(Player player) : this(player.net)
        {
        }
        /// <summary>
        /// crosses over player1 and player2
        /// </summary>
        /// <param name="player1"></param>
        /// <param name="player2"></param>
        public Player(Player player1, Player player2)
        {
            net = new NN.NeuralNet(player1.net, player2.net);
        }

        public List<double> RollDecision(int otherScore)
        {
            List<double> outputs;
            int scoreDifference = otherScore - _score;
            List<int> input = new List<int>();
            if (net._inputLayer._inputNeuronCount == 3)
                input = new List<int> { _roundScore, _score, scoreDifference };
            else if (net._inputLayer._inputNeuronCount == 2)
                input = new List<int> { _roundScore, scoreDifference };

            List<double> inputDouble = input.ConvertAll(x => (double)x);

            outputs = net.ComputeLayers(inputDouble);

            return outputs;
        }

        public void Mutate(double mutationRate)
        {
            net.Mutate(mutationRate);
        }
    }
}