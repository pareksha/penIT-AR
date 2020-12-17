using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class HandleNavigationScript : MonoBehaviour, IPointerClickHandler {
    public GameObject Menu;
    public bool menu_visible = false;

    private int prevSceneToLoad;
    private GameObject menu;

    private bool check_warning(){
    	GameObject clone = GameObject.Find("Message(Clone)");
    	if(clone){
    		return true;
    	}
    	return false;
    }

    private void show_warning(){
    	SSTools.ShowMessage("Press back again to exit!", SSTools.Position.bottom, SSTools.Time.twoSecond);
    }

    public void handleNavigation() {
        if (SceneManager.GetActiveScene().name == "GalleryScene" && CreateGallery.selectedCategory != "") {
            CreateGallery.inGalleryNavigation();
            return;
        }
        if (prevSceneToLoad >= 0) {
            SceneManager.LoadScene(prevSceneToLoad);
        } else if(!check_warning()){
        	show_warning();
        }else{
        	if(check_warning())
	            Application.Quit();
        }
    }

    // Start is called before the first frame update
    void Start() {
        prevSceneToLoad = SceneManager.GetActiveScene().buildIndex - 1;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            handleNavigation();
        }
    }

    public void OnPointerClick(PointerEventData eventData) {
        handleNavigation();
    }

    private void showMenu() {
        GameObject canvas = GameObject.Find("/Canvas");
        menu = (GameObject)Instantiate(Menu, canvas.transform);
        menu.GetComponent<RectTransform>().SetParent(canvas.transform);
        menu_visible = true;
    }

    private void hideMenu() {
        Destroy(menu);
        menu_visible = false;
    }

    public void handleMenu() {
        showMenu();
    }
}