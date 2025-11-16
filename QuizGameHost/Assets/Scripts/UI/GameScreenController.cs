using System;
using Unity.Properties;
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
    [SerializeField, HideInInspector] private float _timer;

    protected override void Awake()
    {
        base.Awake();
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Host Asset Table");
        _resultElement = _ui.Q<VisualElement>("ResultStatusElement");

        TimerElementInit();
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

    #region Timer

    private ProgressBar _questionTimer;
    private VisualElement _progressBarColor;

    private void TimerElementInit()
    {
        _questionTimer = _ui.Q<ProgressBar>("GameViewProgress");
        HideTimer();

        _progressBarColor = _questionTimer.Q<VisualElement>(className: "unity-progress-bar__progress");
        _progressBarColor.AddToClassList("blue-background");
    }

    public void InitTimer(float max)
    {
        _questionTimer.RemoveFromClassList("hide");
        _questionTimer.lowValue = 0f;
        _questionTimer.highValue = max;
    }

    public void HideTimer()
    {
        _questionTimer.AddToClassList("hide");
    }

    public void HandleQuestionTimer(float time)
    {
        //Todo: Show time on panel
        _timer = time;

        float p = time / _questionTimer.highValue;
        if (p > 0.33f && p <= 0.66f)
        {
            _progressBarColor.RemoveFromClassList("blue-background");
            _progressBarColor.RemoveFromClassList("red-background");
            _progressBarColor.AddToClassList("yellow-background");
        }
        else if (p > 0.66f)
        {
            _progressBarColor.RemoveFromClassList("blue-background");
            _progressBarColor.RemoveFromClassList("yellow-background");
            _progressBarColor.AddToClassList("red-background");
        }
    }

    #endregion

    public void OnGameRoundEnded(RoundResult result)
    {
        _resultElement.RemoveFromClassList("hide");

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

        Invoke(nameof(ResetResultText), 3f);
    }

    public void ResetResultText()
    {
        _resultElement.AddToClassList("result-text-start");
    }

    private void HideResultText()
    {
        _resultElement.AddToClassList("hide");
    }

    public void ShowResultsStatistics(Question question, float[] percentages, Action onNext)
    {
        _questionDisplayElement.LoadQuestion(question);
        _questionDisplayElement.LoadPercentages(percentages);
        _questionDisplayElement.RemoveFromClassList("hide");

        HideResultText();
        _questionDisplayElement.SetOnNext(onNext);
    }
}