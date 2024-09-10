using Abp.Domain.Repositories;
using BoilerplateProject.Entities.Products;
using BoilerplateProject.Products.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace BoilerplateProject.Controllers
{
    [Route("api/services/app/[controller]")]
    [ApiController]
    public class ProductController : BoilerplateProjectControllerBase
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductPicture> _productPictureRepository;
        public ProductController(IRepository<Product> productRepository, IRepository<ProductPicture> productPictureRepository)
        {
            _productRepository = productRepository;
            _productPictureRepository = productPictureRepository;
        }

        //[HttpGet("GetPicture/{id}")]
        //public async Task<IActionResult> GetProductPicture(int id)
        //{
        //    var product = _productRepository.GetAll().AsNoTracking()
        //        .Include(x => x.ProductPicture).Where(p => p.Id == id).First();
        //    var picture = product.ProductPicture;

        //    return File(picture.PictureContent, picture.ContentType);
        //}

        //[HttpGet("GetPictures")]
        //public async Task<IActionResult> GetProductPictures([FromQuery] List<int> ids)
        //{
        //    var products = _productRepository.GetAll().AsNoTracking()
        //        .Include(x => x.ProductPicture).Where(p => ids.Contains(p.Id)).ToList();

        //    if (products == null || !products.Any())
        //    {
        //        return NotFound();
        //    }

        //    var results = products.Select(p => new FileContentResult(
        //        p.ProductPicture.PictureContent, p.ProductPicture.ContentType))
        //        .ToList();

        //    return Ok(results);
        //}
    }
}
