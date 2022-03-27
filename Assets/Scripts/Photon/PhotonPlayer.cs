using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Photon
{
    public class PhotonPlayer : IConnectionCallbacks, IDisposable
    {
        private static readonly Lazy<PhotonPlayer> _lazy = 
            new Lazy<PhotonPlayer>(() => new PhotonPlayer());
        
        public static PhotonPlayer Instance => _lazy.Value;
        
        public static string Username
        {
            get => PhotonNetwork.NickName;
            set => PhotonNetwork.NickName = value;
        }

        public event Action ConnectEvent; 
        public event Action DisconnectEvent; 
        
        private bool _isConnecting;
        
        public PhotonPlayer()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void Connect()
        {
            _isConnecting = true;
            
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = Application.version;
                PhotonNetwork.ConnectUsingSettings();
            }
        }
        
        public void Disconnect()
        {
            PhotonNetwork.Disconnect();
        }

        public void OnConnected()
        {
            Debug.Log("Photon Server Connected");
        }

        public void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster() was called by PUN");
            
            if (_isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            
            ConnectEvent?.Invoke();
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Photon Server Disconnected");

            _isConnecting = false;
            
            DisconnectEvent?.Invoke();    
        }
        
        public void Dispose()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        #region not_used_callbacks
        
        public void OnRegionListReceived(RegionHandler regionHandler)
        {
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        #endregion
        
    }
}