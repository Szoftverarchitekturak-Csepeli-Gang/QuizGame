using UnityEngine;

public class WaterAnimation : MonoBehaviour
{
    public MeshRenderer _meshRenderer;
    [SerializeField] private float _scrollSpeedX = 0.1f;
    [SerializeField] private float _scrollSpeedY = 0.0f;

    public string _textureProperty = "_BaseMap";
    private Material _mat;
    private Vector2 _offset;

    void Start()
    {
        if (_meshRenderer == null)
            _meshRenderer = GetComponent<MeshRenderer>();

        _mat = _meshRenderer.material;
        _offset = _mat.GetTextureOffset(_textureProperty);
    }

    void Update()
    {
        _offset += new Vector2(_scrollSpeedX, _scrollSpeedY) * Time.deltaTime;

        _offset.x = Mathf.Repeat(_offset.x, 1f);
        _offset.y = Mathf.Repeat(_offset.y, 1f);

        _mat.SetTextureOffset(_textureProperty, _offset);
    }
}
