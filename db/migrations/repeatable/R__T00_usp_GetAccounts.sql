CREATE OR ALTER PROCEDURE usp_GetAccounts
    @AccountName VARCHAR(100) = NULL,
    @UserId INT = NULL,
    @AccountTypeId INT = NULL,
    @InitialBalanceFrom DECIMAL(18, 2) = NULL,
    @InitialBalanceTo DECIMAL(18, 2) = NULL,
    @CurrentBalanceFrom DECIMAL(18, 2) = NULL,
    @CurrentBalanceTo DECIMAL(18, 2) = NULL,
    @CreatedSince DATETIME = NULL,
    @CreatedUntil DATETIME = NULL
AS
BEGIN

    SELECT
        acc.AccountId,
        acc.AccountName,
        acc.InitialBalance,
        acc.CurrentBalance,
        acc.BalanceUpdatedAt,
        acc.CreatedAt,
        usr.Username,
        acct.AccountTypeName
    FROM
        tbl_Account acc
        INNER JOIN
        tbl_User usr
            ON acc.UserId = usr.UserId
        INNER JOIN
        tbl_AccountType acct
            ON acc.AccountTypeId = acct.AccountTypeId
    WHERE
        (@AccountName IS NULL OR acc.AccountName LIKE '%' + @AccountName + '%') AND
        (@UserId IS NULL OR acc.UserId = @UserId) AND
        (@AccountTypeId IS NULL OR acc.AccountTypeId = @AccountTypeId) AND
        (@InitialBalanceFrom IS NULL OR acc.InitialBalance >= @InitialBalanceFrom) AND
        (@InitialBalanceTo IS NULL OR acc.InitialBalance <= @InitialBalanceTo) AND
        (@CurrentBalanceFrom IS NULL OR acc.CurrentBalance >= @CurrentBalanceFrom) AND
        (@CurrentBalanceTo IS NULL OR acc.CurrentBalance <= @CurrentBalanceTo) AND
        (@CreatedSince IS NULL OR acc.CreatedAt >= @CreatedSince) AND
        (@CreatedUntil IS NULL OR acc.CreatedAt <= @CreatedUntil);

END
