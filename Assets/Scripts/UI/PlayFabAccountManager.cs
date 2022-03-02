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
        private TMP_Text _titleLabel;

        private PlayFabPlayer _playFabPlayer;
        
        private void Start()
        {
            _playFabPlayer = PlayFabPlayer.Instance;
            
            _playFabPlayer.GetAccountSuccessEvent += OnGetAccountSuccess;
            _playFabPlayer.GetAccountFailureEvent += OnFailure;

            _playFabPlayer.GetAccountInfo();
        }

        private void OnGetAccountSuccess(GetAccountInfoResult result)
        {
            _titleLabel.text = $"Welcome back, Player ID {result.AccountInfo.PlayFabId}";
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