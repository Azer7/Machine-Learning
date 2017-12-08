class NeuralNetwork {
    constructor(numI, hArr, numO) {
           this.numIn = numI;
           this.hidNodes = hArr;
           this.outNodes = numO;
    }
}

class Neuron {
    constructor(w) {
        this.weight = w;
    }
}