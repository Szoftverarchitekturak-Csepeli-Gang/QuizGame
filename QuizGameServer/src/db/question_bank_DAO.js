const { prisma } = require('./db_client.js');

module.exports = {
  createQuestionBank,
  deleteQuestionBank,
  updateQuestionBank,
  getPublicQuestionBanks,
  getUserQuestionBanks,
  getAllQuestionBanks,
  getQuestionBankWithId,
  filterQuestionBanksByTitle,
};

async function createQuestionBank(owner_id, title, public = false) {
  //TODO: check if owner exists

  return await prisma.question_Bank.create({
    data: {
      owner_user_id: owner_id,
      title: title,
      public: public,
    },
  });
}

async function deleteQuestionBank(id) {
    _ = await getQuestionBankWithId(id)
  
    return await prisma.question_Bank.delete({
      where: { id },
    });
}

async function updateQuestionBank(id, owner_id, title, public)
{
  //TODO: check if owner exists
  _ = await getQuestionBankWithId(id)

  return await prisma.question_Bank.update({
    where: { id },
    data: { 
      owner_user_id: owner_id,
      title: title,
      public: public,
    },
  });
}

async function getPublicQuestionBanks(hostId) {
  return await prisma.question_Bank.findMany({
    where : { 
      OR: [
        { owner_user_id: hostId },
        { public: true },
      ],
    }      
  });
}

async function getUserQuestionBanks(owner_id) {
  //TODO: check if owner exists

  return await prisma.question_Bank.findMany({
    where : {owner_user_id: owner_id},
  });
}

async function getAllQuestionBanks() {
  return await prisma.question_Bank.findMany();
}

async function getQuestionBankWithId(id) {
  const questionBank = await prisma.question_Bank.findUnique({
    where: { id },
  });

  if(!questionBank){
    throw new Error(`No question bank with id: ${id}`);
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