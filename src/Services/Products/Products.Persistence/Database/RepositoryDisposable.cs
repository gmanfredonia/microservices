using Microsoft.EntityFrameworkCore;

namespace Persistence.Database;

public abstract class RepositoryDisposable<TContext>(TContext dbContext) : IAsyncDisposable, IDisposable where TContext : DbContext
{
    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore().ConfigureAwait(false);

        Dispose(false);
        GC.SuppressFinalize(this);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
        => await dbContext.DisposeAsync().ConfigureAwait(false);

    protected virtual void Dispose(bool disposing)
    {
        if (disposing) dbContext.Dispose();
    }

    protected readonly TContext dbContext = dbContext;
}