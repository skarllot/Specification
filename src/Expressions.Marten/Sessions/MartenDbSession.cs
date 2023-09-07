﻿using Marten;
using Microsoft.Extensions.Logging;
using Raiqub.Expressions.Sessions;

namespace Raiqub.Expressions.Marten.Sessions;

public class MartenDbSession : MartenDbQuerySession, IDbSession
{
    private readonly IDocumentSession _session;

    public MartenDbSession(ILogger<MartenDbSession> logger, IDocumentSession session, ChangeTracking tracking)
        : base(logger, session)
    {
        _session = session;
        Tracking = tracking;
    }

    public ChangeTracking Tracking { get; }

    public ValueTask<IDbSessionTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException(
            "Transactions are not currently supported by Marten implementation of IDbSession");
    }

    public void Add<TEntity>(TEntity entity)
        where TEntity : class => AddRange(new[] { entity });

    public void AddRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class => _session.Store(entities);

    public ValueTask AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        where TEntity : class
    {
        return AddRangeAsync(new[] { entity }, cancellationToken);
    }

    public ValueTask AddRangeAsync<TEntity>(
        IEnumerable<TEntity> entities,
        CancellationToken cancellationToken = default)
        where TEntity : class
    {
        _session.Store(entities);
        return default;
    }

    public void Remove<TEntity>(TEntity entity)
        where TEntity : class => _session.Delete(entity);

    public void RemoveRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class => _session.DeleteObjects(entities);

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _session.SaveChangesAsync(cancellationToken);

    public void Update<TEntity>(TEntity entity)
        where TEntity : class => UpdateRange(new[] { entity });

    public void UpdateRange<TEntity>(IEnumerable<TEntity> entities)
        where TEntity : class => _session.Store(entities);
}
