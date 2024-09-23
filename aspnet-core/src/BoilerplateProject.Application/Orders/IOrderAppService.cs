using Abp.Application.Services;
using BoilerplateProject.Orders.Dto;

namespace BoilerplateProject.Orders
{
    public interface IOrderAppService : IAsyncCrudAppService<OrderDto, int, PagedOrderResultRequestDto, CreateOrderDto, UpdateOrderDto>
    {
    }
}
