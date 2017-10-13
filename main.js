let neuron;
let points = [];

let manual = false;
let errorAmount = Infinity;

function setup() {
    createCanvas(500, 500);
    background("white");

    neuron = new Neuron(3);

    for (let i = 0; i < 100; i++) {
        points.push(new Point());
    }
}

function draw() {
    background(255);
    stroke(0);
    line(pixelX(-10), pixelY(f(-10)), pixelX(10), pixelY(f(10)));

    line(pixelX(-10), pixelY(neuron.lineY(-10)), pixelX(10), pixelY(neuron.lineY(10)));

    if(frameCount % 10 == 0 && errorAmount == 0) {
        points.push(new Point());
    }
    
    for (let i = 0; i < points.length; i++) {
        points[i].show();
    }

    errorAmount = 0;
    for (let i = 0; i < points.length; i++) {
        let pInputs = [points[i].x, points[i].y, points[i].bias];
        if (!manual)
            neuron.train(pInputs, points[i].label);
        let guess = neuron.guess(pInputs);
        if (guess == points[i].label)
            fill(0, 255, 0);
        else {
            errorAmount++;
            fill(255, 0, 0);
        }
        ellipse(points[i].pixelX(), points[i].pixelY(), 8, 8);
    }
}

function pixelX(x) {
    return map(x, -10, 10, 0, width);
}

function pixelY(y) {
    return map(y, -10, 10, height, 0);
}

function mouseClicked() {
    if (manual) {
        for (let i = 0; i < points.length; i++) {
            let pInputs = [points[i].x, points[i].y, points[i].bias];
            neuron.train(pInputs, points[i].label);
        }
    }
}
