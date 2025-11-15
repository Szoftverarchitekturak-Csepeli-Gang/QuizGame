const question_bank_DAO = require('../db/question_bank_DAO.js');
const { tryHandle } = require('../utils/try-handle.js');
const { createValidator, updateValidator, getQueryValidator } = require('../validators/question-bank.validator.js');
const { validate } = require('../utils/validation.js');

const getQuestionBanks = tryHandle(async (req , res) => {
    const value = validate(getQueryValidator, req.query);
    let questionBanks = [];

    if(value.ownerId){
        questionBanks = await question_bank_DAO.getUserQuestionBanks(parseInt(value.ownerId));
    }
    
    if(value.title){
        questionBanks = await question_bank_DAO.filterQuestionBanksByTitle(value.title);
    }

    res.status(200).json(questionBanks);
});

const getQuestionBank = tryHandle(async (req, res) => {
    const questionBank = await question_bank_DAO.getQuestionBankWithId(parseInt(req.params.questionBankId));
    res.status(200).json({questionBank: questionBank});
});

const createQuestionBank = tryHandle(async (req, res) => {
    const value = validate(createValidator, req.body);

    const questionBank = await question_bank_DAO.createQuestionBank(value.ownerId, value.title, value.public);
    res.status(201).json({questionBank: questionBank});  
});

const updateQuestionBank = tryHandle(async (req, res) => {
    const value = validate(updateValidator, req.body);

    const questionBank = await question_bank_DAO.updateQuestionBank(parseInt(req.params.questionBankId), value.ownerId, value.title, value.public);
    res.status(200).json({questionBank: questionBank});  
});

const deleteQuestionBank = tryHandle(async (req, res) => {
    const questionBank = await question_bank_DAO.deleteQuestionBank(parseInt(req.params.questionBankId));
    console.log("Question bank deleted: ", questionBank)
    res.status(200).json(questionBank); 
});

module.exports = {
    getQuestionBanks,
    getQuestionBank,
    createQuestionBank,
    updateQuestionBank,
    deleteQuestionBank,
};
