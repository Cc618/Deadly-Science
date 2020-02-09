using UnityEngine;

namespace ds
{
    public static class Transforms
    {
        public static void DestroyChildren(this Transform t, bool destroyImmediately = false)
        {
            foreach (Transform child in t)
            {
                if (destroyImmediately)
                    MonoBehaviour.DestroyImmediate(child);
                else
                    MonoBehaviour.Destroy(child);
            }
        }
    }
}
