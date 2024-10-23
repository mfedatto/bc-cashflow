using System.Diagnostics.CodeAnalysis;
using Bc.CashFlow.Business;
using Bc.CashFlow.Domain.Account;
using Bc.CashFlow.Domain.AccountType;
using Bc.CashFlow.Domain.Transaction;
using Bc.CashFlow.Domain.User;
using Microsoft.Extensions.Logging;
using Moq;

namespace Bc.CashFlow.BusinessTests;

[TestFixture]
[SuppressMessage("ReSharper", "RedundantEmptyObjectOrCollectionInitializer")]
public class TransactionBusinessTests
{
	private Mock<ILogger<TransactionBusiness>> _logger;
	private Mock<ITransactionService> _transactionServiceMock;
	private Mock<IAccountService> _accountServiceMock;
	private Mock<IAccountTypeService> _accountTypeServiceMock;
	private Mock<IUserService> _userServiceMock;
	private TransactionBusiness _transactionBusiness;

	[SetUp]
	public void Setup()
	{
		_logger = new();
		_transactionServiceMock = new();
		_accountServiceMock = new();
		_accountTypeServiceMock = new();
		_userServiceMock = new();
		_transactionBusiness = new(
			_logger.Object,
			_transactionServiceMock.Object,
			_accountServiceMock.Object,
			_accountTypeServiceMock.Object,
			_userServiceMock.Object);
	}

	public static IEnumerable<TestCaseData> TransactionBusinessGetTransactionFeeSuccessCases
	{
		get
		{
			yield return new(0.01m, TransactionType.Debit, 100m, 0m);
			yield return new(0.02m, TransactionType.Debit, 100m, 0m);
			yield return new(0.005m, TransactionType.Debit, 200m, 0m);
			yield return new(0.01m, TransactionType.Debit, 200m, 0m);
			yield return new(0.01m, TransactionType.Credit, 100m, 1m);
			yield return new(0.02m, TransactionType.Credit, 100m, 2m);
			yield return new(0.005m, TransactionType.Credit, 200m, 1m);
			yield return new(0.01m, TransactionType.Credit, 200m, 2m);
			yield return new(0.01m, 0, 100m, 0m);
			yield return new(0.02m, 0, 100m, 0m);
			yield return new(0.005m, 0, 200m, 0m);
			yield return new(0.01m, 0, 200m, 0m);
			yield return new(0.01m, 1, 100m, 1m);
			yield return new(0.02m, 1, 100m, 2m);
			yield return new(0.005m, 1, 200m, 1m);
			yield return new(0.01m, 1, 200m, 2m);
		}
	}

	[TestCaseSource(nameof(TransactionBusinessGetTransactionFeeSuccessCases))]
	public void GivenTransactionBusiness_WhenGetTransactionFee_ThenReturnsExpectedFee(
		decimal baseFee,
		TransactionType transactionType,
		decimal amount,
		decimal expected)
	{
		// Arrange
		Mock<IAccountType> accountTypeMock = new();

		accountTypeMock
			.Setup(at => at.BaseFee)
			.Returns(baseFee);

		IAccountType accountType = accountTypeMock.Object;

		// Act
		decimal actualFee = TransactionBusiness.GetTransactionFee(
			accountType,
			transactionType,
			amount);

		// Assert
		Assert.That(actualFee, Is.EqualTo(expected));
	}

	public static IEnumerable<TestCaseData> TransactionBusinessGetTransactionFeeInvalidTransactionTypeCases
	{
		get
		{
			yield return new(0.005m, -1, 200m);
			yield return new(0.01m, 3, 200m);
		}
	}

	[TestCaseSource(nameof(TransactionBusinessGetTransactionFeeInvalidTransactionTypeCases))]
	public void GivenTransactionBusiness_WhenGetTransactionFee_ThenThrowsTransactionTypeOutOfRangeException(
		decimal baseFee,
		TransactionType transactionType,
		decimal amount)
	{
		// Arrange
		Mock<IAccountType> accountTypeMock = new();

		accountTypeMock
			.Setup(at => at.BaseFee)
			.Returns(baseFee);

		IAccountType accountType = accountTypeMock.Object;

		// Assert
		Assert.Throws<TransactionTypeOutOfRangeException>(
			() =>
			{
				// Act
				_ = TransactionBusiness.GetTransactionFee(
					accountType,
					transactionType,
					amount);
			});
	}

	public static IEnumerable<TestCaseData> TransactionBusinessGetProjectedRepaymentDateSuccessCases
	{
		get
		{
			yield return new(1, new DateTime(2024, 10, 1), new DateTime(2024, 10, 2));
			yield return new(3, new DateTime(2024, 10, 1), new DateTime(2024, 10, 4));
			yield return new(5, new DateTime(2024, 10, 1), new DateTime(2024, 10, 6));
			yield return new(1, new DateTime(2023, 12, 31), new DateTime(2024, 1, 1));
			yield return new(0, new DateTime(2024, 10, 1), new DateTime(2024, 10, 1));
			yield return new(5, new DateTime(2024, 1, 28), new DateTime(2024, 2, 2));
			yield return new(5, new DateTime(2024, 2, 25), new DateTime(2024, 3, 1));
			yield return new(5, new DateTime(2023, 2, 25), new DateTime(2023, 3, 2));
		}
	}

	[TestCaseSource(nameof(TransactionBusinessGetProjectedRepaymentDateSuccessCases))]
	public void GivenTransactionBusiness_WhenGetProjectedRepaymentDate_ThenReturnsExpectedProjectedRepaymentDate(
		int paymentDueDays,
		DateTime transactionDate,
		DateTime expected)
	{
		// Arrange
		Mock<IAccountType> accountTypeMock = new();

		accountTypeMock
			.Setup(at => at.PaymentDueDays)
			.Returns(paymentDueDays);

		IAccountType accountType = accountTypeMock.Object;

		// Act
		DateTime actual = TransactionBusiness.GetProjectedRepaymentDate(
			accountType,
			transactionDate);

		// Assert
		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void GivenTransactionBusiness_WhenGetProjectedRepaymentDate_ThenThrowsNegativePaymentDueDaysException()
	{
		// Arrange
		Mock<IAccountType> accountTypeMock = new();

		accountTypeMock
			.Setup(at => at.PaymentDueDays)
			.Returns(-1);

		IAccountType accountType = accountTypeMock.Object;

		// Assert
		Assert.Throws<NegativePaymentDueDaysException>(
			() =>
			{
				// Act
				_ = TransactionBusiness.GetProjectedRepaymentDate(
					accountType,
					DateTime.Now);
			});
	}

	public static IEnumerable<TestCaseData> TransactionBusinessGetAdjustedAmountSuccessCases
	{
		get
		{
			yield return new(TransactionType.Debit, 100m, null, -100m);
			yield return new(TransactionType.Debit, 100m, null, -100m);
			yield return new(TransactionType.Debit, 100m, 100m, -100m);
			yield return new(TransactionType.Credit, 100m, null, 100m);
			yield return new(TransactionType.Credit, 100m, 10m, 90m);
			yield return new(0, 100m, null, -100m);
			yield return new(0, 100m, null, -100m);
			yield return new(0, 100m, 100m, -100m);
			yield return new(1, 100m, null, 100m);
			yield return new(1, 100m, 10m, 90m);
		}
	}

	[TestCaseSource(nameof(TransactionBusinessGetAdjustedAmountSuccessCases))]
	public void GivenTransactionBusiness_WhenGetAdjustedAmount_ThenReturnsExpectedAdjustedAmount(
		TransactionType transactionType,
		decimal amount,
		decimal? transactionFee,
		decimal expected)
	{
		// Arrange
		Mock<ITransaction> transactionMock = new();

		transactionMock
			.Setup(t => t.TransactionType)
			.Returns(transactionType);
		transactionMock
			.Setup(t => t.Amount)
			.Returns(amount);
		transactionMock
			.Setup(t => t.TransactionFee)
			.Returns(transactionFee);

		ITransaction transaction = transactionMock.Object;

		// Act
		decimal actual = TransactionBusiness
			.GetAdjustedAmount(transaction);

		// Assert
		Assert.That(actual, Is.EqualTo(expected));
	}

	[Test]
	public void GivenTransactionBusiness_WhenGetAdjustedAmount_ThenThrowsTransactionTypeOutOfRangeException()
	{
		// Arrange
		Mock<ITransaction> transactionMock = new();
		const TransactionType transactionType = (TransactionType)(-1);

		transactionMock
			.Setup(t => t.TransactionType)
			.Returns(transactionType);

		ITransaction transaction = transactionMock.Object;

		// Assert
		Assert.Throws<TransactionTypeOutOfRangeException>(
			() =>
			{
				// Act
				_ = TransactionBusiness
					.GetAdjustedAmount(transaction);
			});
	}

	public static IEnumerable<TestCaseData> TransactionBusinessGetTransactionsSuccessCases
	{
		get
		{
			yield return new(null, null, null, null, null, null, null, null, null, null);
			yield return new(null, null, null, null, null, null, null, null, null, new List<ITransaction>());
			yield return new(null, null, null, null, null, null, null, null, null, new List<ITransaction> { });
			yield return new(null, null, null, null, null, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new(null, null, null, null, null, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });

			yield return new((int?)1, null, null, null, null, null, null, null, null, null);
			yield return new((int?)2, null, null, null, null, null, null, null, null, new List<ITransaction>());
			yield return new((int?)3, null, null, null, null, null, null, null, null, new List<ITransaction> { });
			yield return new((int?)4, null, null, null, null, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new((int?)5, null, null, null, null, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });

			yield return new((int?)1, (int?)11, null, null, null,  null, null, null, null, null);
			yield return new((int?)2, (int?)12, null, null, null,  null, null, null, null, new List<ITransaction>());
			yield return new((int?)3, (int?)13, null, null, null,  null, null, null, null, new List<ITransaction> { });
			yield return new((int?)4, (int?)14, null, null, null,  null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new((int?)5, (int?)15, null, null, null,  null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });

			yield return new((int?)1, (int?)11, TransactionType.Debit, null, null, null, null, null, null, null);
			yield return new((int?)2, (int?)12, TransactionType.Debit, null, null, null, null, null, null, new List<ITransaction>());
			yield return new((int?)3, (int?)13, TransactionType.Debit, null, null, null, null, null, null, new List<ITransaction> { });
			yield return new((int?)4, (int?)14, TransactionType.Debit, null, null, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new((int?)5, (int?)15, TransactionType.Debit, null, null, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });

			yield return new((int?)1, (int?)11, TransactionType.Credit, null, null, null, null, null, null, null);
			yield return new((int?)2, (int?)12, TransactionType.Credit, null, null, null, null, null, null, new List<ITransaction>());
			yield return new((int?)3, (int?)13, TransactionType.Credit, null, null, null, null, null, null, new List<ITransaction> { });
			yield return new((int?)4, (int?)14, TransactionType.Credit, null, null, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new((int?)5, (int?)15, TransactionType.Credit, null, null, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });

			yield return new((int?)1, (int?)11, (TransactionType?)0, null, null, null, null, null, null, null);
			yield return new((int?)2, (int?)12, (TransactionType?)0, null, null, null, null, null, null, new List<ITransaction>());
			yield return new((int?)3, (int?)13, (TransactionType?)0, null, null, null, null, null, null, new List<ITransaction> { });
			yield return new((int?)4, (int?)14, (TransactionType?)0, null, null, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new((int?)5, (int?)15, (TransactionType?)0, null, null, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });

			yield return new((int?)1, (int?)11, (TransactionType?)1, null, null, null, null, null, null, null);
			yield return new((int?)2, (int?)12, (TransactionType?)1, null, null, null, null, null, null, new List<ITransaction>());
			yield return new((int?)3, (int?)13, (TransactionType?)1, null, null, null, null, null, null, new List<ITransaction> { });
			yield return new((int?)4, (int?)14, (TransactionType?)1, null, null, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new((int?)5, (int?)15, (TransactionType?)1, null, null, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });

			yield return new((int?)1, (int?)11, (TransactionType?)1, (decimal?)101, null, null, null, null, null, null);
			yield return new((int?)2, (int?)12, (TransactionType?)1, (decimal?)102, null, null, null, null, null, new List<ITransaction>());
			yield return new((int?)3, (int?)13, (TransactionType?)1, (decimal?)103, null, null, null, null, null, new List<ITransaction> { });
			yield return new((int?)4, (int?)14, (TransactionType?)1, (decimal?)104, null, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new((int?)5, (int?)15, (TransactionType?)1, (decimal?)105, null, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });

			yield return new((int?)1, (int?)11, (TransactionType?)0, (decimal?)101, (decimal?)201, null, null, null, null, null);
			yield return new((int?)2, (int?)12, (TransactionType?)0, (decimal?)102, (decimal?)202, null, null, null, null, new List<ITransaction>());
			yield return new((int?)3, (int?)13, (TransactionType?)0, (decimal?)103, (decimal?)203, null, null, null, null, new List<ITransaction> { });
			yield return new((int?)4, (int?)14, (TransactionType?)0, (decimal?)104, (decimal?)204, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new((int?)5, (int?)15, (TransactionType?)0, (decimal?)105, (decimal?)205, null, null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });

			yield return new((int?)1, (int?)11, null, (decimal?)101, (decimal?)201, DateTime.Now.AddDays(1), null, null, null, null);
			yield return new((int?)2, (int?)12, null, (decimal?)102, (decimal?)202, DateTime.Now.AddDays(2), null, null, null, new List<ITransaction>());
			yield return new((int?)3, (int?)13, null, (decimal?)103, (decimal?)203, DateTime.Now.AddDays(3), null, null, null, new List<ITransaction> { });
			yield return new((int?)4, (int?)14, null, (decimal?)104, (decimal?)204, DateTime.Now.AddDays(4), null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new((int?)5, (int?)15, null, (decimal?)105, (decimal?)205, DateTime.Now.AddDays(5), null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });

			yield return new((int?)1, (int?)11, TransactionType.Credit, (decimal?)101m, (decimal?)201m, DateTime.Now.AddDays(1), DateTime.Now.AddDays(11), null, null, null);
			yield return new((int?)2, (int?)12, TransactionType.Credit, (decimal?)102m, (decimal?)202m, DateTime.Now.AddDays(2), DateTime.Now.AddDays(12), null, null, new List<ITransaction>());
			yield return new((int?)3, (int?)13, TransactionType.Credit, (decimal?)103m, (decimal?)203m, DateTime.Now.AddDays(3), DateTime.Now.AddDays(13), null, null, new List<ITransaction> { });
			yield return new((int?)4, (int?)14, TransactionType.Credit, (decimal?)104m, (decimal?)204m, DateTime.Now.AddDays(4), DateTime.Now.AddDays(14), null, null, new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new((int?)5, (int?)15, TransactionType.Credit, (decimal?)105m, (decimal?)205m, DateTime.Now.AddDays(5), DateTime.Now.AddDays(15), null, null, new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });

			yield return new((int?)1, (int?)11, null, null, null, DateTime.Now.AddDays(1), null, null, null, null);
			yield return new((int?)2, (int?)12, null, null, null, DateTime.Now.AddDays(2), null, null, null, new List<ITransaction>());
			yield return new((int?)3, (int?)13, null, null, null, DateTime.Now.AddDays(3), null, null, null, new List<ITransaction> { });
			yield return new((int?)4, (int?)14, null, null, null, DateTime.Now.AddDays(4), null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new((int?)5, (int?)15, null, null, null, DateTime.Now.AddDays(5), null, null, null, new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });

			yield return new(null, (int?)11, (TransactionType?)1, null, (decimal?)101m, DateTime.Now.AddDays(1), DateTime.Now.AddDays(11), null, null, null);
			yield return new(null, (int?)12, (TransactionType?)1, null, (decimal?)102m, DateTime.Now.AddDays(2), DateTime.Now.AddDays(12), null, null, new List<ITransaction>());
			yield return new(null, (int?)13, (TransactionType?)1, null, (decimal?)103m, DateTime.Now.AddDays(3), DateTime.Now.AddDays(13), null, null, new List<ITransaction> { });
			yield return new(null, (int?)14, (TransactionType?)1, null, (decimal?)104m, DateTime.Now.AddDays(4), DateTime.Now.AddDays(14), null, null, new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new(null, (int?)15, (TransactionType?)1, null, (decimal?)105m, DateTime.Now.AddDays(5), DateTime.Now.AddDays(15), null, null, new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });

			yield return new(null, (int?)11, TransactionType.Debit, (decimal?)101m, null, DateTime.Now.AddDays(1), DateTime.Now.AddDays(11), DateTime.Now.AddDays(1001), null, null);
			yield return new(null, (int?)12, TransactionType.Debit, (decimal?)102m, null, DateTime.Now.AddDays(2), DateTime.Now.AddDays(12), DateTime.Now.AddDays(1002), null, new List<ITransaction>());
			yield return new(null, (int?)13, TransactionType.Debit, (decimal?)103m, null, DateTime.Now.AddDays(3), DateTime.Now.AddDays(13), DateTime.Now.AddDays(1003), null, new List<ITransaction> { });
			yield return new(null, (int?)14, TransactionType.Debit, (decimal?)104m, null, DateTime.Now.AddDays(4), DateTime.Now.AddDays(14), DateTime.Now.AddDays(1004), null, new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new(null, (int?)15, TransactionType.Debit, (decimal?)105m, null, DateTime.Now.AddDays(5), DateTime.Now.AddDays(15), DateTime.Now.AddDays(1005), null, new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });

			yield return new(null, (int?)11, null, null, (decimal?)101m, DateTime.Now.AddDays(1), DateTime.Now.AddDays(11), DateTime.Now.AddDays(1001), DateTime.Now.AddDays(2001), null);
			yield return new(null, (int?)12, null, null, (decimal?)102m, DateTime.Now.AddDays(2), DateTime.Now.AddDays(12), DateTime.Now.AddDays(1002), DateTime.Now.AddDays(2002), new List<ITransaction>());
			yield return new(null, (int?)13, null, null, (decimal?)103m, DateTime.Now.AddDays(3), DateTime.Now.AddDays(13), DateTime.Now.AddDays(1003), DateTime.Now.AddDays(2003), new List<ITransaction> { });
			yield return new(null, (int?)14, null, null, (decimal?)104m, DateTime.Now.AddDays(4), DateTime.Now.AddDays(14), DateTime.Now.AddDays(1004), DateTime.Now.AddDays(2004), new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new(null, (int?)15, null, null, (decimal?)105m, DateTime.Now.AddDays(5), DateTime.Now.AddDays(15), DateTime.Now.AddDays(1005), DateTime.Now.AddDays(2005), new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });

			yield return new(null, null, null, null, (decimal?)101m, DateTime.Now.AddDays(1), DateTime.Now.AddDays(11), null, DateTime.Now.AddDays(2001), null);
			yield return new(null, null, null, null, (decimal?)102m, DateTime.Now.AddDays(2), DateTime.Now.AddDays(12), null, DateTime.Now.AddDays(2002), new List<ITransaction>());
			yield return new(null, null, null, null, (decimal?)103m, DateTime.Now.AddDays(3), DateTime.Now.AddDays(13), null, DateTime.Now.AddDays(2003), new List<ITransaction> { });
			yield return new(null, null, null, null, (decimal?)104m, DateTime.Now.AddDays(4), DateTime.Now.AddDays(14), null, DateTime.Now.AddDays(2004), new List<ITransaction> { new Mock<ITransaction>().Object });
			yield return new(null, null, null, null, (decimal?)105m, DateTime.Now.AddDays(5), DateTime.Now.AddDays(15), null, DateTime.Now.AddDays(2005), new List<ITransaction> { new Mock<ITransaction>().Object, new Mock<ITransaction>().Object });
		}
	}

	[TestCaseSource(nameof(TransactionBusinessGetTransactionsSuccessCases))]
	public async Task GivenTransactionBusiness_WhenGetTransactions_ThenReturnsExpectedTransactions(
		int? userId,
		int? accountId,
		TransactionType? transactionType,
		decimal? amountFrom,
		decimal? amountTo,
		DateTime? transactionDateSince,
		DateTime? transactionDateUntil,
		DateTime? projectedRepaymentDateSince,
		DateTime? projectedRepaymentDateUntil,
		IEnumerable<ITransaction> expected)
	{
		// Arrange
		_transactionServiceMock
			.Setup(
				ts =>
					ts.GetTransactions(
						userId,
						accountId,
						transactionType,
						amountFrom,
						amountTo,
						transactionDateSince,
						transactionDateUntil,
						projectedRepaymentDateSince,
						projectedRepaymentDateUntil,
						It.IsAny<CancellationToken>()))
			.ReturnsAsync(expected);

		// Act
		IEnumerable<ITransaction> actual =
			await _transactionBusiness.GetTransactions(
				userId,
				accountId,
				transactionType,
				amountFrom,
				amountTo,
				transactionDateSince,
				transactionDateUntil,
				projectedRepaymentDateSince,
				projectedRepaymentDateUntil,
				CancellationToken.None);

		// Assert
		Assert.Multiple(
			() =>
			{
				Assert.That(actual, Is.EqualTo(expected));
				_transactionServiceMock.Verify(
					ts =>
						ts.GetTransactions(
							userId,
							accountId,
							transactionType,
							amountFrom,
							amountTo,
							transactionDateSince,
							transactionDateUntil,
							projectedRepaymentDateSince,
							projectedRepaymentDateUntil,
							It.IsAny<CancellationToken>()),
					Times.Once);
			});
	}
}
