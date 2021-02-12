﻿using LiteDB;
using System;

namespace LiteDbDriver
{
    public class BaseDocument
    {
        /// <summary>
        /// The unique identifier for the object.
        /// </summary>
        public ObjectId _id = ObjectId.NewObjectId();

        /// <summary>
        /// The date the object got created.
        /// </summary>
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// The date the object got updated.
        /// </summary>
        public DateTime? DateUpdated { get; set; } = null;
    }
}
