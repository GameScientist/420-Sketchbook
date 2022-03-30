using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TerrainTile : MonoBehaviour
{
    private BoxCollider2D box;
    private SpriteRenderer sprite;
    [SerializeField]
    private Sprite leftSprite;
    [SerializeField]
    private Sprite rightSprite;
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

    private void OnMouseOver()
    {
        if (floor || (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))) return;
        print("Click");
        Toggle();
        GridController2D grid = GridController2D.singleton;
        if (!grid) return;
        if (Input.GetMouseButtonDown(0))
        {
            sprite.sprite = leftSprite;
            grid.ChangeLeftFloor(new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y));
        }
        if (Input.GetMouseButtonDown(1))
        {
            sprite.sprite = rightSprite;
            grid.ChangeRightFloor(new Vector2Int((int)transform.localPosition.x, (int)transform.localPosition.y));
        }
        grid.MakeNodes();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PathfinderController>() != null) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
