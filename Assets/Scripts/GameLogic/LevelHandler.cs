using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour
{
    private int levelNumber = 0;
    public int LevelNumber => levelNumber;
    [SerializeField] private Level[] levels;
    
    private BottlesHandler bottleHandler;
    
    public Action<int> OnLevelCompletedEvent;
    
    private void Start()
    {
        bottleHandler = FindObjectOfType<BottlesHandler>();
        bottleHandler.OnCompletedLevelEvent += HandleLevelCompleted;

        levelNumber = 1;
        CreateLevel(levelNumber);
    }

    public void CreateLevel(int levelNumber)
    {
        ResetLevel();
        List<List<LiquidType>> liquids = new List<List<LiquidType>>();
        Level lvlToCreate = null;
        foreach (var level in levels)
        {
            if (level.LevelNumber == levelNumber)
            {
                lvlToCreate = level;
                break;
            }
        }
        foreach (var v in lvlToCreate.Liquids.data)
        {
            liquids.Add(v.data);
        }
        bottleHandler.SetBottles(lvlToCreate.BottlesNumber, liquids, lvlToCreate.BottlesToBeCompleted);
    }

    private void ResetLevel()
    {
        bottleHandler.ResetBottles();
    }

    private void HandleLevelCompleted()
    {
        levelNumber++;
        // Pantalla de victoria
        OnLevelCompletedEvent?.Invoke(levelNumber);
    }

    private void OnDisable()
    {
        bottleHandler.OnCompletedLevelEvent -= HandleLevelCompleted;
    }
}
