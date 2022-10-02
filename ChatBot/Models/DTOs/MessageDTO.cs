using System;

namespace ChatBot.Models.DTOs
{
    public class MessageDTO
    {
        public MessageDTO() { }
        
        public MessageDTO(Guid authorid, string content, DateTime timestamp)
        {
            AuthorID = authorid;
            Content = content;
            Timestamp = timestamp;
        }
        
        public Guid AuthorID { get; set; }
        
        public string Content { get; set; }
        
        public DateTime Timestamp { get; set; }
    }
}
