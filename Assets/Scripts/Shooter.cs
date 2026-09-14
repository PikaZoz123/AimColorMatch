using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Shooter : MonoBehaviour
{
    [SerializeField] Anchor anchor; //temp
    GameplayColorObject colorObject; // temp

    [HideInInspector] public UnityEvent<GameObject> onTargetHit;
    [HideInInspector] public UnityEvent<GameObject> onTargetShot;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(1);
        UpdateAnchor();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TargetShot(colorObject);
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            TargetHit(colorObject, true);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            TargetHit(colorObject, false);
        }
        else if (Input.GetKeyDown(KeyCode.U))
        {
            UpdateAnchor();
        }
    }

    private void UpdateAnchor()
    {
        Debug.Log($"Update Anchor: {anchor.name}");
        colorObject = anchor.GetColorObject(0);
    }

    private void TargetHit(GameplayColorObject colorObject, bool v)
    {
        if (v)
        {
            Debug.Log($"Hitting Gameplay Object ! {colorObject.GetData().colorItemSO.colorItemID}");
        }
        else
        {
            Debug.Log($"NOT Hitting Gameplay Object ! {colorObject.GetData().colorItemSO.colorItemID}");
        }
        colorObject.TargetMe(v);
    }

    private void TargetShot(GameplayColorObject colorObject)
    {
        TargetHit(colorObject, false); // Disables the preview.

        Debug.Log($"Destroying Gameplay Object ! {colorObject.GetData().colorItemSO.colorItemID}");
        colorObject.DestroyMe();
    }

    private void OnDestroy()
    {
        onTargetHit?.RemoveAllListeners();
        onTargetShot?.RemoveAllListeners();
    }
}

