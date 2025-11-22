const questionBankDAO = require('../db/question-bank.dao.js');
const { tryHandle } = require('../utils/try-handle.js');
const { createValidator, updateValidator, getQueryValidator } = require('../validators/question-bank.validator.js');
const { questionCUValidator} = require('../validators/question.validator.js');
const { validate } = require('../utils/validation.js');
const { HttpError } = require('../utils/HttpError.js');

const getQuestionBanks = tryHandle(async (req , res) => {
    const value = validate(getQueryValidator, req.query);
    let questionBanks = [];

    if(value.ownerId){
        questionBanks = await questionBankDAO.getUserQuestionBanks(parseInt(value.ownerId));
    }
    
    if(value.title){
        questionBanks = await questionBankDAO.filterQuestionBanksByTitle(value.title);
    }

    res.status(200).json(questionBanks);
});

const getQuestionBank = tryHandle(async (req, res) => {
    const questionBank = await questionBankDAO.getQuestionBankWithId(parseInt(req.params.questionBankId));
    if(!questionBank)
    {
        throw new HttpError(`No QuestionBank with id: ${req.params.questionBankId}`, 404);
    }

    res.status(200).json(questionBank);
});

const createQuestionBank = tryHandle(async (req, res) => {
    const value = validate(createValidator, req.body);
    validateQuestions(value.questions);

    const questionBank = await questionBankDAO.createQuestionBank(value);
    console.log("Question bank created: ", questionBank);
    res.status(201).json(questionBank);  
});

const updateQuestionBank = tryHandle(async (req, res) => {
    const value = validate(updateValidator, req.body);
    validateQuestions(value.questions);

    const questionBank = await questionBankDAO.updateQuestionBank(value);
    if(questionBank === undefined){
        throw new HttpError(`No User with id: ${value.ownerId}`, 404);
    }
    if(questionBank == null){
        throw new HttpError(`No QuestionBank with id: ${value.id}`, 404);
    }
    console.log("Question bank updated: ", questionBank);
    res.status(200).json(questionBank);  
});

const deleteQuestionBank = tryHandle(async (req, res) => {
    const questionBank = await questionBankDAO.deleteQuestionBank(parseInt(req.params.questionBankId));
    if(!questionBank){
        throw new HttpError(`No QuestionBank with id: ${req.params.questionBankId}`, 404);
    }
    console.log("Question bank deleted: ", questionBank);
    res.status(200).json(questionBank); 
});

const validateQuestions = (questions) => {    
    if(!questions || questions.length < 0){
        return;
    }

    for(let question of questions){
        validate(questionCUValidator, question);
    }   
};

module.exports = {
    getQuestionBanks,
    getQuestionBank,
    createQuestionBank,
    updateQuestionBank,
    deleteQuestionBank,
};
