namespace Domain.Repository.Abstractions;

public interface IRepositoryManager
{
    public IRepositoryReading Reading { get; }
    public IRepositoryWriting Writing { get; }
}