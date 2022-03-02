using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SignInWindow : AccountWindowBase
    {
        [SerializeField] 
        private Button _signInButton;

        protected override void SubscribeUiElement()
        {
            base.SubscribeUiElement();
            _signInButton.onClick.AddListener(SignIn);
            
            _playFabPlayer.SignInSuccessEvent += OnSignInSuccess;
            _playFabPlayer.LoginFailureEvent += OnSignInFailure;
        }

        protected override void UnSubscribeUiElements()
        {
            base.UnSubscribeUiElements();
            _signInButton.onClick.RemoveAllListeners();
        }

        private void SignIn()
        {
            _playFabPlayer.SignIn(_username, _password);
        }
        
        private void OnSignInSuccess()
        {
            EnterLobbyScene();
        }
        
        private void OnSignInFailure(string errorMessage)
        {
            ShowError(errorMessage);
        }
    }
}