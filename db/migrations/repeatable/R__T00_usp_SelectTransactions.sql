CREATE OR ALTER PROCEDURE usp_SelectTransactions
    @UserId INT = NULL,
    @AccountId INT = NULL,
    @TransactionType BIT = NULL,
    @AmountFrom DECIMAL(18, 2) = NULL,
    @AmountTo DECIMAL(18, 2) = NULL,
    @TransactionDateSince DATETIME = NULL,
    @TransactionDateUntil DATETIME = NULL,
    @ProjectedRepaymentDateSince DATETIME = NULL,
    @ProjectedRepaymentDateUntil DATETIME = NULL
AS
BEGIN

    SELECT
        TransactionId,
        UserId,
        AccountId,
        TransactionType,
        Amount,
        Description,
        TransactionDate,
        TransactionFee,
        ProjectedRepaymentDate
    FROM
        tbl_Transaction
    WHERE
        (@UserId IS NULL OR UserId = @UserId) AND
        (@AccountId IS NULL OR AccountId = @AccountId) AND
        (@TransactionType IS NULL OR TransactionType = @TransactionType) AND
        (@AmountFrom IS NULL OR Amount >= @AmountFrom) AND
        (@AmountTo IS NULL OR Amount <= @AmountTo) AND
        (@TransactionDateSince IS NULL OR TransactionDate >= @TransactionDateSince) AND
        (@TransactionDateUntil IS NULL OR TransactionDate <= @TransactionDateUntil) AND
        (@ProjectedRepaymentDateSince IS NULL OR ProjectedRepaymentDate >= @ProjectedRepaymentDateSince) AND
        (@ProjectedRepaymentDateUntil IS NULL OR ProjectedRepaymentDate <= @ProjectedRepaymentDateUntil);

END
