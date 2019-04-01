using Marketplace.Client.Models.Configuration;
using Marketplace.Client.Models;
using Prolix.Http.Client;

namespace Marketplace.Client.Services
{
    public class CategoryService : BaseApiService<CategoryModel>, ICategoryService
    {
        public CategoryService(ApplicationContext context) : base(context)
        {
        }
    }
}
