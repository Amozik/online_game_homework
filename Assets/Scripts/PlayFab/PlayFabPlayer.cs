using System;
using PlayFab.ClientModels;
using UnityEngine;

namespace PlayFab
{
    public class PlayFabPlayer
    {
        private const string AUTH_GUID_KEY = "auth_guid";
        private const string AUTH_REMEMBERED_KEY = "auth_remembered";
        private static readonly Lazy<PlayFabPlayer> _lazy = 
            new Lazy<PlayFabPlayer>(() => new PlayFabPlayer());

        private bool _isAccountCreated;
        private string _id;

        private string _username;
        private string _email;
        private string _password;
        
        public static PlayFabPlayer Instance => _lazy.Value;
        
        public bool RememberMe => true;

        public bool IsAuthenticated
        {
            get => PlayerPrefs.GetInt(AUTH_REMEMBERED_KEY, 0) != 0;
            private set => PlayerPrefs.SetInt(AUTH_REMEMBERED_KEY, value ? 1 : 0);
        }
        public event Action LoginSuccessEvent; 
        public event Action CreateAccountSuccessEvent; 
        public event Action SignInSuccessEvent;
        public event Action<GetAccountInfoResult> GetAccountSuccessEvent;
        public event Action<string> LoginFailureEvent; 
        public event Action<string> CreateAccountFailureEvent;
        public event Action<string> GetAccountFailureEvent;
        
        private PlayFabPlayer()
        {
            _isAccountCreated = PlayerPrefs.HasKey(AUTH_GUID_KEY);
            _id = PlayerPrefs.GetString(AUTH_GUID_KEY, Guid.NewGuid().ToString());

            Login();
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
            PlayerPrefs.DeleteKey(AUTH_GUID_KEY);
            PlayerPrefs.DeleteKey(AUTH_REMEMBERED_KEY);
        }
        
        public void CreateAccount(string username, string mail, string password)
        {
            _username = username;
            _email = mail;
            _password = password;
            
            if (RememberMe)
            {
                PlayFabClientAPI.AddUsernamePassword(new AddUsernamePasswordRequest()
                {
                    Username = _username,
                    Email = _email,
                    Password = _password,
                }, OnAddUsernameAndPasswordSuccess, OnCreateAccountFailure);
            }
            else
            {
                PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
                {
                    Username = _username,
                    Email = _email,
                    Password = _password,
                    RequireBothUsernameAndEmail = true
                }, OnCreateSuccess, OnCreateAccountFailure);
            }
        }
        
        public void SignIn(string username, string password)
        {
            _username = username;
            _password = password;
            
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
            {
                Username = _username,
                Password = _password
            }, OnSignInSuccess, OnSignInAccountFailure);
        }

        public void GetAccountInfo()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnGetAccountSuccess, OnGetAccountFailure);
        }

        private void OnLoginSuccess(LoginResult result)
        {
            if (!_isAccountCreated)
                PlayerPrefs.SetString(AUTH_GUID_KEY, _id);

            Debug.Log("Congratulations, you made successful API call!");
            
            LoginSuccessEvent?.Invoke();
        }

        private void OnGetAccountSuccess(GetAccountInfoResult result)
        {
            GetAccountSuccessEvent?.Invoke(result);
        }

        private void OnCreateSuccess(RegisterPlayFabUserResult result)
        {
            Debug.Log($"Creation Success: {_username}");
            
            CreateAccountSuccessEvent?.Invoke();
        }
        
        private void OnAddUsernameAndPasswordSuccess(AddUsernamePasswordResult  result)
        {
            Debug.Log($"Creation Success: {_username}");

            IsAuthenticated = true;
            CreateAccountSuccessEvent?.Invoke();
        }

        private void OnSignInSuccess(LoginResult result)
        {
            Debug.Log($"Sign In Success: {_username}");
            
            SignInSuccessEvent?.Invoke();
        }

        private void OnFailure(PlayFabError error, Action<string> callback)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
            
            callback?.Invoke(errorMessage);
        }

        private void OnLoginFailure(PlayFabError error) => OnFailure(error, LoginFailureEvent);
        private void OnCreateAccountFailure(PlayFabError error) => OnFailure(error, CreateAccountFailureEvent);
        private void OnSignInAccountFailure(PlayFabError error) => OnFailure(error, LoginFailureEvent);
        private void OnGetAccountFailure(PlayFabError error) => OnFailure(error, GetAccountFailureEvent);

    }
}