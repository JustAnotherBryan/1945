using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainGame : MonoBehaviour
{
    [SerializeField] RectTransform topPivot;
    [SerializeField] RectTransform bottomPivot;
    [SerializeField] RectTransform NuclearSymbol;

    float NuclearSymbolPosition;
    float NuclearSymbolDestination;

    float NuclearSymbolTimer;
    [SerializeField] float timerMultiplicator = 3f;

    float NuclearSymbolSpeed;
    [SerializeField] float smoothMotion = 1f;

    [SerializeField] RectTransform ControlledArea;
    float ControlledAreaPosition;
    [SerializeField] float ControlledAreaSize = 0.1f;

    float ControlledAreaPullVelocity;
    [SerializeField] float ControlledAreaPullPower = 2f;
    [SerializeField] float ControlledAreaGravityPower = 1f;

    [SerializeField] Image areaImage;

    [SerializeField] RectTransform loseBarContainer;
    float loseProgress;
    [SerializeField] float losePower = 0.5f;
    [SerializeField] float loseProgressDegradationPower = 0.3f;

    private void Start()
    {
        Resize();
        ControlledAreaPosition = 0.5f; // start middle
    }

    private void Update()
    {
        Nuclear();
        AreaCon();
        ProgressCheck();
    }

    private void ProgressCheck()
    {
        // UI bar scaling
        Vector3 ls = loseBarContainer.localScale;
        ls.y = loseProgress;
        loseBarContainer.localScale = ls;

        float min = ControlledAreaPosition - ControlledAreaSize / 2;
        float max = ControlledAreaPosition + ControlledAreaSize / 2;

        if (min < NuclearSymbolPosition && NuclearSymbolPosition < max)
        {
            loseProgress += losePower * Time.deltaTime;
        }
        else
        {
            loseProgress -= loseProgressDegradationPower * Time.deltaTime;
        }

        loseProgress = Mathf.Clamp(loseProgress, 0f, 1f);
    }

    private void Resize()
    {
        float ySize = areaImage.rectTransform.rect.height;

        float distance = Vector2.Distance(
            topPivot.anchoredPosition,
            bottomPivot.anchoredPosition
        );

        Vector2 size = ControlledArea.sizeDelta;
        size.y = (distance / ySize) * ControlledAreaSize;

        ControlledArea.sizeDelta = size;
    }

    private void AreaCon()
    {
        if (Input.GetMouseButton(0))
        {
            ControlledAreaPullVelocity += ControlledAreaPullPower * Time.deltaTime;
        }

        ControlledAreaPullVelocity -= ControlledAreaGravityPower * Time.deltaTime;
        ControlledAreaPullVelocity = Mathf.Clamp(ControlledAreaPullVelocity, -1f, 1f);

        ControlledAreaPosition += ControlledAreaPullVelocity * Time.deltaTime;

        ControlledAreaPosition = Mathf.Clamp(
            ControlledAreaPosition,
            ControlledAreaSize / 2,
            1 - ControlledAreaSize / 2
        );

        ControlledArea.anchoredPosition = Vector2.Lerp(
            bottomPivot.anchoredPosition,
            topPivot.anchoredPosition,
            ControlledAreaPosition
        );
    }

    private void Nuclear()
    {
        NuclearSymbolTimer -= Time.deltaTime;

        if (NuclearSymbolTimer < 0f)
        {
            NuclearSymbolTimer = Random.value * timerMultiplicator;
            NuclearSymbolDestination = Random.value;
        }

        NuclearSymbolPosition = Mathf.SmoothDamp(
            NuclearSymbolPosition,
            NuclearSymbolDestination,
            ref NuclearSymbolSpeed,
            smoothMotion
        );

        NuclearSymbol.anchoredPosition = Vector2.Lerp(
            bottomPivot.anchoredPosition,
            topPivot.anchoredPosition,
            NuclearSymbolPosition
        );
    }