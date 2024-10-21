CREATE OR ALTER PROCEDURE usp_SelectAccountTypes @AccountTypeName VARCHAR(50) = NULL,
                                                 @BaseFeeFrom DECIMAL(10, 2) = NULL,
                                                 @BaseFeeTo DECIMAL(10, 2) = NULL,
                                                 @PaymentDueDaysFrom INT = NULL,
                                                 @PaymentDueDaysTo INT = NULL
AS
BEGIN

    SELECT AccountTypeId,
           AccountTypeName,
           BaseFee,
           PaymentDueDays
    FROM tbl_AccountType
    WHERE (@AccountTypeName IS NULL OR AccountTypeName LIKE '%' + @AccountTypeName + '%')
      AND (@BaseFeeFrom IS NULL OR BaseFee >= @BaseFeeFrom)
      AND (@BaseFeeTo IS NULL OR BaseFee <= @BaseFeeTo)
      AND (@PaymentDueDaysFrom IS NULL OR PaymentDueDays >= @PaymentDueDaysFrom)
      AND (@PaymentDueDaysTo IS NULL OR PaymentDueDays <= @PaymentDueDaysTo);

END
