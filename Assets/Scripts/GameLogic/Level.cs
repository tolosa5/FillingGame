using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelCreator", menuName = "Scriptable Objects/LevelCreator", order = 1)]
public class Level : ScriptableObject
{
    // Diccionario serializable para guardar las botellas y sus respectivos tipos de liquido
    [Serializable]
    public class BottlesMap<T>
    {
        public List<T> data;
    }
    
    [Serializable]
    public class ItemKey : BottlesMap<Bottle>{}
    /* Como cada botella tiene varios liquidos, el valor del generico es otro BottleMap
     con una lista de liquidos ya que si fuese una lista directamente no apareceria en el inspector
    */
    [Serializable]
    public class ItemValue : BottlesMap<BottlesMap<Liquid.LiquidType>>{}
    
    [SerializeField] public ItemKey bottle;
    [SerializeField] public ItemValue liquids;
    
    [SerializeField] private int levelNumber = 0;
    public int LevelNumber => levelNumber;
    
    [SerializeField] private int bottlesNumber;
    public int BottlesNumber => bottlesNumber;
}
