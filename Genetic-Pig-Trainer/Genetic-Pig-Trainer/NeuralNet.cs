using System;
using System.Collections.Generic;
using System.Linq;

namespace NN
{
    class Generation
    {
        public int CurrentGen = 0;
        public List<Pig.Player> Players = new List<Pig.Player>();
        public int currentPlayerIndex = 0;

        public Generation(int numberPerGeneration, int inputCount, int hiddenLayerCount, int hiddenLayerSize, int outputCount)
        {
            for (int i = 0; i < numberPerGeneration; i++)
            {
                NeuralNet ai = new NeuralNet(inputCount, hiddenLayerCount, hiddenLayerSize, outputCount);
                Players.Add(new Pig.Player(ai));
            }
        }

        public void PlayGame()
        {
            Pig.Player p1 = Players[currentPlayerIndex];

            int randomIndex = 0;
            Random indexGen = new Random();
            do
            {
                randomIndex = indexGen.Next(1, Players.Count);
            } while (randomIndex == currentPlayerIndex);

            Pig.Player p2 = Players[randomIndex];

            //get two players
            Pig.Pig game = new Pig.Pig(p1, p2);

            while (!game.hasEnded)
            {
                game.PlayRound();
            }

            currentPlayerIndex++;
        }
    }

    class NeuralNet
    {
        Layer _inputLayer;
        List<Layer> _hiddenLayers;
        Layer _outputLayer;

        public double Fitness = 0;

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public NeuralNet()
        {
        }

        public NeuralNet Clone()
        {
            NeuralNet clonedNet = new NeuralNet();


            return clonedNet;
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
        public NeuralNet(NeuralNet network1, NeuralNet network2)
        {
            //crossover

            _inputLayer = new Layer(network1._inputLayer, network2._inputLayer);
            for (int i = 0; i < _hiddenLayers.Count; i++)
            {
                _hiddenLayers[i] = new Layer(network1._hiddenLayers[i], network2._hiddenLayers[i]);

            }
            _outputLayer = new Layer(network1._outputLayer, network2._outputLayer);
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

        double Tanh(double input)
        {

            return 1 / (1 + Math.Pow(2.71828, -1 * input));
        }

    }

    class Layer
    {
        int _inputNeuronCount;
        int _neuronCount;
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

            List<Neuron> _neurons = new List<Neuron>(_neuronCount);
            Random weightGen = new Random();
            for (int i = 0; i < _neuronCount; i++)
            {
                double randomNeuron = weightGen.NextDouble();
                if (randomNeuron > 0.5) // neuron 1 if greater than 50%
                    _neurons[i] = new Neuron(layer1._neurons[i]);
                else
                    _neurons[i] = new Neuron(layer2._neurons[i]);
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

    class Neuron
    {
        List<double> _weights;

        public Neuron(int inputWeights)
        {

            Random weightGen = new Random(Guid.NewGuid().GetHashCode());

            _weights = Enumerable.Range(1, inputWeights).Select(i => weightGen.NextDouble() * 0.1).ToList();
        }

        /// <summary>
        /// Clones the neuron
        /// </summary>
        /// <param name="neuron"></param>
        public Neuron(Neuron neuron)
        {
            _weights = new List<double>(neuron._weights);
        }

        public double Compute(List<double> inputs)
        {
            double output = 0;

            //sum all weighti * inputi
            for (int i = 0; i < inputs.Count; i++)
            {
                output += _weights[i] * inputs[i];
            }

            return output;
        }


    }
}
