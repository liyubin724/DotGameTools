using System;
using System.Reflection;

namespace ExtractInject
{
    public static class ExtractInjectUtil
    {
        public static void Inject(IExtractInjectContext context,object obj)
        {
            if(obj == null || context == null)
            {
                throw new ArgumentNullException("ExtractInjectUtil::Inject->argument is null.");
            }

            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach(var field in fields)
            {
                var attrs = field.GetCustomAttributes(typeof(ExtractInjectField), true);
                if(attrs == null || attrs.Length == 0)
                {
                    continue;
                }

                if (!(attrs[0] is ExtractInjectField attr) || attr.Usage == ExtractInjectUsage.Out)
                    continue;

                object fieldValue = null;
                Type fieldType = field.FieldType;
                if(typeof(IExtractInjectContext).IsAssignableFrom(fieldType))
                {
                    fieldValue = context;
                }else
                {
                    if(context.TryGetObject(fieldType,out IExtractInjectObject cachedValue))
                    {
                        fieldValue = cachedValue;
                    }

                    if(fieldValue == null && !attr.IsOptional)
                    {
                        throw new DataNotFoundException($"ExtractInjectUtil::Inject->Data not found.type = {fieldType.FullName}");
                    }
                }

                field.SetValue(obj, fieldValue);
            }
        }

        public static void Extract(IExtractInjectContext context,object obj)
        {
            if (obj == null || context == null)
            {
                throw new ArgumentNullException("ExtractInjectUtil::Extract->argument is null");
            }
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var field in fields)
            {
                var attrs = field.GetCustomAttributes(typeof(ExtractInjectField), true);
                if (attrs == null || attrs.Length == 0)
                {
                    continue;
                }
                if (!(attrs[0] is ExtractInjectField attr) || attr.Usage == ExtractInjectUsage.In)
                    continue;

                Type fieldType = field.FieldType;
                if (typeof(IExtractInjectContext).IsAssignableFrom(fieldType))
                {
                    throw new InvalidOperationException("IExtractInjectContext can only be used with the ExtractInjectUsage.In option.");
                }

                IExtractInjectObject fieldValue = field.GetValue(obj) as IExtractInjectObject;
                if(!attr.IsOptional)
                {
                    context.AddObject(fieldType, fieldValue);
                }else if(fieldValue!=null)
                {
                    context.AddObject(fieldType, fieldValue);
                }
            }
        }
    }
}
