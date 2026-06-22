using System;
using TestTask.Editable;
using TestTask.NonEditable;
using UnityEngine;
using Random = UnityEngine.Random;

namespace TestTask.Editable
{
    public class ServerMobsManager
    {
        [field: SerializeField] public MonsterData MonsterData { get; private set; }

        public ServerMobsManager()
        {
            MonsterData = SpawnMonster();
        }

        public MonsterData SpawnMonster()
        {
            var monsterId = Random.Range(1, 1000);
            var monsterType = MonsterNameExtensions.MonsterTypeFromId(monsterId);
            var monsterMaxHealth = Random.Range(50, 201);

            var monsterData = new MonsterData(monsterId, monsterType, monsterMaxHealth, monsterMaxHealth);
            monsterData.MonsterDeath += OnMonsterDied;

            return monsterData;
        }

        private void OnMonsterDied()
        {
            MonsterData.MonsterDeath -= OnMonsterDied;
            MonsterData = SpawnMonster();
        }
    }
}  
