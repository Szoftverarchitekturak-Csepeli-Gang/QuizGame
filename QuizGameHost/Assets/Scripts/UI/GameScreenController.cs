using System;
using UnityEngine;
using UnityEngine.UIElements;

public enum RoundResult
{ 
    VICTORY,
    FAILURE
}

public class GameScreenController : ScreenController
{
    private VisualElement _resultElement;
    [SerializeField, HideInInspector] private string _resultText;

    protected override void Awake()
    {
        base.Awake();
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Host Asset Table");
        _resultElement = _ui.Q<VisualElement>("ResultStatusElement");
        
        QuestionPanelInit();
        VillagePanelInit();
    }

    void Start()
    {
        GameScreenPresenter.Instance.AttachScreen(this);
    }

    void OnDestroy()
    {
        GameScreenPresenter.Instance.DetachScreen();
    }

    void OnEnable()
    {
        _resultText = "DEFEAT";
        QuestionPanelEnable();
        VillagePanelEnable();
    }

    void OnDisable()
    {
        QuestionPanelDisable();
        VillagePanelDisable();
    }

    #region VillagePanel

    [SerializeField, HideInInspector]
    private string _villageName;
    private VisualElement _villageImage;
    private VisualElement _selectVillageElement;

    private Button _conquerButton;
    private Button _exitButton;

    private Action _onConquer;
    private Action _onExit;

    private void VillagePanelInit()
    {
        _selectVillageElement = _ui.Q<VisualElement>("SelectVillageElement");
        _conquerButton = _ui.Q<Button>("ConquerBtn");
        _exitButton = _ui.Q<Button>("SelectVillageExitBtn");
        _villageImage = _ui.Q<VisualElement>("VillageImage");
    }

    private void VillagePanelEnable()
    {
        _conquerButton.clicked += HandleConquerClicked;
        _exitButton.clicked += HandleExitClicked;
    }

    private void VillagePanelDisable()
    {
        _conquerButton.clicked -= HandleConquerClicked;
        _exitButton.clicked -= HandleExitClicked;
    }

    public void ShowVillagePanel(GameObject village, Action onConquer, Action onExit)
    {
        if (village == null)
            return;

        _onConquer = onConquer;
        _onExit = onExit;

        var villageController = village.GetComponent<VillageController>();
        var villageName = villageController.Info.name;

        Texture2D villageImg = Resources.Load<Texture2D>(villageName);
        _villageName = villageName;
        _villageImage.style.backgroundImage = villageImg;

        _selectVillageElement.RemoveFromClassList("hide");
    }

    public void HideVillagePanel()
    {
        _onConquer = null;
        _onExit = null;

        _selectVillageElement.AddToClassList("hide");
    }

    private void HandleConquerClicked()
    {
        _onConquer?.Invoke();
    }

    private void HandleExitClicked()
    {
        _onExit?.Invoke();
    }

    #endregion

    #region QuestionPanel

    private QuestionDisplayElement _questionDisplayElement;


    private void QuestionPanelInit()
    {
        _questionDisplayElement = _ui.Q<QuestionDisplayElement>();
    }

    private void QuestionPanelEnable()
    {
    }

    private void QuestionPanelDisable()
    {
    }

    private void HandleAnswerReceived(int count)
    {
        //Todo: Show answers count on panel
    }

    private void HandleTimeTick(int time)
    { 
        //Todo: Show time on panel
    }

    public void ShowQuestionDisplay(Question question)
    {
        _questionDisplayElement.LoadQuestion(question);
        _questionDisplayElement.ResetPercentages();

        _questionDisplayElement.RemoveFromClassList("hide");
    }

    public void HideQuestionDisplay()
    {
        _questionDisplayElement.AddToClassList("hide");
    }

    #endregion

    public void OnGameRoundEnded(RoundResult result)
    {
        if (result == RoundResult.VICTORY)
        {
            _resultText = "VICTORY";
            _resultElement.AddToClassList("result-text-victory");
            _resultElement.RemoveFromClassList("result-text-defeat");
        }
        else
        {
            _resultText = "DEFEAT";
            _resultElement.AddToClassList("result-text-defeat");
            _resultElement.RemoveFromClassList("result-text-victory");
        }

        _resultElement.RemoveFromClassList("result-text-start");

        Invoke(nameof(ResetResultText), 2f);
    }

    public void ResetResultText()
    {
        _resultElement.AddToClassList("result-text-start");
    }

    private void ShowResultsStatistics(Question question, float[] percentages)
    {
        _questionDisplayElement.LoadQuestion(question);
        _questionDisplayElement.LoadPercentages(percentages);
        _questionDisplayElement.RemoveFromClassList("hide");
    }
}