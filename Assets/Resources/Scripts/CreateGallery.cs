using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using Object = UnityEngine.Object;

public class CreateGallery : MonoBehaviour {
    public GameObject galleryBtnPrefab;
    public GameObject galleryCategoryBtnPrefab;
    public static string selectedCategory = "";
    public GameObject noImageFoundText;

    private GameObject[] templateBtnPrefabs;

    private void instantiateTemplateCategoryPrefabs() {
        noImageFoundText.SetActive(false);

        // Change Content call Size
        GameObject content = GameObject.Find("/Canvas/Scroll View/Viewport/Content");
        content.GetComponent<GridLayoutGroup>().cellSize = new Vector2(850, 850);

        Object[] galleryCategoryImages = Resources.LoadAll("Gallery/CategoryIcons", typeof(Texture2D));
        Array.Resize(ref templateBtnPrefabs, galleryCategoryImages.Length);
        int count = 0;
        foreach (Object img in galleryCategoryImages) {

            // Creating button prefab in scroll view content
            GameObject categoryBtnPrefab = (GameObject)Instantiate(galleryCategoryBtnPrefab);
            categoryBtnPrefab.transform.position = content.transform.position;
            categoryBtnPrefab.GetComponent<RectTransform>().SetParent(content.transform);

            // Correcting scale
            Vector3 newScale = new Vector3(1.0f, 1.0f, 1.0f);
            categoryBtnPrefab.GetComponent<Transform>().localScale = newScale;

            // Add sprite as an image in instantiated prefab
            Texture2D tex = (Texture2D)img;
            Sprite currSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            categoryBtnPrefab.transform.GetChild(1).GetComponent<Image>().sprite = currSprite;

            // Change text of the instantiated prefab
            categoryBtnPrefab.transform.GetChild(2).GetComponent<Text>().text = img.name;

            // Changing template texture in onclick
            categoryBtnPrefab.GetComponent<Button>().onClick.AddListener(() => instantiateTemplatePrefabs(img.name));

            // Add to prefab array
            templateBtnPrefabs[count++] = categoryBtnPrefab;
        }

        // Change selected category of templates
        selectedCategory = "";
    }

    private void instantiateTemplatePrefabs(string templateCategoryName) {
        // Destroy already created prefabs
        destroyTemplateBtnPrefabs();

        // Change Content call Size
        GameObject content = GameObject.Find("/Canvas/Scroll View/Viewport/Content");
        content.GetComponent<GridLayoutGroup>().cellSize = new Vector2(850, 700);

        // Gather templates of selected category
        Object[] galleryImages;
        if (templateCategoryName == "User Added") {
            galleryImages = loadUserAddedTemplates();
        } else
            galleryImages = Resources.LoadAll("Gallery/" + templateCategoryName, typeof(Texture2D));

        Array.Resize(ref templateBtnPrefabs, galleryImages.Length);
        if (galleryImages.Length == 0) {
            noImageFoundText.SetActive(true);
        }

        // Create prefabs of selected category
        int count = 0;
        foreach (Object img in galleryImages) {

            // Creating button prefab in scroll view content
            GameObject btnPrefab = (GameObject)Instantiate(galleryBtnPrefab);
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

            // Add to prefab array
            templateBtnPrefabs[count++] = btnPrefab;
        }

        // Change selected category of templates
        selectedCategory = templateCategoryName;
    }

    private void destroyTemplateBtnPrefabs() {
        foreach (GameObject btnPrefab in templateBtnPrefabs) {
            Destroy(btnPrefab);
        }
        Array.Resize(ref templateBtnPrefabs, 0);
    }

    public Object[] loadUserAddedTemplates() {
        int maxSize = 1000;
        string galleryPath = Path.Combine(Application.persistentDataPath, "gallery");
        string[] filenames = Directory.GetFiles(galleryPath, "*.png");
        Debug.Log(filenames.Length);
        Object[] customImages = new Object[filenames.Length];
        int id = 0;
        foreach (string filename in filenames) {
            customImages[id++] = NativeCamera.LoadImageAtPath(Path.Combine(galleryPath, filename), maxSize);
        }
        return customImages;
    }

    // For navigation purposes
    public static void inGalleryNavigation() {
        CreateGallery createGalleryObj = GameObject.Find("CreateGallery").GetComponent<CreateGallery>();
        createGalleryObj.destroyTemplateBtnPrefabs();
        createGalleryObj.instantiateTemplateCategoryPrefabs();
    }

    void Start() {
        if (selectedCategory == "")
            instantiateTemplateCategoryPrefabs();
        else
            instantiateTemplatePrefabs(selectedCategory);
    }

}
