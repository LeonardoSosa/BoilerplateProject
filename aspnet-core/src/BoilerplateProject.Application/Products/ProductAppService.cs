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
using Abp.BlobStoring;
using Abp.Domain.Entities;
using Abp.Application.Services.Dto;
using System;
using Abp.UI;
using Abp.Runtime.Validation;

namespace BoilerplateProject.Products
{
    public class ProductAppService : AsyncCrudAppService<Product, ProductDto, int, PagedProductResultRequestDto, CreateProductDto, UpdateProductDto>, IProductAppService
    {
        private readonly IRepository<ProductPicture> _productPictureRepository;
        private readonly IBlobContainer<ProductPictureContainer> _pictureContainer;

        public ProductAppService(IRepository<Product, int> repository, IRepository<ProductPicture> productPictureRepository, IBlobContainer<ProductPictureContainer> pictureContainer) : base(repository)
        {
            _productPictureRepository = productPictureRepository;
            _pictureContainer = pictureContainer;
        }

        // Endpoint methods from AsyncCrudAppService:

        // GetAllAsync()

        // GetAsync()

        public Task<byte[]> GetProductPicture(EntityDto<int> input)
        {
            CheckGetPermission();

            var picture = _productPictureRepository.GetAll().AsNoTracking()
                .Where(p => p.ProductId == input.Id).FirstOrDefault();

            if (picture == null)
            {
                throw new UserFriendlyException("No picture found for the given product ID");
            }

            return _pictureContainer.GetAllBytesOrNullAsync(picture.FileName);
        }

        public override async Task<ProductDto> CreateAsync([FromForm] CreateProductDto input)
        {
            CheckCreatePermission();

            // >> Product logic <<

            Product entityProduct = MapToEntity(input);

            //manager.Validate(entity);

            await Repository.InsertAsync(entityProduct);
            await CurrentUnitOfWork.SaveChangesAsync();

            // >> Picture logic <<

            ProductPicture entityPicture;
            byte[] pictureBytes;
            string pictureType = input.ProductPicture.PictureFile.ContentType;
            var fileName = entityProduct.Id.ToString() + pictureType.Replace("image/", ".");

            using (var memoryStream = new MemoryStream())
            {
                input.ProductPicture.PictureFile.CopyTo(memoryStream);
                pictureBytes = memoryStream.ToArray();
            }

            entityPicture = new ProductPicture
            {
                ProductId = entityProduct.Id,
                FileName = fileName,
                ContentType = pictureType
            };

            await _pictureContainer.SaveAsync(fileName, pictureBytes);
            await _productPictureRepository.InsertAsync(entityPicture);

            return MapToEntityDto(entityProduct);
        }

        public override async Task<ProductDto> UpdateAsync([FromForm] UpdateProductDto input)
        {
            CheckUpdatePermission();

            var entity = Repository.GetAll()
                .Include(entity => entity.ProductPicture)
                .Where(entity => entity.Id == input.Id).First();

            MapToEntity(input, entity);
            await CurrentUnitOfWork.SaveChangesAsync();

            return MapToEntityDto(entity);
        }

        protected override async void MapToEntity(UpdateProductDto updateInput, Product entity)
        {
            entity.Name = updateInput.Name;
            entity.Price = updateInput.Price;
            entity.StockAmount = updateInput.StockAmount;
            entity.IsActive = updateInput.IsActive;

            if (updateInput.ProductPicture != null)
            {
                var newPictureType = updateInput.ProductPicture.PictureFile.ContentType;
                var oldFileName = entity.ProductPicture.FileName;
                var newFileName = entity.Id.ToString() + newPictureType.Replace("image/", ".");

                // update picture entity
                entity.ProductPicture.ContentType = newPictureType;
                entity.ProductPicture.FileName = newFileName;

                // update picture blob
                await _pictureContainer.DeleteAsync(oldFileName);

                byte[] pictureBytes;
                using (var memoryStream = new MemoryStream())
                {
                    updateInput.ProductPicture.PictureFile.CopyTo(memoryStream);
                    pictureBytes = memoryStream.ToArray();
                }
                await _pictureContainer.SaveAsync(newFileName, pictureBytes);
            }
        }

        public override Task DeleteAsync(EntityDto<int> input)
        {
            CheckDeletePermission();

            var picture = _productPictureRepository.GetAll().AsNoTracking()
                .Where(p => p.ProductId == input.Id).FirstOrDefault();

            if (picture != null)
            {
                _productPictureRepository.DeleteAsync(picture.Id);
            }

            return Repository.DeleteAsync(input.Id);
        }

        protected override IQueryable<Product> CreateFilteredQuery(PagedProductResultRequestDto input)
        {
            return Repository.GetAll().AsNoTracking()
                .WhereIf(!input.Keyword.IsNullOrWhiteSpace(), x => x.Name.Contains(input.Keyword))
                .WhereIf(input.IsActive.HasValue, x => x.IsActive == input.IsActive);
        }
    }
}
