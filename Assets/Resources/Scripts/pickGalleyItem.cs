using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickGalleyItem : MonoBehaviour
{	
	public GameObject ImageViewer;
	public GameObject viewer;
	public static string image_path;
	
	// uncomment for testing on play button
	// void Start(){
	// 	GameObject canvas = GameObject.Find("/Canvas");
 //        viewer = (GameObject)Instantiate(ImageViewer, canvas.transform);
        
 //        // viewer.transform.position = canvas.transform.position;
 //        viewer.GetComponent<RectTransform>().SetParent(canvas.transform);
 //        Debug.Log("Fuck");
 //        Debug.Log(viewer);
 //        // Correcting scale
 //        Vector3 newScale = new Vector3(1.0f, 1.0f, 1.0f);
 //        viewer.GetComponent<Transform>().localScale = newScale;
	// }

    public void pickMedia()
	{	
		PickImage( 1000 );
	}

	public void clickImage(){
		TakePicture( 1000 );
	}

	private void PickImage( int maxSize )
	{
		bool markTextureNonReadable = false;
		NativeGallery.Permission permission = NativeGallery.GetImageFromGallery( ( path ) =>
		{
			Debug.Log( "Image path: " + path );
			image_path = path;
			if( path != null )
			{
				// Create Texture from selected image
				Texture2D texture = NativeCamera.LoadImageAtPath( path, maxSize, markTextureNonReadable);
				if( texture == null )
				{
					Debug.Log( "Couldn't load texture from " + path );
					return;
				}

				// Creating button prefab in scroll view content
				GameObject canvas = GameObject.Find("/Canvas");
	            viewer = (GameObject)Instantiate(ImageViewer, canvas.transform);
	            
	            // viewer.transform.position = canvas.transform.position;
	            viewer.GetComponent<RectTransform>().SetParent(canvas.transform);
	            // Correcting scale
	            Vector3 newScale = new Vector3(1.0f, 1.0f, 1.0f);
	            viewer.GetComponent<Transform>().localScale = newScale;

	            // Add sprite as an image in instantiated prefab
	            Texture2D tex = (Texture2D)texture;
	            Sprite currSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
	            viewer.transform.GetChild(0).GetComponent<Image>().sprite = currSprite;
			}
		}, "Select a PNG image", "image/png" );

		Debug.Log( "Permission result: " + permission );
	}

	private void TakePicture( int maxSize )
	{
		bool markTextureNonReadable = false;
		NativeCamera.Permission permission = NativeCamera.TakePicture( ( path ) =>
		{
			Debug.Log( "Image path: " + path );
			image_path = path;
			if( path != null )
			{
				// Create a Texture2D from the captured image
				Texture2D texture = NativeCamera.LoadImageAtPath( path, maxSize, markTextureNonReadable);
				if( texture == null )
				{
					Debug.Log( "Couldn't load texture from " + path );
					return;
				}

				// Creating button prefab in scroll view content
				GameObject canvas = GameObject.Find("/Canvas");
	            viewer = (GameObject)Instantiate(ImageViewer, canvas.transform);
	            
	            // viewer.transform.position = canvas.transform.position;
	            viewer.GetComponent<RectTransform>().SetParent(canvas.transform);
	            
	            // Correcting scale
	            Vector3 newScale = new Vector3(1.0f, 1.0f, 1.0f);
	            viewer.GetComponent<Transform>().localScale = newScale;

	            // Add sprite as an image in instantiated prefab
	            Texture2D tex = (Texture2D)texture;
	            Sprite currSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
	            viewer.transform.GetChild(0).GetComponent<Image>().sprite = currSprite;
			}
		}, maxSize );

		Debug.Log( "Permission result: " + permission );
	}

	public void hideImageViewer(){
		Destroy(viewer);
	}

	public void hide_menu(){
		Debug.Log("asdjandsja");
		GameObject Menu = GameObject.Find("Menu(Clone)");
		Destroy(Menu);
	}
}
