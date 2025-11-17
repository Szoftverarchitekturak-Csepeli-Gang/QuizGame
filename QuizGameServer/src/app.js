const express = require('express');
const http = require('http');
const router = require('./router.js');
const { errorHandler } = require('./middlewares/error.middleware.js')
const dbClient = require('./db/db_client.js')
const socket = require('./socket/socket.js')
const registerSocketEvents = require('./socket/events.js')

const app = express();
const port = process.env.EXPRESS_PORT || 8080;

const server = http.createServer(app);
const io = socket.init(server);
registerSocketEvents(io);

app.use(express.json());
app.use(router);
app.use(errorHandler);

async function startServer() {
  try{
    await dbClient.InitDatabase();
    server.listen(port, () => {
      console.log(`QuizGameServer running at http://localhost:${port}`);
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