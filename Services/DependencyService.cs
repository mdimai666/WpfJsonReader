using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace JsonReaderDima
{
    public static class DependencyService
    {
        static Dictionary<Type, object> m_instances;
        public static T GetInstance<T>()
        {
            //lazy initialiser
            if (m_instances == null)
            {
                m_instances = new Dictionary<Type, object>();
            }

            Type forType = typeof(T);
            object instance = null;
            m_instances.TryGetValue(forType, out instance);

            //if (modelInstance == null)
            //{
            //    modelInstance = Activator.CreateInstance<T>() as object;
            //    m_instances.Add(forType, modelInstance);
            //}

            return (T)instance;
        }

        public static void Register<T>(T instance)
        {
            var innerInstance = GetInstance<T>();

            Type forType = typeof(T);

            if (innerInstance == null)
            {
                //instance = Activator.CreateInstance<T>() as object;
                m_instances.Add(forType, instance);
            }

        }

        //note that static classes are never Garbage Collected (for fairly obvious reasons) so we
        //need an alternative to IDisposable for releasing the references...
        public static void Cleanup()
        {
            //UnityEngine.Debug.Log("DependencyService::cleanup");
            foreach (KeyValuePair<Type, object> kvp in m_instances)
            {
                if (kvp.Value is IDisposable)
                {
                    (kvp.Value as IDisposable).Dispose();
                }
            }

            m_instances.Clear();
        }
    }
}
