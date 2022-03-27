using System;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;

namespace PlayFab
{
    public class PlayFabUserData
    {
        private Dictionary<string, string> _data = new Dictionary<string, string>();
        
        private string _playFabId;

        public PlayFabUserData(string playFabId)
        {
            _playFabId = playFabId;
        }
        
        public void SetData(string key, string value)
        {
            var data = new Dictionary<string, string>
            {
                {key, value},
            };
            SetData(data);
        }

        public void SetData(Dictionary<string, string> data)
        {
            PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest {Data = data}, OnSetDataSuccess, OnFailure);
        }

        public void GetUserData(string key, Action<string> callback = null)
        {
            var keys = new List<string>
            {
                key
            };

            GetUserData(keys, () =>
            {
                if (_data.TryGetValue(key, out var value))
                {
                    callback?.Invoke(value);
                }
            });
        }

        public void GetUserData(List<string> keys = null, Action callback = null) { 
            PlayFabClientAPI.GetUserData(new GetUserDataRequest {
                PlayFabId = _playFabId,
                Keys = keys
            }, GetDataSuccess, OnFailure);
            
            void GetDataSuccess(GetUserDataResult result)
            {
                if (result.Data == null)
                {
                    Debug.LogError("No Data");
                    return;
                }
                foreach (var userDataRecord in result.Data)
                {
                    _data[userDataRecord.Key] = userDataRecord.Value.Value;
                }
                callback?.Invoke();
            }
        }

        private void OnFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
        }

        private void OnSetDataSuccess(UpdateUserDataResult result)
        {
            Debug.Log("Successfully updated user data");
        }
    }
}