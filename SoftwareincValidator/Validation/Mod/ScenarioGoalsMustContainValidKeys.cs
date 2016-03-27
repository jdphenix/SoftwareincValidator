using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using SoftwareincValidator.Model.Generated;

namespace SoftwareincValidator.Validation.Mod
{
    public class ScenarioGoalsMustContainValidKeys : IModComponentValidator<Scenario>
    {
        public IEnumerable<ValidationResult> Validate(Scenario component)
        {
            var allowed = new[] {"Money", "Date"};

            // expensive, not sure it would ultimately matter few scenarios
            return component.Goals
                .Select(g => g.Split(','))
                .SelectMany(g => g)
                .Select(g => g.Split(' '))
                .Where(g => !allowed.Contains(g[0]))
                .Select(g => new ValidationResult(
                    $"Goal in {component.Name} defines a goal \"{g[0]} {g[1]}\" which is not valid. Key must be one of {string.Join(",", allowed)}."));
        }
    }
}
