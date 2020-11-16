using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HandleNavigationScript : MonoBehaviour, IPointerClickHandler
{
	private int prevSceneToLoad;

	void handleNavigation(){
		if(prevSceneToLoad>=0){
    		SceneManager.LoadScene(prevSceneToLoad);
    	}else{
    		Application.Quit();
    	}
	}
    // Start is called before the first frame update
    void Start()
    {
        prevSceneToLoad = SceneManager.GetActiveScene().buildIndex - 1;
    }

    // Update is called once per frame
    void update(){
    	if(Input.GetKeyDown(KeyCode.Escape)){
    		handleNavigation();
    	}
    }

    public void OnPointerClick(PointerEventData eventData){
    	handleNavigation();
    }
}