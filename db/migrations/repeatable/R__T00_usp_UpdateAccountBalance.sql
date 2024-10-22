CREATE
OR
ALTER PROCEDURE usp_UpdateAccount
    @AccountId INT,
    @AdjustedAmount DECIMAL
    AS
BEGIN

UPDATE tbl_Account
SET CurrentBalance   = CurrentBalance + @AdjustedAmount,
    BalanceUpdatedAt = GETDATE()
WHERE AccountId = @AccountId;

END
