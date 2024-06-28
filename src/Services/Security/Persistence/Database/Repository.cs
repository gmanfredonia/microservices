using Domain.Repository.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Update;
using System.Data;
using System.Data.Common;
using System.Globalization;

namespace Persistence.Database;

public abstract class Repository<TContext, TEntity>(TContext dbContext) : RepositoryDisposable<TContext>(dbContext),
                                                                          IRepository<TEntity>
                                                                          where TContext : DbContext
                                                                          where TEntity : class, new()
{
    public virtual IEnumerable<TRow> MaterializeData<TRow>(IQueryable<TRow> queriable) => queriable.ToArray();
    public virtual async Task<IEnumerable<TRow>> MaterializeDataAsync<TRow>(IQueryable<TRow> queriable) => await queriable.ToArrayAsync().ConfigureAwait(false);

    public virtual IEnumerable<TEntity> All()
        => dbContext.Set<TEntity>().ToArray();
    public virtual async Task<IEnumerable<TEntity>> AllAsync()
        => await dbContext.Set<TEntity>().ToArrayAsync().ConfigureAwait(false);

    public virtual int Count()
        => dbContext.Set<TEntity>().Count();
    public virtual async Task<int> CountAsync()
        => await dbContext.Set<TEntity>().CountAsync().ConfigureAwait(false);

    public virtual void Attach(TEntity item)
        => dbContext.Set<TEntity>().Attach(item);

    public virtual void Add(TEntity item)
        => dbContext.Set<TEntity>().Add(item);
    public virtual async Task AddAsync(TEntity item)
        => await dbContext.Set<TEntity>().AddAsync(item).ConfigureAwait(false);

    public virtual void AddRange(IEnumerable<TEntity> items)
        => dbContext.Set<TEntity>().AddRange(items);
    public virtual async Task AddRangeAsync(IEnumerable<TEntity> items)
        => await dbContext.Set<TEntity>().AddRangeAsync(items).ConfigureAwait(false);

    public virtual void Update(TEntity item)
        => dbContext.Entry(item).State = EntityState.Modified;

    public virtual void Remove(TEntity item)
        => dbContext.Set<TEntity>().Remove(item);
    public virtual void RemoveRange(IEnumerable<TEntity> items)
         => dbContext.Set<TEntity>().RemoveRange(items);

    public virtual void SetState(TEntity item, RepositoryEntityState entityState)
        => dbContext.Entry(item).State = (EntityState)entityState;

    public virtual string GetKeyName(TEntity entity)
        => dbContext.Model.FindEntityType(typeof(TEntity))!.FindPrimaryKey()!.Properties.Select(x => x.Name).First();

    public virtual K GetKeyValue<K>(TEntity entity) where K : struct
    {
        string keyName;

        keyName = GetKeyName(entity);

        return (K)entity!.GetType().GetProperty(keyName)!.GetValue(entity, null)!;
    }

    public virtual TEntity GetByKey<K>(K key)
        where K : struct
        => dbContext.Set<TEntity>().Find(key);

    public virtual async Task<TEntity> GetByKeyAsync<K>(K key)
        where K : struct
        => await dbContext.Set<TEntity>().FindAsync(key).ConfigureAwait(false);

    public virtual TEntity GetByKey<K1, K2>(K1 key1, K2 key2)
        where K1 : struct
        where K2 : struct
        => dbContext.Set<TEntity>().Find(key1, key2);

    public virtual async Task<TEntity> GetByKeyAsync<K1, K2>(K1 key1, K2 key2)
        where K1 : struct
        where K2 : struct
        => await dbContext.Set<TEntity>().FindAsync(key1, key2);

    public virtual TEntity GetByKey<K1, K2, K3>(K1 key1, K2 key2, K3 key3)
        where K1 : struct
        where K2 : struct
        where K3 : struct
        => dbContext.Set<TEntity>().Find(key1, key2, key3);

    public virtual async Task<TEntity> GetByKeyAsync<K1, K2, K3>(K1 key1, K2 key2, K3 key3)
        where K1 : struct
        where K2 : struct
        where K3 : struct
        => await dbContext.Set<TEntity>().FindAsync(key1, key2, key3).ConfigureAwait(false);

    public virtual TEntity GetByKey<K1, K2, K3, K4>(K1 key1, K2 key2, K3 key3, K4 key4)
        where K1 : struct
        where K2 : struct
        where K3 : struct
        where K4 : struct
        => dbContext.Set<TEntity>().Find(key1, key2, key3, key4);

    public virtual async Task<TEntity> GetByKeyAsync<K1, K2, K3, K4>(K1 key1, K2 key2, K3 key3, K4 key4)
        where K1 : struct
        where K2 : struct
        where K3 : struct
        where K4 : struct
        => await dbContext.Set<TEntity>().FindAsync(key1, key2, key3, key4).ConfigureAwait(false);

    public virtual long NextSequenceValue(string name, string schema = null)
    {
        var sqlGenerator = dbContext.GetService<IUpdateSqlGenerator>();
        var sql = sqlGenerator.GenerateNextSequenceValueOperation(name, schema ?? dbContext.Model.GetDefaultSchema());
        var rawCommandBuilder = dbContext.GetService<IRawSqlCommandBuilder>();
        var command = rawCommandBuilder.Build(sql);
        var connection = dbContext.GetService<IRelationalConnection>();
        var logger = dbContext.GetService<IDiagnosticsLogger<DbLoggerCategory.Database.Command>>();
        var parameters = new RelationalCommandParameterObject(connection, null, null, dbContext, (IRelationalCommandDiagnosticsLogger)logger);
        var result = command.ExecuteScalar(parameters);

        return Convert.ToInt64(result, CultureInfo.InvariantCulture);
    }

    public async Task<IEnumerable<long>> NextSequenceValuesAsync(string name, long count)
    {
        ICollection<long> values = Array.Empty<long>();

        await using DbCommand cmd = dbContext.Database.GetDbConnection().CreateCommand();
        if (cmd.Connection!.State == ConnectionState.Closed)
            await cmd.Connection.OpenAsync().ConfigureAwait(false);

        cmd.CommandText = $"SELECT next value FOR {name} " +
                           "FROM (" +
                          $"    SELECT TOP({count}) 1 X" +
                           "    FROM sys.objects" +
                           ") X";
        await using DbDataReader result = await cmd.ExecuteReaderAsync().ConfigureAwait(false);
        while (await result.ReadAsync().ConfigureAwait(false))
            values.Add(await result.GetFieldValueAsync<long>(0).ConfigureAwait(false));

        return values;
    }
}