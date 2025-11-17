const {createRoomHandler, disconnectHandler, joinRoomHandler, leaveRoomHandler} = require('./event-handlers.js')

module.exports = (io) => {
  io.on("connection", (socket) => {
    console.log("Socket connected:", socket.id);

    socket.on("createRoom", async (questionBankId) => {
      await createRoomHandler(questionBankId, socket);
    });

    socket.on("joinRoom", async (roomID) => {
      await joinRoomHandler(roomID, socket);
    });

    socket.on("leaveRoom", async (roomID) => {
      await leaveRoomHandler(roomID, socket);
    });

    socket.on("disconnect", async () => {
      await disconnectHandler(socket)
    });
  });
};