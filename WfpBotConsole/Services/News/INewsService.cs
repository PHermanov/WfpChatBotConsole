using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace WfpBotConsole.Services.News
{
	public interface INewsService
	{
		Task<IEnumerable<string>> GetNewsAsync(CancellationToken cancellationToken = default);
	}
}