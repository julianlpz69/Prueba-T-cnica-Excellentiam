using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AcademyApp.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTriggersToCourseAndStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Trigger para Courses
            migrationBuilder.Sql(@"
                CREATE TRIGGER trg_Courses_Update
                ON Courses
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;
                    UPDATE Courses
                    SET UpdatedAtUtc = GETUTCDATE()
                    WHERE Id IN (SELECT Id FROM Inserted);
                END
            ");

                    // Trigger para Students
                    migrationBuilder.Sql(@"
                CREATE TRIGGER trg_Students_Update
                ON Students
                AFTER UPDATE
                AS
                BEGIN
                    SET NOCOUNT ON;
                    UPDATE Students
                    SET UpdatedAtUtc = GETUTCDATE()
                    WHERE Id IN (SELECT Id FROM Inserted);
                END
            ");
                }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Courses_Update;");
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS trg_Students_Update;");
        }

    }
}
