CREATE OR ALTER PROCEDURE usp_EnsureAccountType
    @AccountTypeName VARCHAR(50),
    @BaseFee DECIMAL(10, 2) = 0,
    @PaymentDueDays INT = 0
AS
BEGIN

    IF NOT EXISTS (SELECT 1 FROM tbl_AccountType WHERE AccountTypeName = @AccountTypeName)
    BEGIN
        INSERT INTO tbl_AccountType (
            AccountTypeName,
            BaseFee,
            PaymentDueDays)
        VALUES (
            @AccountTypeName,
            @BaseFee,
            @PaymentDueDays);
    END

END
