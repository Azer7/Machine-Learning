function f(x) {
    //  y = mx + b
    return .5 * x + 2;
}

class Point {
    constructor() {
        this.x = random(-10, 10);
        this.y = random(-10, 10);
        this.bias = 1;
        if (this.y > f(this.x))
            this.label = 1;
        else
            this.label = -1;
    }

    pixelX() {
        return map(this.x, -10, 10, 0, width);
    }
    
    pixelY() {
        return map(this.y, -10, 10, height, 0);
    }
    
    show() {
        stroke(0);
        if (this.label == 1)
            fill(0);
        else
            fill(255);

        ellipse(this.pixelX(), this.pixelY(), 16);
    }
}