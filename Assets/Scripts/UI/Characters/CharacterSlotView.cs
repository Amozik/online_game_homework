using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Characters
{
    public class CharacterSlotView : MonoBehaviour
    {
        [SerializeField]
        private Button _createCharacterButton;
        
        [SerializeField]
        private GameObject _emptySlot;
        
        [SerializeField]
        private GameObject _infoCharacterSlot;
        
        [SerializeField]
        private TMP_Text _nameLabel;
        
        [SerializeField]
        private TMP_Text _levelLabel;
        
        [SerializeField]
        private TMP_Text _expLabel;
        
        [SerializeField]
        private TMP_Text _goldLabel;
        
        [SerializeField]
        private TMP_Text _healthLabel;
        
        [SerializeField]
        private TMP_Text _damageLabel;

        public event Action CreateCharacterEvent;

        public void ShowInfoCharacterSlot(string characterName, int level, int exp, int gold, int health, int damage)
        {
            _nameLabel.text = characterName;
            _levelLabel.text = level.ToString();
            _expLabel.text = exp.ToString();
            _goldLabel.text = gold.ToString();
            _healthLabel.text = exp.ToString();
            _damageLabel.text = gold.ToString();
            
            _emptySlot.SetActive(false);
            _infoCharacterSlot.SetActive(true);
        }

        public void ShowEmptySlot()
        {
            _emptySlot.SetActive(true);
            _infoCharacterSlot.SetActive(false);
        }

        private void OnEnable()
        {
            _createCharacterButton.onClick.AddListener(OnCreateCharacterClick);
        }
        private void OnDisable()
        {
            _createCharacterButton.onClick.RemoveListener(OnCreateCharacterClick);
        }

        private void OnCreateCharacterClick()
        {
            CreateCharacterEvent?.Invoke();
        }
    }
}