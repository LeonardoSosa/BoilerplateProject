using Abp.Application.Services;
using Abp.BlobStoring;
using Abp.Dependency;
using System.Threading.Tasks;

namespace BoilerplateProject.Products
{
    public class PictureAppService : ApplicationService, ITransientDependency
    {
        private readonly IBlobContainer _blobContainer;

        public PictureAppService(IBlobContainer blobContainer)
        {
            _blobContainer = blobContainer;
        }

        public async Task SaveBytesAsync(byte[] bytes)
        {
            await _blobContainer.SaveAsync("sample-blob", bytes);
        }

        public async Task<byte[]> GetBytesAsync()
        {
            return await _blobContainer.GetAllBytesOrNullAsync("sample-blob");
        }
    }
}
