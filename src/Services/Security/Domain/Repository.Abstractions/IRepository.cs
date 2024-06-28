namespace Domain.Repository.Abstractions;

public interface IRepository<TEntity> : IAsyncDisposable, IDisposable where TEntity : class, new()
{
    IEnumerable<TRow> MaterializeData<TRow>(IQueryable<TRow> queriable);
    Task<IEnumerable<TRow>> MaterializeDataAsync<TRow>(IQueryable<TRow> queriable);

    IEnumerable<TEntity> All();
    Task<IEnumerable<TEntity>> AllAsync();

    int Count();
    Task<int> CountAsync();

    void Attach(TEntity item);

    void Add(TEntity item);
    Task AddAsync(TEntity item);

    void AddRange(IEnumerable<TEntity> items);
    Task AddRangeAsync(IEnumerable<TEntity> items);

    void Update(TEntity item);

    void Remove(TEntity item);
    void RemoveRange(IEnumerable<TEntity> items);

    void SetState(TEntity item, RepositoryEntityState entityState);

    String GetKeyName(TEntity item);
    KEntity GetKeyValue<KEntity>(TEntity item) where KEntity : struct;

    TEntity GetByKey<K>(K key) where K : struct;
    Task<TEntity> GetByKeyAsync<K>(K key) where K : struct;

    TEntity GetByKey<K1, K2>(K1 key1, K2 key2) where K1 : struct where K2 : struct;
    Task<TEntity> GetByKeyAsync<K1, K2>(K1 key1, K2 key2) where K1 : struct where K2 : struct;

    TEntity GetByKey<K1, K2, K3>(K1 key1, K2 key2, K3 key3) where K1 : struct where K2 : struct where K3 : struct ;
    Task<TEntity> GetByKeyAsync<K1, K2, K3>(K1 key1, K2 key2, K3 key3) where K1 : struct where K2 : struct where K3 : struct;

    TEntity GetByKey<K1, K2, K3, K4>(K1 key1, K2 key2, K3 key3, K4 key4) where K1 : struct where K2 : struct where K3 : struct where K4 : struct;
    Task<TEntity> GetByKeyAsync<K1, K2, K3, K4>(K1 key1, K2 key2, K3 key3, K4 key4) where K1 : struct where K2 : struct where K3 : struct where K4 : struct;

    long NextSequenceValue(String name, String schema = null);
    Task<IEnumerable<long>> NextSequenceValuesAsync(String name, long count);
}

public enum RepositoryEntityState
{
    /// <summary>
    ///     The entity is not being tracked by the context.
    /// </summary>
    Detached = 0,

    /// <summary>
    ///     The entity is being tracked by the context and exists in the database. Its property
    ///     values have not changed from the values in the database.
    /// </summary>
    Unchanged = 1,

    /// <summary>
    ///     The entity is being tracked by the context and exists in the database. It has been marked
    ///     for deletion from the database.
    /// </summary>
    Deleted = 2,

    /// <summary>
    ///     The entity is being tracked by the context and exists in the database. Some or all of its
    ///     property values have been modified.
    /// </summary>
    Modified = 3,

    /// <summary>
    ///     The entity is being tracked by the context but does not yet exist in the database.
    /// </summary>
    Added = 4
}