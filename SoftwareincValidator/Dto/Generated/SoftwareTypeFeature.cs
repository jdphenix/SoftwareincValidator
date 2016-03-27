using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareincValidator.Dto.Generated
{
    public partial class SoftwareTypeFeature
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
}
