namespace Component.Infrastructure
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        void SaveChanges();
    }
}