using UnityEngine;
using System.Collections.Generic;

namespace Mkey
{
    /// <summary>
    /// Create parallax effect along X-axe
    /// </summary>
    public class SpriteParallax : MonoBehaviour
    {
        [SerializeField]
        private ParallaxPlane[] planes;
        [SerializeField]
        private bool infiniteMap = true;
        [SerializeField]
        private float mapSizeX = 20.48f;
        [SerializeField]
        private float mapSizeY = 20.48f;
        [SerializeField]
        private float firstPlaneRelativeOffset = 0;
        [SerializeField]
        private float lastPlaneRelativeOffset = 0.9f;

        public Transform m_Camera;
        private Vector3 camPos;     // camera position
        private Vector3 oldCamPos;  // old camera position
        private ParallaxPlane plane;
        private Vector3 planePos;
        private Vector2 camOffset;
        private int length = 0;
        
        [SerializeField]
        private float[] planeOfsset;

        private List<ParallaxPlane> InfiniteGroup;
        private List<ParallaxPlane>[] InfiniteMap;

        private void OnValidate()
        {
            firstPlaneRelativeOffset = Mathf.Clamp01(firstPlaneRelativeOffset);
            lastPlaneRelativeOffset = Mathf.Clamp01(lastPlaneRelativeOffset);
        }

        void Start()
        {
            //m_Camera = Camera.main.transform;
            camPos = m_Camera.position;
            oldCamPos = camPos;
            length = planes.Length;

            //cache plane offsets
            firstPlaneRelativeOffset = Mathf.Clamp01(firstPlaneRelativeOffset);
            lastPlaneRelativeOffset = Mathf.Clamp01(lastPlaneRelativeOffset);
            float dKP = Mathf.Abs(lastPlaneRelativeOffset - firstPlaneRelativeOffset) / (length - 1);
            planeOfsset = new float[length];

            for (int i = 0; i < length; i++)
            {
                plane = planes[i];
                if (!plane) continue;
                planeOfsset[i] = firstPlaneRelativeOffset + i * dKP;
            }

            if (infiniteMap)
            {
                for (int i = 0; i < length; i++)
                {
                    if (planes[i])
                        planes[i].CreateInfinitePlane(new Vector2(mapSizeX, mapSizeY), camPos);
                }
            }
        }

        void Update()
        {
            camPos = m_Camera.position;
            camOffset = camPos - oldCamPos;

            for (int i = 0; i < length; i++)
            {
                plane = planes[i];
                if (!plane) continue;
                plane.transform.Translate(new Vector3(camOffset.x * planeOfsset[i], camOffset.y * planeOfsset[i], 0), Space.World);

                if (infiniteMap) plane.UpdateInfinitePlane(camPos);
            }
            oldCamPos = camPos;
        }
    }
}