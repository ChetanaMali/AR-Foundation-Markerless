using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceObjectOnPlane : MonoBehaviour
{
    private ARRaycastManager _raycastmanager;
    private Pose placementpose;
    private bool placementpositionIsValid;

    public GameObject positionIndicator;
    public GameObject prefabToPlace;
    public Camera aRCamera;

    private void Awake()
    {
        _raycastmanager = GetComponent<ARRaycastManager>();

    }
    // Update is called once per frame
    void Update()
    {
        UpdatePlacementpose();

        if (placementpositionIsValid && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            PlacementObject();
        }
    }

    private void UpdatePlacementpose()
    {
        var screenCenter = aRCamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();

        _raycastmanager.Raycast(screenCenter, hits, TrackableType.All);

        placementpositionIsValid = hits.Count > 0;

        if (placementpositionIsValid)
        {
            placementpose = hits[0].pose;
            var cameraForward = aRCamera.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;

            placementpose.rotation = Quaternion.LookRotation(cameraBearing);
            positionIndicator.SetActive(true);
            positionIndicator.transform.SetPositionAndRotation(placementpose.position, placementpose.rotation);

        }
        else
        {
            positionIndicator.SetActive(false);
        }
    }

    private void PlacementObject()
    {
        Instantiate(prefabToPlace, placementpose.position, placementpose.rotation);

    }
}
