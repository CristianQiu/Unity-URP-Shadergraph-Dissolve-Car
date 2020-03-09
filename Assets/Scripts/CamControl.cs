using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class CamControl : MonoBehaviour
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

    private Coroutine currCo = null;

    // Start is called before the first frame update
    void Start()
    {
        cm1 = cam1.GetComponent<CinemachineVirtualCamera>();
        cm2 = cam2.GetComponent<CinemachineVirtualCamera>();

        cm1.Priority= 10;
        cm2.Priority = 9;

        paintCarMat = paintCarRenderer.sharedMaterial;
        paintCarMat.SetFloat(dissolveAmount, dissolveProgress);
    }

    // Update is called once per frame
    void Update()
    {
#if !UNITY_EDITOR
        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
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
#else
        if (Input.GetKeyDown(KeyCode.F))
        {
            //OnDissolveButtonPressed();
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
#endif
    }

    public void OnDissolveButtonPressed()
    {
        if (currCo != null)
        {
            StopCoroutine(currCo);
        }
        currCo = StartCoroutine(DissolveCo());
    }

    private IEnumerator DissolveCo()
    {
        if (targetDissolve == 0.0f)
        {
            targetDissolve = 1.0f;
        }
        else
        {
            targetDissolve = 0.0f;
        }

        while (Mathf.Abs(targetDissolve - dissolveProgress) > 0.05f)
        {

            float offsetToTar = targetDissolve - dissolveProgress;

            dissolveProgress =  dissolveProgress + (Mathf.Sign(offsetToTar) * speedDissolve * Time.deltaTime);



            paintCarMat.SetFloat(dissolveAmount, dissolveProgress);

            yield return null;
        }

        yield break;
    }
}
