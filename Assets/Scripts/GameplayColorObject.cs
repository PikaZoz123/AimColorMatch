using System;
using UnityEngine;
using UnityEngine.Events;

public class GameplayColorObject : MonoBehaviour
{
    GameplayColorSO gameplayColorSO;
    [SerializeField] MeshRenderer meshRenderer;
    [HideInInspector] public UnityEvent<GameplayColorObject> onDestroyed;
    [HideInInspector] public UnityEvent<GameplayColorObject, bool> onTargeted;

    public void SetData(GameplayColorSO gameplayColorSO)
    {
        this.gameplayColorSO = gameplayColorSO;
        meshRenderer.material.color = gameplayColorSO.colorItemSO.colorValue;
    }

    public GameplayColorSO GetData() => gameplayColorSO;

    private void OnDestroy()
    {
        onDestroyed?.RemoveAllListeners();
        onTargeted?.RemoveAllListeners();
    }

    public void DestroyMe()
    {
        onDestroyed?.Invoke(this);
        Destroy(gameObject);
    }
    public void TargetMe(bool v)
    {
        onTargeted?.Invoke(this, v);
    }
}
