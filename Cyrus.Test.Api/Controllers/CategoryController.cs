using Cyrus.Test.Model.Products.Categories;
using Cyrus.Test.Service.Products;
using Microsoft.AspNetCore.Mvc;

namespace Cyrus.Test.Api.Controllers
{
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService ?? throw new ArgumentNullException(nameof(categoryService));
        }

        [HttpGet]
        [Route("api/categories")]
        public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
            => Ok(await _categoryService.GetAllAsync(cancellationToken).ConfigureAwait(false));

        [HttpGet]
        [Route("api/categories/{id:guid}")]
        public async Task<IActionResult> GetAsync([FromRoute(Name = "id")] Guid categoryId, CancellationToken cancellationToken = default)
            => Ok(await _categoryService.GetAsync(categoryId, cancellationToken).ConfigureAwait(false));

        [HttpPost]
        [Route("api/categories")]
        public async Task<IActionResult> CreateAsync([FromBody] EditCategoryModel editCategory, CancellationToken cancellationToken = default)
        {
            await _categoryService.CreateAsync(editCategory, cancellationToken).ConfigureAwait(false);
            return Ok(true);
        }

        [HttpPut]
        [Route("api/categories/{id:guid}")]
        public async Task<IActionResult> UpdateAsync([FromRoute(Name = "id")] Guid categoryId, [FromBody] EditCategoryModel editCategory, CancellationToken cancellationToken = default)
        {
            await _categoryService.UpdateAsync(categoryId, editCategory, cancellationToken).ConfigureAwait(false);
            return Ok(true);
        }

        [HttpDelete]
        [Route("api/categories/{id:guid}")]
        public async Task<IActionResult> DeleteAsync([FromRoute(Name = "id")] Guid categoryId, CancellationToken cancellationToken = default)
        {
            await _categoryService.DeleteAsync(categoryId, cancellationToken).ConfigureAwait(false);
            return Ok(true);
        }
    }
}
