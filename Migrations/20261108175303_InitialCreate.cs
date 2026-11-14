using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnterpriseSecureProxy.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "configuration_history",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EntityType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EntityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Action = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ChangeSummary = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: true),
                    ChangedBy = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ChangedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_configuration_history", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "destinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    BaseUrl = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    TimeoutSeconds = table.Column<int>(type: "integer", nullable: false),
                    AuthProfileReference = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    AllowedPorts = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_destinations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "encryption_profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Algorithm = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    KeyReference = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_encryption_profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "request_logs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CorrelationId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    OperationKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DestinationId = table.Column<Guid>(type: "uuid", nullable: true),
                    Forwarded = table.Column<bool>(type: "boolean", nullable: false),
                    StartedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CompletedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DurationMs = table.Column<long>(type: "bigint", nullable: false),
                    UpstreamStatusCode = table.Column<int>(type: "integer", nullable: true),
                    ProxyStatusCode = table.Column<int>(type: "integer", nullable: false),
                    Outcome = table.Column<int>(type: "integer", nullable: false),
                    RejectionReason = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_request_logs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "operations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DestinationId = table.Column<Guid>(type: "uuid", nullable: false),
                    HttpMethod = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    RelativePath = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    MaxRequestSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    MaxResponseSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    RequiresEncryption = table.Column<bool>(type: "boolean", nullable: false),
                    AllowUnknownParameters = table.Column<bool>(type: "boolean", nullable: false),
                    EncryptionProfileId = table.Column<Guid>(type: "uuid", nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_operations_destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_operations_encryption_profiles_EncryptionProfileId",
                        column: x => x.EncryptionProfileId,
                        principalTable: "encryption_profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "header_rules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    HeaderName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_header_rules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_header_rules_operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "parameter_rules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OperationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsRequired = table.Column<bool>(type: "boolean", nullable: false),
                    DataType = table.Column<int>(type: "integer", nullable: false),
                    Location = table.Column<int>(type: "integer", nullable: false),
                    MaxLength = table.Column<int>(type: "integer", nullable: true),
                    MinValue = table.Column<decimal>(type: "numeric", nullable: true),
                    MaxValue = table.Column<decimal>(type: "numeric", nullable: true),
                    RegexPattern = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parameter_rules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_parameter_rules_operations_OperationId",
                        column: x => x.OperationId,
                        principalTable: "operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_configuration_history_EntityId",
                table: "configuration_history",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_configuration_history_EntityType",
                table: "configuration_history",
                column: "EntityType");

            migrationBuilder.CreateIndex(
                name: "IX_destinations_IsActive",
                table: "destinations",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_destinations_Name",
                table: "destinations",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_header_rules_OperationId_HeaderName",
                table: "header_rules",
                columns: new[] { "OperationId", "HeaderName" });

            migrationBuilder.CreateIndex(
                name: "IX_operations_DestinationId",
                table: "operations",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_operations_EncryptionProfileId",
                table: "operations",
                column: "EncryptionProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_operations_IsActive",
                table: "operations",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_operations_OperationKey",
                table: "operations",
                column: "OperationKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_parameter_rules_OperationId_Name",
                table: "parameter_rules",
                columns: new[] { "OperationId", "Name" });

            migrationBuilder.CreateIndex(
                name: "IX_request_logs_CorrelationId",
                table: "request_logs",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_request_logs_OperationKey_StartedUtc",
                table: "request_logs",
                columns: new[] { "OperationKey", "StartedUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_request_logs_StartedUtc",
                table: "request_logs",
                column: "StartedUtc");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "configuration_history");

            migrationBuilder.DropTable(
                name: "header_rules");

            migrationBuilder.DropTable(
                name: "parameter_rules");

            migrationBuilder.DropTable(
                name: "request_logs");

            migrationBuilder.DropTable(
                name: "operations");

            migrationBuilder.DropTable(
                name: "destinations");

            migrationBuilder.DropTable(
                name: "encryption_profiles");
        }
    }
}
