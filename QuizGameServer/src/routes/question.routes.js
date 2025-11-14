const express = require('express');
const router = express.Router({mergeParams: true});
const questionController = require('../controllers/question.controller.js');

router.get('/', questionController.getQuestions);
router.get('/:questionId', questionController.getQuestion);
router.post('/', questionController.createQuestion);
router.delete('/:questionId', questionController.deleteQuestion);
router.put('/:questionId', questionController.updateQuestion);

module.exports = router;