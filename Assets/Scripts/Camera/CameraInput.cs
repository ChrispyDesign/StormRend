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

        int m_target = 60;

        private void Awake()
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        void Update()
        {
            PollMoveInput(); // can perform translation
            PollZoomInput(); // can perform zooming
            
        }

        void PollMoveInput()
        {
            xAxis = Input.GetAxis(xAxisName);
            yAxis = Input.GetAxis(yAxisName);
        }

        void PollZoomInput()
        {
            zoomAxis = Input.mouseScrollDelta.y;
        }
    }
}