using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using Assets.SharedAssets.Networking.Mappers;

public class DashboardScreenController : ScreenController
{
    public enum DashboardTab
    {
        QUESTION_BANKS, CREATE_ROOM
    }

    // sidebar components
    private DashboardTab _currentTab;
    private VisualElement _questionBankPage;
    private VisualElement _createRoomPage;
    private Button _questionBankButton;
    private Button _createRoomButton;

    // question banks tab
    VisualElement _questionBankSettings;
    Label _noQuestionBanksText;
    private DropdownField _questionBanksOfUserDropdown;
    [SerializeField] List<QuestionEditElement> questions = new();
    private ScrollView _questionsScrollView;
    private NewQuestionElement _newQuestionElement;
    public static UnityEvent NewQuestionEvent = new();
    public static UnityEvent<QuestionEditElement> DeleteQuestionEvent = new();

    // create room tab
    private DropdownField _questionBanksOfUserDropdownCreateRoom;
    [SerializeField, HideInInspector] private string _searchInput;
    private List<QuestionBank> _searchedQuestionBanks = new();
    private ListView _bankSearchList;

    // fetched data
    [SerializeField, HideInInspector] private string _bankName;
    private List<QuestionBank> _questionBanksOfUser = new();

    void OnEnable()
    {
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Host Asset Table");

        SetupCreateRoomTab();
        SetupQuestionBankTab();
        SetupTabs();
    }

    private void SetupCreateRoomTab()
    {
        Button waitingRoomButton = _ui.Q<Button>("WaitingRoomBtn");
        waitingRoomButton.clicked += OnCreateRoomClicked;

        _questionBanksOfUserDropdownCreateRoom = _ui.Q<DropdownField>("ChooseBankDropdownCreateRoom");
        _questionBanksOfUserDropdownCreateRoom.RegisterValueChangedCallback(evt =>
        {
            int selectedIndex = _questionBanksOfUserDropdownCreateRoom.index;
            if (selectedIndex >= 0 && selectedIndex < _questionBanksOfUser.Count)
            {
                QuestionBank selected = _questionBanksOfUser[selectedIndex];
                _bankName = selected.Name;
                _bankSearchList.selectedIndex = -1;
                Debug.Log($"Selected '{selected.Name}' with ID = {selected.Id}");
            }
        });

        RoomManager.Instance.RoomCreated += handleRoomCreated;

        SetupBankSearch();
    }

    private void SetupBankSearch()
    {
        TextField searchInputField = _ui.Q<TextField>("SearchInputField");
        _bankSearchList = _ui.Q<ListView>("QuestionBankSearchList");
        _bankSearchList.fixedItemHeight = 42;
        _bankSearchList.makeItem = () =>
        {
            Label label = new Label();
            label.AddToClassList("list-element");
            return label;
        };

        _bankSearchList.bindItem = (element, index) =>
        {
            Label label = (Label)element;
            label.AddToClassList("list-element");
            label.text = _searchedQuestionBanks[index].Name;
        };

        _bankSearchList.itemsSource = _searchedQuestionBanks;

        _bankSearchList.selectionChanged += OnBankSearchListSelection;

        searchInputField.RegisterValueChangedCallback(async evt =>
        {
            if (_searchInput.Length < 1) return;
            _searchedQuestionBanks.Clear();
            var results = await FetchQuestionBanks(_searchInput);
            _searchedQuestionBanks.AddRange(results);

            _bankSearchList.Rebuild();
        });
    }

    private void OnBankSearchListSelection(IEnumerable<object> enumerable)
    {
        int selectedIndex = _bankSearchList.selectedIndex;
        if (selectedIndex >= 0 && selectedIndex < _questionBanksOfUser.Count)
        {
            QuestionBank selected = _searchedQuestionBanks[selectedIndex];
            _bankName = selected.Name;
            _questionBanksOfUserDropdownCreateRoom.index = -1;
        }
    }

    private async void OnCreateRoomClicked()
    {
        int selectedIndex = _questionBanksOfUserDropdownCreateRoom.index;
        if (selectedIndex >= 0 && selectedIndex < _questionBanksOfUser.Count)
        {
            var selectedQuestionBank = _questionBanksOfUser[selectedIndex];
            await RoomManager.Instance.CreateRoom(selectedQuestionBank.Id);
        }
    }

    private void SetupTabs()
    {
        _questionBankPage = _ui.Q<VisualElement>("QuestionBankPage");
        _createRoomPage = _ui.Q<VisualElement>("CreateRoomPage");

        _questionBankButton = _ui.Q<Button>("QuestionBankBtn");
        _questionBankButton.clicked += () => ShowPage(DashboardTab.QUESTION_BANKS);

        _createRoomButton = _ui.Q<Button>("CreateRoomBtn");
        _createRoomButton.clicked += () => ShowPage(DashboardTab.CREATE_ROOM);

        Button logoutButton = _ui.Q<Button>("LogoutBtn");
        logoutButton.clicked += OnLogoutClicked;

        ShowPage(DashboardTab.QUESTION_BANKS, true);
    }

    private void OnLogoutClicked()
    {
        // TODO: handle logout logic
        ScreenManagerBase.Instance.CurrentScreen = AppScreen.MAIN;
    }

    private void ShowPage(DashboardTab dashboardTab, bool firstLoad = false)
    {
        if (_currentTab == dashboardTab && !firstLoad) return;

        if (dashboardTab == DashboardTab.QUESTION_BANKS)
        {
            _questionBankPage.RemoveFromClassList("hide");
            _createRoomPage.AddToClassList("hide");
            _questionBankButton.AddToClassList("active-tab");
            _createRoomButton.RemoveFromClassList("active-tab");
            LoadBanksOfUser();
        }
        else
        {
            _questionBankPage.AddToClassList("hide");
            _createRoomPage.RemoveFromClassList("hide");
            _questionBankButton.RemoveFromClassList("active-tab");
            _createRoomButton.AddToClassList("active-tab");
        }

        _currentTab = dashboardTab;
    }

    private void ResetQuestionScrollView()
    {
        _questionsScrollView.contentContainer.Clear();
        questions.Clear();
        _newQuestionElement = new();
        _questionsScrollView.contentContainer.Add(_newQuestionElement);
    }

    private void SetupQuestionBankTab()
    {
        _questionsScrollView = _ui.Q<ScrollView>("BankEditList");

        NewQuestionEvent.AddListener(OnNewQuestionCreated);
        DeleteQuestionEvent.AddListener(DeleteQuestion);

        ResetQuestionScrollView();

        Button saveButton = _ui.Q<Button>("SaveBtn");
        saveButton.clicked += OnSaveButtonClicked;

        _questionBanksOfUserDropdown = _ui.Q<DropdownField>("ChooseBankDropdown");
        _questionBanksOfUserDropdown.RegisterValueChangedCallback(evt =>
        {
            int selectedIndex = _questionBanksOfUserDropdown.index;
            if (selectedIndex >= 0 && selectedIndex < _questionBanksOfUser.Count)
            {
                QuestionBank selected = _questionBanksOfUser[selectedIndex];
                LoadQuestionBank(selected);
                Debug.Log($"Selected '{selected.Name}' with ID = {selected.Id}");
            }
        });

        Button newBankButton = _ui.Q<Button>("NewBankBtn");
        newBankButton.clicked += OnNewBankClicked;

        Button deleteBankButton = _ui.Q<Button>("DeleteBankBtn");
        deleteBankButton.clicked += OnDeleteBankClicked;

        LoadBanksOfUser();
    }

    private async void OnDeleteBankClicked()
    {
        int selectedIndex = _questionBanksOfUserDropdown.index;
        if (selectedIndex >= 0 && selectedIndex < _questionBanksOfUser.Count)
        {
            QuestionBank selected = _questionBanksOfUser[selectedIndex];
            var response = await NetworkManager.Instance.DeleteQuestionBank(selected.Id);
            _questionBanksOfUserDropdown.index = -1;
            SetupQuestionBankTab();
        }
    }

    private void OnNewBankClicked()
    {
        _questionBanksOfUserDropdown.index = -1;
        ResetQuestionScrollView();
        _questionBankSettings.RemoveFromClassList("hide");
        _noQuestionBanksText.AddToClassList("hide");
    }

    private async void OnSaveButtonClicked()
    {
        int selectedIndex = _questionBanksOfUserDropdown.index;
        if (selectedIndex >= 0 && selectedIndex < _questionBanksOfUser.Count)
        {
            QuestionBank selected = _questionBanksOfUser[selectedIndex];
            var response = await NetworkManager.Instance.UpdateQuestionBank(selected.Id, 1,selected.Name, questions.Select(q => q.GetQuestion()).ToList(), true);
            if(!response.IsSuccess)
            {
                return;
            }
        }
        else
        {
            var response = await NetworkManager.Instance.CreateQuestionBank(1, "tesztbank", questions.Select(q => q.GetQuestion()).ToList(), true);
            if(!response.IsSuccess)
            {
                return;
            }
        }

        _questionBanksOfUserDropdown.index = -1;
        SetupQuestionBankTab();
    }

    private void DeleteQuestion(QuestionEditElement element)
    {
        _questionsScrollView.contentContainer.Remove(element);
        questions.Remove(element);
    }

    private void OnNewQuestionCreated()
    {
        CreateNewQuestion();
    }

    private void CreateNewQuestion(Question question = null)
    {
        QuestionEditElement questionEditElement = new();
        questionEditElement.BindQuestion(question ?? new Question());
        questions.Add(questionEditElement);
        _questionsScrollView.contentContainer.Remove(_newQuestionElement);
        _questionsScrollView.contentContainer.Add(questionEditElement);
        _questionsScrollView.contentContainer.Add(_newQuestionElement);
    }

    private async Task<List<QuestionBank>> FetchQuestionBanks(string search = "", int userID = -1)
    {
        //TODO: show error message on UI (response.ErrorMessage is !response.isSuccess)
        var response = await NetworkManager.Instance.GetQuestionBanks(search, userID);
        return response.IsSuccess ? response.Data : new List<QuestionBank>();
    }

    private async void LoadBanksOfUser()
    {
        _questionBanksOfUser = await FetchQuestionBanks(userID: 1);

        var dropdownList = _questionBanksOfUser.Select(r => r.Name).ToList();
        _questionBanksOfUserDropdown.choices = dropdownList;
        _questionBanksOfUserDropdownCreateRoom.choices = dropdownList;

        _questionBankSettings = _ui.Q<VisualElement>("QuestionBankSettings");
        _noQuestionBanksText = _ui.Q<Label>("NoQuestionBanksText");

        if (_questionBanksOfUser.Count > 0)
        {
            _questionBankSettings.RemoveFromClassList("hide");
            _noQuestionBanksText.AddToClassList("hide");
        }
        else
        {
            _questionBankSettings.AddToClassList("hide");
            _noQuestionBanksText.RemoveFromClassList("hide");
        }

    }

    private async void LoadQuestionBank(QuestionBank selected)
    {
        _bankName = selected.Name;
        ResetQuestionScrollView();
        List<Question> questionsInBank = await FetchQuestions(selected.Id);
        foreach (Question question in questionsInBank)
        {
            CreateNewQuestion(question);
        }
    }

    private async Task<List<Question>> FetchQuestions(int id)
    {
        var response = await NetworkManager.Instance.GetQuestionsFromBank(id);
        return response.IsSuccess ? response.Data : new List<Question>();
    }

    private void handleRoomCreated()
    {
        ScreenManagerBase.Instance.CurrentScreen = AppScreen.WAITROOM;
    }

    public override void ResetUIState()
    {
        _searchInput = _bankName = "";

        if (_bankSearchList != null)
        {
            _bankSearchList.selectedIndex = -1;
        }

        if (_questionBanksOfUserDropdown != null)
        {
            _questionBanksOfUserDropdown.index = -1;
        }

        ResetQuestionScrollView();
    }
}
