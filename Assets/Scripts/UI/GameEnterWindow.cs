using Photon;
using PlayFab;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class GameEnterWindow : MonoBehaviour
    {
        [SerializeField] 
        private Button _signInButon;
        
        [SerializeField] 
        private Button _createAccountButon;
        
        [SerializeField] 
        private Button _signInBackButon;
        
        [SerializeField] 
        private Button _createAccountBackButon;
        
        [SerializeField] 
        private Canvas _enterInGameWindow;
        
        [SerializeField] 
        private Canvas _createAccountWindow;
        
        [SerializeField] 
        private Canvas _signInWindow;

        private void OnEnable()
        {
            InitWindow();
            _signInButon.onClick.AddListener(OpenSignInWindow);
            _createAccountButon.onClick.AddListener(OpenCreateAccountWindow);
            _signInBackButon.onClick.AddListener(InitWindow);
            _createAccountBackButon.onClick.AddListener(InitWindow);
        }

        private void OpenSignInWindow()
        {
            _signInWindow.enabled = true;
            _enterInGameWindow.enabled = false;
        }

        private void OpenCreateAccountWindow()
        {
            _createAccountWindow.enabled = true;
            _enterInGameWindow.enabled = false;
        }
        
        private void InitWindow()
        {
            _signInWindow.enabled = false;
            _createAccountWindow.enabled = false;
            _enterInGameWindow.enabled = true;
        }
        
        private void OnDisable()
        {
            _signInButon.onClick.RemoveAllListeners();
            _createAccountButon.onClick.RemoveAllListeners();
            _signInBackButon.onClick.RemoveAllListeners();
            _createAccountBackButon.onClick.RemoveAllListeners();
        }
    }
}