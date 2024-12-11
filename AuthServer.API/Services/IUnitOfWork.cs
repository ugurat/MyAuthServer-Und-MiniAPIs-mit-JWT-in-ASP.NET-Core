namespace AuthServer.API.Services
{
    public interface IUnitOfWork
    {

        Task CommmitAsync();

        void Commit();


    }
}
