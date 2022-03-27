using System;
using PlayFab;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Photon.Pun.Demo.PunBasics
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
    {
        private const string HEATH_KEY = "Health";
        
        [Tooltip("The current Health of our player")]
        public float Health = 1f;

        [Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
        public static GameObject LocalPlayerInstance;

        [Tooltip("The Player's UI GameObject Prefab")]
        [SerializeField]
        private GameObject playerUiPrefab;

        [Tooltip("The Beams GameObject to control")]
        [SerializeField]
        private GameObject beams;
        
        bool _isFiring;
        private PlayFabUserData _playFabUserData;
        
        public void Awake()
        {
            if (beams == null)
            {
                Debug.LogError("<Color=Red><b>Missing</b></Color> Beams Reference.", this);
            }
            else
            {
                beams.SetActive(false);
            }

            // #Important
            // used in GameManager.cs: we keep track of the localPlayer instance to prevent instanciation when levels are synchronized
            if (photonView.IsMine)
            {
                LocalPlayerInstance = gameObject;
                _playFabUserData = PlayFabPlayer.Instance.PlayFabUserData;
                SaveHealth(Health);
            }

            // #Critical
            // we flag as don't destroy on load so that instance survives level synchronization, thus giving a seamless experience when levels load.
            DontDestroyOnLoad(gameObject);
        }

        public void Start()
        {
            CameraWork _cameraWork = gameObject.GetComponent<CameraWork>();

            if (_cameraWork != null)
            {
                if (photonView.IsMine)
                {
                    _cameraWork.OnStartFollowing();
                }
            }
            else
            {
                Debug.LogError("<Color=Red><b>Missing</b></Color> CameraWork Component on player Prefab.", this);
            }

            // Create the UI
            if (playerUiPrefab != null)
            {
                GameObject _uiGo = Instantiate(playerUiPrefab);
                _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
            }
            else
            {
                Debug.LogWarning("<Color=Red><b>Missing</b></Color> PlayerUiPrefab reference on player Prefab.", this);
            }

#if UNITY_5_4_OR_NEWER
            // Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
            SceneManager.sceneLoaded += OnSceneLoaded;
#endif
        }

        public override void OnDisable()
		{
			// Always call the base to remove callbacks
			base.OnDisable ();

#if UNITY_5_4_OR_NEWER
			SceneManager.sceneLoaded -= OnSceneLoaded;
#endif
		}
        
        public void Update()
        {
            // we only process Inputs and check health if we are the local player
            if (photonView.IsMine)
            {
                ProcessInputs();

                if (Health <= 0f)
                {
                    GameManager.Instance.LeaveRoom();
                }
            }

            if (beams != null && _isFiring != beams.activeInHierarchy)
            {
                beams.SetActive(_isFiring);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (!photonView.IsMine)
            {
                return;
            }


            // We are only interested in Beamers
            // we should be using tags but for the sake of distribution, let's simply check by name.
            if (!other.name.Contains("Beam"))
            {
                return;
            }

            Health -= 0.1f;
            SaveHealth(Health);
        }
        
        public void OnTriggerStay(Collider other)
        {
            // we dont' do anything if we are not the local player.
            if (!photonView.IsMine)
            {
                return;
            }

            // We are only interested in Beamers
            // we should be using tags but for the sake of distribution, let's simply check by name.
            if (!other.name.Contains("Beam"))
            {
                return;
            }

            // we slowly affect health when beam is constantly hitting us, so player has to move to prevent death.
            Health -= 0.1f * Time.deltaTime;
            SaveHealth(Health);
        }

#if !UNITY_5_4_OR_NEWER
        /// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
        void OnLevelWasLoaded(int level)
        {
            this.CalledOnLevelWasLoaded(level);
        }
#endif

        void CalledOnLevelWasLoaded(int level)
        {
            // check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
            if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
            {
                transform.position = new Vector3(0f, 5f, 0f);
            }

            GameObject _uiGo = Instantiate(playerUiPrefab);
            _uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
        }

#if UNITY_5_4_OR_NEWER
		void OnSceneLoaded(Scene scene, LoadSceneMode loadingMode)
		{
			CalledOnLevelWasLoaded(scene.buildIndex);
		}
#endif

        /// <summary>
        /// Processes the inputs. This MUST ONLY BE USED when the player has authority over this Networked GameObject (photonView.isMine == true)
        /// </summary>
        void ProcessInputs()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                // we don't want to fire when we interact with UI buttons for example. IsPointerOverGameObject really means IsPointerOver*UI*GameObject
                // notice we don't use on on GetbuttonUp() few lines down, because one can mouse down, move over a UI element and release, which would lead to not lower the isFiring Flag.
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    //	return;
                }

                if (!_isFiring)
                {
                    _isFiring = true;
                }
            }

            if (Input.GetButtonUp("Fire1"))
            {
                if (_isFiring)
                {
                    _isFiring = false;
                }
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                // We own this player: send the others our data
                stream.SendNext(_isFiring);
                GetHealth(stream.SendNext);
            }
            else
            {
                // Network player, receive data
                _isFiring = (bool)stream.ReceiveNext();
                Health = (float)stream.ReceiveNext();
                SaveHealth(Health);
            }
        }

        private void SaveHealth(float value)
        {
            _playFabUserData.SetData(HEATH_KEY, value.ToString());
        }

        private void GetHealth(Action<string> callback)
        {
            _playFabUserData.GetUserData(HEATH_KEY, callback);
        }
    }
}