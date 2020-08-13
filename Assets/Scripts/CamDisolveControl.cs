using Cinemachine;
using System.Collections;
using UnityEngine;

public class CamDisolveControl : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;

    private CinemachineVirtualCamera cm1;
    private CinemachineVirtualCamera cm2;

    public MeshRenderer paintCarRenderer;
    private Material paintCarMat;

    private int dissolveAmount = Shader.PropertyToID("Vector1_93777DE4");

    private float dissolveProgress = 0.0f;
    private float targetDissolve = 0.0f;

    public float speedDissolve = 5.0f;

    private Coroutine currDisolveCoroutine = null;
    private WaitForSeconds wfs = new WaitForSeconds(5.0f);

    private void Start()
    {
        cm1 = cam1.GetComponent<CinemachineVirtualCamera>();
        cm2 = cam2.GetComponent<CinemachineVirtualCamera>();

        cm1.Priority = 9;
        cm2.Priority = 10;

        paintCarMat = paintCarRenderer.sharedMaterial;
        paintCarMat.SetFloat(dissolveAmount, dissolveProgress);

        StartCoroutine(PingPongCameras());
    }

    public void OnDissolveButtonPressed()
    {
        if (currDisolveCoroutine != null)
            StopCoroutine(currDisolveCoroutine);

        currDisolveCoroutine = StartCoroutine(DissolveCoroutine());
    }

    private IEnumerator PingPongCameras()
    {
        while (true)
        {
            yield return wfs;

            if (cm1.Priority == 10)
            {
                cm2.Priority = 10;
                cm1.Priority = 9;
            }
            else
            {
                cm1.Priority = 10;
                cm2.Priority = 9;
            }
        }
    }

    private IEnumerator DissolveCoroutine()
    {
        if (targetDissolve == 0.0f)
            targetDissolve = 1.0f;
        else
            targetDissolve = 0.0f;

        while (Mathf.Abs(targetDissolve - dissolveProgress) > 0.05f)
        {
            float offsetToTar = targetDissolve - dissolveProgress;

            dissolveProgress = dissolveProgress + (Mathf.Sign(offsetToTar) * speedDissolve * Time.deltaTime);

            paintCarMat.SetFloat(dissolveAmount, dissolveProgress);

            yield return null;
        }

        yield break;
    }
}