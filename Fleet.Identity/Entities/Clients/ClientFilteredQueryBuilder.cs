using Microsoft.EntityFrameworkCore;
using Regira.Entities.EFcore.QueryBuilders.Abstractions;
using Regira.Entities.Keywords.Abstractions;
using Regira.Fleet.Identity.Models.Clients;

namespace Regira.Fleet.Identity.Entities.Clients;

public class ClientFilteredQueryBuilder(IQKeywordHelper qHelper) : FilteredQueryBuilderBase<Client, string, ClientSearchObject>
{
    public override IQueryable<Client> Build(IQueryable<Client> query, ClientSearchObject? so)
    {
        if (so != null)
        {
            // ID
            if (!string.IsNullOrWhiteSpace(so.Id))
            {
                query = query.Where(x => x.Id == so.Id);
            }
            // Code
            if (!string.IsNullOrWhiteSpace(so.Code))
            {
                var upperCode = so.Code.ToUpper();
                query = query.Where(x => x.Code!.ToUpper() == upperCode);
            }
            // Title
            if (!string.IsNullOrWhiteSpace(so.Title))
            {
                var qTitles = qHelper.Parse(so.Title);
                foreach (var q in qTitles)
                {
                    query = query.Where(x => x.Code!.ToUpper() == q.Normalized || EF.Functions.Like(x.NormalizedTitle, q.Q));
                }
            }
            // Culture
            if (!string.IsNullOrWhiteSpace(so.Culture))
            {
                query = query.Where(x => x.DefaultCulture == so.Culture);
            }
            // Q
            if (!string.IsNullOrWhiteSpace(so.Q))
            {
                var keywords = qHelper.Parse(so.Q);
                foreach (var q in keywords)
                {
                    query = query.Where(x => EF.Functions.Like(x.NormalizedTitle, q.QW));
                }
            }
        }

        return query;
    }
}