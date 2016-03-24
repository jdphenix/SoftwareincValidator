using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareincValidator.Model.Generated;

namespace SoftwareincValidator.Validation.Mod
{
    class ResearchedFeaturesDefinedFeaturePart : IModComponentValidator<SoftwareType>
    {
        public IEnumerable<ValidationResult> Validate(SoftwareType component)
        {
            return component.Features
                .Where(x => !string.IsNullOrEmpty(x.Research))
                .Where(x => !component.Features.Any(y => y.Name.Equals(x.Research, StringComparison.Ordinal)))
                .Select(x => new ValidationResult(
                    $"Software type {component.Name} has researched feature {x.Name} that doesn't define itself as part of a feature on the same software type.",
                    ValidationLevel.Warning));
        }
    }
}
