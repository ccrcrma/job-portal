using System;
using System.Collections.Generic;
using static job_portal.ViewModels.SearchFilterViewModel;

namespace job_portal.Util
{
    public static class ExperienceLevelInfoHelper
    {
        private static Dictionary<ExperienceLevel, (int, int)> allExperiences = new Dictionary<ExperienceLevel, (int, int)>
        {
            {ExperienceLevel.A, (1,2)},
            {ExperienceLevel.B, (2,3)},
            {ExperienceLevel.C, (3,6)},
            {ExperienceLevel.D, (6,80)}
        };

        public static (int min, int max) GetNumberOfYears(ExperienceLevel level)
        {
            if (allExperiences.TryGetValue(level, out (int, int) minMax))
            {
                return minMax;
            }
            throw new ArgumentException("Provided experience Level doesn't exist");
        }

        public static (int min, int max) GetNumberOfYears(ExperienceLevel[] levels)
        {
            if (levels == null || levels.Length == 0) throw new ArgumentNullException("Please Provide experience levels");
            var currentMax = GetNumberOfYears(levels[0]).max;
            var currentMin = GetNumberOfYears(levels[0]).min;
            foreach (var level in levels)
            {
                currentMin = currentMin > GetNumberOfYears(level).min ? GetNumberOfYears(level).min : currentMin;
                currentMax = currentMax < GetNumberOfYears(level).max ? GetNumberOfYears(level).max : currentMax;
            }
            return (currentMin, currentMax);
        }
    }
}