using UnityEngine;

public class WaterAnimation : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public float scrollSpeedX = 0.1f;
    public float scrollSpeedY = 0.0f;

    public string textureProperty = "_BaseMap";
    private Material mat;
    private Vector2 offset;

    void Start()
    {
        if (meshRenderer == null)
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        // Get a unique material instance (so you don’t modify the shared one)
        mat = meshRenderer.material;
        offset = mat.GetTextureOffset(textureProperty);
    }

    void Update()
    {
        // Animate texture offset over time
        offset.x += scrollSpeedX * Time.deltaTime;
        offset.y += scrollSpeedY * Time.deltaTime;

        // Prevent large values
        offset.x = Mathf.Repeat(offset.x, 1f);
        offset.y = Mathf.Repeat(offset.y, 1f);

        // Apply to the texture property
        mat.SetTextureOffset(textureProperty, offset);
    }
}
