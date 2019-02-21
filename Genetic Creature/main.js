let genomes = [];
let genomeAmount = 5;

let popSize = 50;
let lifeSpan = 500;
let mutationRate = .03; //1%
let target;

let timeStart;
let speed = 1;
let instant = 1;

let ageP;

let obst = {
    x: 150,
    y: 250,
    w: 500,
    h: 20
}

function setup() {
    createCanvas(800, 600);
    frameRate(60);

    ageP = createP();

    target = createVector(width / 2, 50); //initialize final location

    timeStart = performance.now();
    for (let i = 0; i < genomeAmount; i++) {
        let col = color(i * 50, 255 - i * 40, 150 + i * 25);
        genomes.push(new Genome(popSize, lifeSpan, mutationRate, target, col));
    }
}

function draw() {
    background("white");
    for (let i = 0; i < instant; i++) {
     
        for (let j = 0; j < genomes.length; j++) {
            if (genomes[j].age == lifeSpan) {
                genomes[j].processFitness();
                if (instant != 100 && j == 0)
                console.log("gen " + genomes[0].generation + ":");
                //generate new genomes[j]
                genomes[j].newGeneration();
            }

            genomes[j].update(speed); //To process/move more than once a frame
        }
    }

    gameDraw(); //draw shared game objects

    for (let i = 0; i < genomes.length; i++)
        genomes[i].draw(); //draw to screen

}

function gameDraw() {
    //draw target
    fill("orange")
    noStroke();
    ellipse(target.x, target.y, 30);

    //draw obstacles
    noStroke();
    fill("black");
    rect(obst.x, obst.y, obst.w, obst.h);

    //draw age
    ageP.html(genomes[0].age);
}

function mouseClicked() {
    console.log("mouse location:");
    console.log("x: " + mouseX);
    console.log("y: " + mouseY);
}

function keyPressed() {
    if (keyCode === 107 || keyCode === 189)
        speed *= 2;
    else if ((keyCode === 109 || keyCode === 187) && speed > 1)
        speed /= 2;
    else if (keyCode === 32)
        speed = 1;
    else if (keyCode === 192) {
        if (instant == 1) {//if not instant: switch to instant
            speed = lifeSpan;
            instant = 100;
        } else { //if instant: switch to not instant
            speed = 1;
            instant = 1;
        }
    }
}