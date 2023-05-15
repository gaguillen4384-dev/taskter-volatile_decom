﻿using System;
using System.Collections.Generic;
using Utilities.Taskter.Domain;

namespace Manager.Tests.ProjectManager
{
    //GETTO: Split this up for the different responses/request. 
    public  class DomainUtilityProjectBuilder
    {
        private List<ProjectResponse> _projects;
        private List<ProjectMetadataDetails> _projectMetadataDetails;
        private ProjectResponse _project;
        private ProjectMetadataDetails _metadata;

        public DomainUtilityProjectBuilder()
        {
            _project = new ProjectResponse();
            _metadata = new ProjectMetadataDetails();
            _projects = new List<ProjectResponse>();
            _projectMetadataDetails = new List<ProjectMetadataDetails>();
        }

        #region Builder methods
        #region Project Building
        public DomainUtilityProjectBuilder BuildProjectWithName(string name)
        {
            this._project.Name = name;
            return this;
        }
        public DomainUtilityProjectBuilder BuildProjectWithProjectAcronym(string acronym)
        {
            this._project.ProjectAcronym = acronym;
            return this;
        }

        public DomainUtilityProjectBuilder BuildProjectMetadataWithProjectAcronym(string acronym) 
        {
            this._metadata.ProjectAcronym = acronym;
            return this;
        }
        public DomainUtilityProjectBuilder BuildProjectWithNumberOfActiveStories(int numberOfActiveStories)
        {
            this._metadata.NumberOfActiveStories = numberOfActiveStories;
            return this;
        }
        public DomainUtilityProjectBuilder BuildProjectWithNumberOfCompletedStories(int numberOfCompletedStories)
        {
            this._metadata.NumberOfStoriesCompleted = numberOfCompletedStories;
            return this;
        }
        public DomainUtilityProjectBuilder BuildProjectWithLatestStoryNumber(int latestStoryNumber)
        {
            this._metadata.NumberOfActiveStories = latestStoryNumber;
            return this;
        }
        public ProjectResponse BuildProject()
        {
            return _project;
        }

        public ProjectMetadataDetails BuildProjectMetadata()
        {
            return _metadata;
        }
        #endregion
        #endregion

        #region Setup Methods

        /// <summary>
        /// This is to be used in conjuction with a get method.
        /// </summary>
        public IEnumerable<ProjectResponse> BuildMultipleProjectsAndMetadata(int numberOfProjects, string optionalAcronym = null)
        {

            for (int i = 0; i < numberOfProjects; i++)
            {
                var currentAcronym = string.IsNullOrWhiteSpace(optionalAcronym) ? $"PJT{i}" : optionalAcronym;
                var random = new Random();
                _projects.Add(new DomainUtilityProjectBuilder()
                    .BuildProjectWithProjectAcronym(currentAcronym)
                    .BuildProjectWithName($"Project{i}")
                    .BuildProject());

                _projectMetadataDetails.Add(new DomainUtilityProjectBuilder()
                    .BuildProjectMetadataWithProjectAcronym(currentAcronym)
                    .BuildProjectWithNumberOfCompletedStories(random.Next(i + random.Next(i)))
                    .BuildProjectWithNumberOfActiveStories(random.Next(i + random.Next(i)))
                    .BuildProjectWithLatestStoryNumber(i)
                    .BuildProjectMetadata());
            }

            return _projects;
        }

        /// <summary>
        /// This doesn't build, a build method must have been called befor this.
        /// </summary>
        public IEnumerable<ProjectMetadataDetails> GetMultipleProjectsWithMetadata()
        {
            return _projectMetadataDetails;
        }
        #endregion
    }
}
