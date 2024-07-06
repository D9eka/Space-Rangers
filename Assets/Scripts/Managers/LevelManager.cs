using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public float Width { get; private set; }
        public float Height { get; private set; }
        public float LeftBorder { get; private set; }
        public float RightBorder { get; private set; }
        public float TopBorder { get; private set; }
        public float BottomBorder { get; private set; }

        public static LevelManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            Vector3 cameraBottomLeftAngle = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
            Vector3 cameraTopRightAngle = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));
            Width = cameraTopRightAngle.x - cameraBottomLeftAngle.x;
            Height = cameraTopRightAngle.z - cameraBottomLeftAngle.z;
            LeftBorder = cameraBottomLeftAngle.x;
            RightBorder = cameraTopRightAngle.x;
            TopBorder = cameraTopRightAngle.z;
            BottomBorder = cameraBottomLeftAngle.z;
        }
    }
}
