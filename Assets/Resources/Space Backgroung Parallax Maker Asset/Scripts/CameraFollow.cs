using UnityEngine;

namespace Mkey
{
    public enum TrackMode { Player, Mouse, Gyroscope, Keyboard, Touch }
    public class CameraFollow : MonoBehaviour
    {
        //player follow options
        private Vector2 margin;
        private Vector2 smooth;

        public TrackMode track = TrackMode.Touch;
        public bool ClampPosition;
        public BoxCollider2D ClampField;  // camera motion field

        [SerializeField]
        private GameObject player;
        private Camera m_camera;
        private float camVertSize;
        private float camHorSize;
        private Vector3 acceleration;

        public float ScreenRatio
        {
            get { return ((float)Screen.width / Screen.height); }
        }

        public static CameraFollow Instance;

        #region regular
        void Awake()
        {
            if(!player)  player = GameObject.FindGameObjectWithTag("Player");
            margin = new Vector2(3, 3);
            smooth = new Vector2(1, 1);

            m_camera = GetComponent<Camera>();
            Instance = this;
        }

        private void Start()
        {
            TouchPad.Instance.ScreenDragEvent += TrackTouchDrag;
        }

        void LateUpdate()
        {
            switch (track)
            {
                case TrackMode.Player:
                    TrackPlayer();
                    break;
                case TrackMode.Mouse:
                    TrackMouseMotion();
                    break;
                case TrackMode.Gyroscope:
                    TrackGyroscope();
                    break;
                case TrackMode.Keyboard:
                    TrackKeyboard();
                    break;

            }
        }

        private void OnDesctroy()
        {
            TouchPad.Instance.ScreenDragEvent -= TrackTouchDrag;
        }
        #endregion regular

        /// <summary>
        /// Return true if Player out of X margin
        /// </summary>
        private bool OutOfXMargin
        {
            get { return Mathf.Abs(transform.position.x - player.transform.position.x) > margin.x; }
        }

        /// <summary>
        /// Return true if Player out of Y margin
        /// </summary>
        private bool OutOfYMargin
        {
            get
            {
                return Mathf.Abs(transform.position.y - player.transform.position.y) > margin.y;
            }
        }

        /// <summary>
        /// Camera follow mouse cursor position
        /// </summary>
        private void TrackMouseMotion()
        {
            acceleration = Vector3.Lerp(acceleration, Camera.main.ScreenToViewportPoint(Input.mousePosition), Time.deltaTime);
            Vector3 target = transform.position + new Vector3(acceleration.x - 0.5f, acceleration.y - 0.5f, 0);
            transform.position = Vector3.Lerp(transform.position, target, 5f * Time.deltaTime);
            ClampCameraPosInField();
        }

        /// <summary>
        /// Camera follow Player Gameobject position
        /// </summary>
        private void TrackPlayer()
        {
            if (!player)
            {
                return;
            }
            float targetX = transform.position.x;
            float targetY = transform.position.y;

            if (OutOfXMargin)
                targetX = Mathf.Lerp(transform.position.x, player.transform.position.x, smooth.x * Time.deltaTime);

            if (OutOfYMargin)
                targetY = Mathf.Lerp(transform.position.y, player.transform.position.y, smooth.y * Time.deltaTime);
            transform.position = new Vector3(targetX, targetY, transform.position.z);
            ClampCameraPosInField();
        }

        /// <summary>
        /// Camera rotate to mouse cursor position
        /// </summary>
        private void TrackMouseRotation() //http://answers.unity3d.com/questions/149022/how-to-make-camera-move-with-the-mouse-cursors.html?childToView=1097525#answer-1097525
        {
            if (!m_camera) return;
            float sensitivity = 0.00001f;
            Vector3 vp = m_camera.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, m_camera.nearClipPlane));
            vp.x -= 0.5f;
            vp.y -= 0.5f;
            vp.x *= sensitivity;
            vp.y *= sensitivity;
            vp.x += 0.5f;
            vp.y += 0.5f;
            Vector3 sp = m_camera.ViewportToScreenPoint(vp);
            Vector3 v = m_camera.ScreenToWorldPoint(sp);
            transform.LookAt(v, Vector3.up);
        }

        /// <summary>
        /// Clamp camera position in BoxCollider2D rect. Max and Min camera position dependet from collider size, camera size and screen ratio;
        /// </summary>
        private void ClampCameraPosInField()
        {
            if (ClampPosition)
            {
                if (!ClampField) return;

                camVertSize = m_camera.orthographicSize;
                camHorSize = camVertSize * ScreenRatio;

                Vector2 m_bsize = ClampField.bounds.size / 2.0f;
                m_bsize -= new Vector2(camHorSize, camVertSize);

                Vector3 tFieldPosition = ClampField.transform.position;

                float maxY = tFieldPosition.y + m_bsize.y;
                float minY = tFieldPosition.y - m_bsize.y;

                float maxX = tFieldPosition.x + m_bsize.x;
                float minX = tFieldPosition.x - m_bsize.x;

                float posX = Mathf.Clamp(transform.position.x, minX, maxX);
                float posY = Mathf.Clamp(transform.position.y, minY, maxY);

                transform.position = new Vector3(posX, posY, transform.position.z);
            }
        }

        /// <summary>
        /// Camera follow gyroscope acceleration
        /// </summary>
        private void TrackGyroscope()
        {
            Vector3 dir = Vector3.zero;
            dir.x = Input.acceleration.x;
            dir.y = Input.acceleration.y;
            if (dir.sqrMagnitude > 1) dir.Normalize();
            acceleration = dir;// acceleration = Vector3.Lerp(acceleration, dir, Time.deltaTime);
            Vector3 target = transform.position + new Vector3(acceleration.x, acceleration.y, 0);
            transform.position = Vector3.Lerp(transform.position, target, 1f * Time.deltaTime);
            ClampCameraPosInField();
        }

        /// <summary>
        /// Camera follow touch drag direction
        /// </summary>
        /// <param name="tpea"></param>
        private void TrackTouchDrag(TouchPadEventArgs tpea)
        {
            if (track == TrackMode.Touch)
            {
                Vector3 dir = tpea.DragDirection;
                // dir.x = -tpea.DragDirection.x;
                // dir.y = -tpea.DragDirection.y;

                Vector3 target = transform.position + new Vector3(dir.x, dir.y, 0);
                transform.position = Vector3.Lerp(transform.position, target, 0.02f * Time.fixedDeltaTime);
                ClampCameraPosInField();
            }
        }

        /// <summary>
        /// Camera follow keyboard input
        /// </summary>
        /// <param name="tpea"></param>
        private void TrackKeyboard()
        {
            Vector3 dir = Vector3.zero;
            dir.y = Input.GetAxis("Vertical");
            dir.x = Input.GetAxis("Horizontal");

            Vector3 target = transform.position + new Vector3(dir.x, dir.y, 0);
            transform.position = Vector3.Lerp(transform.position, target, 1.0f * Time.deltaTime);
            ClampCameraPosInField();
        }

    }
}