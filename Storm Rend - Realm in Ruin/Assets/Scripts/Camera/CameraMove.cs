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
    [SerializeField] private float m_moveSpeed = 10;

    [Header("Move Anchors")]
    [SerializeField] private BoxCollider m_cameraBounds = null;

    // movement coroutine reference (for stopping/interrupting)
    private IEnumerator m_moveTo;

    /// <summary>
    /// use this to move the camera by an incremental amount!
    /// </summary>
    /// <param name="axis">the value in each axis to move</param>
    public void MoveBy(Vector3 axis)
    {
        // stop movement if the MoveTo coroutine is already executing
        if (m_moveTo != null && axis != Vector3.zero)
            StopCoroutine(m_moveTo);
        
        float speed = m_moveSpeed * Time.deltaTime;

        // determine the destination of the end of the movement
        Vector3 destination = m_rootTransform.position;
        destination += axis.z * m_rootTransform.forward * speed;
        destination += axis.y * m_rootTransform.up * speed;
        destination += axis.x * m_rootTransform.right * speed;

        // ensure camera stays within bounds
        destination = ClampDestination(destination);

        // perform movement
        m_rootTransform.position = destination;
    }

    /// <summary>
    /// use this to move the camera to a destination over an arbritrary amount of time!
    /// </summary>
    /// <param name="destination">the position to lerp/move to</param>
    /// <param name="time">the amount of time it takes to lerp to the destination</param>
    public void MoveTo(Vector3 destination, float time = 0.3f)
    {
        // stop movement if the MoveTo coroutine is already executing
        if (m_moveTo != null)
            StopCoroutine(m_moveTo); 

        // ensure camera stays within bounds
        destination = ClampDestination(destination);

        // start new MoveTo coroutine
        StartCoroutine(m_moveTo = LerpTo(destination, time));
    }

    /// <summary>
    /// lerp/move coroutine which lerps the camera from it's current position, to a destination in an
    /// arbritrary amount of time
    /// </summary>
    /// <param name="destination">the position to lerp/move to</param>
    /// <param name="time">the amount of time it takes to lerp to the destination</param>
    private IEnumerator LerpTo(Vector3 destination, float time = 0.3f)
    {
        float timer = 0;

        while (timer < time)
        {
            // get lerp percentage & increment timer
            float t = timer / time;
            timer += Time.deltaTime;
            
            // perform incremental movement
            m_rootTransform.position = Vector3.Lerp(m_rootTransform.position, destination, t);
            yield return null;
        }
    }

    /// <summary>
    /// magic function, spend hours on writing this one (no joke)
    /// </summary>
    /// <param name="destination">the destination to clamp</param>
    /// <returns>the clamped destination</returns>
    private Vector3 ClampDestination(Vector3 destination)
    {
        return m_cameraBounds.ClosestPoint(destination);
    }
}