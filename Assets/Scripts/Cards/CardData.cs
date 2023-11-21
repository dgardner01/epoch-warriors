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
    public string title;
    public Type cardType;
    public int cost;
    public int order;
    public int damage;
}
