using System;
using System.Collections.Generic;

namespace Pig
{
    internal class Pig
    {
        public bool hasEnded = false;
        private Player _player1;
        private Player _player2;
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
            double toRoll = _currentPlayer.RollDecision(_otherPlayer._score, _otherPlayer._roundScore)[0];

            if (toRoll > 0.5)
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
            Random dieGen = new Random();
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
            _player1.roundFitness = 0;
            _player2.roundFitness = 0;

            _player1.roundFitness += _player1._roundScore;
            _player2.roundFitness += _player2._roundScore;

            _winnerPlayer.roundFitness += 50;

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
        public double fitness = 0;
        public double roundFitness = 0;

        public NN.NeuralNet AINet;

        public Player(NN.NeuralNet Net)
        {
            AINet = Net;
        }

        public Player(Player player) : this(player.AINet)
        {
        }

        public List<double> RollDecision(int otherScore, int otherRoundScore)
        {
            List<double> outputs;

            List<int> input = new List<int> { _score, _roundScore, otherScore, otherRoundScore };
            List<double> inputDouble = input.ConvertAll(x => (double)x);

            outputs = AINet.ComputeLayers(inputDouble);

            return outputs;
        }
    }
}