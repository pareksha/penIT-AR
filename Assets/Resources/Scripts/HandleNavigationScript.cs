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

    public void handleNavigation() {
        if (SceneManager.GetActiveScene().name == "GalleryScene" && CreateGallery.selectedCategory != "") {
            CreateGallery.inGalleryNavigation();
            return;
        }
        if (menu_visible) {
            hideMenu();
            return;
        }
        if (prevSceneToLoad >= 0) {
            SceneManager.LoadScene(prevSceneToLoad);
        } else {
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