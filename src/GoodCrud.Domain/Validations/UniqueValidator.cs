using System;
using System.Linq;
using System.Reflection;
using FluentValidation.Validators;
using GoodCrud.Contract.Interfaces;
using GoodCrud.Domain.Libraries;

// https://www.damirscorner.com/blog/posts/20140519-EnsuringUniquePropertyValueUsingFluentValidation.html

namespace GoodCrud.Domain.Validations
{
    public class UniqueValidator<T> : PropertyValidator
      where T : class, IIdentifiable
    {
        private readonly IQueryable<T> _items;
        public readonly bool AllowNull;

        public UniqueValidator(IQueryable<T> items, bool allowNull = false)
          : base("{PropertyName} must be unique")
        {
            _items = items;
            AllowNull = allowNull;
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var editedItem = context.Instance as T;
            var newValue = context.PropertyValue as string;
            if (AllowNull && newValue == null) { return true; }

            var matched = Query.CreatePredicate<T>(context.PropertyName, newValue);
            var found = _items.Where(matched).FirstOrDefault();
            return (found == null || found.Id == editedItem.Id);
        }
    }
}