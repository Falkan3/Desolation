using UnityEngine;
using System.Collections;

public class SortingOrderOnlyStart : MonoBehaviour {
    private Renderer objRenderer;

    // Use this for initialization
    void Start () {
        objRenderer = GetComponent<Renderer>();
        objRenderer.sortingOrder = (int)(transform.position.y * -10);
    }
}
