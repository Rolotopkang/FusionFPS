using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    public AnimationCurve RecoilTimeCurve;

    public Vector2 RecoilRange;
    public float RecoilFadeOutTime;
    private Vector2 currentRecoil;

    private float currentRecoilTime;
    private Transform cameraRecoilTransform;
    private FPMouseLook mouseLook;

    private void Start()
    {
        cameraRecoilTransform = transform;
        mouseLook = FindObjectOfType<FPMouseLook>();
    }

    private void Update()
    {
       
//        mouseLook.LookAtRotation(tmp_RecoilOffset.x, tmp_RecoilOffset.y);
    }
}