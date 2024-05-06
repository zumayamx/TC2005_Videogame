'use strict'
import express from 'express'
const app = express();
app.use(express.json());

import { readJSON } from '../../utils.js';
import { validateCard, validatePartialCard } from '../../schemas/card.js';

const cards = readJSON('./cards.json');

/* GET method to get all the cards */
app.get('/', (req, res) => {
    
    if (cards.length === 0 ){
        return res.status(200).json({ message: "There is no cards in the list" });
    } 
        
    res.status(200).json(cards);
})
/* GET method to get a specific card by id */
app.get('/:id', (req, res) => {
    
    const id = req.params.id;
    const card = cards.find(card => card.id == id);
    if (card  == undefined) {
        return res.status(200).json({ message:"Card not found" });
    }
        
    res.status(200).json(card);
    
})
/* POST method to add a new card */
app.post('/', (req, res) => {

    //const id = req.body.id;
    const cardFromRequest = req.body;
    let cardsAdded = 0;
    let cardsNotAdded = 0;

    cardFromRequest.forEach(card => {
        const cardIndex = cards.findIndex(c => c.id == card.id);
        const result = validateCard(card);

        if ( result && cardIndex == -1) {
            cards.push(card);
            cardsAdded++;
        }
        else {
            cardsNotAdded++;
        }
    })
    res.status(200).json({ message: `Cards added: ${cardsAdded}, Cards NOT added: ${cardsNotAdded}` });
})
/* DELETE method to delete a card by id */
app.delete('/:id', (req, res) => {
    const id = Number(req.params.id)
    const cardIndex = cards.findIndex(card => card.id == id);
    
    if ( cardIndex == -1 ) {
        return res.status(200).json({ message:"Card not found to delete" });
    }
    
    cards.splice(cardIndex, 1);
    res.status(200).json({ message:"Card deleted succefully" });

    
})
/* PUT method to update a card by id */
app.put('/:id', (req, res) => {
    const id = (req.params.id);
    const cardIndex = cards.findIndex(card => card.id == id);
    const up_elements = req.body;
    const result = validatePartialCard(req.body);

    if ( cardIndex == -1 || !result ) {
        return res.status(200).json({ message:"Card NOT available to update" });
    }

    cards[cardIndex] = {
        ...cards[cardIndex],
        ...up_elements
    }  
    res.status(200).json( {message:"Card updated succesfully"} )
})


const PORT = process.env.PORT ?? 3000

app.listen(PORT, () => {
    console.log(`Server listening on port http://localhost:${PORT}`)
  })