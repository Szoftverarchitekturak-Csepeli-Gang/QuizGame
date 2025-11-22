const { HttpError } = require("../utils/HttpError");
const jwt = require('jsonwebtoken');

exports.authenticateToken = (req, _res, next) => {
    const token = req.headers['authorization']?.split(' ')[1];

    if (!token) {
        throw new HttpError('Login required!', 401);
    }

    jwt.verify(token, process.env.ACCESS_TOKEN_SECRET, (err, user) => {
        if (err) {
            throw new HttpError('Invalid token!', 403);
        }

        req.user = user;
        
        next();
    });
}