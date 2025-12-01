using Assets.SharedAssets.Networking.Validators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UIElements;

public class MainScreenController : ScreenController
{
    private Button _sendButton;
    private Label _operationText;

    [SerializeField, HideInInspector] private string _username;
    [SerializeField, HideInInspector] private string _password;
    [SerializeField, HideInInspector] private string _confirmPassword;
    private Label _accountMessage;
    private Label _operationMessage;

    private LocalizedString _localizedAccMsg, _localizedSendBtnText, _localizedOpMsg;

    private enum ScreenLayout
    {
        LOGIN, REGISTER
    }

    ScreenLayout _currentLayout;
    private ScreenLayout CurrentLayout
    {
        get => _currentLayout;
        set
        {
            _currentLayout = value;
            UpdateLayout();
        }
    }

    void OnEnable()
    {
        _localizedAccMsg = LocalizationHelper.LoadLocalization("NoAccMsg");
        _localizedSendBtnText = LocalizationHelper.LoadLocalization("Login");
        _localizedOpMsg = LocalizationHelper.LoadLocalization("Register");

        _accountMessage = _ui.Q<Label>("AccountMessage");
        _operationMessage = _ui.Q<Label>("AccountOperation");

        _sendButton = _ui.Q<Button>("SendButton");
        _sendButton.clicked += OnSendButtonClicked;

        _operationText = _ui.Q<Label>("AccountOperation");
        _operationText.AddManipulator(new Clickable(evt =>
        {
            SwitchAccountOperation();
        }));

        CurrentLayout = ScreenLayout.LOGIN;

        _ui.Q<LangButtonElement>("LangButton").LoadAssetReference("Host Asset Table");

        LocalizationHelper.BindLabel(_accountMessage, _localizedAccMsg);
        LocalizationHelper.BindLabel(_operationMessage, _localizedOpMsg);
        LocalizationHelper.BindLabel(_sendButton, _localizedSendBtnText);

    }

    void OnDisable()
    {
        _sendButton.clicked -= OnSendButtonClicked;
    }

    private async void OnSendButtonClicked()
    {

        if (ValidateInputs())
        {
            var response = CurrentLayout == ScreenLayout.LOGIN ?
                await NetworkManager.Instance.Login(_username, _password) :
                await NetworkManager.Instance.RegisterUser(_username, _password);

            if (response.IsSuccess)
            {
                ScreenManagerBase.Instance.CurrentScreen = AppScreen.DASHBOARD;
            }
            else
            {
                ScreenManagerBase.Instance.DisplayErrorMessage(response.ErrorMessage);
                Debug.Log(response.ErrorMessage);
            }
        }

    }

    private bool ValidateInputs()
    {
        string confPassword = CurrentLayout == ScreenLayout.REGISTER ? _confirmPassword : _password;
        List<string> Errors = UserValidator.Validate(_username, _password, confPassword);

        if (Errors.Any())
        {
            ScreenManagerBase.Instance.DisplayErrorMessage(Errors[0]);
            Debug.Log(Errors[0]);
            return false;
        }
        else
        {
            return true;
        }
    }

    private void SwitchAccountOperation()
    {
        CurrentLayout = CurrentLayout == ScreenLayout.LOGIN ? ScreenLayout.REGISTER : ScreenLayout.LOGIN;
    }

    private void UpdateLayout()
    {
        TextField confirmPassField = _ui.Q<TextField>("ConfirmField");
        if (CurrentLayout == ScreenLayout.LOGIN)
        {
            _localizedAccMsg.TableEntryReference = "NoAccMsg";
            _localizedOpMsg.TableEntryReference = "Register";
            _localizedSendBtnText.TableEntryReference = "Login";
            confirmPassField.AddToClassList("hide");
        }
        else
        {
            _localizedAccMsg.TableEntryReference = "HaveAccMsg";
            _localizedOpMsg.TableEntryReference = "Login";
            _localizedSendBtnText.TableEntryReference = "Register";
            confirmPassField.RemoveFromClassList("hide");
        }
    }

    public override void ResetUIState()
    {
        CurrentLayout = ScreenLayout.LOGIN;
        _username = _password = _confirmPassword = "";
    }

}
