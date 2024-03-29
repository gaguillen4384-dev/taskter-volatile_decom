﻿using LiteDB;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Taskter.Domain;

namespace StoriesAccessComponent
{
    /// <summary>
    /// Concrete implementation of <see cref="IStoriesAccess"/>
    /// </summary>
    public class StoriesAccess : IStoriesAccess
    {
        private StoriesResource _storiesConnection;
        private ProjectsMetadataResource _projectNumbersConnection;
        private StoriesReferencesResource _storyReferenceResource;

        public StoriesAccess(IOptions<StoriesResource> storiesConnection,
          IOptions<ProjectsMetadataResource> projectNumbersConnection,
          IOptions<StoriesReferencesResource> storyReferenceResource)
        {
            // This needs to be full path to open .db file
            _storiesConnection = storiesConnection.Value;
            _projectNumbersConnection = projectNumbersConnection.Value;
            _storyReferenceResource = storyReferenceResource.Value;
        }

        /// <summary>
        /// Concrete implementation of <see cref="IStoriesAccess.StartStory(string, StoryCreationRequest)">
        /// </summary>
        public async Task<StoryResponse> StartStory(StoryCreationRequest storyRequest)
        {
            using (var db = new LiteDatabase(_storiesConnection.ConnectionString))
            {
                // this creates or gets collection
                var storiesCollection = db.GetCollection<StoryDocument>("Stories");

                // Index Document on name property
                storiesCollection.EnsureIndex(storyx => storyx.Id);

                // Map from request to story
                var story = StoriesRepositoryMapper.MapCreationRequestToStory(storyRequest);

                //GETTO: Transfer to Manager.
                //var latestStoryNumber = await GetLatestStoryNumberForProject(projectAcronym);
                //var storyNumber = latestStoryNumber++;

                storiesCollection.Insert(story);

                //GETTO: Transfer to Manager.
                //await UpdateStoryReferences(projectAcronym, storyNumber, story.Id);

                return StoriesRepositoryMapper.MapToStoryResponse(story);
            }
        }
        
        /// <summary>
        /// Concrete implementation of <see cref="IStoriesAccess.ReadMultipleStories(IEnumerable{string})
        /// </summary>
        public async Task<IEnumerable<StoryResponse>> ReadMultipleStories(IEnumerable<string> storiesId)
        {
            var listResult = new List<StoryDocument>();
            using (var db = new LiteDatabase(_storiesConnection.ConnectionString))
            {
                foreach (var storyId in storiesId)
                {
                    // this creates or gets collection
                    var storiesCollection = db.GetCollection<StoryDocument>("Stories");
                    var Id = new ObjectId(storyId);
                    var result = storiesCollection.FindById(Id);
                    listResult.Add(result);
                }
            }
            return StoriesRepositoryMapper.MapToStoriesResponse(listResult); ;
        }

        /// <summary>
        /// Concrete implementation of <see cref="IStoriesAccess.ReadStory(string)">
        /// </summary>
        public async Task<StoryResponse> ReadStory(string storyId)
        {
            using (var db = new LiteDatabase(_storiesConnection.ConnectionString))
            {
                // this creates or gets collection
                var storiesCollection = db.GetCollection<StoryDocument>("Stories");

                //GETTO: this needs to be seperated by driver.
                var Id = new ObjectId(storyId);
                var story = storiesCollection.FindById(Id);

                if (story == null) 
                {
                    return new EmptyStoryResponse();
                }

                // use mapper to return what its needed.
                return StoriesRepositoryMapper.MapToStoryResponse(story);
            }
        }

        /// <summary>
        /// Concrete implementation of <see cref="IStoriesAccess.UpdateStory(string, int, StoryUpdateRequest)">
        /// </summary>
        public async Task<StoryResponse> UpdateStory(string storyId, StoryUpdateRequest storyRequest)
        {
            using (var db = new LiteDatabase(_storiesConnection.ConnectionString))
            {
                // this creates or gets collection
                var storiesCollection = db.GetCollection<StoryDocument>("Stories");
                var Id = new ObjectId(storyId);

                var story = storiesCollection.FindById(Id);

                // with the story, map the new updated fields
                var storyUpdated = StoriesRepositoryMapper.UpdateStoryPropertiesFromRequest(story, storyRequest);

                var updated = storiesCollection.Update(storyUpdated);

                //GETTO: move to ProjectMetadataRA or manager?
                //if (storyUpdated.IsCompleted)
                //    UpdateStoryNumberForProject(projectAcronym, storyUpdated.IsCompleted);

                // return a null object if failed to update.
                if (!updated)
                    return StoriesRepositoryMapper.MapToEmptyStoryResponse();

                // use mapper to return what its needed.
                return StoriesRepositoryMapper.MapToStoryResponse(storyUpdated);
            }
        }

        /// <summary>
        /// Concrete implementation of <see cref="IStoriesAccess.RemoveStory(string, int)">
        /// </summary>
        public async Task<bool> RemoveStory(string storyId)
        {
            using (var db = new LiteDatabase(_storiesConnection.ConnectionString))
            {
                // this creates or gets collection
                var storiesCollection = db.GetCollection<StoryDocument>("Stories");
                var Id = new ObjectId(storyId);

                if (!storiesCollection.Delete(Id))
                    return false;

                //GETTO: this needs to be moved to manager
                //if (!DeleteReferenceForStory(storyId))
                //    return false;

                return true;
            }
        }

        #region Private Methods

        //GETTO: Transfer to Manager
        //private async Task UpdateStoryReferences(string projectAcronym, int storyNumber, ObjectId storyId)
        //{
        //    var projectId = await GetProjectId(projectAcronym);

        //    CreateReferenceForProjectAndStory(projectAcronym, storyNumber, storyId, projectId);

        //    UpdateStoryNumberForProject(projectAcronym);
        //}

        #endregion

    }
}
