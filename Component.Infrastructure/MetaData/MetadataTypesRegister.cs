using Component.Infrastructure.SysService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Component.Infrastructure.MetaData
{
    public class MetadataTypesRegister
    {
        static bool installed = false;
        static object installedLock = new object();

        public static void InstallForThisAssembly(string assemblyName)
        {
            if (installed)
            {
                return;
            }

            lock (installedLock)
            {
                if (installed)
                {
                    return;
                }

                foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
                {
                    foreach (MetadataTypeAttribute attrib in type.GetCustomAttributes(typeof(MetadataTypeAttribute), true))
                    {
                        TypeDescriptor.AddProviderTransparent(
                            new AssociatedMetadataTypeTypeDescriptionProvider(type, attrib.MetadataClassType), type);
                    }
                }

                installed = true;
            }
        }

        public static void InstallAllAssembly()
        {
            var typeFinder = new WebAppTypeFinder();

            if (installed)
            {
                return;
            }

            lock (installedLock)
            {
                if (installed)
                {
                    return;
                }

                foreach (var assembly in typeFinder.GetAssemblies().Where(x=>x.FullName.Contains("Component.")))
                {
                    foreach (Type type in assembly.GetTypes())
                    {
                        foreach (MetadataTypeAttribute attrib in type.GetCustomAttributes(typeof(MetadataTypeAttribute), true))
                        {
                            TypeDescriptor.AddProviderTransparent(
                                new AssociatedMetadataTypeTypeDescriptionProvider(type, attrib.MetadataClassType), type);
                        }
                    }
                }

                installed = true;
            }
        }

        public static void RegisterMetadataType(Type type, Type associatedMetadataType)
        {
            TypeDescriptor.AddProviderTransparent(
              new AssociatedMetadataTypeTypeDescriptionProvider(
                  type,
                  associatedMetadataType),
             type);
        }
    }
}
