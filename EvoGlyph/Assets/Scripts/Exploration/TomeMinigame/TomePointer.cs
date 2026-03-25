using System.Collections;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TomePointer : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [SerializeField] float baseSpeed;
    [SerializeField] float speedMultiplier;
    [SerializeField] float boostDuration;
    [SerializeField] float currentSpeed;
    Coroutine boostCoroutine;
    public void Initialize()
    {
        currentSpeed = baseSpeed;
    }

    public void MoveToTarget(Transform target)
    {
        Vector2 pointerPos = transform.position;
        Vector2 currentWorldPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Vector2 mouseDir = (currentWorldPos - pointerPos).normalized;
        //If mouse is exactly on pointer, default upward
        if (mouseDir.magnitude < 0.01f)
            mouseDir = Vector2.up;

        // Face direction
        float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);

        Vector2 toTarget = (target.position - transform.position).normalized;
        float alignment = Vector2.Dot(mouseDir, toTarget);

        float speedFactor = Mathf.Clamp01(alignment);
        float finalSpeed = currentSpeed * speedFactor;
        transform.position = Vector2.MoveTowards(transform.position, target.position, finalSpeed * Time.deltaTime);
    }

    public void Boost()
    {
        if (boostCoroutine != null)
        {
            StopCoroutine(boostCoroutine);
        }
        StartCoroutine(DoBoost(boostDuration, speedMultiplier));

    }
    IEnumerator DoBoost(float duration, float multiplier)
    {
        baseSpeed *= multiplier;
        yield return new WaitForSeconds(duration);
        baseSpeed /= multiplier;
    }
}
