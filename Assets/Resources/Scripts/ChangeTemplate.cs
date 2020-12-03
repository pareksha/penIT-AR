using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeTemplate : MonoBehaviour
{
    public static int templateId = 3;

    public void templateChange(int id){
    	templateId = id;
    	SceneManager.LoadScene("ARScene");
    }
}
