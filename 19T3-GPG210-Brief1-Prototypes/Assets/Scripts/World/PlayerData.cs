using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace World
{
    public class PlayerData : ScriptableObject
    {
        public event Action<int> OnPointsChanged;
        
        [ShowInInspector]
        private int points;

        public int Points
        {
            get => points;
            set
            {
                points = value;
                OnPointsChanged?.Invoke(points);
            }
        }
    }
}