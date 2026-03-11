namespace Persistence.Contracts.Migrations;

public interface IMigrationManager
{
    Task ApplyMigrationsAsync();
}