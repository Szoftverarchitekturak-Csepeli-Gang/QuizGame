const { generateRoomId } = require("../utils/room-id-generator.js");
const room_DAO  = require("../db/room_DAO.js");

module.exports = {
    createRoomHandler,
    disconnectHandler,
    joinRoomHandler,
    leaveRoomHandler,
}

async function createRoomHandler(questionBankId, hostSocket)
{
    const roomID = await generateRoomId();
    try{
        await room_DAO.createRoom(roomID, hostSocket.id, questionBankId);
        hostSocket.join(roomID);
        console.log("Room created: ", roomID);
        hostSocket.emit("roomCreated", roomID);
    }
    catch(error){
        console.log("Error creating room: ", error.message);
        hostSocket.emit("error", error.message);
    }
}

async function joinRoomHandler(roomId, clientSocket)
{
    const existingRoom = await room_DAO.getRoomWithID(roomId);
    if(existingRoom == null){
        clientSocket.emit("error", "Wrong room ID");
        return;
    }

    clientSocket.emit("joinedRoom", roomId)
    clientSocket.join(roomId);
    clientSocket.data.roomId = roomId;
    clientSocket.to(existingRoom.hostId).emit("clientConnected");
}

async function leaveRoomHandler(roomId, clientSocket)
{
    const clientRoom = clientSocket.data.roomId;
    if(roomId != clientRoom){
        clientSocket.emit("error", "Wrong room ID");
        return;
    }
    clientSocket.to(roomId).emit("clientDisconnected")
    clientSocket.leave(roomId);
    clientSocket.data.roomId = null;
}

async function disconnectHandler(socket) {
    console.log("Socket disconnected:", socket.id);

    const existingRoom = await room_DAO.getRoomWithHostId(socket.id);
    const wasHost = existingRoom != null;

    if(wasHost){
        const roomId = existingRoom.id;
        socket.to(roomId).emit("hostDisconnected");
        await room_DAO.deleteRoom(roomId);
        console.log("Room deleted: ", roomId);
    }
    else
    {
        const roomId = socket.data.roomId;
        if (roomId) {
            socket.to(roomId).emit("clientDisconnected");
        }
    }
}