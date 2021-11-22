﻿namespace Examples.Models
{
    // :code-block-start: cud
    public class CustomUserData
    {
        public string _id { get; private set; }

        public string _partition { get; private set; }

        public string FavoriteColor { get; set; }

        public string LocalTimeZone { get; set; }

        public bool IsCool { get; set; }

        public CustomUserData(string id, string partition = "myPart")
        {
            this._id = id;
            this._partition = partition;
        }
    }
    // :code-block-end:
}
