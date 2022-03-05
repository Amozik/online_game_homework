using PlayFab;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CreateAccountWindow : AccountWindowBase
    {
        [SerializeField] 
        private InputField _emailField;
        
        [SerializeField] 
        private Button _createAccountButton;

        private string _email;

        protected override void SubscribeUiElement()
        {
            base.SubscribeUiElement();
            
            _emailField.onValueChanged.AddListener(UpdateEmail);
            _createAccountButton.onClick.AddListener(CreateAccount);
            
            _playFabPlayer.CreateAccountSuccessEvent += OnCreateAccountSuccess;
            _playFabPlayer.CreateAccountFailureEvent += OnCreateAccountFailure;
        }

        protected override void UnSubscribeUiElements()
        {
            base.UnSubscribeUiElements();
            
            _emailField.onValueChanged.RemoveAllListeners();
            _createAccountButton.onClick.RemoveAllListeners();
            
            _playFabPlayer.CreateAccountSuccessEvent -= OnCreateAccountSuccess;
            _playFabPlayer.CreateAccountFailureEvent -= OnCreateAccountFailure;
        }

        private void CreateAccount()
        {
            _playFabPlayer.CreateAccount(_username, _email, _password);
        }

        private void UpdateEmail(string email)
        {
            _email = email;
        }
        
        private void OnCreateAccountSuccess()
        {
            EnterLobbyScene();
        }

        private void OnCreateAccountFailure(string errorMessage)
        {
            ShowError(errorMessage);
        }
    }
}