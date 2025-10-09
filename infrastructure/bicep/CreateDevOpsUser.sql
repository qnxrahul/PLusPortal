-- Grant permissions to the Azure DevOps Service Connection
DECLARE @ADOUser varchar(100) = 'ROCKITSpecialists-ROCK ATS-509bd417-90f3-4522-9e5c-acca191c99c9';

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = @ADOUser)
BEGIN
    -- User does not exist, so create the user
    DECLARE @Sql NVARCHAR(MAX);
    SET @Sql = 'CREATE USER ' + QUOTENAME(@ADOUser) + ' FROM EXTERNAL PROVIDER;';
    EXEC sp_executesql @Sql;

    -- Assign necessary roles
    SET @Sql = 'ALTER ROLE db_datareader ADD MEMBER ' + QUOTENAME(@ADOUser) + ';';
    EXEC sp_executesql @Sql;

    SET @Sql = 'ALTER ROLE db_datawriter ADD MEMBER ' + QUOTENAME(@ADOUser) + ';';
    EXEC sp_executesql @Sql;

    SET @Sql = 'ALTER ROLE db_ddladmin ADD MEMBER ' + QUOTENAME(@ADOUser) + ';';
    EXEC sp_executesql @Sql;
END
