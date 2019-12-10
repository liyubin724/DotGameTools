using System;

namespace ExtractInject
{
    public interface IExtractInjectContext
    {
        bool ContainsObject<T>() where T : IExtractInjectObject;
        bool ContainsObject(Type type);

        T GetObject<T>() where T : IExtractInjectObject;
        IExtractInjectObject GetObject(Type type);

        void AddObject(IExtractInjectObject obj);
        void AddObject(Type type, IExtractInjectObject obj);
        void AddObject<T>(T obj) where T : IExtractInjectObject;

        void DeleteObject(Type type);
        void DeleteObject(IExtractInjectObject obj);
        void DeleteObject<T>() where T : IExtractInjectObject;

        bool TryGetObject<T>(out T obj) where T : IExtractInjectObject;
        bool TryGetObject(Type type, out IExtractInjectObject obj);

        void Clear();
    }
}
