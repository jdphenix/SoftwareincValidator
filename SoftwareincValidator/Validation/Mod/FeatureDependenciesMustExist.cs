using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareincValidator.Model;

namespace SoftwareincValidator.Validation.Mod
{
    public class FeatureDependenciesMustExist : IModComponentValidator<ISoftincModification>
    {
        public IEnumerable<ValidationResult> Validate(ISoftincModification component)
        {
            var allFeatures =
                from softwareType in component.SoftwareTypes
                from feature in softwareType.Features
                select new
                {
                    Parent = softwareType.Name,
                    Software = softwareType.Name,
                    Feature = feature.Name
                };

            var allDependencies =
                from softwareType in component.SoftwareTypes
                from feature in softwareType.Features
                from dependency in feature.Dependencies
                select new
                {
                    Parent = softwareType.Name,
                    Software = dependency.SoftwareType, 
                    Feature = dependency.Feature
                };

            var brokenDependencies = allDependencies.Except(allFeatures);

            return brokenDependencies.Select(x => new ValidationResult(
                $"Software type {x.Parent} defines a feature with a dependency on {x.Software}:{x.Feature} which doesn't exist."));
        }
    }
}
