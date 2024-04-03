using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffCard", menuName = "2Gether/Cards/BuffCard")]
public class BuildingCard : Card
{
    public Building BuildingPrefab;

    public override void OnSubmitCard(GameContext ctx)
    {
        if (GridController.Instance.TryPlace(ctx.cardUi.GetSelectedTileCoords(), BuildingPrefab))
        {
            // place building
        }
    }
}
