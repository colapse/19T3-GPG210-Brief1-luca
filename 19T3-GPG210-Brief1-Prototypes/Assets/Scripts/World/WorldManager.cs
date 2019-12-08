using System;
using UI;
using UnityEngine;
using World;

public class WorldManager : MonoBehaviour
{
    public GameObject PF_playerUI;
    public PlayerData playerData;
    private PlayerUI playerUI;

    
    private void OnEnable()
    {
            playerData = ScriptableObject.CreateInstance<PlayerData>();
            playerUI = Instantiate(PF_playerUI).GetComponent<PlayerUI>();
            playerUI.playerData = playerData;
    }
}