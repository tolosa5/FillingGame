using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    LevelHandler levelHandler;
    MenuHandler menuHandler;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        EnterGameScene();
    }

    #region Game

    private void EnterGameScene()
    {
        levelHandler = FindObjectOfType<LevelHandler>();
        menuHandler = FindObjectOfType<MenuHandler>();
        levelHandler.OnLevelCompletedEvent += menuHandler.LevelCompletedScreen;
        menuHandler.OnNextLevelEvent += NextLevel;
    }

    private void ExitGameScene()
    {
        levelHandler.OnLevelCompletedEvent -= menuHandler.LevelCompletedScreen;
        menuHandler.OnNextLevelEvent -= NextLevel;
        
        levelHandler = null;
        menuHandler = null;
    }

    #endregion

    #region Menu
    
    private void NextLevel()
    {
        levelHandler.CreateLevel(levelHandler.LevelNumber);
    }
    
    #endregion

    private void OnDisable()
    {
        ExitGameScene();
        
    }
}
