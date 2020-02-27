
using System.Linq;
using FluentValidation;
using GoodCrud.Contract.Interfaces;

namespace GoodCrud.Domain.Validations
{
    public static class Extensions
    {
        public static IRuleBuilderOptions<TItem, TProperty> IsUnique<TItem, TProperty>(
            this IRuleBuilder<TItem, TProperty> ruleBuilder, IQueryable<TItem> items)
            where TItem : class, IIdentifiable
        {
            return ruleBuilder.SetValidator(new UniqueValidator<TItem>(items));
        }
    }
}


