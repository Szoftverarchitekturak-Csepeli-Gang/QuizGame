const { PrismaClient } = require('../generated/prisma')
const bcrypt = require('bcrypt')

const prisma = new PrismaClient()

module.exports = {
    prisma,
    initDatabase,
}

// Filling up the Question_Bank and Question tables if empty with initial data.
async function initDatabase() {
    const userCount = await prisma.user.count();
    let user;
    if(userCount == 0){
      const hashedPassword = await bcrypt.hash('admin', 10);
      user = await prisma.user.create({
        data: {
          username: 'admin',
          password: hashedPassword,
        },
      });
    }
    
    const questionBankCount = await prisma.question_Bank.count();
    if (questionBankCount > 0) {
      console.log('Database already initialized.');
      return;
    }

    console.log('Filling up database...');

    const friedCheeseBank = await prisma.question_Bank.create({
      data: {
        ownerId: user.id,
        title: 'Fried cheese basics',
        public: true,
      },
    });

    const formula1Bank = await prisma.question_Bank.create({
      data: {
        ownerId: user.id,
        title: 'Formula1 basics',
        public: true,
      },
    });

    await prisma.question.createMany({
      data: [
        {
          questionBankId: friedCheeseBank.id,
          text: 'What is the best food in the world?',
          optionA: 'Fried Cheese',
          optionB: 'Cheese (fried)',
          optionC: '2x Fried Cheese',
          optionD: 'Fried Cheese with rice',
          correctAnswer: 1,
        },
        {
          questionBankId: friedCheeseBank.id,
          text: '2 fried cheese + 3 fried cheese equals what?',
          optionA: '5 fried cheese',
          optionB: 'Lunch',
          optionC: 'Stomach ache',
          optionD: 'Bankruptcy',
          correctAnswer: 3,
        },
        {
          questionBankId: formula1Bank.id,
          text: 'Who is going to win the 2025 formula 1 driver championship?',
          optionA: 'Max Verstappen',
          optionB: 'Lando Norris',
          optionC: 'Oscar Piastri',
          optionD: 'Lance Stroll',
          correctAnswer: 2,
        },
        {
          questionBankId: formula1Bank.id,
          text: 'How much horsepower a modern formula 1 car generates?',
          optionA: '650',
          optionB: '1000',
          optionC: 'All of it',
          optionD: 'Some of it',
          correctAnswer: 2,
        },
      ],
    });
}
