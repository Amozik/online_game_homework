using System;
using System.Collections.Generic;
using Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

namespace UI
{
    public class Lobby : MonoBehaviour, ILobbyCallbacks
    {
        private const string ROOM_NAME = "myRoom";

        [SerializeField] 
        private Transform _roomsList;

        [SerializeField] 
        private GameObject _roomItem;
        
        private PhotonPlayer _photonPlayer;

        private void Start()
        {
            PhotonNetwork.AddCallbackTarget(this);
            PhotonNetwork.AutomaticallySyncScene = true;
            
            _photonPlayer = PhotonPlayer.Instance;
            
            _photonPlayer.ConnectEvent += PhotonPlayerOnConnect;
            _photonPlayer.DisconnectEvent += PhotonPlayerOnDisconnect;
            
            _photonPlayer.Connect();
        }

        private void PhotonPlayerOnConnect()
        {
            JoinLobby();
        }

        private void PhotonPlayerOnDisconnect()
        {
        }

        public void CloseRoom()
        {
            var currentRoom = PhotonNetwork.CurrentRoom;

            var playersCount = (byte) currentRoom.Players.Count;
            currentRoom.MaxPlayers = playersCount;
        }
        
        
        private static void JoinOrCreateRandomPrivateRoom()
        {
            RoomOptions roomOptions = new RoomOptions
            {
                IsVisible = false,
                IsOpen = true,
            };
            PhotonNetwork.JoinRandomOrCreateRoom(roomName: ROOM_NAME, roomOptions: roomOptions);
            GUIUtility.systemCopyBuffer = ROOM_NAME;
        }

        private void JoinLobby()
        {
            PhotonNetwork.JoinLobby();
        }

        private void OnDestroy()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
            
            _photonPlayer.ConnectEvent -= PhotonPlayerOnConnect;
            _photonPlayer.DisconnectEvent -= PhotonPlayerOnDisconnect;
            
            _photonPlayer.Dispose();
        }
        
        public void OnJoinedLobby()
        {
            JoinOrCreateRandomPrivateRoom();
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            OutputRooms(roomList);
        }

        private void OutputRooms(List<RoomInfo> roomList)
        {
            foreach (var roomInfo in roomList)
            {
                var item = GameObject.Instantiate(_roomItem, _roomsList);
                item.GetComponent<TMP_Text>().text = roomInfo.Name;
            }
        }

        #region not_used_callbacks

        public void OnLeftLobby()
        {
        }
        
        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }

        #endregion
    }
}