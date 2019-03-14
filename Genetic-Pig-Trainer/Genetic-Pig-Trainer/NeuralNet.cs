﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace NN
{
    internal class Generation
    {
        public int currentGen = 0;
        public List<Pig.Player> Players = new List<Pig.Player>();
        public int playerIndex = 0; //count through each player
        public int versusIndex = 0; //count through each versus player

        //input parameters 
        public int _numberPerGeneration = 0;
        public int _inputCount = 0;
        public int _hiddenLayerCount = 0;
        public int _hiddenLayerSize = 0;
        public int _outputCount = 0;

        private double genPlayerFitScale = 0.5;


        public Generation(int numberPerGeneration, int inputCount, int hiddenLayerCount, int hiddenLayerSize, int outputCount)
        {
            _numberPerGeneration = numberPerGeneration;
            _inputCount = inputCount;
            _hiddenLayerCount = hiddenLayerCount;
            _hiddenLayerSize = hiddenLayerSize;
            _outputCount = outputCount;

            for (int i = 0; i < numberPerGeneration; i++)
            {
                NeuralNet ai = new NeuralNet(inputCount, hiddenLayerCount, hiddenLayerSize, outputCount);
                Players.Add(new Pig.Player(ai));
            }
        }

        public void CreateGen()
        {
            List<Pig.Player> newPlayers = new List<Pig.Player>();
            foreach (Pig.Player player in Players)
            {
                player.averageFitness = player.totalFitness / player.gameCount;
            }
            Players.Sort((a, b) => b.averageFitness.CompareTo(a.averageFitness));

            double totalSqrtFitness = 0;
            totalSqrtFitness = Players.Sum(x => Math.Pow(x.averageFitness, genPlayerFitScale));

            newPlayers.Add(Players[0]);
            newPlayers.Add(Players[1]);
            newPlayers.Add(Players[2]);

            for (int i = 3; i < Players.Count; i++)
            {
                Pig.Player cross1 = GetRandomMember(totalSqrtFitness);
                //Pig.Player cross2 = GetRandomMember(totalSqrtFitness - Math.Pow(cross1.averageFitness, genPlayerFitScale), cross1);
                Pig.Player cross2 = GetRandomMember(totalSqrtFitness);

                Pig.Player newPlayer = new Pig.Player(cross1, cross2);
                newPlayers.Add(newPlayer);
            }

            Players = newPlayers; //copy over new players

            //for (int i = 0; i < _numberPerGeneration; i++)
            //{
            //    NeuralNet ai = new NeuralNet(_inputCount, _hiddenLayerCount, _hiddenLayerSize, _outputCount);
            //    Players.Add(new Pig.Player(ai));
            //}                       


        }

        public Pig.Player GetRandomMember(double totalSqrtFitness, Pig.Player exclusion = null)
        {
            if (exclusion != null)
                totalSqrtFitness -= Math.Pow(exclusion.averageFitness, genPlayerFitScale);

            Random memberGen = new Random(Guid.NewGuid().GetHashCode());
            double randomVal = memberGen.NextDouble() * totalSqrtFitness;

            foreach (Pig.Player player in Players)
            {
                double currentFit = Math.Pow(player.averageFitness, genPlayerFitScale);

                if (player == exclusion)
                    continue;

                if (randomVal < currentFit)
                {
                    return player;
                }

                randomVal -= currentFit;
            }

            return Players[-1]; //this will crash and mean that something went wrong- however it should never in theory
        }

        public void PlayGame()
        {
            if (versusIndex == playerIndex)
                versusIndex++;

            if (versusIndex >= Players.Count)
            {
                playerIndex++;
                versusIndex = 0;
            }

            if (playerIndex >= Players.Count)
            {
                CreateGen();
                currentGen++;
                versusIndex = 0;
                playerIndex = 0;

            }
            else
            {
                Pig.Player p1 = Players[playerIndex];
                Pig.Player p2 = Players[versusIndex];
                versusIndex++;

                //get two players
                Pig.Pig game = new Pig.Pig(p1, p2);

                while (!game.hasEnded && game.roundCount < 200)
                {
                    //breaks here
                    game.PlayRound();
                }

                p1.gameCount++;
                p2.gameCount++;
                game.CalculateFitness();
            }
        }
    }

    internal class NeuralNet
    {
        private Layer _inputLayer;
        private List<Layer> _hiddenLayers;
        private Layer _outputLayer;

        public double Fitness = 0;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public NeuralNet()
        {
            _hiddenLayers = new List<Layer>();
        }

        public NeuralNet(NeuralNet copyNet)
        {
            _inputLayer = new Layer(copyNet._inputLayer);
            foreach (Layer layer in copyNet._hiddenLayers)
            {
                _hiddenLayers.Add(new Layer(layer));
            }
            _outputLayer = new Layer(copyNet._outputLayer);
        }

        public NeuralNet(int inputCount, int hiddenLayerCount, int hiddenLayerSize, int outputCount)
        {
            _inputLayer = new Layer(inputCount, hiddenLayerSize); //layer with as many weights as inputs
            _hiddenLayers = Enumerable.Range(1, hiddenLayerCount - 1).Select(i => new Layer(hiddenLayerSize, hiddenLayerSize)).ToList(); //the rest of the hidden layers with constant input & node count
            _outputLayer = new Layer(hiddenLayerSize, outputCount); // outputs to a different amount of nodes
        }

        /// <summary>
        /// Crosses over NeuralNet n1 and NeuralNet n2
        /// </summary>
        /// <param name="n1"></param>
        /// <param name="n2"></param>
        public NeuralNet(NeuralNet network1, NeuralNet network2) : this()
        {
            //crossover

            _inputLayer = new Layer(network1._inputLayer, network2._inputLayer);

            for (int i = 0; i < network1._hiddenLayers.Count; i++)
            {
                _hiddenLayers.Add(new Layer(network1._hiddenLayers[i], network2._hiddenLayers[i]));
            }
            _outputLayer = new Layer(network1._outputLayer, network2._outputLayer);
        }

        /// <summary>
        /// gets a mutated copy of the network
        /// </summary>
        /// <param name="mutateRate">default 1% mutateRate</param>
        /// <returns></returns>
        public void Mutate(double mutateRate = 0.01)
        {
            Random mutateGen = new Random();

            _inputLayer.Mutate(mutateRate);
            foreach (Layer hiddenLayer in _hiddenLayers)
            {
                hiddenLayer.Mutate(mutateRate);
            }
            _outputLayer.Mutate(mutateRate);
        }

        public List<double> ComputeLayers(List<double> inputs)
        {
            List<double> layerOutput;

            layerOutput = _inputLayer.ComputeNeurons(inputs);

            foreach (Layer hiddenLayer in _hiddenLayers)
            {
                layerOutput = hiddenLayer.ComputeNeurons(layerOutput);
            }

            layerOutput = _outputLayer.ComputeNeurons(layerOutput);

            return layerOutput;
        }
    }

    internal class Layer
    {
        private int _inputNeuronCount;
        private int _neuronCount;
        public List<Neuron> _neurons;

        public Layer(int inputNeuronCount, int neuronCount)
        {
            _inputNeuronCount = inputNeuronCount;
            _neuronCount = neuronCount;

            _neurons = Enumerable.Range(1, _neuronCount).Select(i => new Neuron(_inputNeuronCount + 1)).ToList();
        }

        /// <summary>
        /// Crosses over layer 1 and layer 2
        /// </summary>
        /// <param name="l1"></param>
        /// <param name="l2"></param>
        public Layer(Layer layer1, Layer layer2)
        {
            _inputNeuronCount = layer1._inputNeuronCount;
            _neuronCount = layer1._neuronCount;

            List<Neuron> _neurons = new List<Neuron>();
            Random weightGen = new Random();
            for (int i = 0; i < _neuronCount; i++)
            {
                double randomNeuron = weightGen.NextDouble();
                if (randomNeuron > 0.5) // neuron 1 if greater than 50%
                    _neurons.Add(new Neuron(layer1._neurons[i]));
                else
                    _neurons.Add(new Neuron(layer2._neurons[i]));
            }
        }

        public Layer(Layer copyLayer)
        {
            _inputNeuronCount = copyLayer._inputNeuronCount;
            _neuronCount = copyLayer._neuronCount;
            foreach (Neuron neuron in copyLayer._neurons)
            {
                _neurons.Add(new Neuron(neuron));
            }
        }

        public void Mutate(double mutateRate = 0.01)
        {
            foreach (Neuron neuron in _neurons)
            {
                neuron.Mutate(mutateRate);
            }
        }

        public List<double> ComputeNeurons(List<double> inputs)
        {
            List<double> outputs = new List<double>();

            for (int i = 0; i < _neurons.Count; i++)
            {
                outputs.Add(_neurons[i].Compute(inputs.Append(1).ToList()));
            }

            return outputs;
        }
    }

    internal class Neuron
    {
        public List<double> _weights;

        public Neuron(int inputWeights)
        {
            Random weightGen = new Random(Guid.NewGuid().GetHashCode());

            _weights = Enumerable.Range(1, inputWeights).Select(i => weightGen.NextDouble() * 2 - 1).ToList();
        }

        /// <summary>
        /// Clones the neuron
        /// </summary>
        /// <param name="neuron"></param>
        public Neuron(Neuron neuron)
        {
            _weights = new List<double>(neuron._weights);
        }

        public void Mutate(double mutateRate = 0.01)
        {
            Random randomGen = new Random();

            for (int i = 0; i < _weights.Count; i++)
            {
                if (randomGen.NextDouble() < mutateRate)
                {
                    _weights[i] = randomGen.NextDouble();
                }
            }
        }

        public static double Sigmoid(double input)
        {
            return 1 / (1 + Math.Pow(Math.E, -input));
        }

        public static double Tanh(double input)
        {
            return 2 / (1 + Math.Pow(Math.E, -2 * input)) - 1;
        }


        public double Compute(List<double> inputs)
        {
            double output = 0;

            //sum all weighti * inputi
            for (int i = 0; i < inputs.Count; i++)
            {
                output += _weights[i] * inputs[i];
            }

            double activated = Tanh(output);
            return activated;
        }
    }
}

//get pranked