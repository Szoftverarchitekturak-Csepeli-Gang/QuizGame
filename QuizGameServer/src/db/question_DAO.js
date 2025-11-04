const { prisma } = require('./db_client.js');
const { getQuestionBankWithId } = require('./question_bank_DAO.js')

module.exports = {
  createQuestion,
  deleteQuestion,
  updateQuestion,
  getAllQuestions,
  getAllQuestionsFromBank,
  getQuestionWithId,
};

async function createQuestion(questionBank_id, text, optionA, optionB, optionC, optionD, correctAnswer) {
  const questionBankExists = await getQuestionBankWithId(questionBank_id);
  if (!questionBankExists) throw new Error(`No question bank with id: ${questionBank_id}`);

  if(correctAnswer > 4 || correctAnswer < 1) throw new Error(`Correct answer should be in range 1-4`);

  return await prisma.question.create({
    data: {
        question_bank_id: questionBank_id,
        text: text,
        optionA: optionA,
        optionB: optionB,
        optionC: optionC,
        optionD: optionD,
        correctAnswer: correctAnswer
    },
  });
}

async function deleteQuestion(id) {
    const exists = await getQuestionWithId(id);
    if (!exists) {
      throw new Error(`No question with id: ${id}`);
    }
  
    return await prisma.question.delete({
      where: { id },
    });
}

async function updateQuestion(id, questionBank_id, text, optionA, optionB, optionC, optionD, correctAnswer)
{
  const exists = await getQuestionWithId(id);
  if (!exists) {
    throw new Error(`No question with id: ${id}`);
  }

  const questionBankExists = await getQuestionBankWithId(questionBank_id);
  if (!questionBankExists) throw new Error(`No question bank with id: ${questionBank_id}`);

  if(correctAnswer > 4 || correctAnswer < 1) throw new Error(`Correct answer should be in range 1-4`);

  return await prisma.question.update({
    where: { id },
    data: {
        question_bank_id: questionBank_id,
        text: text,
        optionA: optionA,
        optionB: optionB,
        optionC: optionC,
        optionD: optionD,
        correctAnswer: correctAnswer
    },
  });
}

async function getAllQuestions()
{
    return await prisma.question.findMany();
}

async function getAllQuestionsFromBank(questionBank_id)
{
    const questionBankExists = await getQuestionBankWithId(questionBank_id);
    if (!questionBankExists) throw new Error(`No question bank with id: ${questionBank_id}`);

    return await prisma.question.findMany({
        where: {question_bank_id: questionBank_id},
        orderBy: { id: 'asc'},
    });
}

async function getQuestionWithId(id)
{
    return await prisma.question.findUnique(
        {where: { id },
    });
}