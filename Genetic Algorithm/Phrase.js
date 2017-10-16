class Phrase {
    constructor(length) {
        this.chars = [];
        this.fitness = 0;
        for(let i = 0; i < length; i++) {
            this.chars.push(randChar());
        }
    }
    
    getWord() {
        return this.chars.join("");   
    }
    
    calcFitness(target) {   
        let score = 0;
        for(let i = 0; i < this.chars.length; i++) {
            if(this.chars[i] == target.charAt(i))
                score++;
        }
        this.fitness = score / target.length;
    }  
}

function randChar() {
    let charCode = floor(random(63, 122));
    if(charCode == 63)
        charCode = 32;
    else if(charCode == 64)
        charCode = 46;
    
    return String.fromCharCode(charCode);
}
