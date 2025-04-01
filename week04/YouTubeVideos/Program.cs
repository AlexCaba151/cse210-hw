using System;
using System.Collections.Generic;

// Comment class to store comment details
class Comment
{
    public string Name { get; set; }
    public string Text { get; set; }
    
    public Comment(string name, string text)
    {
        Name = name;
        Text = text;
    }
}

// Video class to store video details and list of comments
class Video
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int Length { get; set; } // Length in seconds
    private List<Comment> comments = new List<Comment>();
    
    public Video(string title, string author, int length)
    {
        Title = title;
        Author = author;
        Length = length;
    }
    
    public void AddComment(Comment comment)
    {
        comments.Add(comment);
    }
    
    public int GetCommentCount()
    {
        return comments.Count;
    }
    
    public void DisplayInfo()
    {
        Console.WriteLine($"Title: {Title}\nAuthor: {Author}\nLength: {Length} seconds\nComments ({GetCommentCount()}):");
        foreach (var comment in comments)
        {
            Console.WriteLine($"- {comment.Name}: {comment.Text}");
        }
        Console.WriteLine("--------------------------------");
    }
}

class Program
{
    static void Main()
    {
        // Creating video objects
        Video video1 = new Video("C# Basics", "Tech Guru", 600);
        Video video2 = new Video("OOP Concepts", "Code Master", 720);
        Video video3 = new Video("Design Patterns", "Dev Sensei", 900);

        // Adding comments to video1
        video1.AddComment(new Comment("Alice", "Great explanation!"));
        video1.AddComment(new Comment("Bob", "Very helpful, thanks!"));
        video1.AddComment(new Comment("Charlie", "Can you make a part 2?"));

        // Adding comments to video2
        video2.AddComment(new Comment("Dave", "OOP is amazing!"));
        video2.AddComment(new Comment("Eve", "I love how you explained encapsulation."));
        video2.AddComment(new Comment("Frank", "Could you add more examples?"));

        // Adding comments to video3
        video3.AddComment(new Comment("Grace", "I finally understand Singleton!"));
        video3.AddComment(new Comment("Hank", "Super useful, thanks!"));
        video3.AddComment(new Comment("Ivy", "Looking forward to the next lesson."));

        // Storing videos in a list
        List<Video> videos = new List<Video> { video1, video2, video3 };

        // Displaying all video details
        foreach (var video in videos)
        {
            video.DisplayInfo();
        }
    }
}