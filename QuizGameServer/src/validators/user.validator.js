const Joi = require('joi');
const { validator } = require('../utils/validation.js');

const getUserSchema = Joi.object({
    username: Joi.string()
        .min(5)
        .required(),
    password: Joi.string()
        .min(5)
        .required()
});

const createUserSchema = Joi.object({
    username: Joi.string()
        .min(5)
        .required(),
    password: Joi.string()
        .min(5)
        .required()
});

exports.userGetValidator = validator(getUserSchema);
exports.userCreateValidator = validator(createUserSchema);