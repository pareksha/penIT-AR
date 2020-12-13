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
    public GameObject loadingPanel;

    private GameObject[] templateBtnPrefabs;

    // Create Category prefabs
    private void instantiateTemplateCategoryPrefabs() {
        loadingPanel.SetActive(true);
        noImageFoundText.SetActive(false);
        selectedCategory = "";

        // Destroy already created prefabs
        destroyTemplateBtnPrefabs();

        // Change Content call Size
        GameObject content = GameObject.Find("/Canvas/ScrollView/Viewport/Content");
        content.GetComponent<GridLayoutGroup>().cellSize = new Vector2(850, 850);

        StartCoroutine(instantiateTemplatePrefabsFromResources(content, "CategoryIcons"));
    }

    // Create prefabs of selected category
    private void instantiateTemplatePrefabs(string templateCategoryName) {
        loadingPanel.SetActive(true);
        selectedCategory = templateCategoryName;

        // Destroy already created prefabs
        destroyTemplateBtnPrefabs();

        // Change Content call Size
        GameObject content = GameObject.Find("/Canvas/ScrollView/Viewport/Content");
        content.GetComponent<GridLayoutGroup>().cellSize = new Vector2(850, 700);

        // Gather templates of selected category
        if (templateCategoryName == "User Added") {
            StartCoroutine(instantiateTemplatePrefabsFromInternalStorage(content));
        } else {
            StartCoroutine(instantiateTemplatePrefabsFromResources(content, templateCategoryName));
        }
    }

    IEnumerator instantiateTemplatePrefabsFromResources(GameObject content, string templateCategoryName) {
        yield return new WaitForEndOfFrame();

        Object[] galleryImages = Resources.LoadAll("Gallery/" + templateCategoryName, typeof(Texture2D));
        Array.Resize(ref templateBtnPrefabs, galleryImages.Length);

        yield return new WaitForEndOfFrame();

        // Create prefabs of selected category
        int count = 0;
        foreach (Object img in galleryImages) {
            instantiatePrefabHelper(img, content, count);
            count += 1;
            yield return new WaitForEndOfFrame();
        }

        loadingPanel.SetActive(false);
    }

    IEnumerator instantiateTemplatePrefabsFromInternalStorage(GameObject content) {
        yield return new WaitForEndOfFrame();

        string galleryPath = Path.Combine(Application.persistentDataPath, "gallery");
        string[] filenames = Directory.GetFiles(galleryPath, "*.png");
        Array.Resize(ref templateBtnPrefabs, filenames.Length);

        yield return new WaitForEndOfFrame();

        // Create prefabs of selected category
        int count = 0;
        foreach (string filename in filenames) {
            Object img = NativeCamera.LoadImageAtPath(Path.Combine(galleryPath, filename), 1000);
            yield return new WaitForEndOfFrame();
            instantiatePrefabHelper(img, content, count);
            count += 1;
            yield return new WaitForEndOfFrame();
        }

        loadingPanel.SetActive(false);

        if (filenames.Length == 0) {
            noImageFoundText.SetActive(true);
        }
    }

    private void instantiatePrefabHelper(Object img, GameObject content, int count) {
        // Creating button prefab in scroll view content
        GameObject prefab;
        if (selectedCategory == "")
            prefab = galleryCategoryBtnPrefab;
        else
            prefab = galleryBtnPrefab;

        GameObject btnPrefab = (GameObject)Instantiate(prefab);
        btnPrefab.transform.position = content.transform.position;
        btnPrefab.GetComponent<RectTransform>().SetParent(content.transform);

        // Correcting scale
        Vector3 newScale = new Vector3(1.0f, 1.0f, 1.0f);
        btnPrefab.GetComponent<Transform>().localScale = newScale;

        // Add sprite as an image in instantiated prefab
        Texture2D tex = (Texture2D)img;
        Sprite currSprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);

        // Adding template onclick
        if (selectedCategory == "") {
            btnPrefab.transform.GetChild(1).GetComponent<Image>().sprite = currSprite;
            btnPrefab.transform.GetChild(2).GetComponent<Text>().text = img.name;
            btnPrefab.GetComponent<Button>().onClick.AddListener(() => instantiateTemplatePrefabs(img.name));
        } else {
            btnPrefab.transform.GetChild(0).GetComponent<Image>().sprite = currSprite;
            btnPrefab.GetComponent<Button>().onClick.AddListener(() => ChangeTemplate.templateChange(tex));
        }

        // Add to prefab array
        templateBtnPrefabs[count] = btnPrefab;
    }

    private void destroyTemplateBtnPrefabs() {
        foreach (GameObject btnPrefab in templateBtnPrefabs) {
            Destroy(btnPrefab);
        }
        Array.Resize(ref templateBtnPrefabs, 0);
    }

    // For navigation purposes
    public static void inGalleryNavigation() {
        CreateGallery createGalleryObj = GameObject.Find("CreateGallery").GetComponent<CreateGallery>();
        createGalleryObj.instantiateTemplateCategoryPrefabs();
    }

    void Start() {
        templateBtnPrefabs = new GameObject[0];
        if (selectedCategory == "")
            instantiateTemplateCategoryPrefabs();
        else
            instantiateTemplatePrefabs(selectedCategory);
    }

}
