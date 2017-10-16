class Population {
    constructor(amount, mutationRate, target) {
        //variables
        this.population = [];
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
        for (let i = 0; i < this.population.length; i++) {
            this.population[i].calcFitness(this.target);
        }
    }

    newGeneration() {
        let futureGen = [];
        let maxFitIndex = 0;
        let totalFitness = 0;

        for (let i = 0; i < this.population.length; i++) {
            if (this.population[i].fitness > this.population[maxFitIndex].fitness) {
                maxFitIndex = i;
            }
            totalFitness += this.population[i].fitness;
        }
        let parentA = this.getRandomMember(totalFitness);
        let parentB = this.getRandomMember(totalFitness);
        
        
    }

    getRandomMember(totalFitness) {
        let rand = random(0, totalFitness);
        for (let i = 0; i < this.population.length; i++) {
            if (this.population[i].fitness < rand) {
                return this.population[i];     
            }
            rand -= this.population[i].fitness;
        }
    }
}