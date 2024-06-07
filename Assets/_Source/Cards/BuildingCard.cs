using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "Building Card", menuName = "2Gether/Cards/Building Card")]
public class BuildingCard : Card
{
    public GameObject BuildingPrefab;

    public override void OnBeginUseCard(GameContext ctx)
    {
        ctx.buildingPlacer.StartPlaceMode(this);
    }

    public override void OnCardSubmitted(GameContext ctx)
    {
        ctx.cardUi.DiscardCard(this);
    }

    public override void OnEndUseCard(GameContext ctx)
    {
    }

    //public override void OnSubmitCard(GameContext ctx)
    //{
    //    /*if (GridController.Instance.TryPlace(new Vector2Int(0,0),new Vector2Int(0,0)*10, BuildingPrefab.GetComponent<Building>()))
    //    {
    //        // place building
    //    }*/
    //}

    private void OnValidate()
    {
        if (BuildingPrefab != null)
        {
            if (BuildingPrefab.GetComponent<Building>() == null)
            {
                BuildingPrefab = null;
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("Niew�a�ciwy prefab!", "Na prefabie brakuje komponentu Building!!", "Ok");
#endif
            }
        }
    }
}
