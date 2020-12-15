using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeTemplate : MonoBehaviour {
    public static Texture2D[] templateTex;

    IEnumerator loadSceneAsync() {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("ARScene");
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone) {
            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f) {
                // Activate the Scene
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public static void templateChange(Texture2D[] tex) {
        templateTex = tex;
        CreateGallery.selectedCategory = "";
        CreateGallery createGalleryObj = GameObject.Find("CreateGallery").GetComponent<CreateGallery>();
        createGalleryObj.loadingPanel.SetActive(true);
        ChangeTemplate changeTemplateObj = GameObject.Find("ChangeTemplate").GetComponent<ChangeTemplate>();
        changeTemplateObj.StartCoroutine(changeTemplateObj.loadSceneAsync());
    }
}
