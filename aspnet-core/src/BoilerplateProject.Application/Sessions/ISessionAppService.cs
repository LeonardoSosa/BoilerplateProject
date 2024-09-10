using System.Threading.Tasks;
using Abp.Application.Services;
using BoilerplateProject.Sessions.Dto;

namespace BoilerplateProject.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
