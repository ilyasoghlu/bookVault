using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BookVault.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:Enum:employeestatus", "Active,Inactive,Terminated");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firstname = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    lastname = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    employeeposition = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    hiredate = table.Column<DateOnly>(type: "date", nullable: false),
                    enddate = table.Column<DateOnly>(type: "date", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("employees_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Publishers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    publishername = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    address = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    city = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    website = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("Publishers_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Readers",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    firstname = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    lastname = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    email = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    dateofbirth = table.Column<DateOnly>(type: "date", nullable: false),
                    expirationdate = table.Column<DateOnly>(type: "date", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    createdby = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    updatedby = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Readers_pkey", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Sections",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    sectionnumber = table.Column<int>(type: "integer", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    sectionfloor = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    employeeid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Sections_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_section_employee",
                        column: x => x.employeeid,
                        principalTable: "Employees",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    firstname = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    lastname = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    birthdate = table.Column<DateOnly>(type: "date", nullable: false),
                    deathdate = table.Column<DateOnly>(type: "date", nullable: true),
                    nationality = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    biography = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    publisherid = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Authors_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_author_publisher",
                        column: x => x.publisherid,
                        principalTable: "Publishers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    roomnumber = table.Column<int>(type: "integer", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    roomfloor = table.Column<int>(type: "integer", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    sectionid = table.Column<int>(type: "integer", nullable: true),
                    employeeid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Rooms_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_room_employee",
                        column: x => x.employeeid,
                        principalTable: "Employees",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_room_section",
                        column: x => x.sectionid,
                        principalTable: "Sections",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    isbn = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: false),
                    title = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    category = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    genre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    publicationdate = table.Column<DateOnly>(type: "date", nullable: false),
                    pagecount = table.Column<int>(type: "integer", nullable: false),
                    edition = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    totalcopies = table.Column<int>(type: "integer", nullable: false),
                    language = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    authorid = table.Column<Guid>(type: "uuid", nullable: true),
                    publisherid = table.Column<Guid>(type: "uuid", nullable: true),
                    sectionid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Books_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_book_author",
                        column: x => x.authorid,
                        principalTable: "Authors",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_book_publisher_id",
                        column: x => x.publisherid,
                        principalTable: "Publishers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_book_section",
                        column: x => x.sectionid,
                        principalTable: "Sections",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "BookCases",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    casenumber = table.Column<int>(type: "integer", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    sectionid = table.Column<int>(type: "integer", nullable: true),
                    roomid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("BookCases_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_case_room",
                        column: x => x.roomid,
                        principalTable: "Rooms",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_case_section",
                        column: x => x.sectionid,
                        principalTable: "Sections",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "Borrowings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    duedate = table.Column<DateOnly>(type: "date", nullable: false),
                    returndate = table.Column<DateOnly>(type: "date", nullable: true),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    readerid = table.Column<int>(type: "integer", nullable: false),
                    bookid = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Borrowings_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_borrowing_reader",
                        column: x => x.readerid,
                        principalTable: "Readers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_borrwing_book",
                        column: x => x.bookid,
                        principalTable: "Books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shelves",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    shelvecode = table.Column<int>(type: "integer", nullable: false),
                    capacity = table.Column<int>(type: "integer", nullable: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updatedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    sectionid = table.Column<int>(type: "integer", nullable: true),
                    caseid = table.Column<int>(type: "integer", nullable: true),
                    roomid = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Shelves_pkey", x => x.id);
                    table.ForeignKey(
                        name: "fk_shelve_case",
                        column: x => x.caseid,
                        principalTable: "BookCases",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_shelve_room",
                        column: x => x.roomid,
                        principalTable: "Rooms",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_shelve_section",
                        column: x => x.sectionid,
                        principalTable: "Sections",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Authors_publisherid",
                table: "Authors",
                column: "publisherid");

            migrationBuilder.CreateIndex(
                name: "IX_BookCases_roomid",
                table: "BookCases",
                column: "roomid");

            migrationBuilder.CreateIndex(
                name: "IX_BookCases_sectionid",
                table: "BookCases",
                column: "sectionid");

            migrationBuilder.CreateIndex(
                name: "IX_Books_authorid",
                table: "Books",
                column: "authorid");

            migrationBuilder.CreateIndex(
                name: "IX_Books_publisherid",
                table: "Books",
                column: "publisherid");

            migrationBuilder.CreateIndex(
                name: "IX_Books_sectionid",
                table: "Books",
                column: "sectionid");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_bookid",
                table: "Borrowings",
                column: "bookid");

            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_readerid",
                table: "Borrowings",
                column: "readerid");

            migrationBuilder.CreateIndex(
                name: "employees_email_key",
                table: "Employees",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Publishers_email_key",
                table: "Publishers",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Publishers_website_key",
                table: "Publishers",
                column: "website",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Readers_email_key",
                table: "Readers",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_employeeid",
                table: "Rooms",
                column: "employeeid");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_sectionid",
                table: "Rooms",
                column: "sectionid");

            migrationBuilder.CreateIndex(
                name: "IX_Sections_employeeid",
                table: "Sections",
                column: "employeeid");

            migrationBuilder.CreateIndex(
                name: "IX_Shelves_caseid",
                table: "Shelves",
                column: "caseid");

            migrationBuilder.CreateIndex(
                name: "IX_Shelves_roomid",
                table: "Shelves",
                column: "roomid");

            migrationBuilder.CreateIndex(
                name: "IX_Shelves_sectionid",
                table: "Shelves",
                column: "sectionid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Borrowings");

            migrationBuilder.DropTable(
                name: "Shelves");

            migrationBuilder.DropTable(
                name: "Readers");

            migrationBuilder.DropTable(
                name: "Books");

            migrationBuilder.DropTable(
                name: "BookCases");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "Publishers");

            migrationBuilder.DropTable(
                name: "Sections");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
