using Microsoft.EntityFrameworkCore;
using Regira.Entities.Keywords.Abstractions;
using Regira.Entities.QueryBuilders.Abstractions;
using Regira.Fleet.Data;
using Regira.Fleet.Data.Extensions;
using Regira.Fleet.Entities.InterventionOperators.Normalizers;
using Regira.Fleet.Models.InterventionOperators.ContactData;
using Regira.Fleet.Models.InterventionOperators.Operators;

namespace Regira.Fleet.Entities.InterventionOperators.Operators;

public class OperatorFilteredLikeQueryBuilder(FleetContextBase dbContext, IQKeywordHelper qHelper, ContactDataNormalizer contactDataNormalizer)
    : FilteredQueryBuilderBase<Operator, OperatorSearchObject>
{
    public override IQueryable<Operator> Build(IQueryable<Operator> query, OperatorSearchObject? so)
    {
        if (so != null)
        {
            // Phone
            if (!string.IsNullOrWhiteSpace(so.Phone))
            {
                var q = contactDataNormalizer.Normalize(so.Phone, ContactDataTypes.Phone);
                query = query.Where(x => x.ContactData!
                    .Any(cd => cd.DataType == ContactDataTypes.Phone && EF.Functions.Like(cd.NormalizedValue!, $"%{q}%")));
            }
            // Email
            if (!string.IsNullOrWhiteSpace(so.Email))
            {
                var q = contactDataNormalizer.Normalize(so.Email, ContactDataTypes.Email);
                query = query.Where(x => x.ContactData!
                    .Any(cd => cd.DataType == ContactDataTypes.Email && EF.Functions.Like(cd.NormalizedValue!, $"%{q}%")));
            }
            // Address
            if (!string.IsNullOrWhiteSpace(so.Address))
            {
                var keywords = qHelper.Parse(so.Address);
                foreach (var kw in keywords)
                {
                    query = query.Where(x => x.Addresses!.Any(a => EF.Functions.Like(a.NormalizedContent!, kw.QW!)));
                }
            }
            // Q
            query = query.FilterLikeQ(qHelper.Parse(so.Q));
        }

        return query;
    }
}