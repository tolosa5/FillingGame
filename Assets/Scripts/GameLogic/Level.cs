using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelCreator", menuName = "Scriptable Objects/LevelCreator", order = 1)]
public class Level : ScriptableObject
{
    /* Como cada botella tiene varios liquidos, el valor del generico es otra lista, por lo que se necesita
     una clase a parte para serializar la lista de listas, ya que Unity no puede serializar listas de listas directamente
    */
    [Serializable]
    public class ListWrapper<T>
    {
        public List<T> data;
    }
    
    [Header("Bottles")]
    
    [SerializeField] private ListWrapper<ListWrapper<LiquidType>> liquids =  new ListWrapper<ListWrapper<LiquidType>>();
    public ListWrapper<ListWrapper<LiquidType>> Liquids => liquids;
    
    [SerializeField] private int bottlesNumber;
    public int BottlesNumber => bottlesNumber;
    
    
    [Header("Level Info")]
    [SerializeField] private int levelNumber = 0;
    public int LevelNumber => levelNumber;

    [SerializeField] private int bottlesToBeCompleted;
    public int BottlesToBeCompleted => bottlesToBeCompleted;
}
