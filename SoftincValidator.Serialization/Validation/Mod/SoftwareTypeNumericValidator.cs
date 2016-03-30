using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareincValidator.Model.Generated;

namespace SoftwareincValidator.Validation.Mod
{
    class SoftwareTypeNumericValidator : IModComponentValidator<SoftwareType>
    {
        public IEnumerable<ValidationResult> Validate(SoftwareType component)
        {
            if (component.Unlock != 0 && component.Unlock < 1970)
            {
                yield return new ValidationResult(
                    $"Software type {component.Name} has Unlock {component.Unlock} that seems out of range.",
                    ValidationLevel.Warning);
            }

            if (0 > component.Popularity || component.Popularity > 1)
            {
                yield return new ValidationResult(
                    $"Software type {component.Name} has Popularity {component.Popularity} that seems out of range.",
                    ValidationLevel.Warning);
            }

            if (0 > component.Retention || component.Retention > 1)
            {
                yield return new ValidationResult(
                    $"Software type {component.Name} has Retention {component.Retention} that seems out of range.",
                    ValidationLevel.Warning);
            }

            if (0 > component.Iterative || component.Iterative > 1)
            {
                yield return new ValidationResult(
                    $"Software type {component.Name} has Iterative {component.Iterative} that seems out of range.",
                    ValidationLevel.Warning);
            }

            if (0 > component.Random || component.Random > 1)
            {
                yield return new ValidationResult(
                    $"Software type {component.Name} has random value {component.Random} that seems out of range.",
                    ValidationLevel.Warning);
            }

            foreach (var feature in component.Features.Where(feature => feature.Unlock != 0 && feature.Unlock < 1970))
            {
                yield return new ValidationResult(
                    $"Software type {component.Name} has feature {feature.Name} with an unlock date {feature.Unlock} that seems out of range.",
                    ValidationLevel.Warning);
            }

            if (component.CategoriesDefined)
            {
                foreach (var cat in component.Categories)
                {
                    if (cat.Unlock != 0 && cat.Unlock < 1970)
                    {
                        yield return new ValidationResult(
                            $"Software type {component.Name} has a category {cat.Name} with an Unlock {cat.Unlock} that seems out of range.",
                            ValidationLevel.Warning);
                    }

                    if (0 > cat.Popularity || cat.Popularity > 1)
                    {
                        yield return new ValidationResult(
                            $"Software type {component.Name} has a category {cat.Name} with a Popularity {cat.Popularity} that seems out of range.",
                            ValidationLevel.Warning);
                    }

                    if (0 > cat.Retention || cat.Retention > 1)
                    {
                        yield return new ValidationResult(
                            $"Software type {component.Name} has a category {cat.Name} with a Retention {cat.Retention} that seems out of range.",
                            ValidationLevel.Warning);
                    }

                    if (0 > cat.Iterative || cat.Iterative > 1)
                    {
                        yield return new ValidationResult(
                            $"Software type {component.Name} has a category {cat.Name} with a Iterative {cat.Iterative} that seems out of range.",
                            ValidationLevel.Warning);
                    }

                    if (0 > cat.TimeScale || cat.TimeScale > 1)
                    {
                        yield return new ValidationResult(
                            $"Software type {component.Name} has a category {cat.Name} with a TimeScale {cat.TimeScale} that seems out of range.",
                            ValidationLevel.Warning);
                    }
                }
            }
        }
    }
}
