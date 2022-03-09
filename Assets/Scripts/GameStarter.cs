using Photon;
using PlayFab;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField] 
    private GameEnterWindow _gameEnterWindow;

    private Canvas _canvas;

    private void Start()
    {
        _canvas = FindObjectOfType<Canvas>();
        
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            PlayFabSettings.staticSettings.TitleId = "C11F0";
        }

        PlayFabPlayer.Instance.LoginSuccessEvent += OnPlayFabLogin;
    }

    private void OnPlayFabLogin()
    {
        if (PlayFabPlayer.Instance.IsAuthenticated)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            Instantiate(_gameEnterWindow);
        } 
    }

    private void OnDestroy()
    {
        PlayFabPlayer.Instance.LoginSuccessEvent -= OnPlayFabLogin;
    }
}
