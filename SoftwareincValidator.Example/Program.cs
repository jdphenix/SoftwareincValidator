using System;
using SoftincValidator.Serialization;
using SoftwareincValidator.Model;
using SoftwareincValidator.Model.Generated;

namespace SoftwareincValidator.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            // Get instances to use for loading and saving. 
            var serializer = Components.GetSerializer();
            var loader = Components.GetLoader();


            // The load emits events when loading a mod that are the validation. You can add
            // the ValidationResult objects to a collection to display, or print them out, 
            // or whatever else you need to do with them. 

            // Note that I have a TODO to expose the validation in an API that doesn't require
            // loading to get the validation results. This will primarily help with starting a 
            // new modification. 
            // https://github.com/jdphenix/SoftwareincValidator/issues/34
            loader.XmlValidation += (s, e) => Console.WriteLine(e);
            loader.ModComponentValidation += (s, e) => Console.WriteLine(e);

            //Load the modification at the argument given to Load().
            var mod = loader.Load(args[0]);

            // Example of add a new component to a modification - in this case a base 
            // feature. 
            mod.BaseFeatures.Features.Add(new SoftwareTypeFeature
            {
                Name = "Example Base Feature",
                Category = "Oddball Specialization",
                Description = "This is the description of the feature.",
                Stability = 10,
                Usability = 5,
                Innovation = 5,
                DevTime = 2,
                CodeArt = 1,
                Unlock = 1990
            });

            serializer.Serialize(mod, args[1]);


            // Create a new modification
            var newMod = new SoftincModification("NewMod");
            newMod.Scenarios.Add(new Scenario
            {
                Name = "Example Scenario", 
                Money = new [] { 5000 },
                // These and other properties on the representation like them 
                // will be abstracted away to a more sane representation. 
                // Goals are really a set of dictionaries, and the structure should
                // reflect that.
                Goals = new [] { "Money 50000000,Date 1-1990" }
            });

            serializer.Serialize(newMod, args[1]);
        }
    }
}
