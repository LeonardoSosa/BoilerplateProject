using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using BoilerplateProject.Authorization.Users;
using BoilerplateProject.Entities.Orders;
using BoilerplateProject.Orders.Dto;
using Microsoft.EntityFrameworkCore;

namespace BoilerplateProject.Orders
{
    public class OrderAppService : AsyncCrudAppService<Order, OrderDto, int, PagedOrderResultRequestDto, CreateOrderDto, OrderDto>, IOrderAppService
    {
        private readonly OrderManager _orderManager;
        private readonly UserManager _userManager;

        public OrderAppService(IRepository<Order, int> repository, OrderManager orderManager, UserManager userManager) : base(repository)
        {
            _orderManager = orderManager;
            _userManager = userManager;
        }

        // Endpoint methods from AsyncCrudAppService:

        // DeleteAsync()

        public override async Task<OrderDto> CreateAsync(CreateOrderDto input)
        {
            CheckCreatePermission();

            var currentUser = await _userManager.GetUserByIdAsync(AbpSession.GetUserId());
            var order = new Order(currentUser.Id);
            foreach (var oDto in input.OrderedProducts)
            {
                var orderedProduct = new OrderedProduct(oDto.ProductId, oDto.UnitPrice, oDto.Amount);
                order.AddOrderedProduct(orderedProduct);
            }

            _orderManager.Validate(order);

            await Repository.InsertAsync(order);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(order);
        }

        public override async Task<OrderDto> GetAsync(EntityDto<int> input)
        {
            CheckGetPermission();

            var order = Repository.GetAll().AsNoTracking().Include(x => x.OrderedProducts).Where(order => order.Id == input.Id).First().MapTo<OrderDto>();
            order.Username = _userManager.GetUserByIdAsync(order.UserId).Result.UserName;

            return order;
        }

        public override async Task<PagedResultDto<OrderDto>> GetAllAsync(PagedOrderResultRequestDto input)
        {
            CheckGetAllPermission();

            var query = CreateFilteredQuery(input);

            var totalCount = await AsyncQueryableExecuter.CountAsync(query);

            query = ApplySorting(query, input);
            query = ApplyPaging(query, input);

            var orders = await AsyncQueryableExecuter.ToListAsync(query.AsNoTracking().Include(x => x.OrderedProducts).Include(x => x.User));
            var ordersDto = orders.Select(MapToEntityDto).ToList();

            return new PagedResultDto<OrderDto>(
                totalCount,
                ordersDto
            );
        }

        protected override IQueryable<Order> CreateFilteredQuery(PagedOrderResultRequestDto input)
        {
            return Repository.GetAll()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.User.Name.Contains(input.Keyword));
        }

        protected override OrderDto MapToEntityDto(Order entity)
        {
            var orderDto = ObjectMapper.Map<OrderDto>(entity);
            orderDto.Username = entity.User.UserName;
            return orderDto;
        }
    }
}
