﻿using Raiqub.Expressions.Queries;

namespace Helpdesk.Relational.Incidents.GetCustomerSummary;

public class GetCustomerIncidentsSummaryQueryModel : IQueryModel<(IncidentStatus Key, int Count)>
{
    public GetCustomerIncidentsSummaryQueryModel(Guid customerId) => CustomerId = customerId;

    public Guid CustomerId { get; }

    public IQueryable<(IncidentStatus Key, int Count)> Execute(IQuerySource source)
    {
        return from pending in source.GetSet<Incident>()
            where pending.CustomerId == CustomerId
            group pending by pending.Status
            into g
            select new ValueTuple<IncidentStatus, int>(g.Key, g.Count());
    }
}
