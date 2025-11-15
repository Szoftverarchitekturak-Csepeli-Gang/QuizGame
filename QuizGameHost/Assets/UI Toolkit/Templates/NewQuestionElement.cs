using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

[UxmlElement]
public partial class NewQuestionElement : VisualElement
{
    private LocalizedString _newQuestionText;
    public NewQuestionElement()
    {
        Button newQuestionButton = new() { text = "New Question" };
        newQuestionButton.AddToClassList("blue-button");

        _newQuestionText = LocalizationHelper.LoadLocalization("NewQuestion");
        LocalizationHelper.BindLabel(newQuestionButton, _newQuestionText);

        Add(newQuestionButton);

        newQuestionButton.clicked += OnNewQuestionClicked;
    }

    private void OnNewQuestionClicked()
    {
        DashboardScreenController.NewQuestionEvent.Invoke();
    }
}
