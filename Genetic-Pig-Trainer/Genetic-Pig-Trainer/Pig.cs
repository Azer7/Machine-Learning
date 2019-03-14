using System;
using System.Collections.Generic;

namespace Pig
{
    internal class Pig
    {
        public bool hasEnded = false;
        public int roundCount = 0;
        public Player _player1;
        public Player _player2;
        private Player _currentPlayer;
        private Player _otherPlayer;
        private Player _winnerPlayer;
        private bool _hasEnded = false;

        public Pig(Player player1, Player player2)
        {
            _player1 = player1;
            _player2 = player2;

            _currentPlayer = player1;
            _otherPlayer = player2;
        }

        public void PlayRound()
        {
            roundCount++;
            double toRoll = _currentPlayer.RollDecision(_otherPlayer._score, _otherPlayer._roundScore)[0];

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
            Random dieGen = new Random(Guid.NewGuid().GetHashCode());
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
                _hasEnded = true;
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

            if (_hasEnded)
                _winnerPlayer.gameFitness += 50;

            _player1.totalFitness += _player1.gameFitness;
            _player2.totalFitness += _player2.gameFitness;
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

    internal class Player
    {
        public int _score = 0;
        public int _roundScore = 0;
        public bool IsTurn = false;
        public double totalFitness = 100;
        public double averageFitness = 0;
        public double gameFitness = 0;
        public double gameCount = 0;

        public NN.NeuralNet net;

        public Player(NN.NeuralNet Net)
        {
            net = Net;
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

        public List<double> RollDecision(int otherScore, int otherRoundScore)
        {
            List<double> outputs;

            List<int> input = new List<int> { _score, _roundScore, otherScore, otherRoundScore };
            List<double> inputDouble = input.ConvertAll(x => (double)x);

            outputs = net.ComputeLayers(inputDouble);

            return outputs;
        }
    }
}