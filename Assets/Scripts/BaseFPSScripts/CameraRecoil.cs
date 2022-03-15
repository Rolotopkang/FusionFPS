using UnityEngine;

public class CameraRecoil : MonoBehaviour
{
    public AnimationCurve RecoilCurve;

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
        //计算抖动
        CalculateRecoilOffset();
    }
    
    private void CalculateRecoilOffset()
    {
        currentRecoilTime += Time.deltaTime;
        float tmp_RecoilFraction = currentRecoilTime / RecoilFadeOutTime;
        float tmp_RecoilValue = RecoilCurve.Evaluate(tmp_RecoilFraction);
        currentRecoil = Vector2.Lerp(Vector2.zero, currentRecoil, tmp_RecoilValue);
    }
}