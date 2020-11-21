using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ARTapToPlaceObject : MonoBehaviour {
    public Texture[] textures;
    public GameObject placementIndicator;
    public GameObject objectToPlace;
    public GameObject navbarBottom;
    public GameObject sliderPanel;

    private ARRaycastManager raycastManager;
    private Pose placementPose;
    private bool placementPoseIsValid = false;
    private bool imagePlaced = false;
    private GameObject imageGameObject;
    private int templateid = ChangeTemplate.templateId;

    void setSlidersInactive() {
        Slider[] children = sliderPanel.GetComponentsInChildren<Slider>();
        foreach (Slider child in children) {
            if (child.name == "ZoomSlider")
                child.value = 1.0f;
            else if (child.name == "TransparencySlider")
                child.value = 100.0f;
            else if (child.name == "RotateSlider")
                child.value = 0.0f;
            child.gameObject.SetActive(false);
        }
    }

    void Start() {
        raycastManager = FindObjectOfType<ARRaycastManager>();
        navbarBottom.SetActive(false);
        setSlidersInactive();
    }

    void Update() {
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && !imagePlaced && Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) {
            PlaceObject();
            navbarBottom.SetActive(true);
            imagePlaced = true;
        }
    }

    private void PlaceObject() {
        templateid = ChangeTemplate.templateId;
        objectToPlace.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = textures[templateid];
        // GetComponent<Renderer>().material.mainTexture = textures[templateid];
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
            navbarBottom.SetActive(false);
            setSlidersInactive();
            imagePlaced = false;
        }
    }

    public void loadGallery() {
        SceneManager.LoadScene("GalleryScene");
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
}
