const express = require('express');
const router = express.Router();
const questionBankController = require("../controllers/question-bank.controller.js");
const { authenticateToken } = require('../middlewares/auth.middleware.js');

//router.use(authenticateToken);
router.get('/', questionBankController.getQuestionBanks);
router.get('/:questionBankId', questionBankController.getQuestionBank);
router.post('/', questionBankController.createQuestionBank);
router.delete('/:questionBankId', questionBankController.deleteQuestionBank);
router.put('/:questionBankId', questionBankController.updateQuestionBank);

module.exports = router;