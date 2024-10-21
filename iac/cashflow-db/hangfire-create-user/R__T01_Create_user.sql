IF NOT EXISTS (SELECT * FROM sys.server_principals WHERE name = '${bccf_db_username}')
BEGIN
    CREATE LOGIN [${bccf_db_username}] WITH PASSWORD = '${bccf_db_password}', CHECK_POLICY = OFF;
END

IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = '${bccf_db_username}')
BEGIN
    CREATE USER [${bccf_db_username}] FOR LOGIN [${bccf_db_username}];
END

ALTER LOGIN [${bccf_db_username}] WITH PASSWORD = '${bccf_db_password}', CHECK_POLICY = OFF;

GO;

GRANT CREATE TABLE TO [${bccf_db_username}];
GRANT CREATE VIEW TO [${bccf_db_username}];
GRANT INSERT, UPDATE, DELETE, SELECT TO [${bccf_db_username}];
GRANT ALTER TO [${bccf_db_username}];
GRANT EXECUTE TO [${bccf_db_username}];
GRANT REFERENCES TO [${bccf_db_username}];
GO;
