using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour {

    public string blocktype = "STONE";

	// Use this for initialization
	void Start () {
        switch (blocktype)
        {
            case "STONE":
                gameObject.GetComponent<SpriteRenderer>().color = Color.grey;
                break;
            case "GOLD":
                gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                break;
            case "MITHRIL":
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
