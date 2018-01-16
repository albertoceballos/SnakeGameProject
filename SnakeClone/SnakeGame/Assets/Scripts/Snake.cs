using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour {

    private Snake next;
    static public Action<String> hit;

    public void SetNext(Snake s)
    {
        next = s;
    }

    public Snake GetNext()
    {
        return next;
    }

    public void RemoveTail()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hit != null)
        {
            hit(other.tag);
        }
        if (other.tag == "Food")
        {
            Destroy(other.gameObject);
        }
    }
}
