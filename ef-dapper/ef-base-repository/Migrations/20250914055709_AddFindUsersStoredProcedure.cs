using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ef_base_repository.Migrations
{
    /// <inheritdoc />
    public partial class AddFindUsersStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        CREATE PROCEDURE FindUsersByNameAndAge(
            IN p_firstName VARCHAR(100),
            IN p_minAge INT
        )
        BEGIN
            SELECT *
            FROM Users
            WHERE FirstName LIKE CONCAT('%', p_firstName, '%')
              AND Age >= p_minAge;
        END;
    ");
        }

       
    }
}
