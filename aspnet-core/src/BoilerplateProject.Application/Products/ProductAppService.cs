using Abp.Application.Services;
using Abp.Domain.Repositories;
using BoilerplateProject.Entities.Products;
using BoilerplateProject.Products.Dto;
using System.Linq;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace BoilerplateProject.Products
{
    public class ProductAppService : AsyncCrudAppService<Product, ProductDto, int, PagedProductResultRequestDto, CreateProductDto, ProductDto>, IProductAppService
    {
        private readonly IRepository<ProductPicture> _productPictureRepository;

        public ProductAppService(IRepository<Product, int> repository, IRepository<ProductPicture> productPictureRepository) : base(repository)
        {
            _productPictureRepository = productPictureRepository;
        }

        // Endpoint methods from AsyncCrudAppService:

        // DeleteAsync()

        // GetAllAsync()

        // GetAsync()

        public async Task<IActionResult> GetProductPicture(int id)
        {
            var product = Repository.GetAll().AsNoTracking()
                .Include(x => x.ProductPicture).Where(p => p.Id == id).First();
            var picture = product.ProductPicture;

            return new FileContentResult(picture.PictureContent, picture.ContentType);
        }

        public override async Task<ProductDto> CreateAsync([FromForm] CreateProductDto input)
        {
            CheckCreatePermission();

            byte[] pictureBytes;
            using (var memoryStream = new MemoryStream())
            {
                input.Picture.Picture.CopyTo(memoryStream);
                pictureBytes = memoryStream.ToArray();
            }
            string pictureType = input.Picture.Picture.ContentType;

            var entity = MapToEntity(input);
            entity.ProductPicture = new ProductPicture
            {
                PictureContent = pictureBytes,
                ContentType = pictureType
            };

            //manager.Validate(entity);

            await Repository.InsertAsync(entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        protected override IQueryable<Product> CreateFilteredQuery(PagedProductResultRequestDto input)
        {
            return Repository.GetAll().AsNoTracking()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }
    }
}
