using System;
using System.Collections.Generic;

namespace BrightIdeas.Models
{
    public class User
    {
        public int UserID {get; set;}
        public string Name {get; set;}
        public string Alias {get; set;}
        public string Email {get; set;}
        public string Password {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public List<Like> Likes {get; set;}

        public User(){
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
            Likes = new List<Like>();
        }
    }
}