using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NN
{
    class Generation
    {
        public int CurrentGen;
        List<NeuralNet> neuralNets;

        public Generation(int numberPerGeneration, int inputCount, int hiddenLayerCount, int hiddenLayerSize, int outputCount)
        {
            CurrentGen = 0;
            neuralNets = new List<NeuralNet>();
            //base initialization

            neuralNets = Enumerable.Repeat(new NeuralNet(inputCount, hiddenLayerCount, hiddenLayerSize, outputCount), numberPerGeneration).ToList();       
        }
    }

    class NeuralNet
    {
        Layer _inputLayer;
        List<Layer> _hiddenLayers;
        Layer _outputLayer;       

        /// <summary>
        /// Empty Constructor
        /// </summary>
        public NeuralNet()
        {
        }

        public NeuralNet(int inputCount, int hiddenLayerCount, int hiddenLayerSize, int outputCount)
        {
            _inputLayer = new Layer(inputCount, hiddenLayerSize); //layer with as many weights as inputs
            _hiddenLayers = Enumerable.Range(1, hiddenLayerCount - 1).Select(i => new Layer(hiddenLayerSize, hiddenLayerSize)).ToList(); //the rest of the hidden layers with constant input & node count
            _outputLayer = new Layer(hiddenLayerSize, outputCount); // outputs to a different amount of nodes


        }
        
        NeuralNet Clone()
        {
            NeuralNet clonedNet = new NeuralNet();


            return clonedNet;
        }

        double tanh(double input)
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


    }

    class Neuron
    {
        List<double> _weights;

        public Neuron(int inputWeights)
        {

            Random weightGen = new Random(Guid.NewGuid().GetHashCode());
            _weights = Enumerable.Range(1, inputWeights).Select(i=>weightGen.NextDouble()).ToList();            
        }

        double Compute(List<int> inputs)
        {
            double total = 0;

            //sum all weighti * inputi
            for (int i = 0; i < inputs.Count; i++)
            {
                total += _weights[i] * inputs[i];
            }

            return total;
        }

        
    }
}
