﻿using System.Reflection;

namespace VezonCore
{
    public class IVezonExtension
    {
        public virtual string Name() { return "Unnamed Object"; }
        public virtual string ShortName() { return "object"; }
        public virtual string Version() { return "1.0.0"; }
        public virtual string Author() { return "John Doe"; }
        public virtual string FullInfoString() { return (Name() + " v" + Version() + " by " + Author()); }
        public virtual void OnLoad() 
        {
        }
        public virtual void OnShutdown() 
        {
        }
        public virtual void OnThink() 
        {
        }

        public IEnumerable<T> GetInstancesOfImplementingTypes<T>()
        {
            AppDomain app = AppDomain.CurrentDomain;
            Assembly[] ass = app.GetAssemblies();
            Type[] types;
            Type targetType = typeof(T);

            foreach (Assembly a in ass)
            {
                types = a.GetTypes();
                foreach (Type t in types)
                {
                    if (t.IsInterface) continue;
                    if (t.IsAbstract) continue;
                    foreach (Type iface in t.GetInterfaces())
                    {
                        if (!iface.Equals(targetType)) continue;
                        yield return (T)Activator.CreateInstance(t);
                        break;
                    }
                }
            }
        }
    }
}
