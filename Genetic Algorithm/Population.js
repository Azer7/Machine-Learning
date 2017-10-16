class Population {
    constructor(amount, mutationRate, target) {
        //variables
        this.population = [];
        this.size = amount;
        this.target = target;
        this.mutationRate = mutationRate;
        //state
        this.finished = false;
        this.generations = 0;

        //fill population with random characters
        for (let i = 0; i < amount; i++) {
            this.population.push(new Phrase(this.target.length));
        }
    }

    processFitness() {
        for (let i = 0; i < this.size; i++) {
            this.population[i].calcFitness(this.target);
        }
    }

    newGeneration() {
        let futureGen = [];
        let maxFitIndex = 0;
        let totalFitness = 0;

        for (let i = 0; i < this.size; i++) {
            if (this.population[i].fitness > this.population[maxFitIndex].fitness) {
                maxFitIndex = i;
            }
            totalFitness += this.population[i].fitness;
        }
        futureGen.push(this.population[maxFitIndex]);
        for (let i = 0; i < this.size - 1; i++) {
            //get 2 random parents
            let parentA = this.getRandomMember(totalFitness);
            let parentB = this.getRandomMember(totalFitness);
            while (parentA == parentB) {
                parentA = this.getRandomMember(totalFitness);
                parentB = this.getRandomMember(totalFitness);
            }
            let child;
            child = parentA.crossover(parentB);
            child.mutate();
        }
    }

    getRandomMember(totalFitness) {
        let rand = random(0, totalFitness);
        for (let i = 0; i < this.size; i++) {
            if (this.population[i].fitness < rand) {
                return this.population[i];
            }
            rand -= this.population[i].fitness;
        }
    }
}