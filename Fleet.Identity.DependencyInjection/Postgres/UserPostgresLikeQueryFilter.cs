using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Keywords.Abstractions;
using Regira.Fleet.Identity.Models.Users;

namespace Regira.Fleet.Identity.DependencyInjection.Postgres;

public class UserPostgresLikeQueryFilter(IQKeywordHelper qHelper)
    : FilteredQueryBuilderBase<FleetUser, string, FleetUserSearchObject>
{
    public override IQueryable<FleetUser> Build(IQueryable<FleetUser> query, FleetUserSearchObject? so)
    {
        if (so != null)
        {
            // Username
            if (!string.IsNullOrWhiteSpace(so.UserName))
            {
                var q = qHelper.ParseKeyword(so.UserName);
                query = query.Where(x => EF.Functions.ILike(x.NormalizedUserName!, q.Q!));
            }
            // Title
            if (!string.IsNullOrWhiteSpace(so.Title))
            {
                var qNames = qHelper.Parse(so.Title);
                foreach (var q in qNames)
                {
                    query = query.Where(x => EF.Functions.ILike(x.GivenName!, q.Q!)
                                             || EF.Functions.ILike(x.LastName!, q.Q!));
                }
            }
            // Q
            if (!string.IsNullOrWhiteSpace(so.Q))
            {
                var keywords = qHelper.Parse(so.Q);
                foreach (var q in keywords)
                {
                    query = query.Where(x =>
                        EF.Functions.ILike(x.NormalizedUserName!, q.QW!)
                        || EF.Functions.ILike(x.NormalizedEmail!, q.QW!)
                        || EF.Functions.ILike(x.GivenName!, q.QW!)
                        || EF.Functions.ILike(x.LastName!, q.QW!)
                    );
                }
            }
        }

        return query;
    }
}