using Component.Infrastructure.BaseData;
using System;

namespace Component.Application.Query
{
    public class BaseDataService: IBaseDataService
    {
    }

    public static class BaseDataServiceExtension
    {
        public static IBaseDataService BaseDataService;
    }
}
