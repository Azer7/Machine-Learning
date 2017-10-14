class Neuron {
    constructor(n) {
        this.weights = [];
        for (let i = 0; i < n; i++) {
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
    
    lineY(x) {
        let w0 = this.weights[0];
        let w1 = this.weights[1];
        let w2 = this.weights[2];
        return -w2/w1 - x*w0/w1;
    }

    train(inputs, target) {
        let guess = this.guess(inputs); //make a guess based on the inputs point:(x,y)
        let error = target - guess;

        //change the weights
        for (let i = 0; i < this.weights.length; i++) {
            this.weights[i] += error * inputs[i] * this.lr;
        }
    }
}

function sign(value) {
    if (value >= 0)
        return 1;
    else
        return -1;
}