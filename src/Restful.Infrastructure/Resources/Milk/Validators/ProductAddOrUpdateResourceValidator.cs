using FluentValidation;

namespace Restful.Infrastructure.Resources.Milk.Validators
{
    public class ProductAddOrUpdateResourceValidator<T> 
        : AbstractValidator<T> where T : ProductAddOrUpdateResource
    {
        public ProductAddOrUpdateResourceValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithName("Product Name")
                .WithMessage("{PropertyName} is Required")
                .MaximumLength(20)
                .WithMessage("{PropertyName}.s length can not exceed {MaxLength}");
        }
    }
}
