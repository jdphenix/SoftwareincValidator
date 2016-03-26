using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SoftwareincValidator.Model.Generated;

namespace SoftwareincValidator.Validation.Mod
{
    public class ScenarioGoalsShouldContainPositiveMoney : IModComponentValidator<Scenario>
    {

        private IList<T> AnonymousList<T>(T template)
        {
            var list = new[] { template }.ToList();
            list.Clear();
            return list; 
        } 

        public IEnumerable<ValidationResult> Validate(Scenario component)
        {
            var acc = AnonymousList(new {Amount = 0, Success = false});

            var result = component.Goals
                .Select(g => g.Split(','))
                .SelectMany(g => g)
                .Select(g => g.Split(' '))
                .Where(g => g[0].Equals("Money"))
                .Aggregate(acc, (list, next) =>
                {
                    var number = 0;
                    var parsed = int.TryParse(next[1], out number);
                    list.Add(new { Amount = number, Success = parsed });
                    return list;
                });

            foreach (var r in result)
            {
                if (r.Amount <= 0)
                {
                    yield return new ValidationResult(
                        $"Scenario {component.Name} defines a money goal with an amount {r.Amount}, which is probably not what is intended.",
                        ValidationLevel.Warning);
                } else if (r.Success == false)
                {
                    yield return new ValidationResult($"Scenario {component.Name} defines a money goal that failed to convert to a number.");
                }
            }
        }
    }
}
