using NewsHup.Models;
using TestMVC.Models;

namespace NewsHup.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private NewsContext newsContext;
        public CategoryRepository()
        {
            newsContext = new NewsContext();
        }

        public List<Category> GetAll()
        {
            List<Category> categories = newsContext.Categories.ToList();
            return categories;
        }
    }
}
