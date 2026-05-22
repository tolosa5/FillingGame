using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private int levelNumber = 0;
    [SerializeField] private Level[] levels;
    [SerializeField] private BottlesHandler bottleHandler;

    private void OnEnable()
    {
        bottleHandler.OnCompletedLevelEvent += HandleLevelCompleted;
    }

    public void CreateLevel(int levelNumber)
    {
        List<List<Liquid.LiquidType>> liquids = new List<List<Liquid.LiquidType>>();
        Level lvlToCreate = null;
        foreach (var level in levels)
        {
            if (level.LevelNumber == levelNumber)
            {
                lvlToCreate = level;
                return;
            }
        }
        foreach (var v in lvlToCreate.liquids.data)
        {
            liquids.Add(v.data);
        }
        bottleHandler.SetBottles(levels[levelNumber].bottle.data, liquids);
    }

    private void ResetLevel(int levelToBeCreated)
    {
        bottleHandler.ResetBottles();
        CreateLevel(levelToBeCreated);
    }

    private void HandleLevelCompleted()
    {
        levelNumber++;
        // Pantalla de victoria
        ResetLevel(levelNumber);
    }

    private void OnDisable()
    {
        bottleHandler.OnCompletedLevelEvent -= HandleLevelCompleted;
    }
}
