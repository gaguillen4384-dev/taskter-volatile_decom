﻿using LiteDbDriver;
using System;

namespace ProjectAccessComponent
{
    public class ProjectDocument : BaseDocument
    {
        /// <summary>
        /// The name of the project.
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// Last date the project got worked on.
        /// </summary>
        public DateTime LastWorkedOn = DateTime.UtcNow;

        /// <summary>
        /// The number of active stories in the project.
        /// </summary>
        public int NumberOfActiveStories = 0;

        /// <summary>
        /// The number of completed stories in the project.
        /// </summary>
        public int NumberOfCompletedStories = 0;

        /// <summary>
        /// The project acronym, used as an identifier.
        /// </summary>
        public string ProjectAcronym = string.Empty;

        /// <summary>
        /// Latest story worked on in the project.
        /// </summary>
        // TODO: this might be more of a client thing or Manager.
        public int LatestStoryNumberWorkedOn = 0;
    }
}
