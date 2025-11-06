using System;
using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class NewQuestionElement : VisualElement
{
    public NewQuestionElement()
    {
        Button newQuestionButton = new() { text = "New Question" };
        Add(newQuestionButton);

        newQuestionButton.clicked += OnNewQuestionClicked;
    }

    private void OnNewQuestionClicked()
    {
        DashboardScreenController.NewQuestionEvent.Invoke();
    }
}
