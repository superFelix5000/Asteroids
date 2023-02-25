using UnityEngine;
using System.Collections;

/* 
    Resizes the box collider of the camera to match the frustum size.
    Only for orthographic cameras.
*/
public class CameraColliderResizer : MonoBehaviour {

    void Awake()
    {
        var collider = GetComponent<BoxCollider2D>();
        var camera = GetComponent<Camera>();

        var height = camera.orthographicSize * 2;
        var width = height * (float)Screen.width / (float)Screen.height;

        collider.size = new Vector2(width, height);
    }

}
