class String() {
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
        this.fitness = 0;
        for(let i = 0; i < this.chars.length; i++) {
            if(this.chars[i] == this.target.charAt(i))
                this.fitness++;
        }
    }
    
    
}

function randChar() {
    charCode = charCode(63, 122);
    if(charCode == 63)
        charCode = 32;
    else if(charCode == 64)
        charCode = 46;
    
    return fromCharCode(charCode);
}