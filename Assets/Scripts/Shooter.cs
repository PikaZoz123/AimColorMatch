using UnityEngine;
using UnityEngine.Events;

public class Shooter : MonoBehaviour
{
    [SerializeField] Transform muzzle;
    [SerializeField] Transform lazer;
    [SerializeField] float rotationSmoothing = 10;
    [SerializeField] float scaleSmoothing = 10;

    [HideInInspector] public UnityEvent<GameObject> onTargetHit;
    [HideInInspector] public UnityEvent<GameObject> onTargetShot;
    [SerializeField] float maxDistance = 999f;

    readonly RaycastHit[] cameraResults = new RaycastHit[1];

    GameplayColorObject colorObject;
    Anchor currentAnchor;
    bool hasTarget;
    Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        Aim();
        Shoot();
    }


    void OnDestroy()
    {
        onTargetHit?.RemoveAllListeners();
        onTargetShot?.RemoveAllListeners();
    }

    void Shoot()
    {
        if (!hasTarget)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            TargetShot();
        }
    }

    void Aim()
    {
        var cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);

        lazer.forward = Vector3.Lerp(lazer.forward, cameraRay.direction.normalized, rotationSmoothing);

        if (Physics.RaycastNonAlloc(cameraRay, cameraResults) > 0)
        {
            if (hasTarget)
            {
                return;
            }

            colorObject = cameraResults[0].rigidbody.GetComponent<GameplayColorObject>();
            if (!colorObject)
            {
                return;
            }

            currentAnchor = colorObject.transform.GetComponentInParent<Anchor>();
            hasTarget = true;
            TargetHit(true);
        }
        else
        {
            if (hasTarget)
            {
                hasTarget = false;
                TargetHit(false);
            }
        }
    }


    void TargetHit(bool v)
    {
        colorObject.TargetMe(v);
    }

    void TargetShot()
    {
        TargetHit(false); // Disables the preview.

        //Debug.Log($"Destroying Gameplay Object ! {colorObject.GetData().colorItemSO.colorItemID}");
        colorObject.DestroyMe();
    }

    public Anchor GetTargetedAnchor()
    {
        return currentAnchor;
    }
}