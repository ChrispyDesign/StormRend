using UnityEngine;

namespace StormRend.CameraSystem
{
    public class CameraInput : MonoBehaviour
    {
        //Inspector
        [SerializeField] string xAxisName = "Horizontal";
        [SerializeField] string yAxisName = "Vertical";

        //Properties
        public float xAxis
        {
            get => _xAxis;
            private set => _xAxis = value;
        }
        public float yAxis
        {
            get => _yAxis;
            private set => _yAxis = value;
        }
        public float zoomAxis
        {
            get => _zoomAxis;
            private set => _zoomAxis = value;
        }

        //Members
        float _xAxis, _yAxis, _zoomAxis;

        void Update()
        {
            PollMoveInput(); // can perform translation
            PollZoomInput(); // can perform zooming
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
    }
}