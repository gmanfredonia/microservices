﻿namespace Admin.Domain.Repository.Abstractions;

public interface IRepositoryWriting : IUnitOfWork
{
    IRepositoryProducts Products { get; }
}