using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Photon
{
    public class PhotonPlayer : IConnectionCallbacks, IDisposable
    {
        public event Action ConnectEvent; 
        public event Action DisconnectEvent; 
        
        public PhotonPlayer()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void Connect()
        {
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
            
            ConnectEvent?.Invoke();
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("Photon Server Disconnected");
            
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