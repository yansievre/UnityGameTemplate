using System.Reflection;
using UnityEngine;

namespace Utility.Editor
{
    public static class MethodReflectionUtil
    {
        public static void CallMethodOnGameObject(this GameObject gameObject, string methodName, params object[] parameters)
        {
            for (int i = 0; i < gameObject.GetComponentCount(); i++)
            {
                var target = gameObject.GetComponentAtIndex(i);
                CallMethodOnTarget(target, methodName, parameters);
            }
        }
        public static void CallMethodOnTarget(this object gameObject, string methodName, params object[] parameters)
        {
            var target = gameObject;
            MethodInfo tMethod = target.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if(tMethod != null)
            {
                tMethod.Invoke(target, parameters);
            }
        }
    }
}