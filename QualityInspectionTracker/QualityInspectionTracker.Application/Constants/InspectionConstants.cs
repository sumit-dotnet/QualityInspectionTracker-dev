using System;
using System.Collections.Generic;
using System.Text;

namespace QualityInspectionTracker.Application.Constants
{
    public static class InspectionConstants
    {
        public static class DefectTypes
        {
            public const string WeaveDefect = "WeaveDefect";
            public const string ShadeVariation = "ShadeVariation";
            public const string HoleTear = "HoleTear";
            public const string CountDeviation = "CountDeviation";
            public const string Other = "Other";
        }

        public static class Severities
        {
            public const string Critical = "Critical";
            public const string Major = "Major";
            public const string Minor = "Minor";
        }

        public static class Statuses
        {
            public const string Open = "Open";
            public const string Resolved = "Resolved";
        }

        public static class Sources
        {
            public const string Manual = "manual";
            public const string Sap = "sap";
        }
    }
}
