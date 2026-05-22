using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Liquid : MonoBehaviour
{
    public enum LiquidType
    {
        Red,
        Green,
        Blue,
        Yellow,
        Purple
    }
    [SerializeField] private LiquidType type;
    
}
