using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareincValidator.Model.Generated
{
    // This is not really a generated class, but it's purpose (a DTO) is the
    // same as the other generated code in this project. 
    public class SoftwareType
    {
        public SoftwareType()
        {
            Categories = new List<SoftwareTypeCategory>();
        }

        public override string ToString()
        {
            return $"Name: {Name}, Category: {Category}, Unlock: {Unlock}";
        }

        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public double Random { get; set; }
        public double Popularity { get; set; }
        public IList<SoftwareTypeCategory> Categories { get; set; }  
        public bool CategoriesDefined { get; set; }
        public double Retention { get; set; }
        public double Iterative { get; set; }
        public bool InHouse { get; set; }
        public bool OneClient { get; set; }
        public bool OSSpecific { get; set; }
        public string NameGenerator { get; set; }
        public int Unlock { get; set; }
        public IList<SoftwareTypeFeature> Features { get; set; }
    }

    public class SoftwareTypeFeature
    {
        public SoftwareTypeFeature()
        {
            Dependencies = new List<FeatureDependency>();
        }

        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public int Unlock { get; set; }
        public double DevTime { get; set; }
        public int Innovation { get; set; }
        public int Usability { get; set; }
        public int Stability { get; set; }
        public double CodeArt { get; set; }
        public double Server { get; set; }
        public IList<FeatureDependency> Dependencies { get; set; } 
        public IList<FeatureSoftwareCategory> SoftwareCategories { get; set; }
        public string From { get; set; }
        public bool Forced { get; set; }
        public bool Vital { get; set; }
        public string Research { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Category: {Category}, Unlock: {Unlock}, From: {From}, Forced: {Forced}, Vital: {Vital}, Research: {Research}";
        }
    }

    public class SoftwareTypeCategory
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Popularity { get; set; }
        public double Retention { get; set; }
        public double Iterative { get; set; }
        public double TimeScale { get; set; }
        public int Unlock { get; set; }
        public string NameGenerator { get; set; }

        public override string ToString()
        {
            return $"Name: {Name}, Unlock: {Unlock}";
        }
    }

    public class FeatureSoftwareCategory
    {
        public string Category { get; set; }
        public int Unlock { get; set; }

        public override string ToString()
        {
            return $"Category: {Category}, Unlock: {Unlock}";
        }
    }

    public class FeatureDependency
    {
        public string SoftwareType { get; set; }
        public string Feature { get; set; }

        public override string ToString()
        {
            return $"SoftwareType: {SoftwareType}, Feature: {Feature}";
        }
    }
}
