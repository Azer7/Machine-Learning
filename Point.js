class Point {
    constructor() {
        this.x = random(0, width);
        this.y = random(0, height);
        if (this.x > this.y)
            this.label = 1;
        else
            this.label = -1;
    }

    show() {
        stroke(0);
        if (this.label == 1)
            fill(0);
        else
            fill(255);

        ellipseMode("CENTER");
        ellipse(this.x, this.y, 7);
    }
}