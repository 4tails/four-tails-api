using FourTails.Core.DomainModels;
using FourTails.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FourTails.DataAccess.Repositories;

public class UserRepository : ICrudRepository<User>
{
    private readonly FTDBContext _context;

    public UserRepository(FTDBContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public Task Create(User user)
    {
        return Task.FromResult(_context.Set<User>().Add(user).Entity);
    }

    public Task Delete(User user)
    {
        _context.Set<User>().Remove(user);
        _context.Entry(user).State = EntityState.Deleted;
        _context.SaveChangesAsync();

        return Task.CompletedTask;
    }

    public async Task<User> ReadById(string userId)
    {
       return await _context.Set<User>().AsNoTracking().Where(x => x.Id == userId).FirstAsync();
    }

    public async Task<IEnumerable<User>> ReadAll()
    {
        return await _context.Set<User>().AsNoTracking().ToListAsync();
    }

    public Task Update(User user)
    {
        _context.Set<User>().Attach(user);
        _context.Entry(user).State = EntityState.Modified;
        _context.SaveChangesAsync();

        return Task.CompletedTask;
    }
}