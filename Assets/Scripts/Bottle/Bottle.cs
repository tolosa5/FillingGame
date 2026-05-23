using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bottle : MonoBehaviour, IPointerClickHandler
{
    public enum BottleStates
    {
        EMPTY,
        NOT_FULL,
        FULL,
        COMPLETED
    }

    private BottleStates state = BottleStates.EMPTY;

    [HideInInspector] public bool isSelectable;
    private bool isSelected;
    
    
    [Header("Liquid")]
    
    private readonly int maxLiquidStack = 4;
    private Stack<LiquidType> liquidStack = new Stack<LiquidType>();
    public Stack<LiquidType> LiquidStack => liquidStack;
    private List<LiquidType> liquidsList = new List<LiquidType>();
    
    
    [Header("Events")]
    
    //bool para saber si se selecciona o deselecciona
    public Action<Bottle, bool> OnBottleClickEvent;
    public Action<Bottle> OnBottleCompletedEvent;

    public void SetUp(List<LiquidType> liquids)
    {
        liquidsList = liquids;
        if (liquids.Count <= 0)
        {
            ChangeBottleState();
            return;
        }
        for (int i = liquidsList.Count; i <= 0; i--)
        {
            liquidStack.Push(liquidsList[i]);
        }
        ChangeBottleState();
    }
    
    public void PourLiquid()
    {
        LiquidType previousTopType = liquidStack.Pop();
        liquidsList.RemoveAt(0);
        
        ChangeBottleState();

        if (previousTopType != liquidStack.Peek() || state == BottleStates.EMPTY)
            return;
        
        PourLiquid();
    }

    public void CheckOnPoured(LiquidType type)
    {
        liquidStack.Push(type);
        liquidsList.Add(type);
        ChangeBottleState();
    }

    private void ChangeBottleState()
    {
        switch (liquidStack.Count)
        {
            case 0:
                state = BottleStates.EMPTY;
                break;
            case 1 or 2 or 3:
                state = BottleStates.NOT_FULL;
                break;
            case 4:
                foreach (LiquidType type in liquidsList)
                {
                    LiquidType firstType = liquidsList[0];
                    if (type != firstType)
                    {
                        //si algun tipo es diferente a los demas es que no esta correcta
                        state = BottleStates.FULL;
                        return;
                    }
                }
                BottleCompleted();
                break;
        }
    }

    private void BottleCompleted()
    {
        state = BottleStates.COMPLETED;
        OnBottleCompletedEvent?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnBottleClickEvent?.Invoke(this, isSelected);
    }

    public void Deselect()
    {
        isSelected = false;
    }

    public void Select()
    {
        isSelected = true;
    }

    #region Getters

    public bool IsBottleEmpty() { return state == BottleStates.EMPTY; }
    public bool IsBottleNotFull() { return state == BottleStates.NOT_FULL; }
    public bool IsBottleFull() { return state == BottleStates.FULL; }
    public bool IsBottleCompleted() { return state == BottleStates.COMPLETED; }

    #endregion
}
