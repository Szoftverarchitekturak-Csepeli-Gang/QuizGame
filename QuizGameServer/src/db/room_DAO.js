const { prisma } = require('./db_client.js');
const { getQuestionBankWithId } = require('./question_bank_DAO.js')

module.exports = {
  createRoom,
  deleteRoom,
  getAllRooms,
  getRoomWithID,
  getRoomWithHostId,
  closeRoom,
  openRoom,
};

async function createRoom(id, hostId, questionBankId, isOpen = true) {
  //TODO: check if user exists

  const existingRoom = await getRoomWithID(id);
  if (existingRoom) {
    throw new Error(`Room with the following id already exist: ${id}`);
  }

  const existingHost = await getRoomWithHostId(hostId)
  if (existingHost) {
    throw new Error(`There is already an existing room with this host`);
  }

  const questionBankExists = await getQuestionBankWithId(questionBankId)
  if (!questionBankExists) {
    throw new Error(`No question bank with id: ${questionBankId}`);
  }

  return await prisma.room.create({
    data: {
      id,
      hostId: hostId,
      questionBankId: questionBankId,
      isOpen,
    },
  });
}

async function deleteRoom(id) {
  const exists = await getRoomWithID(id);
  if (!exists) {
    throw new Error(`No room with id: ${id}`);
  }

  return await prisma.room.delete({
    where: { id },
  });
}

async function getAllRooms() {
  return await prisma.room.findMany();
}

async function getRoomWithID(id) {
  return await prisma.room.findUnique({
    where: { id },
  });
}

async function getRoomWithHostId(hostId) {
  return await prisma.room.findUnique({
    where: { hostId: hostId },
  });
}

async function closeRoom(id) {
  const exists = await getRoomWithID(id);
  if (!exists) {
    throw new Error(`No room with id: ${id}`);
  }

  return await prisma.room.update({
    where: { id },
    data: { isOpen: false },
  });
}

async function openRoom(id) {
  const exists = await getRoomWithID(id);
  if (!exists) {
    throw new Error(`No room with id: ${id}`);
  }

  return await prisma.room.update({
    where: { id },
    data: { isOpen: true },
  });
}