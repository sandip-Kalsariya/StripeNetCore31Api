using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StripeNetCoreApi.Migrations
{
    public partial class Add_Order_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<string>(nullable: true),
                    DateModified = table.Column<string>(nullable: true),
                    DateDeleted = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    CardID = table.Column<int>(nullable: true),
                    DiscountCode = table.Column<string>(nullable: true),
                    DiscountType = table.Column<short>(nullable: false),
                    Discount = table.Column<double>(nullable: false),
                    Price = table.Column<double>(nullable: false),
                    Fee = table.Column<double>(nullable: false),
                    Net = table.Column<double>(nullable: false),
                    Charge = table.Column<double>(nullable: false),
                    Status = table.Column<short>(nullable: false),
                    Reason_NotAbleComplete = table.Column<string>(nullable: true),
                    ChargeId = table.Column<string>(nullable: true),
                    PaymentStaus = table.Column<short>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Cards_CardID",
                        column: x => x.CardID,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CardID",
                table: "Orders",
                column: "CardID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_UserId",
                table: "Orders",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
