using System;
using System.Collections.Generic;

namespace BrightIdeas.Models
{
    public class Idea
    {
        public int IdeaID {get; set;}
        public string Content {get; set;}
        public int UserID {get; set;}
        public User User {get; set;}
        public int LikeCount {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public List<Like> Likes {get; set;}

        public Idea(){
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            LikeCount = 0;
            Likes = new List<Like>();
            User = User;
        }

    }
}