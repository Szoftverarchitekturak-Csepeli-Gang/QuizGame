const jwt = require('jsonwebtoken');

module.exports = {
    validateSocketEvent,
}


function validateSocketEvent(payload)
{
  try {
    const token = typeof payload === "object" ? payload.token : null;

    if (!token) {
        return false;
    }

    jwt.verify(token, process.env.ACCESS_TOKEN_SECRET);
    return true;

  } catch (err) {
    return false;
  }
}