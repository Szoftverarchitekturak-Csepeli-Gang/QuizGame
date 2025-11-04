using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class DashboardScreenController : ScreenController
{
    public enum DashboardTab
    {
        QUESTION_BANKS, CREATE_ROOM
    }

    private DashboardTab _currentTab;
    private VisualElement _questionBankPage;
    private VisualElement _createRoomPage;

    [SerializeField] List<QuestionEditElement> questions = new();
    private ScrollView _questionsScrollView;
    private NewQuestionElement _newQuestionElement;
    public static UnityEvent NewQuestionEvent = new();
    public static UnityEvent<QuestionEditElement> DeleteQuestionEvent = new();

    void OnEnable()
    {
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Host Asset Table");

        SetupTabs();
        SetupQuestionBankTab();
    }

    private void SetupTabs()
    {
        _questionBankPage = _ui.Q<VisualElement>("QuestionBankPage");
        _createRoomPage = _ui.Q<VisualElement>("CreateRoomPage");

        ShowPage(DashboardTab.QUESTION_BANKS);

        Button questionBankButton = _ui.Q<Button>("QuestionBankBtn");
        questionBankButton.clicked += () => ShowPage(DashboardTab.QUESTION_BANKS);

        Button createRoomButton = _ui.Q<Button>("CreateRoomBtn");
        createRoomButton.clicked += () => ShowPage(DashboardTab.CREATE_ROOM);

        Button logoutButton = _ui.Q<Button>("LogoutBtn");
        logoutButton.clicked += OnLogoutClicked;
    }

    private void OnLogoutClicked()
    {
        // TODO: handle logout logic
        ScreenManagerBase.Instance.ShowScreen(AppScreen.MAIN);
    }

    private void ShowPage(DashboardTab dashboardTab)
    {
        if (_currentTab == dashboardTab) return;

        if (dashboardTab == DashboardTab.QUESTION_BANKS)
        {
            _questionBankPage.RemoveFromClassList("hide");
            _createRoomPage.AddToClassList("hide");
        }
        else
        {
            _questionBankPage.AddToClassList("hide");
            _createRoomPage.RemoveFromClassList("hide");
        }

        _currentTab = dashboardTab;
    }

    private void SetupQuestionBankTab()
    {
        _questionsScrollView = _ui.Q<ScrollView>("BankEditList");

        NewQuestionEvent.AddListener(CreateNewQuestion);
        DeleteQuestionEvent.AddListener(DeleteQuestion);

        _newQuestionElement = new();
        _questionsScrollView.contentContainer.Add(_newQuestionElement);

        Button saveButton = _ui.Q<Button>("SaveBtn");
        saveButton.clicked += OnSaveButtonClicked;
    }

    private void OnSaveButtonClicked()
    {
        // TODO: send question to server
        throw new NotImplementedException();
    }

    private void DeleteQuestion(QuestionEditElement element)
    {
        _questionsScrollView.contentContainer.Remove(element);
        questions.Remove(element);
    }

    private void CreateNewQuestion()
    {
        QuestionEditElement questionEditElement = new();
        questionEditElement.BindQuestion(new Question());
        questions.Add(questionEditElement);
        _questionsScrollView.contentContainer.Remove(_newQuestionElement);
        _questionsScrollView.contentContainer.Add(questionEditElement);
        _questionsScrollView.contentContainer.Add(_newQuestionElement);
    }
}
