class Phrase {
    constructor(length) {
        this.chars = [];
        this.length = length;
        this.fitness = 0;
        for (let i = 0; i < this.length; i++) {
            this.chars.push(randChar());
        }
    }

    getWord() {
        return this.chars.join("");
    }

    calcFitness(target) {
        let score = 0;
        for (let i = 0; i < this.chars.length; i++) {
            if (this.chars[i] == target.charAt(i))
                score++;
        }
        this.fitness = score / target.length;
    }

    crossover(crossPhrase) {
        let midpoint = random(0, this.length);
        let result = new Phrase(this.length);
        for (let i = 0; i < this.length; i++) {
            if (i < midpoint) result.chars[i] = this.chars[i];
            else result.chars[i] = crossPhrase[i];
        }
        return result;
    }

    mutate() {
        for (let i = 0; i < this.length; i++) {
            if (Math.random < mutationRate)
                this.chars[i] = randChar();
        }
    }
}

function randChar() {
    let charCode = floor(random(63, 122));
    if (charCode == 63)
        charCode = 32;
    else if (charCode == 64)
        charCode = 46;

    return String.fromCharCode(charCode);
}
