using Cyrus.Test.Domain.Products;
using Cyrus.Test.Infrastructure;
using Cyrus.Test.Model.Products.Categories;

namespace Cyrus.Test.Service.Products
{
    public class CategoryService : ICategoryService
    {
        private readonly IDatabaseService _databaseService;

        public CategoryService(IDatabaseService databaseService)
        {
            _databaseService = databaseService ?? throw new ArgumentNullException(nameof(databaseService));
        }

        public async Task<IEnumerable<CategoryModel>> GetAllAsync(CancellationToken cancellationToken = default)
            => await _databaseService.GetAllAsync<CategoryModel>("sp_Category", parameters: new Dictionary<string, object>()
            {
                { "Activity", "GET_ALL" }
            }, cancellationToken: cancellationToken).ConfigureAwait(false);

        public async Task<CategoryModel> GetAsync(Guid categoryId, CancellationToken cancellationToken = default)
        {
            var category = await _databaseService.GetAsync<CategoryModel>("sp_Category", parameters: new Dictionary<string, object>()
            {
                { "Activity", "GET_BY_ID" },
                { "CategoryId", categoryId }
            }, cancellationToken: cancellationToken).ConfigureAwait(false);
            if (category == null)
                throw new Exception("The category is not found.");

            return category;
        }

        public async Task CreateAsync(EditCategoryModel editCategory, CancellationToken cancellationToken = default)
        {
            var duplicatedCategory = await _databaseService.GetAsync<Category>("sp_Category", parameters: new Dictionary<string, object>()
            {
                { "Activity", "CHECK_DUPLICATE" },
                { "Name", editCategory.Name }
            }, cancellationToken: cancellationToken).ConfigureAwait(false);
            if (duplicatedCategory != null)
                throw new Exception("The category is exist.");

            await _databaseService.ExecuteAsync("sp_Category", parameters: new Dictionary<string, object>()
            {
                { "Activity", "INSERT" },
                { "CategoryId", Guid.NewGuid() },
                { "Name", editCategory.Name },
                { "Description", editCategory.Description }
            }, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task UpdateAsync(Guid categoryId, EditCategoryModel editCategory, CancellationToken cancellationToken = default)
        {
            var category = await _databaseService.GetAsync<Category>("sp_Category", parameters: new Dictionary<string, object>()
            {
                { "Activity", "GET_BY_ID" },
                { "CategoryId", categoryId }
            }, cancellationToken: cancellationToken).ConfigureAwait(false);
            if (category == null)
                throw new Exception("The category is not found.");

            var duplicatedCategory = await _databaseService.GetAsync<Category>("sp_Category", parameters: new Dictionary<string, object>()
            {
                { "Activity", "CHECK_DUPLICATE" },
                { "CategoryId", categoryId },
                { "Name", editCategory.Name }
            }, cancellationToken: cancellationToken).ConfigureAwait(false);
            if (duplicatedCategory != null)
                throw new Exception("The category is exist.");

            await _databaseService.ExecuteAsync("sp_Category", parameters: new Dictionary<string, object>()
            {
                { "Activity", "UPDATE" },
                { "CategoryId", categoryId },
                { "Name", editCategory.Name },
                { "Description", editCategory.Description }
            }, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task DeleteAsync(Guid categoryId, CancellationToken cancellationToken = default)
        {
            var category = await _databaseService.GetAsync<Category>("sp_Category", parameters: new Dictionary<string, object>()
            {
                { "Activity", "GET_BY_ID" },
                { "CategoryId", categoryId }
            }, cancellationToken: cancellationToken).ConfigureAwait(false);
            if (category == null)
                throw new Exception("The category is not found.");

            await _databaseService.ExecuteAsync("sp_Category", parameters: new Dictionary<string, object>()
            {
                { "Activity", "DELETE" },
                { "CategoryId", categoryId }
            }, cancellationToken: cancellationToken).ConfigureAwait(false);
        }
    }
}
