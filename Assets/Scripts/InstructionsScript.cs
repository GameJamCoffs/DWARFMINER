using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsScript : MonoBehaviour {

    public void LoadInstructions()
    {
        SceneManager.LoadScene("InstructionsScene", LoadSceneMode.Single);
    }
}
