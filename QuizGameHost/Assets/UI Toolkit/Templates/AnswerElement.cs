using Unity.Properties;
using UnityEngine.Events;
using UnityEngine.UIElements;

public partial class AnswerElement : VisualElement
{
    private int _answerIdx;
    private VisualElement _completionBar;
    private readonly UnityEvent<int> _idxSetEvent;

    public AnswerElement(string labelPath, string completionPath, int answerIdx, UnityEvent<int> idxSetEvent)
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

        _idxSetEvent = idxSetEvent;
        _idxSetEvent.AddListener(OnCorrectAnswerIdxSet);

        SetCorrectAnswer(true);
    }

    ~AnswerElement()
    {
        _idxSetEvent.RemoveListener(OnCorrectAnswerIdxSet);
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