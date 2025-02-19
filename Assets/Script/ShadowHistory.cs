using UnityEngine;

public class ShadowHistory : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length - 1)];
        transform.Rotate(new Vector3(0, 0, 90 * Random.Range(0, 3)));
    }
}
