using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Models;

namespace WfpBotConsole.Data.Repositories
{
	[Inject]
	public class UsersRepository : IUsersRepository
	{
		private readonly IApplicationDbContext _gameContext;

		public UsersRepository(IApplicationDbContext gameContext)
		{
			_gameContext = gameContext;
		}

		public async Task<User> GetUser(int userId)
		{
			return await _gameContext
				.Users
				.Include(user => user.Chats)
				.FirstOrDefaultAsync(user => user.UserId == userId);
		}

		public async Task AddOrUpdateUser(User user)
		{
			_gameContext.Users.Update(user);
			await _gameContext.SaveChangesAsync();
		}
	}
}
