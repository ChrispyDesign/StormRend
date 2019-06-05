using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraMove : MonoBehaviour
{
    private Camera m_camera;
    private IEnumerator m_moveRoutine;

    /// <summary>
    /// cache camera component
    /// </summary>
    void Start()
    {
        m_camera = GetComponent<Camera>();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="desiredPosition"></param>
    /// <param name="lerpTime"></param>
    public void StartMove(Vector3 desiredPosition, float lerpTime)
    {
        if (m_moveRoutine != null)
            StopCoroutine(m_moveRoutine);

        StartCoroutine(m_moveRoutine = MoveLerp(desiredPosition, lerpTime));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="desiredPosition"></param>
    /// <param name="lerpTime"></param>
    private IEnumerator MoveLerp(Vector3 desiredPosition, float lerpTime)
    {
        Vector3 currentPosition = m_camera.transform.position;

        float timer = 0;
        while (timer < lerpTime)
        {
            float t = Mathf.SmoothStep(0, 1, timer / lerpTime);

            m_camera.transform.position = Vector3.Lerp(currentPosition, desiredPosition, t);

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
