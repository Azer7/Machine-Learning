class Creature {
    constructor(lifeSpan, target, color, dna) {
        this.lifeSpan = lifeSpan;
        this.age = 0;
        this.fitness = 0;
        this.pos = createVector(width / 2, height);
        this.velChange = createVector();
        this.vel = createVector();
        this.target = target;
        this.completed = false;
        this.crashed = false;
        this.show = true;

        this.color = color;
        if (dna)
            this.DNA = dna;
        else
            this.DNA = new DNA(lifeSpan);
    }

    update() {
        if (!this.completed && !this.crashed) {
            this.move();
            this.checkCollision();

            this.age++;
        }
    }

    move() {
        this.velChange = this.DNA.forces[this.age];
        this.vel.add(this.velChange);
        this.pos.add(this.vel);
    }

    checkCollision() {
        //barrier
        if (this.pos.x > obst.x && this.pos.x < obst.x + obst.w && this.pos.y > obst.y - 3 && this.pos.y < obst.y + obst.h + 3) {
            this.crashed = true;
        } else if (this.pos.x < 0 || this.pos.x > width || this.pos.y < 0 || this.pos.y > height) {
            this.crashed = true;
        } else if (this.distToTarget() < 10) { //finish blob
            this.completed = true;
        }
    }

    draw() {
        push();
        translate(this.pos.x, this.pos.y);
        rotate(this.vel.heading());

        fill(this.color, 150);
        //rectMode(CENTER);
        //rect(0, 0, 30, 9);
        triangle(-10, 7, -10, -7, 10, 0);

        pop();
    }

    distToTarget() {
        return Math.sqrt(Math.pow(this.target.x - this.pos.x, 2) + Math.pow(this.target.y - this.pos.y, 2));
    }

    //reproduction functions
    calcFitness() {
        let distance = 0;
        distance = this.distToTarget();

        this.fitness = map(distance, 0, width, width, 0);
        if (this.completed) {
            this.fitness = width;
            this.fitness *= 10
            this.fitness -= this.age * 10;
        }
        else if (this.crashed) {
            this.fitness /= 10;
        }

        if (this.fitness < 0)
            this.fitness = 0;
    }

    crossover(partner) {
        let midpoint = Math.floor(random(this.DNA.length));
        let result = new Creature(this.lifeSpan, this.target, this.color);
        for (let i = 0; i < this.DNA.length; i++) {
            if (i < midpoint) result.DNA.forces[i] = this.DNA.forces[i];
            else result.DNA.forces[i] = partner.DNA.forces[i];
        }
        return result;
    }

    copy() {
        let copy = new Creature(this.lifeSpan, this.target, this.color, this.DNA);
        return copy;
    }

    mutate(rate) {
        for (let i = 0; i < this.DNA.length; i++) {
            if (Math.random() < rate)
                this.DNA.forces[i] = this.DNA.randomVector();
        }
    }
}