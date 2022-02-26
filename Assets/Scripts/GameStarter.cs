using System;
using Photon;
using PlayFab;
using UI;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] 
    private AuthForm _authForm;

    private Canvas _canvas;
    private PlayFabPlayer _playFabPlayer;
    private PhotonPlayer _photonPlayer;

    private void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
        
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "C11F0";
        }
        
        _playFabPlayer = new PlayFabPlayer();
        _photonPlayer = new PhotonPlayer();

        var authForm = Instantiate(_authForm, _canvas.transform);
        authForm.Init(_playFabPlayer, _photonPlayer);
    }

    private void OnDestroy()
    {
        _photonPlayer.Dispose();
    }
}
