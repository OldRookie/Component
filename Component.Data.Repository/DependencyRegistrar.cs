using Autofac;
using Component.Infrastructure.DependencyManagement;
using Component.Infrastructure.SysService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.Data.Repository
{
    public class DependencyRegistrar : IDependencyRegistrar
    {
        public int Order => throw new NotImplementedException();

        public void Register(global::Autofac.ContainerBuilder builder, ITypeFinder typeFinder)
        {
            builder.RegisterGeneric(typeof(BaseRepository<>)).AsImplementedInterfaces();
            //builder.RegisterAssemblyTypes(this.GetType().Assembly).AsImplementedInterfaces();
        }
    }
}
