DROP PROCEDURE IF EXISTS usp_GetAccount;
GO;

CREATE OR ALTER PROCEDURE usp_SelectAccount
    @AccountId INT
AS
BEGIN

    SELECT
        acc.AccountId,
        acc.AccountName,
        acc.InitialBalance,
        acc.CurrentBalance,
        acc.BalanceUpdatedAt,
        acc.CreatedAt,
        usr.Username AS UserName,
        acct.AccountTypeName
    FROM
        tbl_Account acc
        INNER JOIN
        tbl_User usr
            ON acc.UserId = usr.UserId
        INNER JOIN
        tbl_AccountType acct
            ON acc.AccountTypeId = acct.AccountTypeId
    WHERE acc.AccountId = @AccountId;

END
