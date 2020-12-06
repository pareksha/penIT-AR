using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Object = UnityEngine.Object;

public class CreateGallery : MonoBehaviour {
    public GameObject galleryBtnPrefab;
    public string[] templateDir;

    void Start() {
        Object[] galleryImages = Resources.LoadAll("Gallery", typeof(Texture2D));
        foreach (Object img in galleryImages) {

            // Creating button prefab in scroll view content
            GameObject btnPrefab = (GameObject)Instantiate(galleryBtnPrefab);
            GameObject content = GameObject.Find("/Canvas/Scroll View/Viewport/Content");
            btnPrefab.transform.position = content.transform.position;
            btnPrefab.GetComponent<RectTransform>().SetParent(content.transform);

            // Correcting scale
            Vector3 newScale = new Vector3(1.0f, 1.0f, 1.0f);
            btnPrefab.GetComponent<Transform>().localScale = newScale;

            // Add sprite as an image in instantiated prefab
            Texture2D tex = (Texture2D)img;
            Sprite currSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            btnPrefab.transform.GetChild(0).GetComponent<Image>().sprite = currSprite;

            // Changing template texture in onclick
            btnPrefab.GetComponent<Button>().onClick.AddListener(() => ChangeTemplate.templateChange(tex));
        }
    }

}
