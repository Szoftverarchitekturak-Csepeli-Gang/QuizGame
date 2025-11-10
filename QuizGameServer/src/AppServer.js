const db_client = require('./db/db_client.js')
const question_bank_DAO = require('./db/question_bank_DAO.js')

async function main() {
    await db_client.InitDatabase();

    try{
      const questionBank = await question_bank_DAO.createQuestionBank(2, "TestBank")
      console.log(questionBank);

      await question_bank_DAO.updateQuestionBank(questionBank.id, 2, "TestBankModified", true)
      const questionBanks = await question_bank_DAO.getAllQuestionBanks()
      console.log(questionBanks);

      await question_bank_DAO.deleteQuestionBank(questionBank.id)

    } catch (error) {
      console.log(error);     
    }
}

main()
  .then(async () => {
    await db_client.prisma.$disconnect()
  })
  .catch(async (e) => {
    console.error(e)
    await db_client.prisma.$disconnect()
    process.exit(1)
  })