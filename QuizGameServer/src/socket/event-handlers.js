const { generateRoomId } = require("../utils/room-id-generator.js");
const { getIO } = require("./socket.js");
const room_DAO  = require("../db/room.dao.js");
const question_DAO  = require("../db/question.dao.js");

module.exports = {
    createRoomHandler,
    joinRoomHandler,
    leaveRoomHandler,
    startGameHandler,
    startRoundHandler,
    answerReceivedHandler,
    GameFinishedHandler,
}

async function createRoomHandler(questionBankId, hostSocket)
{
    const existingRoom = await room_DAO.getRoomWithHostId(hostSocket.id);
    if(existingRoom)
    {
        hostSocket.emit("error", "Already has an active room!");
        return;
    }

    const roomID = await generateRoomId();
    try{
        await room_DAO.createRoom(roomID, hostSocket.id, questionBankId);
        hostSocket.join(roomID);
        hostSocket.data.roomId = roomID;
        console.log("Room created: ", roomID);
        hostSocket.emit("roomCreated", roomID);
    }
    catch(error){
        console.log("Error creating room: ", error.message);
        hostSocket.emit("error", error.message);
    }
}

async function startGameHandler(hostSocket)
{
    const existingRoom = await room_DAO.getRoomWithHostId(hostSocket.id);
    if(!existingRoom)
    {
        hostSocket.emit("error", "No active room with this hostID");
        return;
    }

    try{
        await room_DAO.closeRoom(existingRoom.id);
        hostSocket.to(existingRoom.id).emit("gameStarted");
    }
    catch(error){
        console.log("Error closing room: ", error.message);
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
    else if(!existingRoom.isOpen)
    {
        clientSocket.emit("error", "Room is closed");
        return;
    }
    else if(clientSocket.data.roomID)
    {
        clientSocket.emit("error", "Already connected to a room");
        return;
    }

    clientSocket.emit("joinedRoom", roomId)
    clientSocket.join(roomId);
    clientSocket.data.roomId = roomId;
    clientSocket.to(existingRoom.hostId).emit("clientConnected");
}

async function startRoundHandler(roundIdx, socket)
{
    const existingRoom = await room_DAO.getRoomWithHostId(socket.id);
    if(existingRoom == null){
        socket.emit("error", "No room associated with this socket");
        return;
    }

    const questionsInBank = await question_DAO.getAllQuestionsFromBank(existingRoom.questionBankId);
    if(questionsInBank.length <= roundIdx)
    {
        socket.emit("error", "Question index was out of bounds");
        return;
    }
    const io = getIO();
    io.to(existingRoom.id).emit("newQuestion", questionsInBank[roundIdx]);
}

async function answerReceivedHandler(answer, socket)
{
    if (!Number.isInteger(answer) || answer < 1 || answer > 4) {
        socket.emit("error", "Invalid answer: must be a number between 1 and 4");
        return;
    }

    const roomID = socket.data.roomId;
    if(!roomID)
    {
        socket.emit("error", "No room associated with this socket");
        return;
    }

    const existingRoom = await room_DAO.getRoomWithID(roomID);
    if(existingRoom == null){
        socket.emit("error", "No room associated with this socket");
        return;
    }
    const hostSocketId = existingRoom.hostId;
    socket.to(hostSocketId).emit("answerReceived",answer);
}

async function leaveRoomHandler(socket)
{
    const existingRoom = await room_DAO.getRoomWithHostId(socket.id);
    const wasHost = existingRoom != null;

    if(wasHost){
        const roomId = existingRoom.id;
        socket.to(roomId).emit("hostDisconnected");
        await room_DAO.deleteRoom(roomId);
        const roomSockets = await socket.in(roomId).fetchSockets();
        for (const playerSocket of roomSockets) {
            playerSocket.data.roomId = null;
            playerSocket.leave(roomId);
        }
        console.log("Room deleted: ", roomId);
    }
    else
    {
        const roomId = socket.data.roomId;
        if (roomId) {
            socket.to(roomId).emit("clientDisconnected");
            socket.leave(roomId);
            socket.data.roomId = null;
        }
    }
}

async function GameFinishedHandler(socket)
{
    const existingRoom = await room_DAO.getRoomWithHostId(socket.id);
    const wasHost = existingRoom != null;

    if(wasHost){
        const roomId = existingRoom.id;
        socket.to(roomId).emit("gameEnded");
    }
}
