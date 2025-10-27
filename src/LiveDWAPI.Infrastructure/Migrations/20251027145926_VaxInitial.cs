using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LiveDWAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VaxInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    create OR replace VIEW analytics.universal_semantic_layer.v_age_group AS
                    SELECT DISTINCT age_group_category as age_group
                    FROM analytics.universal_semantic_layer.aggregate_combined_indicators;");

            migrationBuilder.Sql(@"
                    create OR replace VIEW analytics.universal_semantic_layer.v_region_facility AS
                    SELECT DISTINCT facility_name
                    FROM analytics.universal_semantic_layer.aggregate_combined_indicators;");

            migrationBuilder.Sql(@"
                    create OR replace VIEW analytics.universal_semantic_layer.v_sex AS
                    SELECT DISTINCT gender as sex
                    FROM analytics.universal_semantic_layer.aggregate_combined_indicators;");

            migrationBuilder.Sql(@"
                    create OR replace VIEW analytics.universal_semantic_layer.v_vaccine AS
                    SELECT DISTINCT vaccine_name
                    FROM analytics.universal_semantic_layer.aggregate_combined_indicators;");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"drop VIEW v_age_group;");
            migrationBuilder.Sql(@"drop VIEW v_region_facility;");
            migrationBuilder.Sql(@"drop VIEW v_sex;");
            migrationBuilder.Sql(@"drop VIEW v_vaccine;");
        }
    }
}
