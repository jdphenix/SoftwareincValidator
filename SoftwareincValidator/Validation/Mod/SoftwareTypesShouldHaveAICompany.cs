using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using SoftwareincValidator.Dto;
using SoftwareincValidator.Dto.Generated;

namespace SoftwareincValidator.Validation.Mod
{
    class SoftwareTypesShouldHaveAICompany : IModComponentValidator<ISoftincModification>
    {
        private struct CatResult
        {
            public CatResult(string software, string category)
            {
                Software = software;
                Category = category;
            }

            public string Software { get; }
            public string Category { get; }

            public override string ToString()
            {
                return $"Software: {Software}, Category: {Category}";
            }
        }

        public IEnumerable<ValidationResult> Validate(ISoftincModification component)
        {
            var supportedSoftwareTypes = (
                    from c in component.CompanyTypes
                    from t in c.Types
                    select new CatResult(t.Software, t.Category))
                .ToList();

            var supportAllCategories = supportedSoftwareTypes
                .Where(x => x.Category == null)
                .Select(x => x.Software);

            var unsupportedSoftwareTypes = (
                    from t in component.SoftwareTypes
                    where !t.CategoriesDefined
                    where !t.OneClient
                    select new CatResult(t.Name, "Default"))
                .Concat(
                    from t in component.SoftwareTypes
                    where !t.OneClient
                    where t.CategoriesDefined
                    from c in t.Categories
                    select new CatResult(t.Name, c.Name)
                ).Except(supportedSoftwareTypes)
                 .Where(type => !supportAllCategories.Contains(type.Software));



            return unsupportedSoftwareTypes.Select(type => new ValidationResult(
                $"Software type {type.Software} with category {type.Category} does not have a defined company type, and will not be produced by the AI.", 
                ValidationLevel.Warning));
        }
    }
}
