using UnityEngine;
using System.Collections.Generic;
using System;
public class DinamicStatistics : MonoBehaviour
{
    public UIFont font;
    private static Dictionary<string, float> dict;
    private bool isVisible = true;
    private bool isMoving = false;
    float to = 1f;
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

    void FixedUpdate()
    {
        // hide using alpha chanel change
        var panel = this.gameObject.GetComponent<UIPanel>();
        panel.alpha = Mathf.Lerp(panel.alpha, to, Time.smoothDeltaTime); 

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote))
        {
            if (isVisible)
            {
                isVisible = false;
                to = 0f;
            }
            else
            {
                isVisible = true;
                to = 1f;
            }
        }

        //#region Hide/Show Panel Using Position
        //var panel = this.gameObject.GetComponent<UIPanel>();
        //Vector3 unvisiblePosition = new Vector3(0, 100, 0);
        //Vector3 visiblePosition = new Vector3(0,0,0);

        //if (Input.GetKeyDown(KeyCode.BackQuote))
        //{
        //    isVisible = !(isVisible);
        //    isMoving = true;            
        //}
        //if (isMoving)
        //{
        //    if (isVisible)
        //    {
        //        panel.transform.localPosition = Vector3.Lerp(panel.transform.localPosition, visiblePosition, (Time.smoothDeltaTime));
        //        if (panel.transform.position.y == visiblePosition.y)
        //        {
        //            isMoving = false;
        //        }
        //    }
        //    else
        //    {
        //        panel.transform.localPosition = Vector3.Lerp(panel.transform.localPosition, unvisiblePosition, (Time.smoothDeltaTime));
        //        if (panel.transform.position.y == unvisiblePosition.y)
        //        {
        //            isMoving = false;
        //        }
        //    }
        //}
        //#endregion


        var vectorOffset = new Vector3(0, 20, 0);
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

                labelObj.transform.localPosition += vectorOffset;
                vectorOffset += new Vector3(0, -20, 0);

                var label = labelObj.AddComponent<UILabel>();
                label.pivot = UIWidget.Pivot.Left;
                label.font = font;
                label.text = item.Key + item.Value.ToString("#");
            }
        }
    }
}
