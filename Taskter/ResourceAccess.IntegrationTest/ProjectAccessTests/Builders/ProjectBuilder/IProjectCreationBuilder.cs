﻿using System.Collections.Generic;
using Utilities.Taskter.Domain;

namespace ResourceAccess.IntegrationTest.ProjectAccessTests
{
    /// <summary>
    /// This interface establishes the a builder pattern, where each function in its concrete has to have 'this.'
    /// Also each function is a void becuase its going to use its self as a reference.
    /// </summary>
    public interface IProjectCreationBuilder
    {
        IProjectCreationBuilder BuildProjectWithName(string name);

        IProjectCreationBuilder BuildProjectWithProjectAcronym(string projectAcronym);

        // GETTO: Their own builder in ProjectMetadataRA
        //// GETTO: maybe own projectdetails builder?
        //void BuildProjectWithNumberOfActiveStories(int numberOfActiveStories);

        //// GETTO: maybe own projectdetails builder?
        //void BuildProjectWithNumberOfCompletedStories(int numberOfCompletedStories);

        IEnumerable<ProjectCreationRequest> BuildManyProjects(int numberOfProjects);

        ProjectCreationRequest Build();
    }
}
