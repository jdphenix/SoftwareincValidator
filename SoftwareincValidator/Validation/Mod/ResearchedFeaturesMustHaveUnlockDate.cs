using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareincValidator.Model.Generated;

namespace SoftwareincValidator.Validation.Mod
{
    class ResearchedFeaturesMustHaveUnlockDate : IModComponentValidator<SoftwareType>
    {
        public IEnumerable<ValidationResult> Validate(SoftwareType component)
        {
            return component.Features
                .Where(x => !string.IsNullOrEmpty(x.Research))
                .Where(x => x.Unlock < 1970)
                .Select(x => new ValidationResult(
                    $"Software type {component.Name} defines a researched feature {x.Name} with an invalid unlock date.",
                    ValidationLevel.Warning));
        }
    }
}
