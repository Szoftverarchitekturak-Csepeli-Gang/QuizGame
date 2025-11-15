const express = require('express');
const router = require('./router.js');
const { errorHandler } = require('./middlewares/error.middleware.js');
const dbClient = require('./db/db_client.js')

const app = express();
const port = process.env.EXPRESS_PORT || 8080;

app.use(express.json());
app.use(router);
app.use(errorHandler);

async function startServer() {
  try{
    await dbClient.InitDatabase();
    app.listen(port, () => {
      console.log(`QuizGameServer listening at http://localhost:${port}`);
    });
  }
  catch(error){
    console.error('Critical error:', error);
    process.exit(1);
  }
  finally{
    await dbClient.prisma.$disconnect();
  }
}

startServer();