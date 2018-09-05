using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour {

    int MovementModifier = 10;
    float MapXScale = 5;
    float MapYScale = 5;

    public Sprite TheMapVar;

    // Use this for initialization
    void Start()
    {
        TheMapVar = GameObject.Find("TheMap").GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKey(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || Input.GetKey(KeyCode.S)) && checkPosition("BOTTOM"))
        {
            gameObject.transform.Translate(Vector3.down * Time.deltaTime * MovementModifier);
        }
        if ((Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKey(KeyCode.D)) && checkPosition("RIGHT"))
        {
            gameObject.transform.Translate(Vector3.right * Time.deltaTime * MovementModifier);
        }
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A)) && checkPosition("LEFT"))
        {
            gameObject.transform.Translate(Vector3.left * Time.deltaTime * MovementModifier);
        }
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKey(KeyCode.W)) && checkPosition("TOP"))
        {
            gameObject.transform.Translate(Vector3.up * Time.deltaTime * MovementModifier);
        }

    }

    bool checkPosition(string boundary)
    {
        return true;

    }
}
