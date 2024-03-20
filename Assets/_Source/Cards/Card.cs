using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Card : ScriptableObject
{

    public Sprite sprite;
    public GameObject P_GhostObject;
    public CardStatistics SO_cardStatistics;
  

    public abstract void Use();
    public abstract void Update();
    public abstract void Destroy();

}
