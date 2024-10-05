using NewsHup.Models;

namespace NewsHup.Repository
{
    public interface ICategoryRepository
    {
        public List<Category> GetAll();
        public Category GetCategoryBy(Func<Category, bool> GetBy);
    }
}
