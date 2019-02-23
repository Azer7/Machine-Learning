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

        public Pig(Player player1, Player player2)
        {
            _player1 = player1;
            _player2 = player2;

            _currentPlayer = player1;
        }

        public void SwitchTurn(Player currentPlayer, Player otherPlayer)
        {
            currentPlayer.RoundScore = 0;
            //reset score

            currentPlayer.IsTurn = false;
            otherPlayer.IsTurn = true;

            _currentPlayer = otherPlayer;
        }

        public void RollRound(Player currentPlayer, Player otherPlayer)
        {
            Random dieGen = new Random();
            int roll = dieGen.Next(1, 7);

            if(roll == 1)
            {
                //switch turn
                SwitchTurn(currentPlayer, otherPlayer);
            }
            else
            {
                currentPlayer.RoundScore += roll;
            }

        }

        public void HoldRound(Player currentPlayer, Player otherPlayer)
        {
            currentPlayer.Score += currentPlayer.RoundScore;
            SwitchTurn(currentPlayer, otherPlayer);
        } 

    }

    class Player
    {
        public int Score = 0;
        public int RoundScore = 0;
        public bool IsTurn = false;

        public NN.NeuralNet AINet;

        public Player(NN.NeuralNet Net)
        {
            AINet = Net;
        }

        public void MakeDecision()
        {

        }
    }
}
