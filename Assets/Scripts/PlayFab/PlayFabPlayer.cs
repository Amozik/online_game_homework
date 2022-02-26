using System;
using PlayFab.ClientModels;
using UnityEngine;

namespace PlayFab
{
    public class PlayFabPlayer
    {
        private const string AUTH_GUID_KEY = "auth_guid";

        private bool _isAccountCreated;
        private string _id;

        public event Action LoginSuccessEvent; 
        public event Action LoginFailureEvent; 
        
        public PlayFabPlayer()
        {
            _isAccountCreated = PlayerPrefs.HasKey(AUTH_GUID_KEY);
            _id = PlayerPrefs.GetString(AUTH_GUID_KEY, Guid.NewGuid().ToString());
        }

        public void Login()
        {
            
            var request = new LoginWithCustomIDRequest
            {
                CustomId = _id, 
                CreateAccount = !_isAccountCreated,
            };
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }

        public void Logout()
        {
            PlayFabClientAPI.ForgetAllCredentials();
        }

        private void OnLoginSuccess(LoginResult result)
        {
            if (_isAccountCreated)
                PlayerPrefs.SetString(AUTH_GUID_KEY, _id);

            Debug.Log("Congratulations, you made successful API call!");
            
            LoginSuccessEvent?.Invoke();
        }

        private void OnLoginFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
            
            LoginFailureEvent?.Invoke();
        }
    }
}