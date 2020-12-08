using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : MonoBehaviour
{
	GameObject Menu;
	public pickGalleyItem script;
	void Start(){
		Menu = GameObject.Find("Menu(Clone)");
		script = Menu.GetComponent<pickGalleyItem>();
	}

    public void hideImageViewer(){
    	Destroy(script.viewer);
    	Destroy(Menu);
    }

    public void ConvertToGrayscale(){
    	Texture2D old_tex = script.viewer.transform.GetChild(0).GetComponent<Image>().sprite.texture as Texture2D;
    	Texture2D new_tex = ConvertToGrayscale(old_tex);
    	Sprite currSprite = Sprite.Create(new_tex, new Rect(0.0f, 0.0f, new_tex.width, new_tex.height), new Vector2(0.5f, 0.5f), 100.0f);
		script.viewer.transform.GetChild(0).GetComponent<Image>().sprite = currSprite;
		// Destroy(currSprite, 5);
		currSprite = Sprite.Create(old_tex, new Rect(0.0f, 0.0f, new_tex.width, new_tex.height), new Vector2(0.5f, 0.5f), 100.0f);
		script.viewer.transform.GetChild(0).GetComponent<Image>().sprite = currSprite;

    }

    Texture2D ConvertToGrayscale(Texture2D graph)
    {
        Color32[] pixels = graph.GetPixels32();
        Debug.Log(graph.width);
        Debug.Log(graph.height);
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
        graph.Apply(false);
        return graph;
        var bytes = graph.EncodeToPNG();
        System.IO.File.WriteAllBytes(Application.dataPath + "ImageSaveTest.png", bytes);
    }
}
