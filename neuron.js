class Neuron {
    constructor() {
        this.weights = [];
        for (let i = 0; i < 2; i++) {
            this.weights.push(random(-1, 1));
        }

        this.lr = 0.1;
    }

    guess(inputs) { //create a guess based on the inputs
        let sum = 0;

        for (let i = 0; i < this.weights.length; i++) {
            sum += inputs[i] * this.weights[i];
        }
        return sign(sum);
    }

    train(inputs, target) {
        let guess = this.guess(inputs); //make a guess based on the inputs point:(x,y)
        let error = target - guess;

        //change the weights
        for (let i = 0; i < this.weights.length; i++) {
            this.weights[i] += error * inputs[i] * lr;
        }
    }
}

function sign(value) {
    if (value >= 0)
        return 1;
    else
        return -1;
}