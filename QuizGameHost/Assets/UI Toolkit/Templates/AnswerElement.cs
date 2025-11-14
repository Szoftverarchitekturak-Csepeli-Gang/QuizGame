using System;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UIElements;

public partial class AnswerElement : VisualElement
{
    private int _answerIdx;
    private VisualElement _completionBar;

    public AnswerElement(string labelPath, string completionPath, int answerIdx)
    {
        _completionBar = new();
        _completionBar.AddToClassList("completion-bar");
        _completionBar.SetBinding("style.flexGrow", new DataBinding
        {
            dataSourcePath = new PropertyPath(completionPath),
            bindingMode = BindingMode.ToTarget,
        });
        Add(_completionBar);

        Label answerLabel = new();
        answerLabel.SetBinding("text", new DataBinding
        {
            dataSourcePath = new PropertyPath(labelPath),
            bindingMode = BindingMode.ToTarget
        });
        answerLabel.AddToClassList("answer-label");
        Add(answerLabel);

        _answerIdx = answerIdx;
        //GameManager.CorrectAnswerIdx += OnCorrectAnswerIdxSet;

        SetCorrectAnswer(true);
    }

    ~AnswerElement()
    {
        //GameManager.CorrectAnswerIdx -= OnCorrectAnswerIdxSet;
    }

    private void OnCorrectAnswerIdxSet(int idx)
    {
        SetCorrectAnswer(idx == _answerIdx);
    }

    public void SetCorrectAnswer(bool isCorrect)
    {
        if (isCorrect)
        {
            _completionBar.AddToClassList("correct-answer");
        }
        else
        {
            _completionBar.RemoveFromClassList("correct-answer");
        }
    }
}