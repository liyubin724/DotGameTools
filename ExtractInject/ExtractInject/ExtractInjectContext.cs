using System;
using System.Collections.Generic;

namespace ExtractInject
{
    public class ExtractInjectContext : IExtractInjectContext
    {
        private Dictionary<Type, IExtractInjectObject> typeToObjectDic;

        public ExtractInjectContext()
        {
            typeToObjectDic = new Dictionary<Type, IExtractInjectObject>();
        }

        public ExtractInjectContext(params IExtractInjectObject[] objs) : this()
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

        public bool ContainsObject<T>() where T : IExtractInjectObject
        {
            return ContainsObject(typeof(T));
        }

        public bool ContainsObject(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("ExtractInjectContext:ContainsObject->Argument is Null");

            return typeToObjectDic.ContainsKey(type);
        }

        public T GetObject<T>() where T : IExtractInjectObject
        {
            IExtractInjectObject obj = GetObject(typeof(T));
            if(obj !=null)
            {
                return (T)obj;
            }
            return default(T);
        }

        public IExtractInjectObject GetObject(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("ExtractInjectContext:GetObject->Argument is Null");

            if(TryGetObject(type,out IExtractInjectObject obj))
            {
                return obj;
            }

            return null;
        }

        public void AddObject(IExtractInjectObject obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("ExtractInjectContext::AddObject->obj is null");
            }
            AddObject(obj.GetType(), obj);
        }

        public void AddObject<T>(T obj) where T : IExtractInjectObject
        {
            AddObject(typeof(T), obj);
        }

        public void AddObject(Type type, IExtractInjectObject obj)
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

        public bool TryGetObject<T>(out T obj) where T : IExtractInjectObject
        {
            if (typeToObjectDic.TryGetValue(typeof(T), out IExtractInjectObject value))
            {
                obj = (T)value;
                return true;
            }

            obj = default(T);
            return false;
        }

        public bool TryGetObject(Type type, out IExtractInjectObject obj)
        {
            if(typeToObjectDic.TryGetValue(type,out IExtractInjectObject value) && type.IsInstanceOfType(value))
            {
                obj = value;
                return true;
            }
            obj = null;
            return false;
        }

        public void DeleteObject<T>() where T : IExtractInjectObject
        {
            DeleteObject(typeof(T));
        }

        public void DeleteObject(IExtractInjectObject obj)
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
