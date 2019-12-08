using System;
using UnityEngine;
using UnityEngine.UI;
using World;

namespace UI
{
    public class PlayerUI : MonoBehaviour
    {
        public PlayerData playerData;

        public Text txtPoints;
        
        private void Start()
        {
            if (playerData != null)
            {
                playerData.OnPointsChanged += HandlePointsChanged;
            }
        }

        private void OnDestroy()
        {
            if (playerData != null)
            {
                playerData.OnPointsChanged -= HandlePointsChanged;
            }
        }

        private void HandlePointsChanged(int points)
        {
            if(txtPoints != null)
                txtPoints.text = points.ToString();
        }
    }
}