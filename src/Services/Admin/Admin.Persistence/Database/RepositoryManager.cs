using Admin.Domain.Repository.Abstractions;

namespace Admin.Persistence.Database;

public sealed class RepositoryManager(Lazy<IRepositoryReading> repositoryReading, Lazy<IRepositoryWriting> repositoryWriting) : IRepositoryManager
{
    public IRepositoryReading Reading => this.repositoryReading.Value;
    public IRepositoryWriting Writing => this.repositoryWriting.Value;

    private readonly Lazy<IRepositoryReading> repositoryReading = repositoryReading;
    private readonly Lazy<IRepositoryWriting> repositoryWriting = repositoryWriting;
}