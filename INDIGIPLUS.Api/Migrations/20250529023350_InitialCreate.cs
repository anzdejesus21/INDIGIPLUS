using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace INDIGIPLUS.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quizzes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quizzes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quizzes_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Questions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuestionText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Questions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Questions_Quizzes_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quizzes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCorrect = table.Column<bool>(type: "bit", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answers_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Lessons",
                columns: new[] { "Id", "Content", "CreatedAt", "Description", "IsActive", "OrderIndex", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, "C++ is a general-purpose programming language...", new DateTime(2025, 5, 29, 2, 33, 49, 914, DateTimeKind.Utc).AddTicks(8695), "Basic concepts of C++ programming", true, 1, "Introduction to C++", new DateTime(2025, 5, 29, 2, 33, 49, 914, DateTimeKind.Utc).AddTicks(8698) },
                    { 2, "In C++, variables are containers for storing data values...", new DateTime(2025, 5, 29, 2, 33, 49, 915, DateTimeKind.Utc).AddTicks(3), "Understanding C++ variables and data types", true, 2, "Variables and Data Types", new DateTime(2025, 5, 29, 2, 33, 49, 915, DateTimeKind.Utc).AddTicks(3) },
                    { 3, "Control structures allow you to control the flow of execution...", new DateTime(2025, 5, 29, 2, 33, 49, 915, DateTimeKind.Utc).AddTicks(8), "Loops and conditional statements in C++", true, 3, "Control Structures", new DateTime(2025, 5, 29, 2, 33, 49, 915, DateTimeKind.Utc).AddTicks(9) }
                });

            migrationBuilder.InsertData(
                table: "Quizzes",
                columns: new[] { "Id", "CreatedAt", "Description", "IsActive", "LessonId", "Title", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 29, 2, 33, 49, 915, DateTimeKind.Utc).AddTicks(6105), "Test your knowledge of C++ basics", true, 1, "C++ Basics Quiz", new DateTime(2025, 5, 29, 2, 33, 49, 915, DateTimeKind.Utc).AddTicks(6106) },
                    { 2, new DateTime(2025, 5, 29, 2, 33, 49, 915, DateTimeKind.Utc).AddTicks(7116), "Quiz about C++ variables and data types", true, 2, "Variables Quiz", new DateTime(2025, 5, 29, 2, 33, 49, 915, DateTimeKind.Utc).AddTicks(7116) }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CreatedAt", "OrderIndex", "QuestionText", "QuizId", "Type" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 5, 29, 2, 33, 49, 915, DateTimeKind.Utc).AddTicks(8172), 1, "What does C++ stand for?", 1, "MultipleChoice" },
                    { 2, new DateTime(2025, 5, 29, 2, 33, 49, 915, DateTimeKind.Utc).AddTicks(9339), 2, "C++ is an object-oriented programming language.", 1, "TrueFalse" },
                    { 3, new DateTime(2025, 5, 29, 2, 33, 49, 915, DateTimeKind.Utc).AddTicks(9342), 1, "Which data type is used to store whole numbers?", 2, "MultipleChoice" }
                });

            migrationBuilder.InsertData(
                table: "Answers",
                columns: new[] { "Id", "AnswerText", "IsCorrect", "QuestionId" },
                values: new object[,]
                {
                    { 1, "C Plus Plus", true, 1 },
                    { 2, "C Programming Plus", false, 1 },
                    { 3, "Computer Plus Plus", false, 1 },
                    { 4, "True", true, 2 },
                    { 5, "False", false, 2 },
                    { 6, "int", true, 3 },
                    { 7, "float", false, 3 },
                    { 8, "char", false, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answers_QuestionId",
                table: "Answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_OrderIndex",
                table: "Lessons",
                column: "OrderIndex");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_OrderIndex",
                table: "Questions",
                column: "OrderIndex");

            migrationBuilder.CreateIndex(
                name: "IX_Questions_QuizId",
                table: "Questions",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_Quizzes_LessonId",
                table: "Quizzes",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Questions");

            migrationBuilder.DropTable(
                name: "Quizzes");

            migrationBuilder.DropTable(
                name: "Lessons");
        }
    }
}
