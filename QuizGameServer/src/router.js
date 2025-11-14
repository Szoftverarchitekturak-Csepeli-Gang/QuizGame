const express = require('express');
const router = express.Router();
const questionBankRoutes = require('./routes/question-bank.routes.js');
const questionRoutes = require('./routes/question.routes.js');

router.use('/questionbanks',questionBankRoutes);
router.use('/questionbanks/:questionBankId/questions', questionRoutes);

module.exports = router;