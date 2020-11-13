using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class LoadPreviousScreenOnImageClick : MonoBehaviour, IPointerClickHandler
{
	private int prevSceneToLoad;
    // Start is called before the first frame update
    void Start()
    {
        prevSceneToLoad = SceneManager.GetActiveScene().buildIndex - 1;
    }

    // Update is called once per frame
    public void OnPointerClick(PointerEventData eventData){
    	Debug.Log(prevSceneToLoad);
    	if(prevSceneToLoad>=0){
    		SceneManager.LoadScene(prevSceneToLoad);
    	}
    }
}