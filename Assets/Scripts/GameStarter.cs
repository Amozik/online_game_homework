using System;
using Photon;
using PlayFab;
using UI;
using UnityEngine;

public class GameStarter : MonoBehaviour
{
    [SerializeField] 
    private GameEnterWindow _gameEnterWindow;

    private Canvas _canvas;
    private PhotonPlayer _photonPlayer;

    private void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
        
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "C11F0";
        }
        
        _photonPlayer = new PhotonPlayer();

        Instantiate(_gameEnterWindow);
    }

    private void OnDestroy()
    {
        _photonPlayer.Dispose();
    }
}
