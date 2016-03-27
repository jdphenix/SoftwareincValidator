using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareincValidator.Dto.Generated;

namespace SoftwareincValidator.Validation.Mod
{
    class ResearchedFeatureMustNotHaveBlankPart : IModComponentValidator<SoftwareType>
    {
        public IEnumerable<ValidationResult> Validate(SoftwareType component)
        {
            return component.Features
                .Where(x => x.Research != null)
                .Where(x => x.Research.Equals(string.Empty))
                .Select(x => new ValidationResult(
                    $"Software type {component.Name} has researched feature {x.Name} with an invalid part."));
        }
    }
}
