using System;
using Photon;
using PlayFab;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class AccountWindowBase : MonoBehaviour
    {
        [SerializeField] 
        private InputField _usernameField;
        
        [SerializeField] 
        private InputField _passwordField;
        
        [SerializeField] 
        private Text _errorLabel;

        protected string _username;
        protected string _password;

        protected PlayFabPlayer _playFabPlayer;

        protected virtual void SubscribeUiElement()
        {
            _usernameField.onValueChanged.AddListener(UpdateUsername);
            _passwordField.onValueChanged.AddListener(UpdatePassword);
        }

        protected virtual void UnSubscribeUiElements()
        {
            _usernameField.onValueChanged.RemoveAllListeners();
            _passwordField.onValueChanged.RemoveAllListeners();
        }

        protected void ShowError(string errorMessage)
        {
            _errorLabel.text = errorMessage;
            _errorLabel.gameObject.SetActive(true);
        }

        protected void EnterLobbyScene()
        {
            SceneManager.LoadScene(1);
        }

        private void Awake()
        {
            _playFabPlayer = PlayFabPlayer.Instance;
        }

        private void OnEnable()
        {
            SubscribeUiElement();
        }

        private void OnDisable()
        {
            UnSubscribeUiElements();
        }

        private void UpdatePassword(string password)
        {
            _password = password;
        }

        private void UpdateUsername(string username)
        {
            _username = username;
        }
    }
}