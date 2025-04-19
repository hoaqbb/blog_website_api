using blog_website_api.Data.Entities;
using blog_website_api.Helpers.Params;

namespace blog_website_api.Specifications
{
    public class PostSpecification : BaseSpecification<Post>
    {
        public PostSpecification(PostSpecificationParams postSpecificationParams, Guid userId)
            : base(x =>
                //Nếu vế trái đúng(true), thì cả biểu thức đúng → bỏ qua điều kiện category.
                (string.IsNullOrEmpty(postSpecificationParams.Category) || 
                // Nếu vế trái sai(false), thì sẽ đánh giá vế phải.
                x.Category!.Slug == postSpecificationParams.Category!.ToLower()) &&
                x.AuthorId != userId &&
                x.Status == 1
            )
        {
            ApplyOrderByDescending(x => x.CreateAt);
            ApplyPaging(
                postSpecificationParams.PageSize * (postSpecificationParams.PageIndex - 1),
                postSpecificationParams.PageSize
                );
        }
    }
}
