namespace Domain.Repository.Abstractions;

public interface IUnitOfWork
{
    int SaveChanges();
    int SaveChanges(bool openTransaction);
    Task<int> SaveChangesAsync();
    Task<int> SaveChangesAsync(bool openTransaction);

    void BeginTransaction();
    Task BeginTransactionAsync();

    int CommitTransaction();
    int CommitTransaction(bool saveChanges);
    Task<int> CommitTransactionAsync();
    Task<int> CommitTransactionAsync(bool saveChanges);
}