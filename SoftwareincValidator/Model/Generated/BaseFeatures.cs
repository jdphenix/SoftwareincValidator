using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SoftwareincValidator.Model.Generated
{
    public class BaseFeatures
    {
        public IList<SoftwareTypeFeature> Features { get; set; }
        public bool OverridesBaseFeatures { get; set; }

        public override string ToString()
        {
            return $"BaseFeatures: {string.Join("},{", Features)}, OverridesBaseFeatures: {OverridesBaseFeatures}";
        }
    }
}
