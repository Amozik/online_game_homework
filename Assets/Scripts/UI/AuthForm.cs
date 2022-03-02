using Photon;
using PlayFab;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class AuthForm : MonoBehaviour
    {
        [SerializeField] 
        private Button _playFabLoginButton;

        [SerializeField] 
        private TMP_Text _statusText;

        [SerializeField] 
        private Button _photonButton;
        
        [SerializeField] 
        private TMP_Text _photonButtonText;
        
        private PlayFabPlayer _playFabPlayer;
        private PhotonPlayer _photonPlayer;

        private bool _isPhotonConnected;

        public void Init(PlayFabPlayer playFabPlayer, PhotonPlayer photonPlayer)
        {
            if (playFabPlayer == null || photonPlayer == null)
                return;

            _playFabPlayer = playFabPlayer;
            _photonPlayer = photonPlayer;
            
            _playFabLoginButton.onClick.AddListener(OnLoginClick);
            _photonButton.onClick.AddListener(OnPhotonClick);
            
            _playFabPlayer.LoginSuccessEvent += PlayFabPlayerOnLoginSuccess;
            _playFabPlayer.LoginFailureEvent += PlayFabPlayerOnLoginFailure;
            
            _photonPlayer.ConnectEvent += PhotonPlayerOnConnect;
            _photonPlayer.DisconnectEvent += PhotonPlayerOnDisconnect;
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

        private void PlayFabPlayerOnLoginFailure(string errorMessage)
        {
            _statusText.text = errorMessage;
            _statusText.color = Color.red;
        }
        
        private void OnPhotonClick()
        {
            if (_isPhotonConnected)
            {
                _photonPlayer.Disconnect();
            }
            else
            {
                _photonPlayer.Connect();
            }
        }
        
        private void PhotonPlayerOnConnect()
        {
            _isPhotonConnected = true;
            _photonButtonText.text = "Photon Disconnect";
            _photonButton.image.color = Color.cyan;
        }

        private void PhotonPlayerOnDisconnect()
        {
            _isPhotonConnected = false;
            _photonButtonText.text = "Photon Connect";
            _photonButton.image.color = Color.white;
        }

        private void OnDisable()
        {
            _playFabLoginButton.onClick.RemoveAllListeners();
            _photonButton.onClick.RemoveAllListeners();
            _playFabPlayer.LoginSuccessEvent -= PlayFabPlayerOnLoginSuccess;
            _playFabPlayer.LoginFailureEvent -= PlayFabPlayerOnLoginFailure;
            _photonPlayer.ConnectEvent -= PhotonPlayerOnConnect;
            _photonPlayer.DisconnectEvent -= PhotonPlayerOnDisconnect;
        }
    }
}