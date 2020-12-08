using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageViewer : MonoBehaviour
{
	GameObject Menu;
	void Start(){
		Menu = GameObject.Find("Menu(Clone)");
	}

    public void hideImageViewer(){
    	pickGalleyItem x = Menu.GetComponent<pickGalleyItem>();
    	Destroy(x.viewer);
    	Destroy(Menu);
    }
}
