using FluentValidation;

namespace Backend.Common
{
    public static class Validated
    {
        public static IRuleBuilderOptions<T, int?> ValidateEntityId<T>(this IRuleBuilder<T, int?> ruleBuilder, string fieldName)
        {
            return ruleBuilder
                .NotEmpty().WithMessage($"Debes seleccionar {fieldName}.")
                .Must(id => id.HasValue && id.Value > 0).WithMessage($"El id de {fieldName} no es válido.");
        }
    }
}
