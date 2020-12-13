using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ARTapToPlaceObject : MonoBehaviour {
    public GameObject placementIndicator;
    public GameObject objectToPlace;
    public GameObject navbarBottom;
    public GameObject sliderPanel;
    public GameObject infoPanel;
    public GameObject loadingPanel;

    private ARRaycastManager raycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool imagePlaced = false;
    private GameObject imageGameObject;
    private static bool showInfo = true;
    private static bool templateSet = false;

    private void changeAlpha(Material mat, float alphaVal) {
        Color oldColor = mat.color;
        Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, alphaVal);
        mat.SetColor("_Color", newColor);
    }

    private void setSlidersInactive() {
        Slider[] children = sliderPanel.GetComponentsInChildren<Slider>();
        foreach (Slider child in children) {
            if (child.name == "ZoomSlider")
                child.value = 1.0f;
            else if (child.name == "TransparencySlider")
                child.value = 1.0f;
            else if (child.name == "RotateSlider")
                child.value = 0.0f;
            child.gameObject.SetActive(false);
        }
    }

    private void scaleImageQuad() {
        float pixelCountForOneUnit = Screen.height * 0.5f / Camera.main.orthographicSize;
        float scaleX = ChangeTemplate.templateTex.width / pixelCountForOneUnit;
        float scaleY = ChangeTemplate.templateTex.height / pixelCountForOneUnit;

        Vector3 scale = new Vector3(scaleX, scaleY, 1.0f);
        objectToPlace.transform.GetChild(0).GetComponent<Transform>().localScale = scale;
    }

    private void setDefaultTemplate() {
        if (!templateSet) {
            ChangeTemplate.templateTex = (Texture2D)Resources.LoadAll("Gallery", typeof(Texture2D))[0];
            templateSet = true;
        }
        scaleImageQuad();
    }

    void Start() {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        navbarBottom.SetActive(false);
        loadingPanel.SetActive(false);
        setSlidersInactive();
        if (showInfo) {
            infoPanel.SetActive(true);
            showInfo = false;
        } else {
            infoPanel.SetActive(false);
        }
        changeAlpha(placementIndicator.transform.GetChild(0).GetComponent<Renderer>().material, 1);
        setDefaultTemplate();
    }

    void Update() {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && !imagePlaced && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            PlaceObject();
            changeAlpha(placementIndicator.transform.GetChild(0).GetComponent<Renderer>().material, 0);
            navbarBottom.SetActive(true);
            imagePlaced = true;
        }
    }

    private void PlaceObject() {
        Texture2D selectedGalleryTexture = ChangeTemplate.templateTex;
        objectToPlace.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = selectedGalleryTexture;
        scaleImageQuad();
        imageGameObject = Instantiate(objectToPlace, placementPose.position, placementPose.rotation);
    }

    private void UpdatePlacementIndicator() {
        if (placementPoseIsValid) {
            placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        } else {
            placementIndicator.SetActive(false);
        }
    }

    private void UpdatePlacementPose() {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        raycastManager.Raycast(screenCenter, hits, TrackableType.Planes);

        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid) {
            placementPose = hits[0].pose;
        }
        var cameraForward = Camera.current.transform.forward;
        var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        placementPose.rotation = Quaternion.LookRotation(cameraBearing);
    }

    public void resetImage() {
        if (imagePlaced) {
            Destroy(imageGameObject);
            changeAlpha(placementIndicator.transform.GetChild(0).GetComponent<Renderer>().material, 1);
            navbarBottom.SetActive(false);
            setSlidersInactive();
            imagePlaced = false;
        }
    }

    IEnumerator loadSceneAsync() {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("GalleryScene");
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone) {
            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f) {
                // Activate the Scene
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void loadGallery() {
        loadingPanel.SetActive(true);
        StartCoroutine(loadSceneAsync());
    }

    public void toggleInfo() {
        infoPanel.SetActive(!infoPanel.activeSelf);
    }

    public void toggleSlider(GameObject slider) {
        bool sliderActive = slider.activeInHierarchy;
        slider.SetActive(!sliderActive);
    }

    public void zoomFunc(float value) {
        Vector3 newScale = new Vector3(value, value, value);
        imageGameObject.GetComponent<Transform>().localScale = newScale;
    }

    public void rotateFunc(float value) {
        Vector3 rot = imageGameObject.GetComponent<Transform>().rotation.eulerAngles;
        Quaternion newRotation = new Quaternion();
        newRotation.eulerAngles = new Vector3(rot.x, value, rot.z);
        imageGameObject.GetComponent<Transform>().rotation = newRotation;
    }

    public void transparencyFunc(float value) {
        Material mat = imageGameObject.transform.GetChild(0).GetComponent<Renderer>().material;
        changeAlpha(mat, value);
    }
}
