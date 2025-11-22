const questionDao = require('../db/question.dao.js');
const { tryHandle } = require('../utils/try-handle.js');

const getQuestions = tryHandle(async (req, res) => {
    const questions = await questionDao.getAllQuestionsFromBank(parseInt(req.params.questionBankId));
    if(!questions){
        throw new HttpError(`No QuestionBank with id: ${questionBankId}`, 404);
    }
    res.status(200).json(questions);
});

const getQuestion = tryHandle(async (req, res) => {
    const question = await questionDao.getQuestionWithId(parseInt(req.params.questionId));
    res.status(200).json(question);
});

module.exports = {
    getQuestions,
    getQuestion,
};