using System.Threading.Tasks;
using Abp.Application.Services;
using BoilerplateProject.Authorization.Accounts.Dto;

namespace BoilerplateProject.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
