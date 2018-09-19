using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternScript : MonoBehaviour {

    int TheLevel = 1;

    double LanternTimer = 0;

    int MAXLANTERNTIME = 20;

    public GameObject Player;

	// Use this for initialization
	void Start () {
        TheLevel = Player.GetComponent<MovementScript>().GetTheLevel();
    }
	
	// Update is called once per frame
	void Update () {
        LanternTimer += Time.deltaTime;

        if(LanternTimer >= (MAXLANTERNTIME + TheLevel))
        {
            Destroy(gameObject);
            MovementScript ms = FindObjectOfType<MovementScript>();
            ms.removeLantern();
        }
	}
}
