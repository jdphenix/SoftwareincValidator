using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareincValidator.Model.Generated;

namespace SoftwareincValidator.Validation.Impl
{
    public class ModValidator : IModValidator
    {
        private readonly IModComponentValidator<Scenario> _scenarioValidator;
        private readonly IModComponentValidator<PersonalityGraph> _personalitiesValidator;
        private readonly IModComponentValidator<CompanyType> _companyTypeValidator;
        private readonly IModComponentValidator<SoftwareType> _softwareTypeValidator;

        public ModValidator(
            IModComponentValidator<PersonalityGraph> personalitiesValidator,
            IModComponentValidator<Scenario> scenarioValidator,
            IModComponentValidator<CompanyType> companyTypeValidator,
            IModComponentValidator<SoftwareType> softwareTypeValidator)
        {
            if (personalitiesValidator == null)
            {
                throw new ArgumentNullException(nameof(personalitiesValidator));
            }

            if (scenarioValidator == null)
            {
                throw new ArgumentNullException(nameof(scenarioValidator));
            }

            if (companyTypeValidator == null)
            {
                throw new ArgumentNullException(nameof(companyTypeValidator));
            }

            if (softwareTypeValidator == null)
            {
                throw new ArgumentNullException(nameof(softwareTypeValidator));
            }

            _personalitiesValidator = personalitiesValidator;
            _scenarioValidator = scenarioValidator;
            _companyTypeValidator = companyTypeValidator;
            _softwareTypeValidator = softwareTypeValidator;
        }

        public IEnumerable<ValidationResult> Validate(CompanyType component)
        {
            return _companyTypeValidator.Validate(component);
        }

        public IEnumerable<ValidationResult> Validate(SoftwareType component)
        {
            return _softwareTypeValidator.Validate(component);
        }

        public IEnumerable<ValidationResult> Validate(Scenario component)
        {
            return _scenarioValidator.Validate(component);
        }

        public IEnumerable<ValidationResult> Validate(PersonalityGraph component)
        {
            return _personalitiesValidator.Validate(component);
        }
    }
}
