using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardData : MonoBehaviour
{
    public enum Type
    {
        Attack,
        Block,
        Grab
    }
    public Type cardType;
    public int cost;
    public int order;
}
