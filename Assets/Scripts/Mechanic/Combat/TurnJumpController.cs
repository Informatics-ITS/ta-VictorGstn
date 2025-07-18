using UnityEngine;
using System.Collections;

public class TurnJumpController : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpDistance = 0.5f;
    public float jumpHeight = 0.3f;
    public float jumpDuration = 0.25f;

    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.position;
    }

    public IEnumerator JumpIn(bool isPlayer)
    {
        Vector3 target = originalPosition + new Vector3(isPlayer ? jumpDistance : -jumpDistance, 0, 0);
        yield return JumpTo(target);
    }

    public IEnumerator JumpBack()
    {
        yield return JumpTo(originalPosition);
    }

    private IEnumerator JumpTo(Vector3 destination)
    {
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < jumpDuration)
        {
            float t = elapsed / jumpDuration;
            float curve = 4 * jumpHeight * t * (1 - t); // Parabola

            transform.position = Vector3.Lerp(start, destination, t) + new Vector3(0, curve, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = destination;
    }

    public IEnumerator JumpEvade(bool isPlayer)
    {
        Vector3 start = transform.position;
        Vector3 evadePos = start + new Vector3(isPlayer ? -0.5f : 0.5f, 0, 0); // small hop back
        float duration = 0.25f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float curve = 4 * 0.2f * t * (1 - t); // low arc

            transform.position = Vector3.Lerp(start, evadePos, t) + new Vector3(0, curve, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = evadePos;

        // Optional: jump back to original
        elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            float curve = 4 * 0.2f * t * (1 - t);
            transform.position = Vector3.Lerp(evadePos, start, t) + new Vector3(0, curve, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = start;
    }

}
