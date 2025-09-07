namespace Regira.Fleet.Data.MySQL;

public static class StatisticsViews
{
    public const string VEHICLETYPES_PER_MONTH = @"CREATE OR REPLACE VIEW stats_vehicletypes_per_month
 AS
 SELECT q.tenant_id,
    q.year,
    q.month,
    q.vehicle_type_code,
    q.total
   FROM ( SELECT b.tenant_id, EXTRACT(year FROM ii.invoice_date) AS year,
            EXTRACT(month FROM ii.invoice_date) AS month,
            ct.code AS vehicle_type_code,
            sum(ii.price_incl) AS total
           FROM interventions b
             LEFT JOIN intervention_invoices ii ON ii.intervention_id = b.id
             JOIN vehicles c ON b.vehicle_id = c.id
             LEFT JOIN vehicle_types ct ON c.vehicle_type_id = ct.id
          GROUP BY b.tenant_id, (EXTRACT(year FROM ii.invoice_date)), (EXTRACT(month FROM ii.invoice_date)), ct.code) q
  ORDER BY q.tenant_id, q.year, q.vehicle_type_code, q.month;";
    public const string VEHICLES_PER_VEHICLETYPES_PER_MONTH = @"CREATE OR REPLACE VIEW stats_vehicles_per_vehicletypes_per_month
 AS
 SELECT q.tenant_id,
    q.year,
    q.month,
    q.vehicle_code,
    q.vehicle_brand_code,
    q.vehicle_type_id,
    q.model,
    q.total
   FROM ( SELECT b.tenant_id, EXTRACT(year FROM ii.invoice_date) AS year,
            EXTRACT(month FROM ii.invoice_date) AS month,
            ct.id AS vehicle_type_id,
            c.code AS vehicle_code,
            cb.code AS vehicle_brand_code,
            c.model,
            sum(ii.price_incl) AS total
           FROM interventions b
             LEFT JOIN intervention_invoices ii ON ii.intervention_id = b.id
             JOIN vehicles c ON b.vehicle_id = c.id
             LEFT JOIN vehicle_types ct ON c.vehicle_type_id = ct.id
             LEFT JOIN vehicle_brands cb ON c.brand_id = cb.id
          GROUP BY b.tenant_id, (EXTRACT(year FROM ii.invoice_date)), (EXTRACT(month FROM ii.invoice_date)), ct.id, c.code, cb.code, c.model) q
  ORDER BY q.tenant_id, q.year, q.month, q.vehicle_code;";
    public const string VEHICLES_PER_MONTH = @"CREATE OR REPLACE VIEW stats_vehicles_per_month
 AS
 SELECT q.tenant_id,
    q.year,
    q.month,
    q.vehicle_code,
    q.vehicle_brand_code,
    q.model,
    q.total
   FROM ( SELECT b.tenant_id, EXTRACT(year FROM ii.invoice_date) AS year,
            EXTRACT(month FROM ii.invoice_date) AS month,
            c.code AS vehicle_code,
            cb.code AS vehicle_brand_code,
            c.model,
            sum(ii.price_incl) AS total
           FROM interventions b
             LEFT JOIN intervention_invoices ii ON ii.intervention_id = b.id
             JOIN vehicles c ON b.vehicle_id = c.id
             LEFT JOIN vehicle_brands cb ON c.brand_id = cb.id
          GROUP BY b.tenant_id, (EXTRACT(year FROM ii.invoice_date)), (EXTRACT(month FROM ii.invoice_date)), c.code, cb.code, c.model) q
  ORDER BY q.tenant_id, q.year, q.month, q.vehicle_code;";
    public const string INTERVENTIONTYPES_PER_MONTH = @"CREATE OR REPLACE VIEW stats_interventiontypes_per_month
 AS
 SELECT q.tenant_id,
    q.year,
    q.month,
    q.interventiontype_code,
    q.total
   FROM ( SELECT b.tenant_id, EXTRACT(year FROM ii.invoice_date) AS year,
            EXTRACT(month FROM ii.invoice_date) AS month,
            it.code AS interventiontype_code,
            sum(ii.price_incl) AS total
           FROM interventions b
             LEFT JOIN intervention_invoices ii ON ii.intervention_id = b.id
             JOIN intervention_types it ON b.intervention_type_id = it.id
          GROUP BY b.tenant_id, (EXTRACT(year FROM ii.invoice_date)), (EXTRACT(month FROM ii.invoice_date)), it.code) q
  ORDER BY q.tenant_id, q.year, q.interventiontype_code, q.month;";
    public const string INTERVENTIONOPERATORS_PER_MONTH = @"CREATE OR REPLACE VIEW stats_interventionoperators_per_month
 AS
 SELECT q.tenant_id,
    q.year,
    q.month,
    q.intervention_operator_id,
    q.supplier,
    q.total
   FROM ( SELECT b.tenant_id, EXTRACT(year FROM ii.invoice_date) AS year,
            EXTRACT(month FROM ii.invoice_date) AS month,
            s.id AS intervention_operator_id,
            s.title AS supplier,
            COALESCE(sum(ii.price_incl), 0) AS total
           FROM interventions b
             LEFT JOIN intervention_invoices ii ON ii.intervention_id = b.id
             JOIN intervention_operators s ON b.operator_id = s.id
          GROUP BY b.tenant_id, (EXTRACT(year FROM ii.invoice_date)), (EXTRACT(month FROM ii.invoice_date)), s.id, s.title) q
  ORDER BY q.tenant_id, q.year, q.intervention_operator_id;";
    public const string INTERVENTIONTYPES_AND_VEHICLETYPES_PER_MONTH = @"CREATE OR REPLACE VIEW stats_interventiontypes_and_vehicletypes_per_month
 AS
 SELECT q.tenant_id,
    q.year,
    q.month,
    q.interventiontype_code,
    q.vehicle_type,
    q.total
   FROM ( SELECT b.tenant_id, EXTRACT(year FROM ii.invoice_date) AS year,
            EXTRACT(month FROM ii.invoice_date) AS month,
            it.code AS interventiontype_code,
            ct.code AS vehicle_type,
            sum(ii.price_incl) AS total
           FROM interventions b
             LEFT JOIN intervention_invoices ii ON b.id = ii.intervention_id
             JOIN intervention_types it ON b.intervention_type_id = it.id
             JOIN vehicles c ON b.vehicle_id = c.id
             LEFT JOIN vehicle_types ct ON c.vehicle_type_id = ct.id
          GROUP BY b.tenant_id, (EXTRACT(year FROM ii.invoice_date)), (EXTRACT(month FROM ii.invoice_date)), it.code, ct.code) q
  ORDER BY q.tenant_id, q.year, q.interventiontype_code, q.month, q.vehicle_type;";

    public static string[] All => [VEHICLETYPES_PER_MONTH, VEHICLES_PER_VEHICLETYPES_PER_MONTH, VEHICLES_PER_MONTH, INTERVENTIONTYPES_PER_MONTH, INTERVENTIONOPERATORS_PER_MONTH, INTERVENTIONTYPES_AND_VEHICLETYPES_PER_MONTH];
}