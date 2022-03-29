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

    public void Toggle()
    {
        floor = !floor;
        UpdateTile();
    }

    private void OnMouseDown()
    {
        if (floor) return;
        Toggle();
        GridController2D grid = GridController2D.singleton;
        if (!grid) return;
        grid.ChangeFloors(new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y));
        grid.MakeNodes();
    }
}
