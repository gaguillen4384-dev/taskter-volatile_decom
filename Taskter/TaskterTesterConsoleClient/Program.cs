﻿using Microsoft.Extensions.DependencyInjection;
using StoriesAccessComponent.Repositories;
using System;

namespace TaskterTesterConsoleClient
{
    public class Program
    {
        // TODO: Get DI out of program into startup
        private readonly static IServiceProvider _serviceProvider;

        private readonly static IStoriesAccess _storiesAccess;

        public static void Main(string[] args)
        {
            // TODO: Integrate App-v/next Polly for resiliance in the CLients 
            // Clients should have Caching specially mobile clients SQLlite net
            // Akavache helps with caching -UserAccount -Secure -InMemory
            //setup our DI
            var serviceProvider = new ServiceCollection()
                .AddTransient<IStoriesAccess, StoriesAccess>()
                .BuildServiceProvider();

            //do the actual work here
            var StoriesAccess = serviceProvider.GetService<StoriesAccess>();
            var story = StoriesAccess.GetSingleStory("TST", 15);
        }
    }
}