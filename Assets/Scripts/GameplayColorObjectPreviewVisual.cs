using UnityEngine;

public class GameplayColorObjectPreviewVisual : MonoBehaviour
{
    static readonly int MainColor = Shader.PropertyToID("_MainColor");
    [SerializeField] MeshRenderer meshRenderer;

    public void SetColor(Color color)
    {
        meshRenderer.material.SetColor(MainColor, color);
    }
}