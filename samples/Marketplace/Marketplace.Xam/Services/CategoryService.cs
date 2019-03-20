using Marketplace.Models.Configuration;
using Marketplace.Xam.Models;
using Prolix.Http.Client;

namespace Marketplace.Xam.Services
{
    public class CategoryService : BaseApiService<CategoryModel>, ICategoryService
    {
        public CategoryService(ApplicationContext context) : base(context)
        {
        }
    }
}
