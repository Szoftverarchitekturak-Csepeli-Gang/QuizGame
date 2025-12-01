const { createRoomHandler,
        joinRoomHandler,
        leaveRoomHandler,
        startGameHandler,
        startRoundHandler,
        answerReceivedHandler,  
        GameFinishedHandler} = require('./event-handlers.js')

module.exports = (io) => {
  io.on("connection", (socket) => {
    console.log("Socket connected:", socket.id);

    socket.on("createRoom", async (payload) => {
      await createRoomHandler(payload, socket);
    });

    socket.on("joinRoom", async (roomID) => {
      await joinRoomHandler(roomID, socket);
    });

    socket.on("leaveRoom", async () => {
      await leaveRoomHandler(socket);
    });
    
    socket.on("gameStarted", async (payload) => {
      await startGameHandler(payload, socket)
    });

    socket.on("gameFinished", async (payload) => {
      await GameFinishedHandler(payload, socket)
    });

    socket.on("submitAnswer", async (answer) => {
      await answerReceivedHandler(answer, socket)
    });

    socket.on("nextRoundStarted", async (payload) => {
      await startRoundHandler(payload, socket)
    });

    socket.on("disconnect", async () => {
      console.log("Socket disconnected: ", socket.id);
      await leaveRoomHandler(socket)
    });
  });
};