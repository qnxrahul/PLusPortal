DECLARE @AppName varchar(100) = 'rockats-webapp-neu-dev';

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = @AppName)
BEGIN
    -- User does not exist, so create the user
    DECLARE @Sql NVARCHAR(MAX);
    SET @Sql = 'CREATE USER ' + QUOTENAME(@AppName) + ' FROM EXTERNAL PROVIDER;';
    EXEC sp_executesql @Sql;

    -- Assign necessary roles
    SET @Sql = 'ALTER ROLE db_datareader ADD MEMBER ' + QUOTENAME(@AppName) + ';';
    EXEC sp_executesql @Sql;

    SET @Sql = 'ALTER ROLE db_datawriter ADD MEMBER ' + QUOTENAME(@AppName) + ';';
    EXEC sp_executesql @Sql;

    SET @Sql = 'ALTER ROLE db_ddladmin ADD MEMBER ' + QUOTENAME(@AppName) + ';';
    EXEC sp_executesql @Sql;
END
