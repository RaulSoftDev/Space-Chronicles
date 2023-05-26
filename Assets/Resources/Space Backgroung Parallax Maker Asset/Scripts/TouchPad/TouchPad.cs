using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using System.Linq;

/*
    changes
    110219 
        remove link to GameObjectExt class
        remove private fields isActive, touch
        add  ScreenTouchPos 
        add events
            public Action<TouchPadEventArgs> ScreenDragEvent;
            public Action<TouchPadEventArgs> ScreenPointerDownEvent;
            public Action<TouchPadEventArgs> ScreenPointerUpEvent;
        add TouchPadEventArgs class
        add ICustomMessageTarget : IEventSystemHandler


 */
namespace Mkey
{
    public class TouchPad : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IBeginDragHandler, IDropHandler, IPointerExitHandler
    {
        public List<Collider2D> hitList;
        public List<Collider2D> newHitList;
        public Action<TouchPadEventArgs> ScreenDragEvent;
        public Action<TouchPadEventArgs> ScreenPointerDownEvent;
        public Action<TouchPadEventArgs> ScreenPointerUpEvent;

        /// <summary>
        /// Return drag direction in screen coord
        /// </summary>
        public Vector2 ScreenDragDirection
        {
            get { return ScreenTouchPos - oldPosition; }
        }

        /// <summary>
        /// Return world position of touch.
        /// </summary>
        public Vector3 WorldTouchPos
        {
            get { return Camera.main.ScreenToWorldPoint(ScreenTouchPos); }
        }

        public Vector2 ScreenTouchPos { get; private set; }

        [SerializeField]
        private bool dlog = false;

        public static TouchPad Instance;

        private TouchPadEventArgs tpea;
        private int pointerID;
        private  Vector2 oldPosition;

        /// <summary>
        /// Return true if touchpad is touched with mouse or finger
        /// </summary>
        public bool IsTouched
        {
            get; private set;
        }

        /// <summary>
        /// Return true if touch activity enabled
        /// </summary>
        public bool IsActive
        {
            get; private set;
        }

        #region regular
        void Awake()
        {
            IsActive = true;
            hitList = new List<Collider2D>();
            newHitList = new List<Collider2D>();
            tpea = new TouchPadEventArgs();

            if (Instance) Destroy(gameObject);
            else Instance = this;
        }
        #endregion regular

        #region raise events
        public void OnPointerDown(PointerEventData data)
        {
            if (IsActive)
            {
                if (!IsTouched)
                {
                    if (dlog) Debug.Log("----------------POINTER Down--------------( " + data.pointerId);
                    IsTouched = true;
                    tpea = new TouchPadEventArgs();
                    ScreenTouchPos = data.position;
                    oldPosition = ScreenTouchPos;
                    pointerID = data.pointerId;

                    tpea.SetTouch(ScreenTouchPos, Vector2.zero, TouchPhase.Began);
                    hitList = new List<Collider2D>();
                    hitList.AddRange(tpea.hits);
                    if (hitList.Count > 0)
                    {
                        for (int i = 0; i < hitList.Count; i++)
                        {
                            ExecuteEvents.Execute<ICustomMessageTarget>(hitList[i].transform.gameObject, null, (x, y) => x.PointerDown(tpea));
                            if (tpea.firstSelected == null) tpea.firstSelected = GetInterface<ICustomMessageTarget>(hitList[i].transform.gameObject);
                        }
                    }
                    ScreenPointerDownEvent?.Invoke(tpea);
                }
            }
        }

        public void OnBeginDrag(PointerEventData data)
        {
            if (IsActive)
            {
                if (data.pointerId == pointerID)
                {
                    if (dlog) Debug.Log("----------------BEGIN DRAG--------------( " + data.pointerId);
                    ScreenTouchPos = data.position;
                    tpea.SetTouch(ScreenTouchPos, ScreenTouchPos - oldPosition, TouchPhase.Moved);
                    oldPosition = ScreenTouchPos;

                    //0 ---------------------------------- send drag begin message --------------------------------------------------
                    for (int i = 0; i < hitList.Count; i++)
                    {
                        if (hitList[i]) ExecuteEvents.Execute<ICustomMessageTarget>(hitList[i].transform.gameObject, null, (x, y) => x.DragBegin(tpea));
                    }
                    ScreenDragEvent?.Invoke(tpea);
                }
            }
        }

        public void OnDrag(PointerEventData data)
        {
            if (IsActive)
            {
                if (data.pointerId == pointerID)
                {
                    if (dlog) Debug.Log("---------------- ONDRAG --------------( " + data.pointerId + " : " + pointerID);

                    ScreenTouchPos = data.position;
                    tpea.SetTouch(ScreenTouchPos, ScreenTouchPos - oldPosition, TouchPhase.Moved);
                    oldPosition = ScreenTouchPos;

                    newHitList = new List<Collider2D>(tpea.hits); // garbage

                    //1 ------------------ send drag exit message and drag message --------------------------------------------------
                    foreach (Collider2D cHit in hitList)
                    {
                        if (newHitList.IndexOf(cHit) == -1)
                        {
                            if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.DragExit(tpea));
                        }
                        else
                        {
                            if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.Drag(tpea));
                        }

                    }

                    //2 ------------------ send drag enter message -----------------------------------------------------------------
                    for (int i = 0; i < newHitList.Count; i++)
                    {
                        if (hitList.IndexOf(newHitList[i]) == -1)
                        {
                            if (newHitList[i]) ExecuteEvents.Execute<ICustomMessageTarget>(newHitList[i].gameObject, null, (x, y) => x.DragEnter(tpea));
                        }
                    }

                    hitList = newHitList;
                    ScreenDragEvent?.Invoke(tpea);
                }
            }
        }

        public void OnPointerUp(PointerEventData data)
        {
            if (IsActive)
            {
                if (dlog) Debug.Log("----------------POINTER UP--------------( " + data.pointerId + " : " + pointerID);
                if (data.pointerId == pointerID)
                {

                    ScreenTouchPos = data.position;
                    tpea.SetTouch(ScreenTouchPos, ScreenTouchPos - oldPosition, TouchPhase.Ended);
                    oldPosition = ScreenTouchPos;

                    IsTouched = false;
                    foreach (Collider2D cHit in hitList)
                    {
                        if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.PointerUp(tpea));
                    }

                    newHitList = new List<Collider2D>(tpea.hits);
                    foreach (Collider2D cHit in newHitList)
                    {
                        if (hitList.IndexOf(cHit) == -1)
                        {
                            if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.PointerUp(tpea));
                        }
                        if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.DragDrop(tpea));
                    }
                    if (dlog) Debug.Log("clear lists");
                    hitList = new List<Collider2D>();
                    newHitList = new List<Collider2D>();
                    ScreenPointerUpEvent?.Invoke(tpea);
                }
            }
        }

        public void OnPointerExit(PointerEventData data)
        {
            if (IsActive)
            {
                if (data.pointerId == pointerID)
                {
                    if (dlog) Debug.Log("----------------POINTER EXIT--------------( " + data.pointerId + " : " + pointerID);
                    ScreenTouchPos = data.position;
                    tpea.SetTouch(ScreenTouchPos, ScreenTouchPos - oldPosition, TouchPhase.Ended);
                    oldPosition = ScreenTouchPos;

                    IsTouched = false;
                    foreach (Collider2D cHit in hitList)
                    {
                        if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.PointerUp(tpea));
                    }

                    newHitList = new List<Collider2D>(tpea.hits);
                    foreach (Collider2D cHit in newHitList)
                    {
                        if (hitList.IndexOf(cHit) == -1)
                        {
                            if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.PointerUp(tpea));
                        }
                        if (cHit) ExecuteEvents.Execute<ICustomMessageTarget>(cHit.transform.gameObject, null, (x, y) => x.DragDrop(tpea));
                    }
                    hitList = new List<Collider2D>();
                    newHitList = new List<Collider2D>();
                }
            }
        }

        public void OnDrop(PointerEventData data)
        {
            if (IsActive)
            {
                if (data.pointerId == pointerID)
                {
                    if (dlog) Debug.Log("----------------ONDROP--------------( " + data.pointerId + " : " + pointerID);
                }
            }

        }
        #endregion raise events

        /// <summary>
        /// Enable or disable touch pad callbacks handling.
        /// </summary>
        public void SetTouchActivity(bool activity)
        {
            IsActive = activity;
#if UNITY_EDITOR
            if(Instance && Instance.dlog) Debug.Log("touch activity: " + activity);
#endif
        }

        /// <summary>
        /// Returns all monobehaviours (casted to T)
        /// </summary>
        /// <typeparam name="T">interface type</typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        private T[] GetInterfaces<T>(GameObject gObj)
        {
            if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
            var mObjs = MonoBehaviour.FindObjectsOfType<MonoBehaviour>();
            return (from a in mObjs where a.GetType().GetInterfaces().Any(k => k == typeof(T)) select (T)(object)a).ToArray();
        }

        /// <summary>
        /// Returns the first monobehaviour that is of the interface type (casted to T)
        /// </summary>
        /// <typeparam name="T">Interface type</typeparam>
        /// <param name="gObj"></param>
        /// <returns></returns>
        private  T GetInterface<T>(GameObject gObj)
        {
            if (!typeof(T).IsInterface) throw new SystemException("Specified type is not an interface!");
            return GetInterfaces<T>(gObj).FirstOrDefault();
        }
    }

    /// <summary>
    /// Interface for handling touchpad events.
    /// </summary>
    public interface ICustomMessageTarget : IEventSystemHandler
    {
        void PointerDown(TouchPadEventArgs tpea);
        void DragBegin(TouchPadEventArgs tpea);
        void DragEnter(TouchPadEventArgs tpea);
        void DragExit(TouchPadEventArgs tpea);
        void DragDrop(TouchPadEventArgs tpea);
        void PointerUp(TouchPadEventArgs tpea);
        void Drag(TouchPadEventArgs tpea);
        GameObject GetDataIcon();
        GameObject GetGameObject();
        bool IsDraggable();
    }

    [Serializable]
    public class TouchPadEventArgs
    {
        /// <summary>
        /// First selected object.
        /// </summary>
        public ICustomMessageTarget firstSelected;
        /// <summary>
        /// The cast results.
        /// </summary>
        public Collider2D[] hits;
        /// <summary>
        /// Priority dragging direction.  (0,1) or (1,0)
        /// </summary>
        public Vector2 PriorAxe
        {
            get { return priorityAxe; }
        }
        /// <summary>
        /// Touch delta position in screen coordinats;
        /// </summary>
        public Vector2 DragDirection
        {
            get { return touchDeltaPosRaw; }
        }
        /// <summary>
        /// Last drag direction.
        /// </summary>
        public Vector2 LastDragDirection
        {
            get { return lastDragDir; }
        }
        /// <summary>
        /// Return touch world position.
        /// </summary>
        public Vector3 WorldPos
        {
            get { return wPos; }
        }

        private Vector2 touchDeltaPosRaw;
        private Vector2 priorityAxe;
        private Vector2 lastDragDir;
        private Vector3 wPos;
        private Vector2 touchPos;

        /// <summary>
        /// Fill touch arguments from touch object;
        /// </summary>
        public void SetTouch(Touch touch)
        {
            touchPos = touch.position;
            wPos = Camera.main.ScreenToWorldPoint(touchPos);
            hits = Physics2D.OverlapPointAll(new Vector2(wPos.x, wPos.y));
            touchDeltaPosRaw = touch.deltaPosition;

            if (touch.phase == TouchPhase.Moved)
            {
                lastDragDir = touchDeltaPosRaw;
                priorityAxe = GetPriorityOneDirAbs(touchDeltaPosRaw);
            }
        }

        /// <summary>
        /// Fill touch arguments.
        /// </summary>
        public void SetTouch(Vector2 position, Vector2 deltaPosition, TouchPhase touchPhase)
        {
            touchPos = position;
            wPos = Camera.main.ScreenToWorldPoint(touchPos);
            hits = Physics2D.OverlapPointAll(new Vector2(wPos.x, wPos.y));
            touchDeltaPosRaw = deltaPosition;

            if (touchPhase == TouchPhase.Moved)
            {
                lastDragDir = touchDeltaPosRaw;
                priorityAxe = GetPriorityOneDirAbs(touchDeltaPosRaw);
            }
        }

        /// <summary>
        /// Return drag icon for firs touched elment or null.
        /// </summary>
        public GameObject GetIconDrag()
        {
            if (firstSelected != null)
            {
                GameObject icon = firstSelected.GetDataIcon();
                return icon;
            }
            else
            {
                return null;
            }

        }

        private Vector2 GetPriorityOneDirAbs(Vector2 sourceDir)
        {

            if (Mathf.Abs(sourceDir.x) > Mathf.Abs(sourceDir.y))
            {
                float x = (sourceDir.x > 0) ? 1 : 1;
                return new Vector2(x, 0f);
            }
            else
            {
                float y = (sourceDir.y > 0) ? 1 : 1;
                return new Vector2(0f, y);
            }
        }
    }

}