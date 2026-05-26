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
    public List<LiquidType> LiquidsList => liquidsList;
    
    // 0 = Arriba, 1 = Medio arriba, 2 = Medio abajo, 3 Abajo
    [SerializeField] private GameObject liquidsVisualHolder;
    
    
    [Header("Events")]
    
    //bool para saber si se selecciona o deselecciona
    public Action<Bottle, bool> OnBottleClickEvent;
    public Action<Bottle> OnBottleCompletedEvent;

    public void SetUp(List<LiquidType> liquids)
    {
        //TODO: arreglar para que sea compatible con lista
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
        Debug.Log("Pouring liquid");
        LiquidType previousTopType = liquidStack.Pop();
        liquidsList.RemoveAt(0);
        
        ChangeBottleState();
        UpdateLiquidVisual();

        if (previousTopType != liquidStack.Peek() || state == BottleStates.EMPTY)
            return;
        
        PourLiquid();
    }

    public void CheckOnPoured(LiquidType type, Bottle pouringBottle, int iterations = 0)
    {
        Debug.Log("Being poured liquid");
        liquidStack.Push(pouringBottle.LiquidsList[iterations]);
        liquidsList.Add(pouringBottle.LiquidsList[iterations]);
        
        ChangeBottleState();
        UpdateLiquidVisual();

        LiquidType previousTopType = pouringBottle.LiquidsList[iterations];
        //Segun va iterando, hay que mirar mas abajo en la botella que echa liquido
        iterations++;
        if (previousTopType != pouringBottle.LiquidsList[iterations] ||
            pouringBottle.IsBottleEmpty() || IsBottleCompleted())
        {
            return;
        }
        
        CheckOnPoured(previousTopType, pouringBottle, iterations);
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

    private void UpdateLiquidVisual()
    {
        //0 = Abajo, 1 = Medio abajo, 2 = Medio arriba, 3 Arriba
        GameObject holder = liquidsVisualHolder.transform.GetChild(liquidsList.Count - 1).gameObject;
        //El color es el del liquido que esta en la parte de abajo, que es el ultimo de la lista
        int color = (int)liquidsList[^1];
        holder.transform.GetChild(color).gameObject.SetActive(true);
        
        //Todos los espacios encima del ultimo se quitan
        for (int i = liquidsList.Count; i < maxLiquidStack; i++)
        {
            Transform aux = liquidsVisualHolder.transform.GetChild(i);
            for (int j = 0; j < aux.childCount; j++)
            {
                aux.GetChild(j).gameObject.SetActive(false);
            }
        }
    }

    private void BottleCompleted()
    {
        state = BottleStates.COMPLETED;
        OnBottleCompletedEvent?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Bottle clicked");
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
