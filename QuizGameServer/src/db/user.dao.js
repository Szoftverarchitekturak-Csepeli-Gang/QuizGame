const { prisma } = require('./db-client.js');

module.exports = {
    getUser,
    createUser,
    updateUser,
    deleteUser,
    getUserById,
    getUserByUsername,
};

async function getUserByUsername(username) {
    const user = await prisma.user.findUnique({
    where: { username },
  });

  if(!user){
    return null;
  }

  return user;
}

async function getUserById(id) {
    const user = await prisma.user.findUnique({
    where: { id },
  });

  if(!user){
    return null;
  }

  return user;
}

async function createUser({username, password}) {
    return await prisma.user.create({
        data: {
            username: username,
            password: password
        }
    });
}

async function getUser({username, password}) {
    const user = await getUserByUsername(username);

    if(!user){
        return null;
    }

    return user;
}

async function updateUser({id, username, password}) {
    const user = getUserById(id);

    if(!user){
        return null;
    }

    return await prisma.user.update({
        where: { id },
        data: { 
            username: username,
            password: password,
        },
    });
}

async function deleteUser(id) {
    const user = getUserById(id);

    if(!user){
        return null;
    }

    return await prisma.user.delete({
        where: { id },
    });
}