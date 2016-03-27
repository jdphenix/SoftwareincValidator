using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareincValidator.Dto.Generated;

namespace SoftwareincValidator.Validation.Mod
{
    class SoftwareTypeMustHaveForcedValidator : IModComponentValidator<SoftwareType>
    {
        public IEnumerable<ValidationResult> Validate(SoftwareType component)
        {
            var forcedFeatures = component.Features.Where(x => x.Forced);

            if (!forcedFeatures.Any())
            {
                yield return new ValidationResult($"Software type {component.Name} doesn't have a Forced feature defined.");
            }
        }
    }
}
