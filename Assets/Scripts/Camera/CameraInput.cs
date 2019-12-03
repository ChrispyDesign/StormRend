using UnityEngine;

namespace StormRend.CameraSystem
{
    public class CameraInput : MonoBehaviour
    {
        //Inspector
        [SerializeField] string xAxisName = "Horizontal";
        [SerializeField] string yAxisName = "Vertical";

        //Properties
        public float xAxis { get ; private set; }
        public float yAxis { get; private set; }
        public float zoomAxis { get; private set; }
		public Vector2 mousePosition { get; private set; }

        void Update()
        {
            PollMoveInput(); // can perform translation
            PollZoomInput(); // can perform zooming
			PollMousePosition();	//Edge panning
        }

        void PollMoveInput()
        {
            xAxis = Input.GetAxisRaw(xAxisName);
            yAxis = Input.GetAxisRaw(yAxisName);
        }

        void PollZoomInput()
        {
            zoomAxis = Input.mouseScrollDelta.y;
        }

		void PollMousePosition()
		{
			mousePosition = Input.mousePosition;
		}

		void PollMouseDrag()
		{

		}
    }
}