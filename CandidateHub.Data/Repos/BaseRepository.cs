using Microsoft.EntityFrameworkCore;

namespace CandidateHub.Data.Repos;

public class BaseRepository<T> where T : class
{
    private readonly AppDbContext _context;
    readonly DbSet<T> _dbSet;

    public BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    protected async Task<T?> GetByStringIdAsync(string id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<T> InsertAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<T> UpsertAsync(T entity, string id)
    {
        var existingEntity = await _dbSet.FindAsync(id);

        if (existingEntity is not null)
        {
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            _dbSet.Update(existingEntity);
        }
        else
            await _dbSet.AddAsync(entity);

        await _context.SaveChangesAsync();
        return entity;
    }
}