using Admin.Domain.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Products.Persistence.Database;

public abstract class UnitOfWork<TContext>(TContext dbContext) : RepositoryDisposable<TContext>(dbContext), IUnitOfWork where TContext : DbContext
{
    public int SaveChanges()
       => SaveChanges(false);
    public int SaveChanges(bool openTransaction)
    {
        int result;

        if (openTransaction)
        {
            BeginTransaction();
            result = CommitTransaction();
        }
        else
            result = dbContext.SaveChanges();

        return result;
    }
    public async Task<int> SaveChangesAsync()
        => await SaveChangesAsync(false).ConfigureAwait(false);
    public async Task<int> SaveChangesAsync(bool openTransaction)
    {
        int result;

        if (openTransaction)
        {
            await BeginTransactionAsync().ConfigureAwait(false);
            result = await CommitTransactionAsync().ConfigureAwait(false);
        }
        else
            result = await dbContext.SaveChangesAsync().ConfigureAwait(false);

        return result;
    }

    public void BeginTransaction()
    {
        if (dbTransaction != null) throw new InvalidOperationException("Another transaction is already open!");
        dbTransaction = dbContext.Database.BeginTransaction();
    }
    public async Task BeginTransactionAsync()
    {
        if (dbTransaction != null) throw new InvalidOperationException("Another transaction is already open!");
        dbTransaction = await dbContext.Database.BeginTransactionAsync().ConfigureAwait(false);
    }

    public int CommitTransaction()
        => CommitTransaction(true);
    public int CommitTransaction(bool saveChanges)
    {
        int result;

        try
        {
            result = 0;
            if (saveChanges) result = dbContext.SaveChanges();
            dbTransaction?.Commit();
        }
        catch
        {
            dbTransaction?.Rollback();
            throw;
        }

        return result;
    }
    public async Task<int> CommitTransactionAsync()
        => await CommitTransactionAsync(true).ConfigureAwait(false);
    public async Task<int> CommitTransactionAsync(bool saveChanges)
    {
        int result;

        try
        {
            result = 0;
            if (saveChanges) result = await dbContext.SaveChangesAsync().ConfigureAwait(false);
            if (dbTransaction != null) await dbTransaction.CommitAsync().ConfigureAwait(false);
        }
        catch
        {
            if (dbTransaction != null) await dbTransaction.RollbackAsync().ConfigureAwait(false);
            throw;
        }

        return result;
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await dbContext.DisposeAsync().ConfigureAwait(false);
        if (dbTransaction != null) await dbTransaction.DisposeAsync().ConfigureAwait(false);
    }
    protected override void Dispose(bool disposing)
    {
        if (disposing) dbContext.Dispose();
        dbTransaction?.Dispose();
    }

    private IDbContextTransaction dbTransaction;
}