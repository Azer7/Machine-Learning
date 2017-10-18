class Population {
    constructor(amount, lifeSpan, mutationRate, targetLocation) {
        //variables
        this.population = [];
        this.size = amount;
        this.lifeSpan = lifeSpan;
        this.targetLocation = targetLocation;
        this.mutationRate = mutationRate;
        //state
        this.finished = false;
        this.generation = 0;

        this.bestIndex = 0;
        this.totalFitness;
        //fill population with random characters
        for (let i = 0; i < amount; i++) {
            this.population.push(new Creature(lifeSpan));
        }
    }

    processFitness() {
        for (let i = 0; i < this.size; i++) {
            this.population[i].calcFitness(this.target);
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
        
        //form new generation
        futureGen.push(this.population[this.bestIndex]);
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
            child.mutate();
            futureGen.push(child);
        }
        this.population = futureGen;
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
    
    evaluate() { //should be called after newGeneration()
        if(this.population[this.bestIndex].getWord() == this.target)
            this.finished = true;
        
        let averageFitness = this.totalFitness / this.size;
        
        let info = $("#info");
        info.empty();
        //display info
        info.append("Best phrase:  " + this.population[this.bestIndex].getWord() + "<br/>");
        info.append("Average fitness:  " + Math.round(averageFitness*1000)/1000 + "<br/>");
        info.append("Time Taken:  " + Math.round((performance.now() - timeStart)) + "ms <br/>");
    
        let phraseBox = $("#phrase-box");
        
        phraseBox.empty();
        for(let i = 0; i < (this.size < 150 ? this.size : 150); i++) {
            phraseBox.append(this.population[i].getWord() + "<br/>");
        }
    }
}