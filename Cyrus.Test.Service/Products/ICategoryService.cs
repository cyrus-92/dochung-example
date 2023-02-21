using Cyrus.Test.Model.Products.Categories;

namespace Cyrus.Test.Service.Products
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryModel>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<CategoryModel> GetAsync(Guid categoryId, CancellationToken cancellationToken = default);

        Task CreateAsync(EditCategoryModel editCategory, CancellationToken cancellationToken = default);

        Task UpdateAsync(Guid categoryId, EditCategoryModel editCategory, CancellationToken cancellationToken = default);

        Task DeleteAsync(Guid categoryId, CancellationToken cancellationToken = default);
    }
}
