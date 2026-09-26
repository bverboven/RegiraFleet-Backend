using Microsoft.EntityFrameworkCore;
using Regira.Entities.Keywords.Abstractions;
using Regira.Entities.QueryBuilders.Abstractions;
using Regira.Fleet.Identity.Models.Users;

namespace Regira.Fleet.Identity.Entities.Users;

public class UserLikeQueryFilter(IQKeywordHelper qHelper)
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
                query = query.Where(x => EF.Functions.Like(x.NormalizedUserName, q.Q));
            }
            // Title
            if (!string.IsNullOrWhiteSpace(so.Title))
            {
                var qNames = qHelper.Parse(so.Title);
                foreach (var q in qNames)
                {
                    query = query.Where(x => EF.Functions.Like(x.GivenName, q.TrimmedQ) 
                                             || EF.Functions.Like(x.LastName, q.TrimmedQ));
                }
            }
            // Q
            if (!string.IsNullOrWhiteSpace(so.Q))
            {
                var keywords = qHelper.Parse(so.Q);
                foreach (var q in keywords)
                {
                    query = query.Where(x =>
                        EF.Functions.Like(x.NormalizedUserName, q.QW)
                        || EF.Functions.Like(x.NormalizedEmail, q.QW)
                        || EF.Functions.Like(x.GivenName, q.TrimmedQW)
                        || EF.Functions.Like(x.LastName, q.TrimmedQW)
                    );
                }
            }
        }

        return query;
    }
}