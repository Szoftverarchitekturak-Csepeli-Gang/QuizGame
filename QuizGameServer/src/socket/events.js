const { createRoomHandler,
        joinRoomHandler,
        leaveRoomHandler,
        startGameHandler,
        startRoundHandler,
        answerReceivedHandler  } = require('./event-handlers.js')

module.exports = (io) => {
  io.on("connection", (socket) => {
    console.log("Socket connected:", socket.id);

    socket.on("createRoom", async (questionBankId) => {
      await createRoomHandler(questionBankId, socket);
    });

    socket.on("joinRoom", async (roomID) => {
      await joinRoomHandler(roomID, socket);
    });

    socket.on("leaveRoom", async () => {
      await leaveRoomHandler(socket);
    });
    
    socket.on("gameStarted", async () => {
      await startGameHandler(socket)
    });

    socket.on("submitAnswer", async (answer) => {
      await answerReceivedHandler(answer, socket)
    });

    socket.on("nextRoundStarted", async (roundIdx) => {
      await startRoundHandler(roundIdx, socket)
    });

    socket.on("disconnect", async () => {
      await leaveRoomHandler(socket)
    });
  });
};