using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareincValidator.Model.Generated;

namespace SoftwareincValidator.Validation.Mod
{
    class SoftwareTypeFeaturesNumericValidator : IModComponentValidator<SoftwareType>
    {
        public IEnumerable<ValidationResult> Validate(SoftwareType component)
        {
            foreach (var feature in component.Features)
            {
                if (feature.Unlock != 0 && feature.Unlock < 1970)
                {
                    yield return new ValidationResult(
                        $"Software type {component.Name} has feature {feature.Name} with an Unlock {feature.Unlock} that seems out of range.",
                        ValidationLevel.Warning);
                }

                if (0 > feature.CodeArt || feature.CodeArt > 1)
                {
                    yield return new ValidationResult(
                        $"Software type {component.Name} has feature {feature.Name} with a CodeArt {feature.CodeArt} that seems out of range.",
                        ValidationLevel.Warning);
                }

                if (0 > feature.DevTime)
                {
                    yield return new ValidationResult(
                        $"Software type {component.Name} has feature {feature.Name} with a DevTime {feature.DevTime} that seems out of range.",
                        ValidationLevel.Warning);
                }

                if (0 > feature.Innovation)
                {
                    yield return new ValidationResult(
                        $"Software type {component.Name} has feature {feature.Name} with a Innovation {feature.Innovation} that seems out of range.",
                        ValidationLevel.Warning);
                }

                if (0 > feature.Usability)
                {
                    yield return new ValidationResult(
                        $"Software type {component.Name} has feature {feature.Name} with a Usability {feature.Usability} that seems out of range.",
                        ValidationLevel.Warning);
                }

                if (0 > feature.Stability)
                {
                    yield return new ValidationResult(
                        $"Software type {component.Name} has feature {feature.Name} with a Stability {feature.Stability} that seems out of range.",
                        ValidationLevel.Warning);
                }
            }
        }
    }
}
