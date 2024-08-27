using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TATK.Utils
{
    public static class ComponentExt
    {
        public static TATKResult TryGetComponent<T>(this Component self, out T result) where T : Component
        {
            result = self.GetComponent<T>();
            if (result == null)
            {
                return TATKResult.Error_InvalidOutput;
            }
            return TATKResult.Success;
        }
    }
}


