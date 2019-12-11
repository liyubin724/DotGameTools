using System;
using System.Collections.Generic;

namespace ExtractInject
{
    public class EIContext : IEIContext
    {
        private Dictionary<Type, IEIContextObject> typeToObjectDic;

        public EIContext()
        {
            typeToObjectDic = new Dictionary<Type, IEIContextObject>();
        }

        public EIContext(params IEIContextObject[] objs) : this()
        {
            if(objs != null)
            {
                foreach (var obj in objs)
                {
                    if (obj != null)
                    {
                        AddObject(obj);
                    }
                }
            }
        }

        public bool ContainsObject<T>() where T : IEIContextObject
        {
            return ContainsObject(typeof(T));
        }

        public bool ContainsObject(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("ExtractInjectContext:ContainsObject->Argument is Null");

            return typeToObjectDic.ContainsKey(type);
        }

        public T GetObject<T>() where T : IEIContextObject
        {
            IEIContextObject obj = GetObject(typeof(T));
            if(obj !=null)
            {
                return (T)obj;
            }
            return default(T);
        }

        public IEIContextObject GetObject(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("ExtractInjectContext:GetObject->Argument is Null");

            if(TryGetObject(type,out IEIContextObject obj))
            {
                return obj;
            }

            return null;
        }

        public void AddObject(IEIContextObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("ExtractInjectContext::AddObject->obj is null");
            }
            AddObject(obj.GetType(), obj);
        }

        public void AddObject<T>(T obj) where T : IEIContextObject
        {
            AddObject(typeof(T), obj);
        }

        public void AddObject(Type type, IEIContextObject obj)
        {
            if(obj== null)
            {
                throw new ArgumentNullException("ExtractInjectContext::AddObject->obj is null");
            }

            if (!type.IsInstanceOfType(obj))
            {
                throw new InvalidOperationException($"'{obj.GetType()}' is not of passed in type '{type}'.");
            }

            if (typeToObjectDic.ContainsKey(type))
            {
                typeToObjectDic[type] = obj;
            }
            else
            {
                typeToObjectDic.Add(type, obj);
            }
        }

        public bool TryGetObject<T>(out T obj) where T : IEIContextObject
        {
            if (typeToObjectDic.TryGetValue(typeof(T), out IEIContextObject value))
            {
                obj = (T)value;
                return true;
            }

            obj = default(T);
            return false;
        }

        public bool TryGetObject(Type type, out IEIContextObject obj)
        {
            if(typeToObjectDic.TryGetValue(type,out IEIContextObject value) && type.IsInstanceOfType(value))
            {
                obj = value;
                return true;
            }
            obj = null;
            return false;
        }

        public void DeleteObject<T>() where T : IEIContextObject
        {
            DeleteObject(typeof(T));
        }

        public void DeleteObject(IEIContextObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("ExtractInjectContext::DeleteObject->obj is null");
            }
            DeleteObject(obj.GetType());
        }

        public void DeleteObject(Type type)
        {
            if(typeToObjectDic.ContainsKey(type))
            {
                typeToObjectDic.Remove(type);
            }
        }

        public void Clear()
        {
            typeToObjectDic.Clear();
        }
    }
}
