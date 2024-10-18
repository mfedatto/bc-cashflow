DROP PROCEDURE IF EXISTS usp_GetAccount;
GO;

CREATE OR ALTER PROCEDURE usp_SelectAccounts
    @AccountName VARCHAR(100) = NULL,
    @UserId INT = NULL,
    @AccountTypeId INT = NULL,
    @InitialBalanceFrom DECIMAL(18, 2) = NULL,
    @InitialBalanceTo DECIMAL(18, 2) = NULL,
    @CurrentBalanceFrom DECIMAL(18, 2) = NULL,
    @CurrentBalanceTo DECIMAL(18, 2) = NULL,
    @BalanceUpdatedSince DATETIME = NULL,
    @BalanceUpdatedUntil DATETIME = NULL,
    @CreatedSince DATETIME = NULL,
    @CreatedUntil DATETIME = NULL
AS
BEGIN

    SELECT
        AccountId
    FROM
        tbl_Account
    WHERE
        (@AccountName IS NULL OR AccountName LIKE '%' + @AccountName + '%') AND
        (@UserId IS NULL OR UserId = @UserId) AND
        (@AccountTypeId IS NULL OR AccountTypeId = @AccountTypeId) AND
        (@InitialBalanceFrom IS NULL OR InitialBalance >= @InitialBalanceFrom) AND
        (@InitialBalanceTo IS NULL OR InitialBalance <= @InitialBalanceTo) AND
        (@CurrentBalanceFrom IS NULL OR CurrentBalance >= @CurrentBalanceFrom) AND
        (@CurrentBalanceTo IS NULL OR CurrentBalance <= @CurrentBalanceTo) AND
        (@BalanceUpdatedSince IS NULL OR BalanceUpdatedAt >= @BalanceUpdatedSince) AND
        (@BalanceUpdatedUntil IS NULL OR BalanceUpdatedAt <= @BalanceUpdatedUntil) AND
        (@CreatedSince IS NULL OR CreatedAt >= @CreatedSince) AND
        (@CreatedUntil IS NULL OR CreatedAt <= @CreatedUntil);

END
