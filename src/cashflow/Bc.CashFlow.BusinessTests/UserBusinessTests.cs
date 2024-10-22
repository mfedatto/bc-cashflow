using Bc.CashFlow.Business;
using Bc.CashFlow.Domain.User;
using Microsoft.Extensions.Logging;
using Moq;

namespace Bc.CashFlow.BusinessTests;

[TestFixture]
public class UserBusinessTests
{
	private Mock<ILogger<UserBusiness>> _loggerMock;
	private Mock<IUserService> _userServiceMock;
	private UserBusiness _userBusiness;

	[SetUp]
	public void Setup()
	{
		_loggerMock = new();
		_userServiceMock = new();
		_userBusiness = new(
			_loggerMock.Object,
			_userServiceMock.Object);
	}

	[Test]
	public async Task GivenUserBusiness_WhenGetSingleUser_ThenReturnsSameUserObtainedFromService()
	{
		// Arrange
		const int userId = 1;
		IUser expected = new Mock<IUser>().Object;

		_userServiceMock.Setup(
				s =>
					s.GetSingleUser(
						userId,
						It.IsAny<CancellationToken>()))
			.ReturnsAsync(expected);

		// Act
		IUser actual =
			await _userBusiness.GetSingleUser(
				userId,
				CancellationToken.None);

		// Assert
		Assert.Multiple(
			() =>
			{
				Assert.That(actual, Is.Not.Null);
				Assert.That(actual, Is.EqualTo(expected));
				_userServiceMock.Verify(
					s =>
						s.GetSingleUser(
							userId,
							It.IsAny<CancellationToken>()),
					Times.Once);
			});
	}
}
