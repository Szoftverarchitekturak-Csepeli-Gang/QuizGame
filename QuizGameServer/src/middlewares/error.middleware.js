const { HttpError } = require("../utils/HttpError");

exports.errorHandler = (error, _req, res, _next) => {
    if(error.name === "ValidationError"){
        const messages = error.details.map(detail => detail.message);

        console.log(messages);
        return res.status(400).send({
            message: messages,
        });
    }

    if(error instanceof HttpError){
        console.log(error);
        return res.status(error.statusCode).send({
            message: error.message,
        });
    }

    console.log(error);
    return res.status(500).send("Something went wrong!");
}