using Component.Data;
using Component.Infrastructure;

namespace Component.Application
{
    public class BaseAppService
    {
        IUnitOfWork _uow;
        public BaseAppService()
        {
           
        }

        public virtual void BeginTransaction()
        {
            _uow = new UnitOfWork();
            _uow.BeginTransaction();
        }

        public virtual void Commit()
        {
            _uow.SaveChanges();
        }
    }
}
