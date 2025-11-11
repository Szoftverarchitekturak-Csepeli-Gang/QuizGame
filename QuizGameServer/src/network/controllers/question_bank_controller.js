const express = require('express');
const router = express.Router();

module.exports = router

const question_bank_DAO = require('../../db/question_bank_DAO.js')

//Returns all question banks.
router.get("/", async (req, res) => {
    const quenstionBanks = await question_bank_DAO.getAllQuestionBanks()
    res.status(200).send(quenstionBanks);
})

//Returns question bank created by a user
router.get("/user/:userId", async (req, res) => {
    //TODO: Check if user exists
    const user_id = parseInt(req.params.userId, 10);

    if (isNaN(user_id)) {
        return res.status(400).send("Invalid user ID");
    }

    const quenstionBanks = await question_bank_DAO.getUserQuestionBanks(user_id)
    res.status(200).send(quenstionBanks)
})

//Create new question bank
router.post("/", async (req, res) => {
  const { owner_user_id, title, public } = req.body;

  //TODO: check if user exists

  if (!owner_user_id || !title) {
    return res.status(400).json({ error: "Missing owner_user_id or title" });
  }

  try {
    const newBank = await question_bank_DAO.createQuestionBank(
      owner_user_id,
      title,
      public ?? false
    );
    console.log("Question bank created:", newBank)
    res.status(201).json(newBank); // 201 Created
  } catch (error) {
    console.error("Create failed:", error);
    res.status(500).json({ error: "Failed to create question bank" });
  }
});

//Returns question bank with given id (null if nonexistent).
router.get("/:id", async (req, res) => {
    const questionBank_id = parseInt(req.params.id, 10);

    if (isNaN(questionBank_id)) {
        return res.status(400).send("Invalid ID");
    }

    const quenstionBank = await question_bank_DAO.getQuestionBankWithId(questionBank_id)
    res.status(200).send(quenstionBank);
})

//Returned filtered question banks (filtered by title).
router.get("/filter/:title_filter", async (req, res) => {
    const title_filter = req.params.title_filter;

    const quenstionBanks = await question_bank_DAO.filterQuestionBanksByTitle(title_filter)
    res.status(200).send(quenstionBanks);
})

//Delete question bank
router.delete("/:id", async (req, res) => {
  const id = parseInt(req.params.id, 10);

  if (isNaN(id)) {
    return res.status(400).json({ error: "Invalid ID" });
  }

  try {
    const deletedBank = await question_bank_DAO.deleteQuestionBank(id);
    console.log("Question bank deleted:", deletedBank)
    res.status(200).json({ message: "Question bank deleted", id: deletedBank.id });
  } catch (error) {
    if (error.message.includes("No question bank")) {
      return res.status(404).json({ error: error.message });
    }
    console.error("Delete failed:", error);
    res.status(500).json({ error: "Failed to delete question bank" });
  }
});

//Modify question bank
router.put("/:id", async (req, res) => {
  const id = parseInt(req.params.id, 10);
  if (isNaN(id)) {
    return res.status(400).json({ error: "Invalid ID" });
  }

  const { owner_user_id, title, public } = req.body;

  if (!owner_user_id || !title || public === undefined) {
    return res.status(400).json({ error: "Invalid question bank" });
  }

  try {
    const updatedBank = await question_bank_DAO.updateQuestionBank(
      id,
      owner_user_id,
      title,
      public
    );
    console.log("Question bank updated: ", updatedBank);
    res.status(200).json(updatedBank);
  } catch (error) {
    if (error.message.includes("No question bank")) {
      return res.status(404).json({ error: error.message });
    }
    console.error("Update failed:", error);
    res.status(500).json({ error: "Failed to update question bank" });
  }
});
