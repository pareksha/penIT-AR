using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickGalleyItem : MonoBehaviour
{	
	public GameObject ImageViewer;

    public void pickMedia()
	{	
		PickImage( 3000 );
	}

	public void clickImage(){
		TakePicture( 3000 );
	}

	private void PickImage( int maxSize )
	{
		NativeGallery.Permission permission = NativeGallery.GetImageFromGallery( ( path ) =>
		{
			Debug.Log( "Image path: " + path );
			if( path != null )
			{
				// Create Texture from selected image
				Texture2D texture = NativeGallery.LoadImageAtPath( path, maxSize );
				if( texture == null )
				{
					Debug.Log( "Couldn't load texture from " + path );
					return;
				}

				// Creating button prefab in scroll view content
				GameObject canvas = GameObject.Find("/Canvas");
	            GameObject viewer = (GameObject)Instantiate(ImageViewer, canvas.transform);
	            
	            // viewer.transform.position = canvas.transform.position;
	            viewer.GetComponent<RectTransform>().SetParent(canvas.transform);
	            Debug.Log("Fuck");
	            Debug.Log(viewer);
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
		NativeCamera.Permission permission = NativeCamera.TakePicture( ( path ) =>
		{
			Debug.Log( "Image path: " + path );
			if( path != null )
			{
				// Create a Texture2D from the captured image
				Texture2D texture = NativeCamera.LoadImageAtPath( path, maxSize );
				if( texture == null )
				{
					Debug.Log( "Couldn't load texture from " + path );
					return;
				}

				// Creating button prefab in scroll view content
				GameObject canvas = GameObject.Find("/Canvas");
	            GameObject viewer = (GameObject)Instantiate(ImageViewer, canvas.transform);
	            
	            // viewer.transform.position = canvas.transform.position;
	            viewer.GetComponent<RectTransform>().SetParent(canvas.transform);
	            Debug.Log("Fuck");
	            Debug.Log(viewer);
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
}
