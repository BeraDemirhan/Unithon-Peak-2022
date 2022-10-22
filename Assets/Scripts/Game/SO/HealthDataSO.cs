using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{

    [CreateAssetMenu(fileName = "HealthDataSO", menuName = "ScriptableObjects/HealthDataSO")]
    public class HealthDataSO : ScriptableObject
    {
        [SerializeField] private int maxHealth;

        public int MaxHealth => maxHealth;
    }
}