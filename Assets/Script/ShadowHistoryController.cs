using UnityEngine;

public class ShadowHistoryController : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[Random.Range(0, sprites.Length - 1)];
    }
}
