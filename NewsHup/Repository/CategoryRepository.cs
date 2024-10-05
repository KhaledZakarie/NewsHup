using NewsHup.Models;
using TestMVC.Models;

namespace NewsHup.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        //  private NewsContext newsContext;

        private readonly NewsContext _newsContext;
        // Use Dependency Injection to inject NewsContext
        public CategoryRepository(NewsContext newsContext)
        {
            _newsContext = newsContext;
        }

        public List<Category> GetAll()
        {
            List<Category> categories = _newsContext.Categories.ToList();
            return categories;
        }

        public Category GetCategoryBy(Func<Category, bool> GetBy)
        {
            Category category = _newsContext.Categories.FirstOrDefault(GetBy);
            return category;
        }
    }
}
