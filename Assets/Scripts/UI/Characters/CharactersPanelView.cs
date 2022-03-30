using System;
using System.Collections.Generic;
using System.Linq;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Characters
{
    public class CharactersPanelView : MonoBehaviour
    {
        public const string CHARACTER_TOKEN = "character_token";
        
        [SerializeField]
        private GameObject _newCharacterPanel;
        
        [SerializeField]
        private Button _createCharacterButton;
        
        [SerializeField]
        private Button _closeButton;
        
        [SerializeField]
        private TMP_InputField _inputField;

        [SerializeField] 
        private List<CharacterSlotView> _slots;

        private string _characterName;
        
        private void Start()
        {
            UpdateCharactersSlots();

            foreach (var characterSlotView in _slots)
            {
                characterSlotView.CreateCharacterEvent += ShowNewCharacterPanel;
            }
            
            _closeButton.onClick.AddListener(CloseNewCharacterPanel);
            _createCharacterButton.onClick.AddListener(CreateCharacterWithItem);
            _inputField.onValueChanged.AddListener(OnNameChanged);
        }

        private void UpdateCharactersSlots()
        {
            PlayFabClientAPI.GetAllUsersCharacters(new ListUsersCharactersRequest(),
                result =>
                {
                    Debug.Log($"Characters owned: + {result.Characters.Count}");
                    ShowCharactersSlots(result.Characters);
                }, Debug.LogError);
        }

        private void ShowCharactersSlots(List<CharacterResult> characters)
        {
            for (var i = 0; i < _slots.Count; i++)
            {
                var character = characters.ElementAtOrDefault(i);
                if (character != null)
                {
                    FillSlotByCharacterInfo(character, _slots[i]);
                }
                else
                {
                    _slots[i].ShowEmptySlot();
                }
            }
        }

        private void FillSlotByCharacterInfo(CharacterResult character, CharacterSlotView characterSlotView)
        {
            PlayFabClientAPI.GetCharacterStatistics(new GetCharacterStatisticsRequest
            {
                CharacterId = character.CharacterId,
            }, result =>
            {
                var characterStatistics = result.CharacterStatistics;
                var level = characterStatistics["Level"];
                var exp = characterStatistics["Exp"];
                var gold = characterStatistics["Gold"];
                var health = characterStatistics["Health"];
                var damage = characterStatistics["Damage"];
                characterSlotView.ShowInfoCharacterSlot(character.CharacterName, level,
                    exp, gold, health, damage);
            }, Debug.LogError);
        }
 
        private void OnNameChanged(string inputValue)
        {
            _characterName = inputValue;
        }

        private void CreateCharacterWithItem()
        {
            PlayFabPlayer.Instance.PlayFabCatalog.PurchaseItem(CHARACTER_TOKEN, callback: GrantCharacter);
        }

        private void GrantCharacter()
        {
            PlayFabClientAPI.GrantCharacterToUser(new GrantCharacterToUserRequest
            {
                CharacterName = _characterName,
                ItemId = CHARACTER_TOKEN,
            }, result => { UpdateCharacterStatistics(result.CharacterId); }, Debug.LogError);
        }

        private void UpdateCharacterStatistics(string characterId)
        {
            PlayFabClientAPI.UpdateCharacterStatistics(new UpdateCharacterStatisticsRequest()
            {
                CharacterId = characterId,
                CharacterStatistics = new Dictionary<string, int>()
                {
                    {"Level", 1},
                    {"Exp", 0},
                    {"Gold", 500},
                    {"Damage", 1000},
                    {"Health", 5000},
                }
            }, result =>
            {
                CloseNewCharacterPanel();
                UpdateCharactersSlots();
            }, Debug.LogError);
        }

        private void ShowNewCharacterPanel()
        {
            _newCharacterPanel.SetActive(true);
        }

        private void CloseNewCharacterPanel()
        {
            _newCharacterPanel.SetActive(false);
        }
        
        private void OnDestroy()
        {
            foreach (var characterSlotView in _slots)
            {
                characterSlotView.CreateCharacterEvent -= ShowNewCharacterPanel;
            }
            
            _closeButton.onClick.RemoveListener(CloseNewCharacterPanel);
            _createCharacterButton.onClick.RemoveListener(CreateCharacterWithItem);
            _inputField.onValueChanged.RemoveListener(OnNameChanged);
        }
    }
}