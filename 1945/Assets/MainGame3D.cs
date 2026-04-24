using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainGame3D : MonoBehaviour
{
    [Header("Target Path")]
    [SerializeField] Transform targetTopPoint;
    [SerializeField] Transform targetBottomPoint;

    [Header("Area Path")]
    [SerializeField] Transform areaTopPoint;
    [SerializeField] Transform areaBottomPoint;

    [Header("Target (Nuclear Symbol)")]
    [SerializeField] Transform target;

    float targetPosition;
    float targetDestination;
    [SerializeField] float targetTimer;
    [SerializeField] float timerMultiplicator = 3f;
    float targetSpeed;
    [SerializeField] float smoothMotion = 1f;

    [Header("Controlled Area")]
    [SerializeField] Transform controlledArea;
    float areaPosition;
    [SerializeField] float areaSize = 0.2f;

    float areaVelocity;
    [SerializeField] float pullPower = 5f;
    [SerializeField] float gravityPower = 3f;

    [Header("Fail Bar")]
    float failProgress;

    [SerializeField] float failGain = 0.6f;
    [SerializeField] float failLoss = 0.4f;

    [SerializeField] Transform failBar; // visual bar

    [SerializeField] string loseSceneName = "GameOver"; //load scene

    private void Start()
    {
        areaPosition = 0.5f;
        ResizeArea();
    }

    private void Update()
    {
        UpdateTarget();
        UpdateArea();
        CheckFailProgress();
    }

    private void CheckFailProgress()
    {
        float min = areaPosition - areaSize / 2;
        float max = areaPosition + areaSize / 2;

        bool inside = targetPosition > min && targetPosition < max;

        if (!inside)
        {
            // NOT on target → increase fail bar
            failProgress += failGain * Time.deltaTime;
        }
        else
        {
            // On target → reduce fail bar (optional)
            failProgress -= failLoss * Time.deltaTime;
        }

        failProgress = Mathf.Clamp01(failProgress);

        UpdateFailBar();

        if (failProgress >= 1f)
        {
            SceneManager.LoadScene(loseSceneName);
        }
    }

    private void UpdateFailBar()
    {
        if (failBar == null) return;

        Vector3 scale = failBar.localScale;
        scale.y = failProgress;
        failBar.localScale = scale;
    }

    // 🎯 TARGET MOVEMENT (Random smooth motion)
    private void UpdateTarget()
    {
        targetTimer -= Time.deltaTime;

        if (targetTimer < 0f)
        {
            targetTimer = Random.value * timerMultiplicator;
            targetDestination = Random.value;
        }

        targetPosition = Mathf.SmoothDamp(
            targetPosition,
            targetDestination,
            ref targetSpeed,
            smoothMotion
        );

        target.position = Vector3.Lerp(
            targetBottomPoint.position,
            targetTopPoint.position,
            targetPosition
        );
    }

    // 🎮 PLAYER CONTROL AREA
    private void UpdateArea()
    {
        if (Input.GetMouseButton(0))
        {
            areaVelocity += pullPower * Time.deltaTime;
        }

        areaVelocity -= gravityPower * Time.deltaTime;
        areaVelocity = Mathf.Clamp(areaVelocity, -1f, 1f);

        areaPosition += areaVelocity * Time.deltaTime;

        areaPosition = Mathf.Clamp(
            areaPosition,
            areaSize / 2,
            1 - areaSize / 2
        );

        controlledArea.position = Vector3.Lerp(
            areaBottomPoint.position,
            areaTopPoint.position,
            areaPosition
        );
    }



    // 📏 SCALE AREA VISUALLY
    private void ResizeArea()
    {
        float distance = Vector3.Distance(areaTopPoint.position, areaBottomPoint.position);

        Vector3 scale = controlledArea.localScale;
        scale.y = distance * areaSize;

        controlledArea.localScale = scale;
    }
}