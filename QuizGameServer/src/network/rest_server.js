const express = require('express');
const restAPI = express();

restAPI.use(express.json())

const questionBankRouter = require('./controllers/question_bank_controller.js');
restAPI.use('/questionbanks', questionBankRouter);

module.exports = restAPI