using System.Threading.Tasks;
using WfpBotConsole.Models;

namespace WfpBotConsole.Data.Repositories
{
	public interface IUsersRepository
	{
		Task<User> GetUser(int userId);
	}
}