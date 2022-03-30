using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The player character walks across these to reach its destination.
/// </summary>
public class TerrainTile : MonoBehaviour
{
    /// <summary>
    /// If there is a floor placed over this tile.
    /// </summary>
    public bool floor;
    /// <summary>
    /// Kills the player on collision if active.
    /// </summary>
    private BoxCollider2D box;
    /// <summary>
    /// The sprite used to render a the left floor.
    /// </summary>
    [SerializeField]
    private Sprite leftSprite;
    /// <summary>
    /// The sprite used to render a the left floor.
    /// </summary>
    [SerializeField]
    private Sprite rightSprite;
    /// <summary>
    /// Renders the current sprite.
    /// </summary>
    private SpriteRenderer sprite;

    private void OnValidate() => SetUpTile();

    // Start is called before the first frame update
    void Start() => SetUpTile();

    /// <summary>
    /// Gets all of the components of a tile before an initial update.
    /// </summary>
    private void SetUpTile()
    {
        box = GetComponent<BoxCollider2D>();
        sprite = GetComponent<SpriteRenderer>();
        UpdateTile();
    }

    /// <summary>
    /// Sets the activation status of the sprite renderer and collider.
    /// </summary>
    private void UpdateTile()
    {
        sprite.enabled = floor;
        box.enabled = !floor;
    }

    /// <summary>
    /// Flips the floor boolean and updates the tile.
    /// </summary>
    public void Toggle()
    {
        floor = !floor;
        UpdateTile();
    }

    private void OnMouseOver()
    {
        if (floor || (!Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))) return;
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
