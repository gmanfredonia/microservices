namespace Admin.Domain.Repository.Abstractions;

public interface IRepositoryWriting : IUnitOfWork
{
    IRepositoryUsers Users { get; }
}