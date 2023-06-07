using UnityEngine;

namespace Assets.Scripts.Table.UI
{
    public class DragAndDrop : MonoBehaviour
    {
        public static object Data { get; private set; }
        public static bool IsGrabbing { get; private set; }

        public static void StartDrag(object data)
        {
            Data = data;
            IsGrabbing = true;
        }

        public static bool IsValidDataType<T>()
        {
            if (Data == null)
                return false;

            return Data is T || Data?.GetType() == typeof(T);
        }

        public static bool IsValidDataType<T>(T data)
        {
            if (!IsValidDataType<T>())
                return false;

            if (Data.Equals(data))
                return false;

            return true;
        }

        public static void EndDrag()
        {
            Data = null;
            IsGrabbing = false;
        }
    }
}