using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardStatDisplay : MonoBehaviour
{
    private List<TextMeshProUGUI> _pool = new List<TextMeshProUGUI>();

    [SerializeField]
    private GameObject _template;

    private void OnEnable()
    {
        for (int i = 0; i < 15; i++) {
            _pool.Add(Instantiate(_template, transform).GetComponent<TextMeshProUGUI>());
            _pool[_pool.Count - 1].gameObject.SetActive(false);
        }
    }


    public void Display(List<(string, string)> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            var pair = data[i];

            _pool[i].gameObject.SetActive(true);
            _pool[i].text = pair.Item1 + ": " + pair.Item2;
        }
    }

    public void Clean()
    {
        foreach (var pair in _pool)
        {
            pair.gameObject.SetActive(false);
        }
    }
}
