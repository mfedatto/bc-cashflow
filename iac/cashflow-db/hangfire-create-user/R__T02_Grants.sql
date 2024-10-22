GRANT CREATE TABLE TO [${hangfire_db_username}];
GRANT CREATE VIEW TO [${hangfire_db_username}];
GRANT INSERT, UPDATE, DELETE, SELECT TO [${hangfire_db_username}];
GRANT ALTER TO [${hangfire_db_username}];
GRANT EXECUTE TO [${hangfire_db_username}];
GRANT REFERENCES TO [${hangfire_db_username}];
GO;
