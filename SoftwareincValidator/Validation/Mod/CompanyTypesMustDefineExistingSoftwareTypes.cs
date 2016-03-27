using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareincValidator.Dto.Generated;

namespace SoftwareincValidator.Validation.Mod
{
    class CompanyTypesMustDefineExistingSoftwareTypes : IModComponentValidator<CompanyType>
    {
        public IEnumerable<ValidationResult> Validate(CompanyType component)
        {
            var softwareTypes = component.Modification.SoftwareTypes.Select(x => x.Name).ToList();

            return component.Types
                .Where(software => !softwareTypes.Contains(software.Software))
                .Select(software => new ValidationResult(
                    $"Company type {component.Specialization} defines a software type {software.Software} which doesn't exist."));
        }
    }
}
