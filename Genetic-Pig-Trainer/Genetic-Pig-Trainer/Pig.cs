using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Pig
{
    class Pig
    {
        public bool hasEnded = false;
        Player _player1;
        Player _player2;
        Player _currentPlayer;
        Player _otherPlayer;
        bool _hasEnded = false;

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
                _hasEnded = true;
            else
                SwitchTurn(currentPlayer, otherPlayer);
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

    class Player
    {
        public int _score = 0;
        public int _roundScore = 0;
        public bool IsTurn = false;
        public double fitness = 0;

        public NN.NeuralNet AINet;

        public Player(NN.NeuralNet Net)
        {
            AINet = Net;
        }

        public Player(Player player) : this(player.AINet) { }

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
