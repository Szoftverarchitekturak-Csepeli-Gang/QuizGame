const express = require('express');
const router = express.Router({mergeParams: true});
const questionController = require('../controllers/question.controller.js');
const { authenticateToken } = require('../middlewares/auth.middleware.js');

//router.use(authenticateToken);
router.get('/', questionController.getQuestions);
router.get('/:questionId', questionController.getQuestion);

module.exports = router;