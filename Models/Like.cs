using System;
using System.Collections.Generic;

namespace BrightIdeas.Models
{
    public class Like
    {
        public int LikeID {get; set;}
        public int UserID {get; set;}
        public User User {get; set;}
        public int IdeaID {get; set;}
        public Idea Idea {get; set;}

        public Like(){
            User = User;
            Idea = Idea;
        }
        
    }
}