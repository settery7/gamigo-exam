using System;
using TestTask.NonEditable;
using UnityEngine;
using UnityEngine.UI;

namespace TestTask.Editable
{
    public class ClientMobsManager : MonoBehaviour
    {
        [Serializable]
        private struct MonsterVisualEntry
        {
            public MonsterNames MonsterType;
            public Sprite Sprite;
        }

        [SerializeField] private MonsterVisualEntry[] _monsterSprites;
        [SerializeField] private Image _image;
        [SerializeField] private Image _healthBar;
        [SerializeField] private float _damageAmount = 10f;

        public MonsterData CurrentMonster { get; private set; }
        public event Action MonsterChanged;

        public void DealDamage()
        {
            if (CurrentMonster == null)
                return;

            ClientPacketsHandler.SendMonsterDamage(CurrentMonster.MonsterId, _damageAmount);
        }

        public void SetMonster(MonsterData monsterData)
        {
            CurrentMonster = monsterData;

            if (_image != null)
                _image.sprite = GetSprite(monsterData.MonsterType);

            if (_healthBar != null)
                _healthBar.fillAmount = monsterData.MonsterCurrentHealth / monsterData.MonsterMaxHealth;

            MonsterChanged?.Invoke();
        }

        private Sprite GetSprite(MonsterNames monsterType)
        {
            foreach (var entry in _monsterSprites)
            {
                if (entry.MonsterType == monsterType)
                    return entry.Sprite;
            }
            return null;
        }
    }
}
