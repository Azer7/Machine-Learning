let population;

let popSize = 100;
let target = "I want butter";
let mutationRate = 0.01; //1%

function setup() {
    population = new Population(popSize, mutationRate, target);
}

function draw() {
    population.processFitness();
    
    population.naturalSelection();
    
    population.crossOver();
    
    //and?
}