const {getRoomWithID} = require("../db/room_DAO");

exports.generateRoomId = async () => {
    let roomId;
    let alreadyExists;
    
    do {
        roomId = Math.floor(100000 + Math.random() * 900000);
        alreadyExists = await getRoomWithID(roomId);
    } while (alreadyExists);

    return roomId;
}