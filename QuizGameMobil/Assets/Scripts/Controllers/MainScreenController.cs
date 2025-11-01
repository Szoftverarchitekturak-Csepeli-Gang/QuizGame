using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MainScreenController : ScreenController
{
    private Button _joinButton;
    private Label _inputErrorLabel;

    [SerializeField, HideInInspector] private int _roomID;
    [SerializeField, HideInInspector] private string _roomPassword;
    [SerializeField, HideInInspector] private string _errorMessage = "Error message";

    void OnEnable()
    {
        _joinButton = _ui.Q<Button>("JoinButton");
        _joinButton.clicked += OnJoinButtonClick;

        _inputErrorLabel = _ui.Q<Label>("InputError");
        _inputErrorLabel.style.visibility = Visibility.Hidden;
    }

    void OnDisable()
    {
        _joinButton.clicked -= OnJoinButtonClick;
    }

    private void OnJoinButtonClick()
    {
        Debug.Log($"id: {_roomID}, pass: {_roomPassword}");
        if (ValidateInputs())
        {
            _inputErrorLabel.style.visibility = Visibility.Hidden;
            // TODO: join room
            ScreenManagerMobil.Instance.ShowScreen(AppScreen.WAITROOM);
        }
        else
        {
            _errorMessage = "Wrong room ID or password";
            _inputErrorLabel.style.visibility = Visibility.Visible;
            // TODO: display error message
        }
    }

    private bool ValidateInputs()
    {
        return true;
    }
}
