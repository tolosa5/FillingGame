using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Bottle : MonoBehaviour, IPointerClickHandler
{
    private enum BottleStates
    {
        EMPTY,
        NOT_FULL,
        FULL,
        COMPLETED
    }

    private BottleStates state = BottleStates.EMPTY;

    [HideInInspector] public bool isSelectable;
    private bool isSelected;
    [SerializeField] private GameObject highlightVisual;
    
    
    [Header("Liquid")]
    
    private readonly int maxLiquidsQuantity = 4;
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
        ClearLiquidVisual();
        foreach (LiquidType liquid in liquids)
        {
            liquidsList.Add(liquid);
            UpdateLiquidVisual();
        }
        ChangeBottleState();
    }
    
    public void PourLiquid()
    {
        Debug.Log("Pouring liquid");
        LiquidType previousTopType = liquidsList.Last();
        liquidsList.RemoveAt(liquidsList.Count - 1);
        
        ChangeBottleState();
        UpdateLiquidVisual();

        if (previousTopType != liquidsList[0] || state == BottleStates.EMPTY)
            return;
        
        PourLiquid();
    }

    public void CheckOnPoured(LiquidType type, Bottle pouringBottle, int iterations = 0)
    {
        liquidsList.Add(pouringBottle.LiquidsList[iterations]);
        Debug.Log("Being poured liquid");
        
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
        switch (liquidsList.Count)
        {
            case 0:
                state = BottleStates.EMPTY;
                break;
            case 1 or 2 or 3:
                state = BottleStates.NOT_FULL;
                break;
            case 4:
                LiquidType firstType = liquidsList[0];
                foreach (LiquidType type in liquidsList)
                {
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
        int color = (int)liquidsList.Last();
        GameObject subHolder = holder.transform.GetChild(color).gameObject;
        subHolder.SetActive(true);
        
        //Todos los espacios encima del ultimo se quitan
        for (int i = liquidsList.Count; i < maxLiquidsQuantity; i++)
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

    private void ClearLiquidVisual()
    {
        for (int i = 0; i < liquidsVisualHolder.transform.childCount; i++)
        {
            Transform aux = liquidsVisualHolder.transform.GetChild(i);
            for (int j = 0; j < aux.childCount; j++)
            {
                aux.GetChild(j).gameObject.SetActive(false);
            }
        }
    }

    #region Selection

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Bottle clicked");
        OnBottleClickEvent?.Invoke(this, isSelected);
    }

    public void Deselect()
    {
        isSelected = false;
        highlightVisual.SetActive(false);
    }

    public void Select()
    {
        isSelected = true;
        highlightVisual.SetActive(true);
    }

    #endregion


    #region Getters

    public bool IsBottleEmpty() { return state == BottleStates.EMPTY; }
    public bool IsBottleNotFull() { return state == BottleStates.NOT_FULL; }
    public bool IsBottleFull() { return state == BottleStates.FULL; }
    public bool IsBottleCompleted() { return state == BottleStates.COMPLETED; }

    #endregion
}
