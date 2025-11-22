const { prisma } = require('./db-client.js');
const userDao = require('./user.dao.js');

module.exports = {
  createQuestionBank,
  deleteQuestionBank,
  updateQuestionBank,
  getPublicQuestionBanks,
  getUserQuestionBanks,
  getQuestionBankWithId,
  filterQuestionBanksByTitle,
};

async function createQuestionBank({ownerId, title, questions, public = false}) {
  const user = await userDao.getUserById(ownerId);
  if(!user){
    return undefined;
  }

  return await prisma.$transaction(async (trans) => {
    const questionBank = await prisma.question_Bank.create({
      data: {
        ownerId: ownerId,
        title: title,
        public: public,
      },
    });

    for (let question of questions) {
      await trans.question.create({
        data: { 
            text: question.text,
            questionBankId: questionBank.id,
            optionA: question.optionA,
            optionB: question.optionB,
            optionC: question.optionC,
            optionD: question.optionD,
            correctAnswer: question.correctAnswer
        }
      });
    }

    return await trans.question_Bank.findUnique({
      where: { id: questionBank.id },
      include: { 
        questions: {
          select: {
            id: true,
            text: true,
            optionA: true,
            optionB: true,
            optionC: true,
            optionD: true,
            correctAnswer: true
          }
        }
      }
    });
  });
}

async function deleteQuestionBank(id) {
    const questionBank = await getQuestionBankWithId(id)
    if(!questionBank)
    {
      return null;
    }
  
    return await prisma.question_Bank.delete({
      where: { id },
    });
}

async function updateQuestionBank({id, ownerId, title, questions, public})
{
  const user = await userDao.getUserById(ownerId);
  if(!user){
    return undefined;
  }

  const questionBank = await getQuestionBankWithId(id)
  if(!questionBank)
  {
    return null;
  }

  return await prisma.$transaction(async (trans) => {
    await trans.question_Bank.update({
      where: { id },
      data: { 
        ownerId: ownerId,
        title: title,
        public: public,
      },
    });

    if (!questions){
      return await trans.question_Bank.findUnique({
        where: { id: questionBank.id },
        include: { 
          questions: {
            select: {
              id: true,
              text: true,
              optionA: true,
              optionB: true,
              optionC: true,
              optionD: true,
              correctAnswer: true
            }
          }
        }
      });
    };

    const currentQuestions = await trans.question.findMany({
      where: { questionBankId: questionBank.id },
    });
    const currentIds = currentQuestions.map((q) => q.id);

    const incomingIds = questions
      .filter((q) => q.id)
      .map((q) => q.id);

    const idsToDelete = currentIds.filter((id) => !incomingIds.includes(id));

    if (idsToDelete.length > 0) {
      await trans.question.deleteMany({
        where: { id: { in: idsToDelete } },
      });
    }

    const operations = [];

    for (let question of questions) {
      if (question.id > 0) {
        const originalQ = currentQuestions.find(q => q.id === question.id);

        if (originalQ) {
           const hasChanged = 
             originalQ.text !== question.text ||
             originalQ.optionA !== question.optionA ||
             originalQ.optionB !== question.optionB ||
             originalQ.optionC !== question.optionC ||
             originalQ.optionD !== question.optionD ||
             originalQ.correctAnswer !== question.correctAnswer;

           if (!hasChanged) {
             continue;
           }
        }

        operations.push(trans.question.update({
          where: { id: question.id },
          data: { 
              text: question.text,
              optionA: question.optionA,
              optionB: question.optionB,
              optionC: question.optionC,
              optionD: question.optionD,
              correctAnswer: question.correctAnswer
          }
        }));
      } 
      else {
        operations.push(trans.question.create({
          data: { 
              text: question.text,
              questionBankId: questionBank.id,
              optionA: question.optionA,
              optionB: question.optionB,
              optionC: question.optionC,
              optionD: question.optionD,
              correctAnswer: question.correctAnswer
          }
        }));
      }
    }
    
    await Promise.all(operations);

    return await trans.question_Bank.findUnique({
      where: { id: questionBank.id },
      include: { 
        questions: {
          select: {
            id: true,
            text: true,
            optionA: true,
            optionB: true,
            optionC: true,
            optionD: true,
            correctAnswer: true
          }
        }
      }
    });
  });
}

async function getPublicQuestionBanks(hostId) {
  return await prisma.question_Bank.findMany({
    where : { 
      OR: [
        { ownerId: hostId },
        { public: true },
      ],
    }      
  });
}

async function getUserQuestionBanks(ownerId) {
  await userDao.getUserById(ownerId);

  return await prisma.question_Bank.findMany({
    where : {ownerId: ownerId},
  });
}

async function getQuestionBankWithId(id) {
  const questionBank = await prisma.question_Bank.findUnique({
    where: { id },
  });

  if(!questionBank){
    return null;
  }

  return questionBank;
}

async function filterQuestionBanksByTitle(title_filter) {
  if (!title_filter || typeof title_filter !== 'string') {
    return [];
  }

  return await prisma.question_Bank.findMany({
    where: {
      title: {
        contains: title_filter
      },
      public : true,
    },
  });
}