let population;

let popSize = 800;
let target = "I am doing lots of interesting work yay... but wait...";
let mutationRate = .007; //1%
let timeStart;

let speed = 1;

function setup() {
    createCanvas(600, 800);

    timeStart = performance.now();
    population = new Population(popSize, mutationRate, target);
}

function draw() {
    if (frameCount % 200 == 0) {
        population.processFitness();
        population.evaluate();
        
        //generate new population
        population.newGeneration(); 
    }

    //population.checkCollision();

    if (frameCount % speed == 0) {
        population.draw();
    }
    if (population.finished == true)
        noLoop();
}
