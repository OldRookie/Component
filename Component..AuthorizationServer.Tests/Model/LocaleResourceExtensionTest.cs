using Component.Infrastructure;
using Component.Infrastructure.BaseDataModel.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Component.Tests.Model
{
    public class LocaleResourceExtensionTest
    {
      
        [Fact]
        public void ResourceValueTest()
        {
            ComponentContext.Current.Initialize();
            "ModuleName.Code".ResourceValue("zh-cn");
        }

    }
}
