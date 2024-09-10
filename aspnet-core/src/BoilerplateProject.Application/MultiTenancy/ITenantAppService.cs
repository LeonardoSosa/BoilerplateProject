using Abp.Application.Services;
using BoilerplateProject.MultiTenancy.Dto;

namespace BoilerplateProject.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

