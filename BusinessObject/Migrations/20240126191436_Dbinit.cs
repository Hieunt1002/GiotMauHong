using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusinessObject.Migrations
{
    /// <inheritdoc />
    public partial class Dbinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Activate",
                columns: table => new
                {
                    Activateid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameActivate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Datepost = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activate", x => x.Activateid);
                });

            migrationBuilder.CreateTable(
                name: "Bloodtypes",
                columns: table => new
                {
                    Bloodtypeid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameBlood = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bloodtypes", x => x.Bloodtypeid);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Img = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    ImgId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Activateid = table.Column<int>(type: "int", nullable: false),
                    Img = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.ImgId);
                    table.ForeignKey(
                        name: "FK_Images_Activate_Activateid",
                        column: x => x.Activateid,
                        principalTable: "Activate",
                        principalColumn: "Activateid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bloodbank",
                columns: table => new
                {
                    Bloodbankid = table.Column<int>(type: "int", nullable: false),
                    NameBloodbank = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bloodbank", x => x.Bloodbankid);
                    table.ForeignKey(
                        name: "FK_Bloodbank_Users_Bloodbankid",
                        column: x => x.Bloodbankid,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Hospitals",
                columns: table => new
                {
                    Hospitalid = table.Column<int>(type: "int", nullable: false),
                    NameHospital = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hospitals", x => x.Hospitalid);
                    table.ForeignKey(
                        name: "FK_Hospitals_Users_Hospitalid",
                        column: x => x.Hospitalid,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Userid = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Datepost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notification_Users_Userid",
                        column: x => x.Userid,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Volunteers",
                columns: table => new
                {
                    Volunteerid = table.Column<int>(type: "int", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Fullname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CCCD = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Volunteers", x => x.Volunteerid);
                    table.ForeignKey(
                        name: "FK_Volunteers_Users_Volunteerid",
                        column: x => x.Volunteerid,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    Requestid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hospitalid = table.Column<int>(type: "int", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Starttime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Endtime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ward = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    District = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.Requestid);
                    table.ForeignKey(
                        name: "FK_Requests_Hospitals_Hospitalid",
                        column: x => x.Hospitalid,
                        principalTable: "Hospitals",
                        principalColumn: "Hospitalid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SendBlood",
                columns: table => new
                {
                    SendBloodid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hospitalid = table.Column<int>(type: "int", nullable: false),
                    Bloodbankid = table.Column<int>(type: "int", nullable: false),
                    Bloodtypeid = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Datesend = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendBlood", x => x.SendBloodid);
                    table.ForeignKey(
                        name: "FK_SendBlood_Bloodbank_Bloodbankid",
                        column: x => x.Bloodbankid,
                        principalTable: "Bloodbank",
                        principalColumn: "Bloodbankid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SendBlood_Bloodtypes_Bloodtypeid",
                        column: x => x.Bloodtypeid,
                        principalTable: "Bloodtypes",
                        principalColumn: "Bloodtypeid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SendBlood_Hospitals_Hospitalid",
                        column: x => x.Hospitalid,
                        principalTable: "Hospitals",
                        principalColumn: "Hospitalid");
                });

            migrationBuilder.CreateTable(
                name: "Takebloods",
                columns: table => new
                {
                    Takebloodid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hospitalid = table.Column<int>(type: "int", nullable: false),
                    Bloodbankid = table.Column<int>(type: "int", nullable: false),
                    Bloodtypeid = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Datetake = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Takebloods", x => x.Takebloodid);
                    table.ForeignKey(
                        name: "FK_Takebloods_Bloodbank_Bloodbankid",
                        column: x => x.Bloodbankid,
                        principalTable: "Bloodbank",
                        principalColumn: "Bloodbankid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Takebloods_Bloodtypes_Bloodtypeid",
                        column: x => x.Bloodtypeid,
                        principalTable: "Bloodtypes",
                        principalColumn: "Bloodtypeid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Takebloods_Hospitals_Hospitalid",
                        column: x => x.Hospitalid,
                        principalTable: "Hospitals",
                        principalColumn: "Hospitalid");
                });

            migrationBuilder.CreateTable(
                name: "Registers",
                columns: table => new
                {
                    RegisterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Volunteerid = table.Column<int>(type: "int", nullable: false),
                    Requestid = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Bloodtypeid = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registers", x => x.RegisterId);
                    table.ForeignKey(
                        name: "FK_Registers_Bloodtypes_Bloodtypeid",
                        column: x => x.Bloodtypeid,
                        principalTable: "Bloodtypes",
                        principalColumn: "Bloodtypeid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registers_Requests_Requestid",
                        column: x => x.Requestid,
                        principalTable: "Requests",
                        principalColumn: "Requestid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registers_Volunteers_Volunteerid",
                        column: x => x.Volunteerid,
                        principalTable: "Volunteers",
                        principalColumn: "Volunteerid");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Images_Activateid",
                table: "Images",
                column: "Activateid");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_Userid",
                table: "Notification",
                column: "Userid");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_Bloodtypeid",
                table: "Registers",
                column: "Bloodtypeid");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_Requestid",
                table: "Registers",
                column: "Requestid");

            migrationBuilder.CreateIndex(
                name: "IX_Registers_Volunteerid",
                table: "Registers",
                column: "Volunteerid");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_Hospitalid",
                table: "Requests",
                column: "Hospitalid");

            migrationBuilder.CreateIndex(
                name: "IX_SendBlood_Bloodbankid",
                table: "SendBlood",
                column: "Bloodbankid");

            migrationBuilder.CreateIndex(
                name: "IX_SendBlood_Bloodtypeid",
                table: "SendBlood",
                column: "Bloodtypeid");

            migrationBuilder.CreateIndex(
                name: "IX_SendBlood_Hospitalid",
                table: "SendBlood",
                column: "Hospitalid");

            migrationBuilder.CreateIndex(
                name: "IX_Takebloods_Bloodbankid",
                table: "Takebloods",
                column: "Bloodbankid");

            migrationBuilder.CreateIndex(
                name: "IX_Takebloods_Bloodtypeid",
                table: "Takebloods",
                column: "Bloodtypeid");

            migrationBuilder.CreateIndex(
                name: "IX_Takebloods_Hospitalid",
                table: "Takebloods",
                column: "Hospitalid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "Registers");

            migrationBuilder.DropTable(
                name: "SendBlood");

            migrationBuilder.DropTable(
                name: "Takebloods");

            migrationBuilder.DropTable(
                name: "Activate");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "Volunteers");

            migrationBuilder.DropTable(
                name: "Bloodbank");

            migrationBuilder.DropTable(
                name: "Bloodtypes");

            migrationBuilder.DropTable(
                name: "Hospitals");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
