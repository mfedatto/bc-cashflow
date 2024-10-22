IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = '${hangfire_db_username}')
BEGIN

    CREATE
        LOGIN [${hangfire_db_username}]
    WITH
        PASSWORD = '${hangfire_db_password}',
        CHECK_POLICY = OFF;

END

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = '${hangfire_db_username}')
BEGIN

    CREATE USER
        [${hangfire_db_username}]
    FOR
        LOGIN [${hangfire_db_username}];

END

ALTER
    LOGIN [${hangfire_db_username}]
WITH
    PASSWORD = '${hangfire_db_password}',
    CHECK_POLICY = OFF;

GO;
