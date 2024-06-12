using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffCard", menuName = "2Gether/Cards/BuffCard")]
public class BuffCard : Card
{
    public override void OnBeginUseCard(GameContext ctx)
    {
        ctx.cardUi.DiscardCard(this);
    }

    public override void OnCardSubmitted(GameContext ctx)
    {
    }

    public override void OnEndUseCard(GameContext ctx)
    {
    }


}
