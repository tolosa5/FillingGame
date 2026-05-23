using System;
using System.Collections.Generic;
using UnityEngine;

public class BottlesHandler : MonoBehaviour
{
    [SerializeField] private List<Bottle> bottles = new List<Bottle>();
    [SerializeField] private GameObject bottlePrefab;
    private Bottle selectedBottle;
    private int bottlesToComplete;

    public Action OnCompletedLevelEvent;

    private void Start()
    {
        foreach (var bottle in bottles)
        {
            bottle.OnBottleClickEvent += HandleBottleClick;
            bottle.OnBottleCompletedEvent += CheckAllBottlesCompleted;
        }
    }
    
    public void SetBottles(int bottlesNumber, List<List<LiquidType>> liquids, int bottlesToBeCompleted)
    {
        for (int i = 0; i < bottlesNumber; i++)
        {
            Instantiate(bottlePrefab, transform, true);
            bottles.Add(bottlePrefab.GetComponent<Bottle>());
        }
        
        for (int i = 0; i < this.bottles.Count; i++)
        {
            this.bottles[i].SetUp(liquids[i]);
        }
        bottlesToComplete = bottlesToBeCompleted;
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

    private void HandleBottleClick(Bottle bottle, bool isSelected)
    {
        if (bottle.IsBottleCompleted())
            return;
        
        if (bottle.IsBottleEmpty() && selectedBottle == null)
            return;
        
        if (isSelected)
        {
            //Si la botella clicada esta seleccionada, se deselecciona
            selectedBottle.Deselect();
            selectedBottle = null;
        }
        else
        {
            if (selectedBottle == null)
            {
                //Si la botella clicada no esta seleccionada Y no hay ninguna seleccionada, se selecciona la botella clicada
                selectedBottle = bottle;
                selectedBottle.Select();
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

        if (aux < bottlesToComplete)
            return;
        
        OnCompletedLevelEvent?.Invoke();
    }
}
