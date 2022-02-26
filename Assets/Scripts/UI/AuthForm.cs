using System;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AuthForm : MonoBehaviour
    {
        [SerializeField] 
        private Button _loginButton;

        [SerializeField] 
        private TMP_Text _statusText;

        private PlayFabPlayer _playFabPlayer;

        public void Init(PlayFabPlayer playFabPlayer)
        {
            if (playFabPlayer == null)
                return;

            _playFabPlayer = playFabPlayer;
            
            _loginButton.onClick.AddListener(OnLoginClick);
            
            _playFabPlayer.LoginSuccessEvent += PlayFabPlayerOnLoginSuccess;
            _playFabPlayer.LoginFailureEvent += PlayFabPlayerOnLoginFailure;
        }

        private void OnEnable()
        {
            _statusText.text = "";
        }

        private void OnLoginClick()
        {
            _playFabPlayer.Login();
        }
        
        private void PlayFabPlayerOnLoginSuccess()
        {
            _statusText.text = "Login Success";
            _statusText.color = Color.green;
        }

        private void PlayFabPlayerOnLoginFailure()
        {
            _statusText.text = "Login Error";
            _statusText.color = Color.red;
        }

        private void OnDisable()
        {
            _loginButton.onClick.RemoveAllListeners();
            _playFabPlayer.LoginSuccessEvent -= PlayFabPlayerOnLoginSuccess;
            _playFabPlayer.LoginFailureEvent -= PlayFabPlayerOnLoginFailure;
        }
    }
}