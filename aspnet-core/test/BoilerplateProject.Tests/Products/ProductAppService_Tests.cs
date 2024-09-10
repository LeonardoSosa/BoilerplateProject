using BoilerplateProject.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BoilerplateProject.Products;
using Xunit;
using BoilerplateProject.Products.Dto;
using Shouldly;
using Microsoft.EntityFrameworkCore;


namespace BoilerplateProject.Tests.Products
{
    public class ProductAppService_Tests : BoilerplateProjectTestBase
    {
        private readonly IProductAppService _productAppService;

        public ProductAppService_Tests()
        {
            _productAppService = Resolve<IProductAppService>();
        }

        [Fact]
        public async Task Should_Get_Products_Test()
        {
            // Act
            var output = await _productAppService.GetAllAsync(new PagedProductResultRequestDto { MaxResultCount=20, SkipCount=0 });

            // Assert
            output.Items.Count.ShouldBeGreaterThan(0);
        }

        [Fact(DisplayName = "Should create a product")]
        public async Task Should_Create_Product_Test()
        {
            // Act
            await _productAppService.CreateAsync(
                new CreateProductDto
                {
                    Name = "TestProduct",
                    Price = 0.5,
                    StockAmount = 1,
                    IsActive = true
                });

            // Assert
            await UsingDbContextAsync(async context =>
            {
                var testProduct = await context.Products.FirstOrDefaultAsync(p => p.Name == "TestProduct");
                testProduct.ShouldNotBeNull();
            });
        }
    }
}
