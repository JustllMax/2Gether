using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;
public class CardStatDisplay : MonoBehaviour
{
    private List<GameObject> _pool = new List<GameObject>();
    private List<(TMP_Text, TMP_Text)> statTextPair = new List<(TMP_Text, TMP_Text)>(); 
    [SerializeField]
    private GameObject _template;

    public void Display(List<(string, string)> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            var pair = data[i];

            //_pool[i].text = pair.Item1 + ": " + pair.Item2;
            
            statTextPair[i].Item1.SetText(pair.Item1);
            statTextPair[i].Item2.SetText(pair.Item2);
        }
    }

    public void Clean()
    {
        foreach (var pair in _pool)
        {
            pair.gameObject.SetActive(false);
        }
    }

    public void SetUpDisplay(List<(string, string)> data)
    {
        if(data.Count <= 0)
        {
            return;
        }

      
        int rowsToSpawn = data.Count == 1 ? 1 : (int)Mathf.Ceil(data.Count / 2.0f);

        Debug.Log(this + " rows to spawn: " + rowsToSpawn);
        //Spawn X rows, each row has 2 TMP_Text components
        for (int i = 0; i < rowsToSpawn; i++)
        {   
            _pool.Add(Instantiate(_template, transform));

            List<TMP_Text> textComponents = _pool[_pool.Count - 1].GetComponentsInChildren<TMP_Text>().ToList();
            Debug.Log("Lista ");

            foreach (var t in textComponents)
            {
                Debug.Log(t.name);
            }
            Debug.Log("Pary ");

            foreach (var e in statTextPair)
            {
                Debug.Log(e.Item1);
                Debug.Log(e.Item2);
            }
            statTextPair.Add((textComponents[0], textComponents[1]));
            statTextPair.Add((textComponents[2], textComponents[3]));

        }

        Display(data);
    }
    

    
}
