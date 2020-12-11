using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.IO;

public class ImageViewer : MonoBehaviour
{
	GameObject Menu;
	public pickGalleyItem script;
	private const string API_KEY = "npRNEKsuRR2jhqYJVysirvrE";
	public static int id = 0;
	void Start(){
		Menu = GameObject.Find("Menu(Clone)");
		// Menu = GameObject.Find("Menu");
		script = Menu.GetComponent<pickGalleyItem>();
	}

    public void hideImageViewer(){
    	Destroy(script.viewer);
    	Destroy(Menu);
    }

    public void reset(){
    	int maxSize = 3000;
    	bool markTextureNonReadable = false;
    	Texture2D texture = NativeCamera.LoadImageAtPath(pickGalleyItem.image_path, maxSize, markTextureNonReadable);
    	// Add sprite as an image in instantiated prefab
        Texture2D tex = (Texture2D)texture;
        Sprite currSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        script.viewer.transform.GetChild(0).GetComponent<Image>().sprite = currSprite;
    }

    public void ConvertToGrayscale(){
    	Texture2D old_tex = script.viewer.transform.GetChild(0).GetComponent<Image>().sprite.texture as Texture2D;
    	Texture2D new_tex = ConvertToGrayscale(old_tex);
    	Sprite currSprite = Sprite.Create(new_tex, new Rect(0.0f, 0.0f, new_tex.width, new_tex.height), new Vector2(0.5f, 0.5f), 100.0f);
		script.viewer.transform.GetChild(0).GetComponent<Image>().sprite = currSprite; 
    }

    Texture2D ConvertToGrayscale(Texture2D graph)
    {
        Color32[] pixels = graph.GetPixels32();
        for(int x = 0; x < graph.width; x++)
        {
            for (int y = 0; y < graph.height; y++)
            {
                Color32 pixel = pixels[x + y * graph.width];
                int p = ((256 * 256 + pixel.r) * 256 + pixel.b) * 256 + pixel.g;
                int b = p % 256;
                p = Mathf.FloorToInt(p / 256);
                int g = p % 256;
                p = Mathf.FloorToInt(p / 256);
                int r = p % 256;
                float l = (0.2126f * r / 255f) + 0.7152f * (g / 255f) + 0.0722f * (b / 255f);
                Color c = new Color(l, l, l, 1);
                graph.SetPixel(x, y, c);
            }
        }
        graph.Apply();
        return graph;
    }

    public void removeBackground()
    {	
    	Texture2D old_tex = script.viewer.transform.GetChild(0).GetComponent<Image>().sprite.texture as Texture2D;
    	StartCoroutine(upload_image(old_tex));
    	Texture2D new_tex = old_tex;
    	 
    }

	public IEnumerator upload_image(Texture2D texture)
	{
		WWWForm form = new WWWForm();
		var imageData = texture.EncodeToPNG();
		form.AddField("size", "auto");
		form.AddBinaryData("image_file", imageData);
		UnityWebRequest www = UnityWebRequest.Post("https://api.remove.bg/v1.0/removebg", form);
		www.SetRequestHeader("X-Api-Key", "npRNEKsuRR2jhqYJVysirvrE");
		Debug.Log("yupz");
		yield return www.SendWebRequest();
		if (www.error != null)
			Debug.Log(www.error);
		else{
			Texture2D Tex2D;
		 	Tex2D = new Texture2D(2, 2);
		    if (Tex2D.LoadImage(www.downloadHandler.data)){
		    	Sprite currSprite = Sprite.Create(Tex2D, new Rect(0.0f, 0.0f, Tex2D.width, Tex2D.height), new Vector2(0.5f, 0.5f), 100.0f);
				script.viewer.transform.GetChild(0).GetComponent<Image>().sprite = currSprite;
		    }    
		}
	}

	public void save_image(){
		id++;
		Texture2D graph = script.viewer.transform.GetChild(0).GetComponent<Image>().sprite.texture as Texture2D;
		var bytes = graph.EncodeToPNG();
		string path = Path.Combine(Application.persistentDataPath, "gallery");
		if(!Directory.Exists(path))
      		Directory.CreateDirectory(path);
        System.IO.File.WriteAllBytes(Path.Combine(path,"template" + id + ".png"), bytes);
        hideImageViewer();
	}
}
