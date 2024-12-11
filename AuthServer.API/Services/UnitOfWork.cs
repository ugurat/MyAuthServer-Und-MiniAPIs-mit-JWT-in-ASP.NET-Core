using AuthServer.API.DATA;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.API.Services
{
    public class UnitOfWork : IUnitOfWork
    {


        // DI
        private readonly DbContext _context;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }



        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommmitAsync()
        {
            await _context.SaveChangesAsync();
        }


    }
}
