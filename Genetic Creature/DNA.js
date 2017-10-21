class DNA {
    constructor(length) {
        this.length = length;

        this.forces = [];
        for (let i = 0; i < length; i++) {
            this.forces.push(this.randomVector());
        }
    }

    randomVector() {
        let resultVector = p5.Vector.random2D();
        resultVector.setMag(0.1);
        return resultVector;
    }
}