using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareincValidator.Dto.Generated
{
    // This is not really a generated class, but it's purpose (a DTO) is the
    // same as the other generated code in this project. 
    public partial class SoftwareType
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
