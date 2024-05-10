using UnityEngine;

public class PathSlot : MonoBehaviour
{
    [SerializeField] public Slot slot;
    [SerializeField] public Vector2Int pos;

    public void RotateWays(Vector3 axis, float rotationAngle)
    {
        pos = slot.pos;
        gameObject.transform.GetChild(0).Rotate(axis, rotationAngle); ;
    }
    public void RotateSlot(Vector3 axis, float rotationAngle)
    {
        this.RotateWays(axis, rotationAngle);
    }
}
