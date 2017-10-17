let population;

let popSize = 1000;
let target = "I would enjoy a pink and yellow unicorn";
let mutationRate = 0.01; //1%
let timeStart;

function setup() {
    createCanvas(600, 800);

    timeStart = performance.now();
    population = new Population(popSize, mutationRate, target);
}

function draw() {
    population.processFitness();

    population.newGeneration(); //generate new population

    if (frameCount % 3 == 0) {
        population.evaluate();
    }
    if (population.finished == true)
        noLoop();
}
