using UnityEngine;
using System.Collections;

public class Aspect : MonoBehaviour {

    //float cameraHeight;
    //float desiredAspect = 16f / 9f;
    //float oldAspect;
    private float sceneWidth = 18f;

    // Use this for initialization
    /*
        cameraRef = GetComponent<Camera>();
        cameraHeight = cameraRef.orthographicSize;
        if (oldAspect != cameraRef.aspect)
        {
            FixCameraSize();
        }
    */
    void Start()
    {
        Camera.main.orthographicSize = 1f / Camera.main.aspect * sceneWidth * 0.5f;
    }

    /*
    void FixCameraSize()
    {
        oldAspect = cameraRef.aspect;
        if (cameraHeight * desiredAspect > cameraHeight * cameraRef.aspect)
        {
            float ratio = desiredAspect / cameraRef.aspect;
            Camera.main.orthographicSize = cameraHeight * ratio;
        }
        else
        {
            cameraRef.orthographicSize = cameraHeight;
        }
    }
    */
}
