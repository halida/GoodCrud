
using Books.Domain;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;


namespace Books.Domain.Validations
{
    public static class ValidationsConfig
    {
        public static void AddValidations(this IServiceCollection services)
        {
            services.AddTransient<IValidator<Book>, BookValidator>();
        }
    }

    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator()
        {
            RuleFor(e => e.Title).NotNull();
        }
    }

}
