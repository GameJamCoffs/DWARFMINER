using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanternScript : MonoBehaviour {

    double LanternTimer = 0;

    int MAXLANTERNTIME = 20;

    public GameObject Player;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        LanternTimer += Time.deltaTime;

        if(LanternTimer >= MAXLANTERNTIME)
        {
            Destroy(gameObject);
            MovementScript ms = FindObjectOfType<MovementScript>();
            ms.removeLantern();
        }
	}
}
