using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    [SerializeField] GameObject _head;
    [SerializeField] GameObject _hand;
    public GameObject Player { get; set;}

    void Start()
    {
        
    }

    void LateUpdate()
    {
        if (Player != null)
        {
            Player.transform.position = _hand.transform.position - _hand.transform.up * 1.4f - _hand.transform.right * 0.2f + new Vector3(0.0f, -0.66f, 0.0f);
            Vector3 lookAt = _head.transform.position - Player.transform.position;
            Vector3 lookAtEuler = Quaternion.LookRotation(lookAt).eulerAngles;
            //Player.transform.rotation = Quaternion.Lerp(Player.transform.rotation, Quaternion.Euler(lookAtEuler.x, lookAtEuler.y, 0), Time.deltaTime);
        }
    }
}
