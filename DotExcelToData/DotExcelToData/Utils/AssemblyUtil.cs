using System;
using System.Reflection;

namespace Dot.Tools.ETD.Fields
{
    public static class AssemblyUtil
    {
        public static Type GetTypeByName(string name,bool ingnoreCase = true)
        {
            if(string.IsNullOrEmpty(name))
            {
                return null;
            }
            if(ingnoreCase)
            {
                name = name.ToLower();
            }
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach(var assembly in assemblies)
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        string typeName = ingnoreCase ? type.Name.ToLower() : type.Name;
                        if (typeName == name)
                        {
                            return type;
                        }
                    }
                }
                catch
                {

                }
                
            }

            return null;
        }
    }
}
