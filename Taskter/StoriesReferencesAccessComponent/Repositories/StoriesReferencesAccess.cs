﻿using LiteDB;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Taskter.Domain;

namespace StoriesReferencesAccessComponent
{
    /// <summary>
    /// Concrete implementation of <see cref="IStoriesReferencesAccess"/>
    /// </summary>
    public class StoriesReferencesAccess : IStoriesReferencesAccess
    {
        private StoriesReferencesResource _storiesReferenceResource;
        public StoriesReferencesAccess(IOptions<StoriesReferencesResource> storyReferenceResource)
        {
            // This needs to be full path to open .db file
            _storiesReferenceResource = storyReferenceResource.Value;
        }

        /// <summary>
        /// Concrete implementation of <see cref="IStoriesReferencesAccess.StartStoriesReferenceForProject"/>
        /// </summary>
        public async Task StartStoriesReferenceForProject(string projectAcronym, string projectId)
        {
            using (var db = new LiteDatabase(_storiesReferenceResource.ConnectionString))
            {
                // this creates or gets collection
                var storiesReferenceCollection = db.GetCollection<StoryReferenceDocument>("StoriesReferences");

                storiesReferenceCollection.EnsureIndex(reference => reference.ProjectAcronym);

                var storyReference = new StoryReferenceDocument()
                {
                    ProjectAcronym = projectAcronym,
                    ProjectId = projectId
                };

                //GETTO: this should start a new document for the project?
                storiesReferenceCollection.Insert(storyReference);

                // GETTO: What to do if insert fails?
            }
        }

        /// <summary>
        /// Concrete implementation of <see cref="IStoriesReferencesAccess.MakeReferenceForStoryInProject"/>
        /// </summary>
        //GETTO: Change name to something like TyingStoryToParent()
        public async Task MakeReferenceForStoryInProject(string projectAcronym, int storyNumber, string storyId, string projectId)
        {
            using (var db = new LiteDatabase(_storiesReferenceResource.ConnectionString))
            {
                // this creates or gets collection
                var storiesReferenceCollection = db.GetCollection<StoryReferenceDocument>("StoriesReferences");

                storiesReferenceCollection.EnsureIndex(reference => reference.ProjectAcronym);

                var storyReference = new StoryReferenceDocument()
                {
                    ProjectAcronym = projectAcronym,
                    StoryNumber = storyNumber,
                    StoryId = storyId,
                    ProjectId = projectId
                };

                storiesReferenceCollection.Insert(storyReference);

            }
        }

        /// <summary>
        /// Concrete implementation of <see cref="IStoriesReferencesAccess.GetSingleStoryId"/>
        /// </summary>
        public async Task<string> GetSingleStoryId(string projectAcronym, int storyNumber)
        {
            using (var db = new LiteDatabase(_storiesReferenceResource.ConnectionString))
            {
                // this creates or gets collection
                var storiesReferenceCollection = db.GetCollection<StoryReferenceDocument>("StoriesReferences");

                // This needs to be generic in a driver.
                var result = storiesReferenceCollection.FindOne(Query.And(
                    Query.EQ("ProjectAcronym", projectAcronym),
                    Query.EQ("StoryNumber", storyNumber)));

                if (result == null)
                    return string.Empty;

                if (string.IsNullOrWhiteSpace(result.StoryId))
                    return string.Empty;

                return result.StoryId;
            }
        }

        /// <summary>
        /// Concrete implementation of <see cref="IStoriesReferencesAccess.GetProjectStoriesIds"/>
        /// </summary>
        public async Task<IEnumerable<string>> GetProjectStoriesIds(string projectAcronym)
        {
            //GETTO: this gets to change because it could be in a project but the refrence be another story.
            using (var db = new LiteDatabase(_storiesReferenceResource.ConnectionString))
            {
                // this creates or gets collection
                var storiesReferenceCollection = db.GetCollection<StoryReferenceDocument>("StoriesReferences");

                // This needs to be generic in a driver.
                var result = storiesReferenceCollection.Find(Query.EQ("ProjectAcronym", projectAcronym));

                var listResult = result.ToList();

                var listIdsResult = listResult.Select(reference => reference.StoryId);

                return listIdsResult;
            }
        }

        /// <summary>
        /// Concrete implementation of <see cref="IStoriesReferencesAccess.GetProjectId"/>
        /// </summary>
        public async Task<string> GetProjectId(string projectAcronym)
        {
            //GETTO: this gets to change because it could be in a project but the refrence be another story.
            using (var db = new LiteDatabase(_storiesReferenceResource.ConnectionString))
            {
                // this creates or gets collection
                var storiesReferenceCollection = db.GetCollection<StoryReferenceDocument>("StoriesReferences");

                // This needs to be generic in a driver.
                var result = storiesReferenceCollection.FindOne(Query.EQ("ProjectAcronym", projectAcronym));

                if (result == null)
                    return string.Empty;

                return result.ProjectId;
            }
        }

        /// <summary>
        /// Concrete implementation of <see cref="IStoriesReferencesAccess.UpdateStoryReferenceAcronym"/>
        /// </summary>
        public async Task UpdateStoryReferenceAcronym(string updateProjectAcronym, string projectId)
        {
            //GETTO: This became a bit useless since acronym is not important.
            using (var db = new LiteDatabase(_storiesReferenceResource.ConnectionString))
            {
                // this creates or gets collection
                var storiesReferenceCollection = db.GetCollection<StoryReferenceDocument>("StoriesReferences");

                var projectStories = storiesReferenceCollection.Find(Query.EQ("ProjectId", projectId));
                var updateProjectStoriesList = new HashSet<StoryReferenceDocument>();
                foreach (var storyReference in projectStories)
                {
                    storyReference.DateUpdated = DateTime.UtcNow;
                    storyReference.ProjectAcronym = updateProjectAcronym;
                    updateProjectStoriesList.Add(storyReference);
                }

                storiesReferenceCollection.Update(updateProjectStoriesList);
            }
        }

        /// <summary>
        /// Concrete implementation of <see cref="IStoriesReferencesAccess.RemoveReferenceOfStory"/>
        /// </summary>
        public Task<bool> RemoveReferenceOfStory(string storyId)
        {
            using (var db = new LiteDatabase(_storiesReferenceResource.ConnectionString))
            {
                // this creates or gets collection
                var storiesReferenceCollection = db.GetCollection<StoryReferenceDocument>("StoriesReferences");

                // This needs to be generic in a driver.
                var result = storiesReferenceCollection.FindOne(Query.EQ("StoryId", storyId));

                if (!storiesReferenceCollection.Delete(result.Id))
                    return Task.FromResult(false);

                return Task.FromResult(true);
            }
        }
    }
}
