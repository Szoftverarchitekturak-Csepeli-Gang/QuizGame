exports.errorHandler = (error, _req, res, _next) => {
    if(error.name === "ValidationError"){
        const messages = error.details.map(detail => detail.message);

        return res.status(400).send({
            type: "Validation Error",
            details: messages,
        });
    }

    if(error.message){
        console.log(error.message)
        return res.status(404).send({
            type: "Not Found",
            details: error.message,
        });
    }

    return res.status(500).send("Something went wrong!");
}