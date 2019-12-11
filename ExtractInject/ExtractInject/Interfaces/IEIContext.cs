using System;

namespace ExtractInject
{
    public interface IEIContext
    {
        bool ContainsObject<T>() where T : IEIContextObject;
        bool ContainsObject(Type type);

        T GetObject<T>() where T : IEIContextObject;
        IEIContextObject GetObject(Type type);

        void AddObject(IEIContextObject obj);
        void AddObject(Type type, IEIContextObject obj);
        void AddObject<T>(T obj) where T : IEIContextObject;

        void DeleteObject(Type type);
        void DeleteObject(IEIContextObject obj);
        void DeleteObject<T>() where T : IEIContextObject;

        bool TryGetObject<T>(out T obj) where T : IEIContextObject;
        bool TryGetObject(Type type, out IEIContextObject obj);

        void Clear();
    }
}
