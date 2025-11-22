const userDao = require('../db/user.dao.js');
const { tryHandle } = require('../utils/try-handle.js');
const { userGetValidator, userCreateValidator } = require('../validators/user.validator.js');
const { validate } = require('../utils/validation.js');
const bcrypt = require('bcrypt');
const jwt = require('jsonwebtoken');
const { HttpError } = require('../utils/HttpError.js');
const SALT_ROUNDS = 10;

const login = tryHandle(async (req, res) => {
    const value = validate(userGetValidator, req.body);

    const user = await userDao.getUser({ username: value.username, password: value.password });
    if(!user){
        throw new HttpError(`No User with username: ${value.username}`, 404);
    }

    const isPasswordValid = await bcrypt.compare(value.password, user.password);
    if (!isPasswordValid) {
        throw new HttpError('Invalid username or password!', 401);
    }

    const token = jwt.sign(
        { id: user.id, username: user.username },
        process.env.ACCESS_TOKEN_SECRET,
        { expiresIn: '6h' }
    );
    res.status(200).json({token, user: { id: user.id, username: user.username }});
});

const register = tryHandle(async (req, res) => {
    const value = validate(userCreateValidator, req.body);
    const existingUser = await userDao.getUserByUsername(value.username);
    
    if (existingUser) {
        throw new HttpError(`Username is taken: ${value.username}`, 401);
    }

    const hashedPassword = await bcrypt.hash(value.password, SALT_ROUNDS);
    const user = await userDao.createUser({ username: value.username, password: hashedPassword });
    const token = jwt.sign(
        { id: user.id, username: user.username },
        process.env.ACCESS_TOKEN_SECRET,
        { expiresIn: '6h' }
    );
    res.status(200).json({token, user: { id: user.id, username: user.username }});
});

module.exports = {
    login,
    register,
};