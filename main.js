let neuron;

let guess;
let points = [];

let learningRate = 

function setup() {
	createCanvas(500, 500);
	background("white");

	neuron = new Neuron();

	for (let i = 0; i < 100; i++) {
	    points.push(new Point());
	}

	//console.log(output);
}

function draw() {
    background(255);
    stroke(0);
    line(0, 0, width, height);
    for (let i = 0; i < points.length; i++) {
        points[i].show();
    }
/*
    for (let i = 0; i < points.length; i++) {
        let pInputs = [points[i].x, points[i].y];
        neuron.train(pInputs, points[i].label);
    } */
}