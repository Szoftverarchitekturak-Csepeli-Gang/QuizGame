const Joi = require('joi');
const { validator } = require('../utils/validation.js');

const createSchema = Joi.object({
    ownerId: Joi.number()
        .integer()
        .min(0)
        .required(),
    title: Joi.string()
        .trim()
        .min(5)
        .required(),
    public: Joi.boolean()
        .optional(),
});

const updateSchema = Joi.object({
    ownerId: Joi.number()
        .integer()
        .min(0)
        .required(),
    title: Joi.string()
        .trim()
        .min(5)
        .required(),
    public: Joi.boolean()
        .required(),
});

const getQuerySchema = Joi.object({
    ownerId: Joi.number()
        .integer()
        .min(0)
        .optional(),
    title: Joi.string()
        .trim()
        .min(1)
        .optional(),
})
.or('ownerId', 'title');

exports.createValidator = validator(createSchema);
exports.updateValidator = validator(updateSchema);
exports.getQueryValidator = validator(getQuerySchema);