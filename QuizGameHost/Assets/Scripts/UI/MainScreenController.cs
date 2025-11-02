using System;
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
        _localizedAccMsg = LoadLocalization("NoAccMsg");
        _localizedSendBtnText = LoadLocalization("Login");
        _localizedOpMsg = LoadLocalization("Register");

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

        BindLabel(_accountMessage, _localizedAccMsg);
        BindLabel(_operationMessage, _localizedOpMsg);
        BindLabel(_sendButton, _localizedSendBtnText);

    }

    private LocalizedString LoadLocalization(string stringRef)
    {
        return new LocalizedString
        {
            TableReference = "Host String Table",
            TableEntryReference = stringRef
        };
    }

    void OnDisable()
    {
        _sendButton.clicked -= OnSendButtonClicked;
    }

    private void OnSendButtonClicked()
    {
        if (ValidateInputs())
        {
            // TODO: send data to backend
        }
        throw new NotImplementedException();
    }

    private bool ValidateInputs()
    {
        // TODO: validate fields
        return true;
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

    private void BindLabel(TextElement element, LocalizedString localized)
    {
        localized.StringChanged += s => element.text = s;
        localized.RefreshString();
    }

}
