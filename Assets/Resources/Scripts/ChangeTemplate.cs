using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeTemplate : MonoBehaviour
{
    public static int templateId = 0;

    // Start is called before the first frame update
    public void templateChange(int id){
    	templateId = id;
    	SceneManager.LoadScene("ARScene");
    }
}
