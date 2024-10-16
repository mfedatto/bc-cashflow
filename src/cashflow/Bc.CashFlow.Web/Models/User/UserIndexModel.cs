using Bc.CashFlow.Domain.User;

namespace Bc.CashFlow.Web.Models.User;

public class UserIndexModel
{
	public UserIndexModel(
		IEnumerable<IUser> usersList)
	{
		UsersList = usersList;
	}
	
	public IEnumerable<IUser> UsersList { get; init; }
}
