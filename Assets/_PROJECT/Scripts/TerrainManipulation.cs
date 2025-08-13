using UnityEngine;

public class TerrainManipulation : MonoBehaviour
{
    private Renderer _materialRenderer;
    [SerializeField] private float speed;
    private void Awake()
    {
        _materialRenderer = GetComponent<Renderer>();
    }
    private void FixedUpdate()
    {
        if (!_materialRenderer) return;
        var textureOffsetVector =
            new Vector2(_materialRenderer.material.GetTextureOffset("_BaseColorMap").x + speed,
                _materialRenderer.material.GetTextureOffset("_BaseColorMap").y);
            
        _materialRenderer.material.SetTextureOffset("_BaseColorMap", textureOffsetVector);
    }
}

