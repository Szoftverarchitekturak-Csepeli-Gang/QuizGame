const questionDao = require('../db/question_DAO.js');
const { tryHandle } = require('../utils/try-handle.js');
const { createUpdateValidator } = require('../validators/question.validator.js');
const { validate } = require('../utils/validation.js');

const createQuestion = tryHandle(async (req, res) => {
    const value = validate(createUpdateValidator, req.body);

    const question = await questionDao.createQuestion(parseInt(req.params.questionBankId), value.text, value.optionA, value.optionB, value.optionC, value.optionD, value.correctAnswer);
    res.status(201).json({question: question});
});

const updateQuestion = tryHandle(async (req, res) => {
    const value = validate(createUpdateValidator, req.body);

    const question = await questionDao.updateQuestion(parseInt(req.params.questionId), parseInt(req.params.questionBankId), value.text, value.optionA, value.optionB, value.optionC, value.optionD, value.correctAnswer);
    res.status(200).json({question: question});
});

const deleteQuestion = tryHandle(async (req, res) => {
    const question = await questionDao.deleteQuestion(parseInt(req.params.questionId));
    res.status(200).json({question: question});
});

const getQuestions = tryHandle(async (req, res) => {
    const questions = await questionDao.getAllQuestionsFromBank(parseInt(req.params.questionBankId));
    res.status(200).json(questions);
});

const getQuestion = tryHandle(async (req, res) => {
    const question = await questionDao.getQuestionWithId(parseInt(req.params.questionId));
    res.status(200).json({question: question});
});

module.exports = {
    createQuestion,
    updateQuestion,
    deleteQuestion,
    getQuestions,
    getQuestion,
};