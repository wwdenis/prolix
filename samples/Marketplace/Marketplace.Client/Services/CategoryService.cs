using Marketplace.Client.Models.Configuration;
using Marketplace.Client.Models;
using Prolix.Client.Services;

namespace Marketplace.Client.Services
{
    public class CategoryService : BaseApiService<CategoryModel>, ICategoryService
    {
        public CategoryService(ApplicationContext context) : base(context)
        {
        }
    }
}
