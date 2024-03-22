using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffCard", menuName = "2Gether/Cards/BuffCard")]
public class BuffCard : Card
{
    public override void OnSubmitCard(GameContext ctx)
    {
        Log.Debug("Card submitted: " + this.CardName);
    }
}
