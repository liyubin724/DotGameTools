using System;
using System.Reflection;

namespace ExtractInject
{
    public static class EIUtil
    {
        public static void Inject(IEIContext context,object obj)
        {
            if(obj == null || context == null)
            {
                throw new ArgumentNullException("ExtractInjectUtil::Inject->argument is null.");
            }

            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach(var field in fields)
            {
                var attrs = field.GetCustomAttributes(typeof(EIField), true);
                if(attrs == null || attrs.Length == 0)
                {
                    continue;
                }

                if (!(attrs[0] is EIField attr) || attr.Usage == EIFieldUsage.Out)
                    continue;

                object fieldValue = null;
                Type fieldType = field.FieldType;
                if(typeof(IEIContext).IsAssignableFrom(fieldType))
                {
                    fieldValue = context;
                }else
                {
                    if(context.TryGetObject(fieldType,out IEIContextObject cachedValue))
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

        public static void Extract(IEIContext context,object obj)
        {
            if (obj == null || context == null)
            {
                throw new ArgumentNullException("ExtractInjectUtil::Extract->argument is null");
            }
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var field in fields)
            {
                var attrs = field.GetCustomAttributes(typeof(EIField), true);
                if (attrs == null || attrs.Length == 0)
                {
                    continue;
                }
                if (!(attrs[0] is EIField attr) || attr.Usage == EIFieldUsage.In)
                    continue;

                Type fieldType = field.FieldType;
                if (typeof(IEIContext).IsAssignableFrom(fieldType))
                {
                    throw new InvalidOperationException("IExtractInjectContext can only be used with the ExtractInjectUsage.In option.");
                }

                IEIContextObject fieldValue = field.GetValue(obj) as IEIContextObject;
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
