using System;
using Assets.Scripts.Networking.Data;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UIElements;

public class GameScreenController : ScreenController
{
    private VisualElement _resultElement;
    [SerializeField, HideInInspector] private string _resultText;

    private QuestionDisplayElement _questionDisplayElement;

    [SerializeField, HideInInspector] private string _villageName;
    private VisualElement _villageImage;
    private VisualElement _selectVillageElement;
    private Button _conquerButton;
    private Button _exitButton;
    private Village _selectedVillage;

    protected override void Awake()
    {
        base.Awake();
        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Host Asset Table");
        _resultElement = _ui.Q<VisualElement>("ResultElement");
        _questionDisplayElement = _ui.Q<QuestionDisplayElement>();

        _selectVillageElement = _ui.Q<VisualElement>("SelectVillageElement");
        _conquerButton = _ui.Q<Button>("ConquerBtn");
        _exitButton = _ui.Q<Button>("SelectVillageExitBtn");
        _villageImage = _ui.Q<VisualElement>("VillageImage");
    }

    void OnEnable()
    {
        _resultText = "DEFEAT";
        GameManager.GameRoundEnded += OnGameRoundEnded;
        _conquerButton.clicked += OnConquerClicked;
        _exitButton.clicked += OnExitClicked;
        VillageRaycastHandler.OnVillageSelectChanged += OnVillageSelected;
    }

    void OnDisable()
    {
        GameManager.GameRoundEnded -= OnGameRoundEnded;
        _conquerButton.clicked -= OnConquerClicked;
        _exitButton.clicked -= OnExitClicked;
        VillageRaycastHandler.OnVillageSelectChanged -= OnVillageSelected;
    }

    private void OnGameRoundEnded(RoundResult result)
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

    private void ResetResultText()
    {
        _resultElement.AddToClassList("result-text-start");
    }

    private void ShowResultsStatistics(Question question, float[] percentages)
    {
        _questionDisplayElement.LoadQuestion(question);
        _questionDisplayElement.LoadPercentages(percentages);
        _questionDisplayElement.RemoveFromClassList("hide");
    }

    public void ShowQuestionDisplay(Question question)
    {
        _questionDisplayElement.LoadQuestion(question);
        _questionDisplayElement.ResetPercentages();
        _questionDisplayElement.RemoveFromClassList("hide");
    }

    private void OnVillageSelected(GameObject obj)
    {
        Village village = new()
        {
            ID = 0,
            Name = obj.name,
            ImagePath = "village1"
        };

        _selectedVillage = village;

        Invoke(nameof(ShowSelectVillageDialog), 2f);
    }

    public void ShowSelectVillageDialog()
    {
        if (_selectedVillage == null) return;

        _selectVillageElement.RemoveFromClassList("hide");
        _villageName = _selectedVillage.Name;
        Texture2D villageImg = Resources.Load<Texture2D>(_selectedVillage.ImagePath);
        _villageImage.style.backgroundImage = villageImg;
    }

    private void OnConquerClicked()
    {
        // TODO: send conquer request to server
        // TODO: get next question data
        // TODO: get timer value

        _selectVillageElement.AddToClassList("hide");
    }

    private void OnExitClicked()
    {
        _selectVillageElement.AddToClassList("hide");
    }
}
