using UnityEngine;

public class PathSlot : MonoBehaviour
{
    [SerializeField] public Slot slot;
    [SerializeField] public Vector2Int pos;

    public void RotateWays(Vector3 axis, float rotationAngle)
    {
        pos = slot.pos;
        var slotWays = gameObject.transform.GetChild(0);
        slotWays.Rotate(axis, rotationAngle);
        for(int i = 0; i < slotWays.transform.childCount; i++)
        {
            if(slotWays.transform.GetChild(i).tag == "Terrain")
            {
                slotWays.transform.GetChild(i).Rotate(axis, rotationAngle*(-1));
            }
        }
        
    }
    public void RotateSlot(Vector3 axis, float rotationAngle)
    {
        this.RotateWays(axis, rotationAngle);
    }
}
