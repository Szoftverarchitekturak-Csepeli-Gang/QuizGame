const Joi = require('joi');
const { validator } = require('../utils/validation.js');

const message = {
    'any.invalid' : 'The answer options cannot be the same.'
}

const createUpdateSchema = Joi.object({
    id: Joi.number()
        .integer()
        .min(0)
        .optional(),
    questionBankId: Joi.number()
        .integer()
        .min(0)
        .optional(),
    text: Joi.string()
        .trim()
        .min(5)
        .required(),
    optionA: Joi.string()
        .trim()
        .min(1)
        .required(),
    optionB: Joi.string()
        .trim()
        .min(1)
        .required()
        .invalid(Joi.ref('optionA'))
        .messages(message),
    optionC: Joi.string()
        .trim()
        .min(1)
        .required()
        .invalid(Joi.ref('optionA'), Joi.ref('optionB'))
        .messages(message),
    optionD: Joi.string()
        .trim()
        .min(1)
        .required()
        .invalid(Joi.ref('optionA'), Joi.ref('optionB'), Joi.ref('optionC'))
        .messages(message),
    correctAnswer: Joi.number()
        .integer()
        .valid(1,2,3,4)
        .required(),
});

exports.questionCUValidator = validator(createUpdateSchema);