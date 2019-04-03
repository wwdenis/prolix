using Marketplace.Client.Models.Configuration;
using Marketplace.Client.Models;

namespace Marketplace.Client.Services
{
    public class CategoryService : BaseApiService<CategoryModel>, ICategoryService
    {
        public CategoryService(ApplicationContext context) : base(context)
        {
        }
    }
}
