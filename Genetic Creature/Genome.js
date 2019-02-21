class Genome {
    constructor(amount, lifeSpan, mutationRate, target, color) {
        //variables
        this.population = [];
        this.size = amount;
        this.age = 0;
        this.lifeSpan = lifeSpan;
        this.target = target;
        this.mutationRate = mutationRate;
        //state
        this.finished = false;
        this.generation = 0;

        this.bestIndex = 0;
        this.totalFitness;

        //fill population with random characters
        for (let i = 0; i < amount; i++) {
            this.population.push(new Creature(lifeSpan, target, color));
        }
    }

    processFitness() {
        for (let i = 0; i < this.size; i++) {
            this.population[i].calcFitness();
        }
    }

    newGeneration() {
        let futureGen = [];
        this.bestIndex = 0;
        this.totalFitness = 0;

        for (let i = 0; i < this.size; i++) {
            if (this.population[i].fitness > this.population[this.bestIndex].fitness) {
                this.bestIndex = i;
            }
            this.totalFitness += this.population[i].fitness;
        }

        if (this.population[this.bestIndex].completed == true) {
            this.mutationRate = 0.01;
        }
        
        if (instant != 100) {
            console.log(this.population[this.bestIndex].age);
        }
            //form new generation
        futureGen.push(this.population[this.bestIndex].copy());
        futureGen[0].show = true;
        for (let i = 0; i < this.size - 1; i++) {
            //get 2 random parents
            let parentA = this.getRandomMember(this.totalFitness);
            let parentB = this.getRandomMember(this.totalFitness);
            while (parentA == parentB) {
                parentA = this.getRandomMember(this.totalFitness);
                parentB = this.getRandomMember(this.totalFitness);
            }
            let child;
            child = parentA.crossover(parentB);
            child.mutate(this.mutationRate);
            futureGen.push(child);
        }
        this.population = futureGen;
        this.age = 0;
        this.generation++;
    }

    getRandomMember(totalFitness) {
        let rand = random(0, totalFitness);
        for (let i = 0; i < this.size; i++) {
            if (rand < this.population[i].fitness) {
                return this.population[i];
            }
            rand -= this.population[i].fitness;
        }
    }

    update(times) {
        for (let i = 0; i < times && this.age < this.lifeSpan; i++) {
            this.age++;
            for (let j = 0; j < this.size; j++) {
                this.population[j].update();
            }
        }
    }

    draw() {
        //draw creatures
        for (let i = 0; i < this.size; i++) {
            if (this.population[i].show)
                this.population[i].draw();
        }
    }
}