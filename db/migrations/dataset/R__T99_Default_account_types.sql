EXEC usp_EnsureAccountType @AccountTypeName = 'Cash';
EXEC usp_EnsureAccountType @AccountTypeName = 'Bello Debit', @BaseFee = 1.75, @PaymentDueDays = 2;
EXEC usp_EnsureAccountType @AccountTypeName = 'Bello Credit', @BaseFee = 2.50, @PaymentDueDays = 12;
EXEC usp_EnsureAccountType @AccountTypeName = 'NetCard Debit', @BaseFee = 1.15, @PaymentDueDays = 3;
EXEC usp_EnsureAccountType @AccountTypeName = 'NetCard Credit', @BaseFee = 4.00, @PaymentDueDays = 6;
