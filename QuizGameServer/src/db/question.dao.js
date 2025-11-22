const { prisma } = require('./db-client.js');
const { getQuestionBankWithId } = require('./question-bank.dao.js')

module.exports = {
  getAllQuestionsFromBank,
  getQuestionWithId,
};

async function getAllQuestionsFromBank(questionBankId)
{
    const questionBankExists = await getQuestionBankWithId(questionBankId);
    if (!questionBankExists){
        return null;
    }

    return await prisma.question.findMany({
        where: {questionBankId: questionBankId},
        orderBy: { id: 'asc'},
    });
}

async function getQuestionWithId(id)
{
    return await prisma.question.findUnique(
        {where: { id },
    });
}