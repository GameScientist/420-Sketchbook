using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTile : MonoBehaviour
{
    private BoxCollider2D box;
    private SpriteRenderer sprite;
    public bool floor;

    private void OnValidate() => SetUpTile();

    // Start is called before the first frame update
    void Start() => SetUpTile();

    private void UpdateTile()
    {
        sprite.enabled = floor;
        box.enabled = !floor;
    }

    private void SetUpTile()
    {
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        UpdateTile();
    }

    private void OnMouseDown()
    {
        floor = !floor;
        UpdateTile();
        if (GridController.singleton) GridController.singleton.MakeNodes();
    }
}
