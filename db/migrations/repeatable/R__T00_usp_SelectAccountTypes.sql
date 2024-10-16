CREATE OR ALTER PROCEDURE usp_SelectAccountTypes
AS
BEGIN

    SELECT
        AccountTypeId,
        AccountTypeName,
        BaseFee,
        PaymentDueDays
    FROM
        tbl_AccountType;

END
