using System.Collections.Generic;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PhotonLauncher : MonoBehaviour, IMatchmakingCallbacks
    {
        [SerializeField]
        private byte _maxPlayersPerRoom = 4;
        
        [SerializeField] 
        private Button _launchButton;
        
        [SerializeField] 
        private GameObject _loadingScreen;

        private PhotonPlayer _photonPlayer;
        
        private void Start()
        {
            PhotonNetwork.AddCallbackTarget(this);
            PhotonNetwork.AutomaticallySyncScene = true;
            
            _photonPlayer = PhotonPlayer.Instance;

            _launchButton.onClick.AddListener(Connect);
        }

        private void Connect()
        {
            _photonPlayer.Connect();
            _loadingScreen.SetActive(true);
        }

        public void OnJoinedRoom()
        {
            Debug.Log($"Room: {PhotonNetwork.CurrentRoom.Name} {PhotonNetwork.CurrentRoom.PlayerCount}");
            
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                _loadingScreen.SetActive(false);
                PhotonNetwork.LoadLevel("Game");
            }
        }
        
        public void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log(message);
            
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = _maxPlayersPerRoom});
        }
        
        #region not_used_callbacks
        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnCreatedRoom()
        {
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
        }

        public void OnLeftRoom()
        {
        }
        
        #endregion
    }
}