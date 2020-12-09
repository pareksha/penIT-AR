using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeTemplate : MonoBehaviour {
    public static Texture2D templateTex;

    public static void templateChange(Texture2D tex) {
        templateTex = tex;
        CreateGallery.selectedCategory = "";
        SceneManager.LoadScene("ARScene");
    }
}
