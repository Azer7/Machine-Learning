class Population() {
    constructor(amount, mutationRate, target) {
        //variables
        this.population = [];
        this.target = target;
        this.mutationRate = mutationRate;
        //state
        this.finished = false;
        this.generations = 0;
        
        //fill population with random characters
        for(let i = 0; i < amount; i++) {
            this.population.push(new String(this.target.length));
        }
    }
    
    processFitness() {
        for(let i = 0; i < this.population.length; i++) {
           this.population[i].calcFitness();    
            
        }
    }
    
}