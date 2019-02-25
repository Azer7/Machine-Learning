using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NN
{
    class Generation
    {
        public int CurrentGen = 0;
        public List<Pig.Player> Players;
        public int currentPlayerIndex = 0;

        public Generation(int numberPerGeneration, int inputCount, int hiddenLayerCount, int hiddenLayerSize, int outputCount)
        {
            for(int i = 0; i < numberPerGeneration; i++)
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
            
            while(!game.hasEnded)
            {
                
            }
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
        int _inputNodes;
        int _nodes;
        List<Neuron> _neurons;

        public Layer(int inputNodes, int nodes)
        {
            _inputNodes = inputNodes;
            _nodes = nodes;
        
            _neurons = Enumerable.Range(1, nodes).Select(i => new Neuron(inputNodes + 1)).ToList();
        }

        public List<double> ComputeNeurons(List<double> inputs)
        {
            List<double> outputs = new List<double>();

            for(int i = 0; i < _neurons.Count; i++)
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
            
            _weights = Enumerable.Range(1, inputWeights).Select(i=>weightGen.NextDouble()).ToList();            
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
