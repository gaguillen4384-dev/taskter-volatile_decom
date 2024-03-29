﻿using System;
using System.Collections.Generic;
using Utilities.Taskter.Domain;

namespace ResourceAccess.IntegrationTest.StoryAccessTests
{
    public class StoriesBuilder : IStoriesBuilder
    {
        private List<StoryCreationRequest> _stories;
        private StoryCreationRequest _storyToCreate;
        private StoryUpdateRequest _storyToUpdate;

        // NEED TO INSTANTIATE THE BUILDER PROPERTIES BEFORE THEY GET USED
        public StoriesBuilder()
        {
            _storyToCreate = new StoryCreationRequest();
            _stories = new List<StoryCreationRequest>();
            _storyToUpdate = new StoryUpdateRequest();
        }

        #region Creation builder

        public IStoriesBuilder BuildStoryWithName(string name)
        {
            _storyToCreate.Name = name;
            return this;
        }

        public IStoriesBuilder BuildStoryWithStoryNumber(int storyNumber)
        {
            _storyToCreate.StoryNumber = storyNumber;
            return this;
        }

        public IStoriesBuilder BuildStoryWithDetails(int numberOfDetails)
        {
            _storyToCreate.Details = PopulateStoryDetails(numberOfDetails);
            return this;
        }

        public IStoriesBuilder BuildStoryWithIsRecurrant(bool isRecurrant)
        {
            _storyToCreate.IsRecurrant = isRecurrant;
            return this;
        }

        public IEnumerable<StoryCreationRequest> BuildStoriesOut(int numberOfStories)
        {
            for (int i = 0; i < numberOfStories; i++)
            {
                _stories.Add(new StoriesBuilder()
                    .BuildStoryWithStoryNumber(i)
                    .BuildStoryWithName($"{NaturalValues.StoryName}{i}")
                    .BuildStoryWithDetails(i)
                    .BuildCreateRequest());
            }

            return _stories;
        }

        #endregion

        #region Update builder
        public IStoriesBuilder UpdateStoryWithDetails(int numberOfDetails)
        {
            _storyToUpdate.Details = PopulateStoryDetails(numberOfDetails, update: true);
            return this;
        }

        public IStoriesBuilder UpdateStoryWithName(string name)
        {
            _storyToUpdate.Name = name;
            return this;
        }

        public IStoriesBuilder UpdateStoryWithIsCompleted(bool flag)
        {
            _storyToUpdate.IsCompleted = flag;
            return this;
        }

        public IStoriesBuilder UpdateStoryWithIsRecurrant(bool flag)
        {
            _storyToUpdate.IsRecurrant = flag;
            return this;
        }

        #endregion

        #region Build Methods

        public StoryCreationRequest BuildCreateRequest()
        {
            return _storyToCreate;
        }
        public StoryUpdateRequest BuildUpdateRequest()
        {
            return _storyToUpdate;
        }

        #endregion

        #region Private methods

        private IEnumerable<StoryDetail> PopulateStoryDetails(int numberOfDetails, bool update = false) 
        {
            var details = new List<StoryDetail>();
            var randomizer = new Random();
            for (int i = 0; i < numberOfDetails; i++)
            {
                var lineNumber = randomizer.Next(0, numberOfDetails);
                var storyDetail = new StoryDetail()
                {
                    LevelIndentation = lineNumber,
                    Line = NaturalValues.StoryDetailLine + lineNumber
                    
                };

                if (update)
                    storyDetail.Line = NaturalValues.UpdatedStoryDetailLine + lineNumber;

                details.Add(storyDetail);
            }

            return details;
        }
        #endregion
    }
}
