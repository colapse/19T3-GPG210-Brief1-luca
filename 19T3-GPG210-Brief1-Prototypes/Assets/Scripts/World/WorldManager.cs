using System;
using UI;
using UnityEngine;
using World;

public class WorldManager : MonoBehaviour
{
    public GameObject PF_playerUI;
    public PlayerData playerData;
    private PlayerUI playerUI;

    public GameMaster gameMaster;
    
    private void OnEnable()
    {
            playerData = ScriptableObject.CreateInstance<PlayerData>();
            playerUI = Instantiate(PF_playerUI).GetComponent<PlayerUI>();
            playerUI.playerData = playerData;
            
    }

    private void Start()
    {
        gameMaster = FindObjectOfType<GameMaster>();
    }

    public void LevelEndReached()
    {
        gameMaster?.LoadScene(GameMaster.SceneNames.MENU);
    }
}