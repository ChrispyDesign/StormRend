using System.Collections;
using UnityEngine;

/// <summary>
/// camera movement class, responsible for the translation of the camera
/// </summary>
public class CameraMove : MonoBehaviour
{
    [Header("Root Transform")]
    [SerializeField] private Transform m_rootTransform = null;

    [Header("Move Speed")]
    [SerializeField] private float m_moveSpeed = 1;

    [Header("Move Anchors")]
    [SerializeField] private BoxCollider m_cameraBounds = null;

    public void Move(Vector2 axis)
    {
        float speed = m_moveSpeed * Time.deltaTime;

        m_rootTransform.position += axis.y * m_rootTransform.forward * speed;
        m_rootTransform.position += axis.x * m_rootTransform.right * speed;

        ClampPosition();
    }

    public void MoveTo(Vector3 destination, float time = 0.3f)
    {
        StartCoroutine(LerpTo(destination, time));

        ClampPosition();
    }

    private IEnumerator LerpTo(Vector3 destination, float time = 0.3f)
    {
        Vector3 origin = m_rootTransform.position;
        float timer = 0;

        while (timer < time)
        {
            float t = timer / time;
            timer += Time.deltaTime;
            
            m_rootTransform.position = Vector3.Lerp(m_rootTransform.position, destination, t);

            yield return null;
        }
    }

    private void ClampPosition()
    {
        //Vector3 bounds = m_cameraBounds.bounds.extents;

        //float angleX = Mathf.Atan2(m_rootTransform.position.x, m_rootTransform.position.z) % Mathf.PI;
        //float angleZ = Mathf.Atan2(m_rootTransform.position.z, m_rootTransform.position.x) % Mathf.PI;

        //if (angleX < 0)
        //    angleX += Mathf.PI;

        //if (angleZ < 0)
        //    angleZ += Mathf.PI;

        //float xBound = bounds.x * angleX;
        //float zBound = bounds.z * angleZ;

        //float clampedX = Mathf.Clamp(m_rootTransform.position.x, -xBound, xBound);
        //float clampedZ = Mathf.Clamp(m_rootTransform.position.z, -zBound, zBound);

        //m_rootTransform.position = new Vector3(clampedX, 0, clampedZ);
    }
}
