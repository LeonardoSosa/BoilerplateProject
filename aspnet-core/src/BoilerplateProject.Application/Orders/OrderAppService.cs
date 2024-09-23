using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.UI;
using BoilerplateProject.Authorization.Users;
using BoilerplateProject.Entities.Orders;
using BoilerplateProject.Orders.Dto;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BoilerplateProject.Orders
{
    public class OrderAppService : AsyncCrudAppService<Order, OrderDto, int, PagedOrderResultRequestDto, CreateOrderDto, UpdateOrderDto>, IOrderAppService
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

            var order = Repository.GetAll().AsNoTracking()
                .Include(x => x.OrderedProducts).Where(order => order.Id == input.Id)
                .First().MapTo<OrderDto>();
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

            var orders = await AsyncQueryableExecuter.ToListAsync(
                query.AsNoTracking().Include(x => x.OrderedProducts).Include(x => x.User));
            var ordersDto = orders.Select(MapToEntityDto).ToList();

            return new PagedResultDto<OrderDto>(
                totalCount,
                ordersDto
            );
        }

        public override async Task<OrderDto> UpdateAsync(UpdateOrderDto input)
        {
            CheckUpdatePermission();

            var entity = Repository.GetAll()
                .Include(entity => entity.OrderedProducts).Include(x => x.User)
                .Where(entity => entity.Id == input.Id).First();

            MapToEntity(input, entity);
            _orderManager.Validate(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        protected override void MapToEntity(UpdateOrderDto updateInput, Order entity)
        {
            // check user exists
            if(!DoesUserExists(updateInput.UserId))
            {
                throw new UserFriendlyException("Cannot find user with given userId");
            }

            var productsToRemove = new List<OrderedProduct>();

            // Update OrderedProducts first
            foreach (var orderedInput in updateInput.OrderedProducts)
            {
                // if product exists in original list -> update
                if (entity.OrderedProducts.Any(e => e.ProductId == orderedInput.ProductId))
                {
                    var orederedEntity = entity.OrderedProducts.Find(e => e.ProductId == orderedInput.ProductId);
                    orederedEntity.UnitPrice = orderedInput.UnitPrice;
                    orederedEntity.Amount = orderedInput.Amount;
                    orederedEntity.Subtotal = orderedInput.UnitPrice * orderedInput.Amount;
                }
                // if new product -> add
                else
                {
                    var temp = new OrderedProduct(orderedInput.ProductId, orderedInput.UnitPrice, orderedInput.Amount);
                    entity.AddOrderedProduct(temp);
                }
            }

            // if no longer exists in new list -> remove
            foreach (var orederedEntity in entity.OrderedProducts)
            {
                if (!updateInput.OrderedProducts.Any(p => p.ProductId == orederedEntity.ProductId))
                {
                    productsToRemove.Add(orederedEntity);
                }
            }
            foreach (var productToRemove in productsToRemove)
            {
                entity.OrderedProducts.Remove(productToRemove);
            }

            // Update remaining properties
            entity.UpdateProperties(updateInput.UserId);
        }

        protected override IQueryable<Order> CreateFilteredQuery(PagedOrderResultRequestDto input)
        {
            return Repository.GetAll().AsNoTracking()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.User.Name.Contains(input.Keyword));
        }

        protected override OrderDto MapToEntityDto(Order entity)
        {
            var orderDto = ObjectMapper.Map<OrderDto>(entity);
            orderDto.Username = entity.User.UserName;
            return orderDto;
        }

        protected bool DoesUserExists(long userId)
        {
            try
            {
                _userManager.GetUserById(userId);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
