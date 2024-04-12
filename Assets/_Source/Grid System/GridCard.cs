using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Card/new Card", fileName = "New Card", order = 51)]
public class GridCard : ScriptableObject
{
    public Sprite icon;
    public GameObject prefab;
    public int cost;
}
