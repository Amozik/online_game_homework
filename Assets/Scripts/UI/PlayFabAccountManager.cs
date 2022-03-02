using System;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;

namespace UI
{
    public class PlayFabAccountManager : MonoBehaviour
    {
        [SerializeField] 
        private TMP_Text _username;
        
        [SerializeField] 
        private TMP_Text _playFabId;

        [SerializeField] 
        private GameObject _loadingScreen;

        private PlayFabPlayer _playFabPlayer;
        
        private void Start()
        {
            _playFabPlayer = PlayFabPlayer.Instance;
            
            _playFabPlayer.GetAccountSuccessEvent += OnGetAccountSuccess;
            _playFabPlayer.GetAccountFailureEvent += OnFailure;

            _playFabPlayer.GetAccountInfo();
            _loadingScreen.SetActive(true);
        }

        private void OnGetAccountSuccess(GetAccountInfoResult result)
        {
            _loadingScreen.SetActive(false);
            _username.text = $"Username:  {result.AccountInfo.Username}";
            _playFabId.text = $"PlayFabId:  {result.AccountInfo.PlayFabId}";
        }

        private void OnFailure(string errorMessage)
        {
            Debug.LogError($"Something went wrong: {errorMessage}");
        }

        private void OnDestroy()
        {
            _playFabPlayer.GetAccountSuccessEvent += OnGetAccountSuccess;
            _playFabPlayer.GetAccountFailureEvent += OnFailure;
        }
    }
}