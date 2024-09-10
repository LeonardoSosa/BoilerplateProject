using System.Threading.Tasks;
using BoilerplateProject.Models.TokenAuth;
using BoilerplateProject.Web.Controllers;
using Shouldly;
using Xunit;

namespace BoilerplateProject.Web.Tests.Controllers
{
    public class HomeController_Tests: BoilerplateProjectWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}