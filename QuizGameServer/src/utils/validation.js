exports.validator = (schema) => (payload) => 
    schema.validate(payload, {abortEarly: false});

exports.validate = (concreteValidator, param) => {
    const {error, value} = concreteValidator(param);

    if(error){
        throw error;
    }

    return value;
}