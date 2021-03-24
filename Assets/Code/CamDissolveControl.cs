using Cinemachine;
using System.Collections;
using UnityEngine;

public class CamDissolveControl : MonoBehaviour
{
    #region Private Attributes

    [Header("Dissolve")]
    [SerializeField] private float dissolveSpeed = 0.75f;
    [SerializeField] private MeshRenderer carPaintRenderer = null;

    [Header("Cinemachine cameras")]
    [SerializeField] private CinemachineVirtualCamera cmCamFront = null;
    [SerializeField] private CinemachineVirtualCamera cmCamBack = null;

    private Material carPaintMat;

    private float dissolveProgress = 0.0f;
    private float targetDissolveValue = 0.0f;

    private readonly int dissolveAmountId = Shader.PropertyToID("Dissolve_Progress");

    private Coroutine currDisolveCoroutine;
    private WaitForSeconds wfsAlternateCams = new WaitForSeconds(6.0f);

    #endregion

    #region MonoBehaviour Methods

    private void Start()
    {
        carPaintMat = carPaintRenderer.sharedMaterial;
        carPaintMat.SetFloat(dissolveAmountId, dissolveProgress);

        StartCoroutine(AlternateCams());
    }

    private void OnValidate()
    {
        dissolveSpeed = Mathf.Clamp(dissolveSpeed, 0.0f, dissolveSpeed);
    }

    #endregion

    #region Coroutine Methods

    private IEnumerator AlternateCams()
    {
        while (true)
        {
            yield return wfsAlternateCams;

            if (cmCamFront.Priority == 2)
            {
                cmCamBack.Priority = 2;
                cmCamFront.Priority = 1;
            }
            else
            {
                cmCamFront.Priority = 2;
                cmCamBack.Priority = 1;
            }
        }
    }

    private IEnumerator DissolveCoroutine()
    {
        targetDissolveValue = (targetDissolveValue == 0.0f) ? 1.0f : 0.0f;

        while (Mathf.Abs(targetDissolveValue - dissolveProgress) > 0.0f)
        {
            float offsetToTar = targetDissolveValue - dissolveProgress;

            dissolveProgress = dissolveProgress + (Mathf.Sign(offsetToTar) * dissolveSpeed * Time.deltaTime);
            dissolveProgress = Mathf.Clamp(dissolveProgress, 0.0f, 1.0f);

            carPaintMat.SetFloat(dissolveAmountId, dissolveProgress);

            yield return null;
        }

        yield break;
    }

    #endregion

    #region UI Methods

    public void OnDissolveButtonPressed()
    {
        if (currDisolveCoroutine != null)
            StopCoroutine(currDisolveCoroutine);

        currDisolveCoroutine = StartCoroutine(DissolveCoroutine());
    }

    #endregion
}