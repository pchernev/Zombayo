using UnityEngine;
using System.Collections.Generic;
using System;
public class DinamicStatistics : MonoBehaviour {
    public UIFont font;
    private static Dictionary<string, float> dict;

    private void InitStatsContainer() 
    {
        if (dict == null)
        {
            dict = new Dictionary<string, float>();
        }
    }

    public DinamicStatistics()
    {
        InitStatsContainer();
    }

    void Start()
    {
        this.InitStatsContainer();

        //Debug.Log("In Start -> DinamicStatistics");
        //var vectorOffset = new Vector3(0, 0, 0);
        //foreach (var item in dict)
        //{
        //    GameObject labelObj = NGUITools.AddChild(this.gameObject);
        //    labelObj.name = item.Key;
        //    vectorOffset -= new Vector3(0, 20, 0);
        //    labelObj.transform.localPosition = vectorOffset;
        //    var label = labelObj.AddComponent<UILabel>();
        //    label.pivot = UIWidget.Pivot.Left;
        //    UIFont font = Resources.Load<UIFont>("SciFi/SciFi Font - Header");
        //    label.font = font;
        //    label.text = item.Key + item.Value.ToString("#");
        //}
      
    }

    public float this[string key]
    {
        get
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, 0);                
            }
            return dict[key];
        }
        set
        {
            if (!dict.ContainsKey(key))
            {
                dict.Add(key, value);                   
            }
            else
            {
                dict[key] = value;
            }
        }
    }    
    void Update()
    {
        var vectorOffset = new Vector3(0, 30, 0);
        foreach (var item in dict)
        {
            var go = GameObject.Find(item.Key);
            if (go != null)
            {
                var label = go.GetComponent<UILabel>();
                label.text = item.Key + item.Value.ToString("#");
            }
            else
	        {
                GameObject labelObj = NGUITools.AddChild(this.gameObject);
                labelObj.name = item.Key;
  
                labelObj.transform.localPosition -= vectorOffset;
                vectorOffset -= new Vector3(0, 20, 0);
               

                var label = labelObj.AddComponent<UILabel>();
                label.pivot = UIWidget.Pivot.Left;
                label.font = font;
                label.text = item.Key + item.Value.ToString("#");
	        } 
        }        
    }

    void OnClick()
    {
        GameObject panel = GameObject.Find("Panel");
        NGUITools.SetActive(panel, false);
    }
}
