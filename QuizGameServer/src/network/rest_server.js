const express = require('express');

const question_bank_DAO = require('../db/question_bank_DAO.js')

const restAPI = express();
restAPI.use(express.json())

module.exports = restAPI

//Returns all question banks.
restAPI.get("/questionbanks", async (req, res) => {
    const quenstionBanks = await question_bank_DAO.getAllQuestionBanks()
    res.send(quenstionBanks);
})

//Returns question bank with given id (null if nonexistent).
restAPI.get("/questionbanks/:id", async (req, res) => {
    const questionBank_id = parseInt(req.params.id, 10);

    if (isNaN(questionBank_id)) {
        return res.status(400).send("Invalid ID");
    }

    const quenstionBank = await question_bank_DAO.getQuestionBankWithId(questionBank_id)
    res.send(quenstionBank);
})

//Returned filtered question banks (filtered by title).
restAPI.get("/questionbanks/filter/:title_filter", async (req, res) => {
    const title_filter = req.params.title_filter;

    const quenstionBanks = await question_bank_DAO.filterQuestionBanksByTitle(title_filter)
    res.send(quenstionBanks);
})
