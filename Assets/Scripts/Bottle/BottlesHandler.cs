using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottlesHandler : MonoBehaviour
{
    [SerializeField] private List<Bottle> bottles = new List<Bottle>();
    [SerializeField] private GameObject bottlePrefab;
    private Bottle selectedBottle;

    public Action OnCompletedLevelEvent;

    private void Start()
    {
        foreach (var bottle in bottles)
        {
            bottle.OnBottleClickEvent += HandleBottleClick;
            bottle.OnBottleCompletedEvent += CheckAllBottlesCompleted;
        }
    }

    private void HandleBottleClick(Bottle bottle, bool isSelected)
    {
        if (bottle.IsBottleCompleted())
            return;
        
        if (bottle.IsBottleEmpty() && selectedBottle == null)
            return;
        
        if (isSelected)
        {
            bottle.Deselect();
            selectedBottle = null;
        }
        else
        {
            if (selectedBottle == null)
            {
                //Si la botella clicada no esta seleccionada Y no hay ninguna seleccionada, se selecciona la botella clicada
                selectedBottle = bottle;
                bottle.Select();
            }
            else
            {
                //Si hay alguna seleccionada diferente, es que quiere echar liquido
                selectedBottle.PourLiquid();
                bottle.CheckOnPoured(selectedBottle.LiquidStack.Peek());
            }
        }
    }

    private void CheckAllBottlesCompleted(Bottle bottle)
    {
        int aux = 0;
        foreach (Bottle a in bottles)
        {
            if (a.IsBottleCompleted())
                aux++;
        }

        if (aux < bottles.Count + 1)
            return;
        
        OnCompletedLevelEvent?.Invoke();
    }
    
    public void SetBottles(List<Bottle> bottles, List<List<Liquid.LiquidType>> liquids)
    {
        foreach (Bottle bottle in bottles)
        {
            Instantiate(bottlePrefab, transform, true);
            this.bottles.Add(bottle);
        }
        
        for (int i = 0; i < this.bottles.Count; i++)
        {
            this.bottles[i].SetUp(liquids[i]);
        }
    }
    
    public void ResetBottles()
    {
        foreach (Bottle bottle in bottles)
        {
            Destroy(bottle.gameObject);
        }
        bottles.Clear();
        selectedBottle = null;
    }
}
