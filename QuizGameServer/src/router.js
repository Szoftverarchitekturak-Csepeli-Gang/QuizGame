const express = require('express');
const router = express.Router();
const questionBankRoutes = require('./routes/question-bank.routes.js');
const questionRoutes = require('./routes/question.routes.js');
const authRoutes = require('./routes/auth.routes.js');

router.use('/', authRoutes);
router.use('/questionbanks', questionBankRoutes);
router.use('/questionbanks/:questionBankId/questions', questionRoutes);

module.exports = router;