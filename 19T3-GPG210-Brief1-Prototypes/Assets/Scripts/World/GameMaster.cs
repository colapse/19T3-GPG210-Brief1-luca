using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : SerializedMonoBehaviour
{
    
    public enum SceneNames
    {
        MENU,
        LVL1
    }
    
    public GameObject loadingScreen;
    [OdinSerialize]
    public Dictionary<SceneNames, string> scenes = new Dictionary<SceneNames, string>();

    public SceneNames activeScene = SceneNames.MENU;
    private bool isLoadingScene = false;
    
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        ToggleLoadingScreen(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(SceneNames sceneName)
    {
        if(isLoadingScene)
            return;
        StartCoroutine(Co_LoadScene(sceneName));
    }

    private IEnumerator Co_LoadScene(SceneNames sceneName)
    {
        if(isLoadingScene)
            yield break;
        isLoadingScene = true;
        ToggleLoadingScreen(true);  
        yield return new WaitForSeconds(1);
        SceneManager.UnloadSceneAsync(scenes[activeScene]);
        AsyncOperation asOpLoader = SceneManager.LoadSceneAsync(scenes[sceneName]);
        asOpLoader.completed += operation =>
            {
                activeScene = sceneName;
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(scenes[sceneName]));
                isLoadingScene = false;
            };
        yield return new WaitForSeconds(1);
        ToggleLoadingScreen(false);

        yield return 0;
    }

    private void ToggleLoadingScreen(bool show)
    {
        loadingScreen.SetActive(show);
    }

    public void HandleBtnPlayClicked()
    {
        LoadScene(SceneNames.LVL1);
    }
}
