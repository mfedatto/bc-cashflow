using System.Diagnostics.CodeAnalysis;
using Bc.CashFlow.Business;
using Bc.CashFlow.Domain.Transaction;
using Moq;

namespace Bc.CashFlow.BusinessTests;

[TestFixture]
public class DailyReportBusinessTests
{
	[SetUp]
	public void Setup()
	{
	}

	#region TransactionsBalanceReport

	[SuppressMessage("Performance", "CA1861:Avoid constant arrays as arguments")]
	public static IEnumerable<TestCaseData> TransactionsBalanceReportSuccessCases
	{
		get
		{
			yield return new(
				new[] { 10.1m, 10.2m, 10.3m },
				new[] { 100.1m, 100.2m, 100.3m, 100.4m },
				new[] { 1.1m, 1.2m, 1.3m, 1.4m },
				30.6m,
				401.0m,
				5.0m,
				370.4m,
				365.4m);
			yield return new(
				new[] { 15.1m, 15.2m, 15.3m },
				new[] { 200.1m, 200.2m },
				new[] { 2.1m, 2.2m },
				45.6m,
				400.3m,
				4.3m,
				354.7m,
				350.4m);
			yield return new(
				new[] { 9.9m },
				new[] { 90.1m, 90.2m, 90.3m },
				new[] { 0.9m, 0.8m, 0.7m },
				9.9m,
				270.6m,
				2.4m,
				260.7m,
				258.3m);
			yield return new(
				new[] { 22.5m, 22.6m },
				new[] { 150.1m, 150.2m, 150.3m, 150.4m, 150.5m },
				new[] { 1.9m, 1.8m, 1.7m },
				45.1m,
				751.5m,
				5.4m,
				706.4m,
				701.0m);
			yield return new(
				new[] { 5.5m, 5.6m, 5.7m, 5.8m },
				new[] { 50.1m },
				new[] { 3.1m },
				22.6m,
				50.1m,
				3.1m,
				27.5m,
				24.4m);
			yield return new(
				new[] { 18.8m, 18.9m },
				new[] { 110.1m, 110.2m, 110.3m, 110.4m },
				new[] { 2.5m, 2.6m },
				37.7m,
				441.0m,
				5.1m,
				403.3m,
				398.2m);
			yield return new(
				new[] { 8.8m, 8.9m, 9.0m },
				new[] { 80.1m, 80.2m },
				new[] { 0.5m, 0.6m, 0.7m, 0.8m },
				26.7m,
				160.3m,
				2.6m,
				133.6m,
				131.0m);
			yield return new(
				new[] { 12.3m },
				new[] { 140.1m, 140.2m, 140.3m },
				new[] { 1.4m, 1.5m },
				12.3m,
				420.6m,
				2.9m,
				408.3m,
				405.4m);
			yield return new(
				new[] { 25.5m, 25.6m, 25.7m, 25.8m, 25.9m },
				new[] { 180.1m },
				new[] { 3.2m, 3.3m },
				128.5m,
				180.1m,
				6.5m,
				51.6m,
				45.1m);
			yield return new(
				new[] { 11.1m, 11.2m },
				new[] { 120.1m, 120.2m, 120.3m },
				new[] { 1.9m },
				22.3m,
				360.6m,
				1.9m,
				338.3m,
				336.4m);
			yield return new(
				Array.Empty<decimal>(),
				new[] { 120.1m, 120.2m, 120.3m },
				new[] { 1.9m },
				0.0m,
				360.6m,
				1.9m,
				360.6m,
				358.7m);
			yield return new(
				new[] { 120.1m, 120.2m, 120.3m },
				Array.Empty<decimal>(),
				new[] { 1.9m },
				360.6m,
				0.0m,
				1.9m,
				-360.6m,
				-362.5m);
			yield return new(
				new[] { 120.1m, 120.2m, 120.3m },
				new[] { 1.9m },
				Array.Empty<decimal>(),
				360.6m,
				1.9m,
				0.0m,
				-358.7m,
				-358.7m);
			yield return new(
				new[] { 120.1m, 120.2m, 120.3m },
				Array.Empty<decimal>(),
				Array.Empty<decimal>(),
				360.6m,
				0.0m,
				0.0m,
				-360.6m,
				-360.6m);
			yield return new(
				Array.Empty<decimal>(),
				new[] { 120.1m, 120.2m, 120.3m },
				Array.Empty<decimal>(),
				0.0m,
				360.6m,
				0.0m,
				360.6m,
				360.6m);
			yield return new(
				Array.Empty<decimal>(),
				Array.Empty<decimal>(),
				new[] { 120.1m, 120.2m, 120.3m },
				0.0m,
				0.0m,
				360.6m,
				0.0m,
				-360.6m);
			yield return new(
				Array.Empty<decimal>(),
				Array.Empty<decimal>(),
				Array.Empty<decimal>(),
				0.0m,
				0.0m,
				0.0m,
				0.0m,
				0.0m);
			yield return new(
				new[] { 0.0m, 0.0m },
				new[] { 0.0m, 0.0m },
				new[] { 0.0m, 0.0m },
				0.0m,
				0.0m,
				0.0m,
				0.0m,
				0.0m);
		}
	}

	[TestCaseSource(nameof(TransactionsBalanceReportSuccessCases))]
	public void GivenTransactionsBalanceReportDefaultCtor_WhenSuccessData_ThenReturnsCorrectBalances(
		decimal[] debitsList,
		decimal[] creditsList,
		decimal[] feesList,
		decimal expectedTotalDebits,
		decimal expectedTotalCredits,
		decimal expectedTotalFees,
		decimal expectedBalanceBeforeFees,
		decimal expectedFinalBalance)
	{
		// Arrange
		DailyReportBusiness.TransactionsBalanceReport report = new();

		// Act
		foreach (decimal debit in debitsList)
		{
			report.AddDebit(debit);
		}

		foreach (decimal credit in creditsList)
		{
			report.AddCredit(credit);
		}

		foreach (decimal fee in feesList)
		{
			report.AddFee(fee);
		}

		decimal actualTotalDebits = report.TotalDebits;
		decimal actualTotalCredits = report.TotalCredits;
		decimal actualTotalFees = report.TotalFees;
		decimal actualBalanceBeforeFees = report.BalanceBeforeFees;
		decimal actualFinalBalance = report.FinalBalance;

		// Assert
		Assert.Multiple(
			() =>
			{
				Assert.That(actualTotalDebits, Is.EqualTo(expectedTotalDebits));
				Assert.That(actualTotalCredits, Is.EqualTo(expectedTotalCredits));
				Assert.That(actualTotalFees, Is.EqualTo(expectedTotalFees));
				Assert.That(actualBalanceBeforeFees, Is.EqualTo(expectedBalanceBeforeFees));
				Assert.That(actualFinalBalance, Is.EqualTo(expectedFinalBalance));
			});
	}

	[TestCaseSource(nameof(TransactionsBalanceReportSuccessCases))]
	public void GivenTransactionsBalanceReportCtorWithZeros_WhenSuccessData_ThenReturnsCorrectBalances(
		decimal[] debitsList,
		decimal[] creditsList,
		decimal[] feesList,
		decimal expectedTotalDebits,
		decimal expectedTotalCredits,
		decimal expectedTotalFees,
		decimal expectedBalanceBeforeFees,
		decimal expectedFinalBalance)
	{
		// Arrange
		DailyReportBusiness.TransactionsBalanceReport report = new(0.0m, 0.0m, 0.0m);

		// Act
		foreach (decimal debit in debitsList)
		{
			report.AddDebit(debit);
		}

		foreach (decimal credit in creditsList)
		{
			report.AddCredit(credit);
		}

		foreach (decimal fee in feesList)
		{
			report.AddFee(fee);
		}

		decimal actualTotalDebits = report.TotalDebits;
		decimal actualTotalCredits = report.TotalCredits;
		decimal actualTotalFees = report.TotalFees;
		decimal actualBalanceBeforeFees = report.BalanceBeforeFees;
		decimal actualFinalBalance = report.FinalBalance;

		// Assert
		Assert.Multiple(
			() =>
			{
				Assert.That(actualTotalDebits, Is.EqualTo(expectedTotalDebits));
				Assert.That(actualTotalCredits, Is.EqualTo(expectedTotalCredits));
				Assert.That(actualTotalFees, Is.EqualTo(expectedTotalFees));
				Assert.That(actualBalanceBeforeFees, Is.EqualTo(expectedBalanceBeforeFees));
				Assert.That(actualFinalBalance, Is.EqualTo(expectedFinalBalance));
			});
	}

	[TestCaseSource(nameof(TransactionsBalanceReportSuccessCases))]
	public void GivenTransactionsBalanceReportCtorWithInitValue_WhenSuccessData_ThenReturnsCorrectBalances(
		decimal[] debitsList,
		decimal[] creditsList,
		decimal[] feesList,
		decimal expectedTotalDebits,
		decimal expectedTotalCredits,
		decimal expectedTotalFees,
		decimal expectedBalanceBeforeFees,
		decimal expectedFinalBalance)
	{
		// Arrange
		decimal initDebit = debitsList.Length != 0
			? debitsList.First()
			: 0.0m;
		decimal initCredit = creditsList.Length != 0
			? creditsList.First()
			: 0.0m;
		decimal initFee = feesList.Length != 0
			? feesList.First()
			: 0.0m;
		DailyReportBusiness.TransactionsBalanceReport report = new(
			initDebit,
			initCredit,
			initFee);

		// Act
		for (int i = 1; i < debitsList.Length; i++)
		{
			decimal debit = debitsList[i];

			report.AddDebit(debit);
		}

		for (int i = 1; i < creditsList.Length; i++)
		{
			decimal credit = creditsList[i];

			report.AddCredit(credit);
		}

		for (int i = 1; i < feesList.Length; i++)
		{
			decimal fee = feesList[i];

			report.AddFee(fee);
		}

		decimal actualTotalDebits = report.TotalDebits;
		decimal actualTotalCredits = report.TotalCredits;
		decimal actualTotalFees = report.TotalFees;
		decimal actualBalanceBeforeFees = report.BalanceBeforeFees;
		decimal actualFinalBalance = report.FinalBalance;

		// Assert
		Assert.Multiple(
			() =>
			{
				Assert.That(actualTotalDebits, Is.EqualTo(expectedTotalDebits));
				Assert.That(actualTotalCredits, Is.EqualTo(expectedTotalCredits));
				Assert.That(actualTotalFees, Is.EqualTo(expectedTotalFees));
				Assert.That(actualBalanceBeforeFees, Is.EqualTo(expectedBalanceBeforeFees));
				Assert.That(actualFinalBalance, Is.EqualTo(expectedFinalBalance));
			});
	}

	#endregion

	#region GetTransactionsBalanceReport

	public static IEnumerable<TestCaseData> GetTransactionsBalanceReportSuccessCases
	{
		get
		{
			yield return new(
				new[] { 6.5m, 7.8m, 5.2m, 8.1m, 9.0m, 5.5m, 6.9m, 10.3m, 7.2m, 11.6m },
				new[] { (decimal?)1.5m, null, 2.8m, null, 4.0m, 3.1m, null, 1.0m, 0.5m, null },
				new[]
				{
					TransactionType.Credit, TransactionType.Debit, TransactionType.Credit, TransactionType.Debit,
					TransactionType.Credit, TransactionType.Credit, TransactionType.Debit, TransactionType.Credit,
					TransactionType.Credit, TransactionType.Debit
				},
				34.4m,
				43.7m,
				12.9m
			);
			yield return new(
				new[] { 12.4m, 5.8m, 9.2m, 6.7m, 8.0m, 5.4m, 7.3m, 10.9m, 6.1m, 12.2m },
				new[] { (decimal?)2.0m, null, 1.5m, 0.9m, null, 3.2m, null, 1.1m, 0.8m, null },
				new[]
				{
					TransactionType.Credit, TransactionType.Debit, TransactionType.Credit, TransactionType.Credit,
					TransactionType.Debit, TransactionType.Credit, TransactionType.Debit, TransactionType.Credit,
					TransactionType.Credit, TransactionType.Debit
				},
				33.3m,
				50.7m,
				9.5m
			);
			yield return new(
				new[] { 11.7m, 6.2m, 7.5m, 9.8m, 5.1m, 7.9m, 10.2m, 6.6m, 8.4m },
				new[] { (decimal?)null, 1.2m, 2.3m, null, 3.8m, null, 1.7m, 2.0m, null },
				new[]
				{
					TransactionType.Debit, TransactionType.Credit, TransactionType.Credit, TransactionType.Debit,
					TransactionType.Credit, TransactionType.Debit, TransactionType.Credit, TransactionType.Credit,
					TransactionType.Debit
				},
				37.8m,
				35.6m,
				11.0m
			);
			yield return new(
				new[] { 8.5m, 9.4m, 6.1m, 5.9m, 7.2m, 12.5m, 11.3m, 5.7m, 8.6m, 10.8m },
				new[] { (decimal?)null, null, 2.5m, 1.8m, null, 4.3m, null, 0.6m, 1.3m, 3.0m },
				new[]
				{
					TransactionType.Debit, TransactionType.Debit, TransactionType.Credit, TransactionType.Credit,
					TransactionType.Debit, TransactionType.Credit, TransactionType.Debit, TransactionType.Credit,
					TransactionType.Credit, TransactionType.Credit
				},
				36.4m,
				49.6m,
				13.5m
			);
			yield return new(
				new[] { 10.1m, 6.6m, 12.9m, 5.3m, 9.8m, 7.7m, 11.1m, 6.4m },
				new[] { (decimal?)2.7m, null, 1.9m, 3.3m, null, 0.9m, null, 2.2m },
				new[]
				{
					TransactionType.Credit, TransactionType.Debit, TransactionType.Credit, TransactionType.Credit,
					TransactionType.Debit, TransactionType.Credit, TransactionType.Debit, TransactionType.Credit
				},
				27.5m,
				42.4m,
				11.0m
			);
			yield return new(
				new[] { 6.3m, 5.8m, 9.1m, 8.7m, 7.4m, 10.6m, 12.2m, 5.9m, 9.3m },
				new[] { (decimal?)null, 2.4m, 1.0m, 2.9m, null, 3.6m, null, 0.7m, 2.1m },
				new[]
				{
					TransactionType.Debit, TransactionType.Credit, TransactionType.Credit, TransactionType.Credit,
					TransactionType.Debit, TransactionType.Credit, TransactionType.Debit, TransactionType.Credit,
					TransactionType.Credit
				},
				25.9m,
				49.4m,
				12.7m
			);
		}
	}

	[TestCaseSource(nameof(GetTransactionsBalanceReportSuccessCases))]
	public void GivenGetTransactionsBalanceReport_WhenSuccessData_ThenProducesExpectedBalanceReport(
		decimal[] amountsList,
		decimal?[] feesList,
		TransactionType[] transactionTypesList,
		decimal expectedTotalDebits,
		decimal expectedTotalCredits,
		decimal expectedTotalFees)
	{
		// Arrange
		List<ITransaction> transactionsList = [];

		for (int i = 0; i < amountsList.Length; i++)
		{
			decimal amount = amountsList[i];
			decimal? fee = feesList[i];
			TransactionType transactionType = transactionTypesList[i];

			Mock<ITransaction> transactionMock = new();

			transactionMock.Setup(t => t.Amount).Returns(amount);
			transactionMock.Setup(t => t.TransactionFee).Returns(fee);
			transactionMock.Setup(t => t.TransactionType).Returns(transactionType);

			transactionsList.Add(transactionMock.Object);
		}

		DailyReportBusiness.TransactionsBalanceReport expected =
			new(
				expectedTotalDebits,
				expectedTotalCredits,
				expectedTotalFees);

		// Act
		DailyReportBusiness.TransactionsBalanceReport actual =
			DailyReportBusiness.GetTransactionsBalanceReport(transactionsList);

		// Assert
		Assert.Multiple(() =>
		{
			Assert.That(actual.TotalDebits, Is.EqualTo(expected.TotalDebits));
			Assert.That(actual.TotalCredits, Is.EqualTo(expected.TotalCredits));
			Assert.That(actual.TotalFees, Is.EqualTo(expected.TotalFees));
			Assert.That(actual.BalanceBeforeFees, Is.EqualTo(expected.BalanceBeforeFees));
			Assert.That(actual.FinalBalance, Is.EqualTo(expected.FinalBalance));
		});
	}

	#endregion
}
