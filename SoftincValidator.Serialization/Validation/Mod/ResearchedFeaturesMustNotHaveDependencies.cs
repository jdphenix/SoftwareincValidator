using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareincValidator.Model.Generated;

namespace SoftwareincValidator.Validation.Mod
{
    class ResearchedFeaturesMustNotHaveDependencies : IModComponentValidator<SoftwareType>
    {
        // Features that require research should not have dependencies in the same software type.
        public IEnumerable<ValidationResult> Validate(SoftwareType component)
        {
            var researchedFeatures = component.Features
                .Where(x => !string.IsNullOrEmpty(x.Research))
                .ToList();

            foreach (SoftwareTypeFeature feature in component.Features)
                foreach (var dependency in feature.Dependencies)
                {
                    if (dependency.SoftwareType.Equals(component.Name) && researchedFeatures.Any(x => x.Name.Equals(dependency.Feature)))
                        yield return new ValidationResult(
                            $"Feature {feature.Name} has a dependency {dependency.Feature} which is a " +
                            $"researched feature on software type {component.Name}, which is not allowed.", 
                            ValidationLevel.Warning);
                }
        }
    }
}
