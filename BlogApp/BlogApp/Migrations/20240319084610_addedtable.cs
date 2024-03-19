using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogApp.Migrations
{
    public partial class addedtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blogs_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CommentAddedUserId = table.Column<int>(type: "int", nullable: true),
                    BlogId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Blogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "Blogs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_User_CommentAddedUserId",
                        column: x => x.CommentAddedUserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "Id", "CreatedTime", "Email", "ImageUrl", "Name", "Password" },
                values: new object[] { 1, new DateTime(2024, 3, 19, 14, 16, 10, 426, DateTimeKind.Local).AddTicks(3187), "rk9.kumar21@gmail.com", "https://pluspng.com/img-png/user-png-icon-young-user-icon-2400.png", "Rakesh", "raka@123" });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "Author", "CreatedBy", "CreatedTime", "Description", "ImageUrl", "Title", "UserId" },
                values: new object[] { 1, "Rakesh", "rk9.kumar21@gmail.com", new DateTime(2024, 3, 19, 8, 46, 10, 426, DateTimeKind.Utc).AddTicks(3296), "This is my first blog", "https://www.thewowstyle.com/wp-content/uploads/2015/01/images-of-nature-4.jpg", "First Blog", 1 });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "Author", "CreatedBy", "CreatedTime", "Description", "ImageUrl", "Title", "UserId" },
                values: new object[] { 2, "Rakesh", "rk9.kumar21@gmail.com", new DateTime(2024, 3, 19, 8, 46, 10, 426, DateTimeKind.Utc).AddTicks(3297), "This is my second blog", "https://www.thewowstyle.com/wp-content/uploads/2015/01/images-of-nature-4.jpg", "Second Blog", 1 });

            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "Id", "Author", "CreatedBy", "CreatedTime", "Description", "ImageUrl", "Title", "UserId" },
                values: new object[] { 3, "Rakesh", "rk9.kumar21@gmail.com", new DateTime(2024, 3, 19, 8, 46, 10, 426, DateTimeKind.Utc).AddTicks(3298), "This is my third blog", "https://www.thewowstyle.com/wp-content/uploads/2015/01/images-of-nature-4.jpg", "Third Blog", 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UserId",
                table: "Blogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BlogId",
                table: "Comments",
                column: "BlogId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_CommentAddedUserId",
                table: "Comments",
                column: "CommentAddedUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Blogs");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
