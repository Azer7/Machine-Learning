class Creature {
    constructor(lifeSpan) {
        this.forces = [];
        this.lifeSpan = lifeSpan;
        this.position = createVector();
        this.velocity = createVector();
        this.velChange = createVector();
        
        this.fitness = 0;
        for (let i = 0; i < this.lifeSpan; i++) {
            this.forces.push(randomVector());
        }
    }

    calcFitness(target) {
        let score = 0;

        this.fitness = score / target.length;
        this.fitness = pow(this.fitness, 4) + 0.0001;
    }

    crossover(crossPhrase) {
        let midpoint = Math.floor(random(this.length));
        let result = new Phrase(this.length);
        for (let i = 0; i < this.length; i++) {
            if (i < midpoint) result.chars[i] = this.chars[i];
            else result.chars[i] = crossPhrase.chars[i];
        }
        return result;
    }

    update() {
        
    }
    
    mutate() {
        for (let i = 0; i < this.length; i++) {
            if (Math.random() < mutationRate)
                this.chars[i] = randChar();
        }
    }
    
}

function randomVector() {
    let resultVector = p5.Vector.random2D();
    resultVector.setMag(0.1);
    return resultVector();
}