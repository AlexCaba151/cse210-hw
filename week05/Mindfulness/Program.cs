using System;
using System.Collections.Generic;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Mindfulness Program");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Listing Activity");
            Console.WriteLine("3. Reflecting Activity");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an option: ");
            string choice = Console.ReadLine();

            Activity activity = null;

            switch (choice)
            {
                case "1":
                    activity = new BreathingActivity();
                    break;
                case "2":
                    activity = new ListingActivity();
                    break;
                case "3":
                    activity = new ReflectingActivity();
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press Enter to continue...");
                    Console.ReadLine();
                    continue;
            }

            activity.Run();
        }
    }
}

class Activity
{
    protected string _name;
    protected string _description;
    protected int _duration;

    public Activity()
    {
    }

    protected void DisplayStartingMessage()
    {
        Console.Clear();
        Console.WriteLine($"Welcome to the {_name}.");
        Console.WriteLine(_description);
        Console.Write("Enter the duration in seconds: ");
        _duration = int.Parse(Console.ReadLine());
        Console.WriteLine("Get ready...");
        ShowSpinner(3);
    }

    protected void DisplayEndingMessage()
    {
        Console.WriteLine($"\nGood job! You have completed the {_name} for {_duration} seconds.");
        ShowSpinner(3);
    }

    protected void ShowSpinner(int seconds)
    {
        string[] spinner = { "|", "/", "-", "\\" };
        DateTime end = DateTime.Now.AddSeconds(seconds);
        int i = 0;
        while (DateTime.Now < end)
        {
            Console.Write(spinner[i % spinner.Length]);
            Thread.Sleep(200);
            Console.Write("\b");
            i++;
        }
    }

    protected void ShowCountDown(int seconds)
    {
        for (int i = seconds; i > 0; i--)
        {
            Console.Write(i);
            Thread.Sleep(1000);
            Console.Write("\b \b");
        }
    }

    public virtual void Run() { }
}

class BreathingActivity : Activity
{
    public BreathingActivity()
    {
        _name = "Breathing Activity";
        _description = "This activity will help you relax by guiding you through slow breathing.";
    }

    public override void Run()
    {
        DisplayStartingMessage();

        for (int i = 0; i < _duration; i += 6)
        {
            Console.Write("\nBreathe in... ");
            ShowCountDown(3);
            Console.Write("\nNow breathe out... ");
            ShowCountDown(3);
        }

        DisplayEndingMessage();
    }
}

class ListingActivity : Activity
{
    private int _count;
    private List<string> _prompts;

    public ListingActivity()
    {
        _name = "Listing Activity";
        _description = "This activity will help you reflect on the good things in your life by listing as many as you can.";
        _prompts = new List<string>
        {
            "Who are people that you appreciate?",
            "What are personal strengths of yours?",
            "What are you thankful for today?"
        };
    }

    public override void Run()
    {
        DisplayStartingMessage();
        GetRandomPrompt();
        Console.WriteLine("Start listing. Press Enter after each item. You have limited time!");
        List<string> items = GetListFromUser();
        _count = items.Count;
        Console.WriteLine($"You listed {_count} items.");
        DisplayEndingMessage();
    }

    private void GetRandomPrompt()
    {
        Random rand = new Random();
        string prompt = _prompts[rand.Next(_prompts.Count)];
        Console.WriteLine($"\nPrompt: {prompt}");
        Console.WriteLine("You may begin in:");
        ShowCountDown(5);
    }

    private List<string> GetListFromUser()
    {
        List<string> items = new List<string>();
        DateTime endTime = DateTime.Now.AddSeconds(_duration);

        while (DateTime.Now < endTime)
        {
            Console.Write("> ");
            if (!string.IsNullOrWhiteSpace(Console.ReadLine()))
            {
                items.Add(Console.ReadLine());
            }
        }

        return items;
    }
}

class ReflectingActivity : Activity
{
    private List<string> _prompts;
    private List<string> _questions;

    public ReflectingActivity()
    {
        _name = "Reflecting Activity";
        _description = "This activity will help you reflect on times in your life when you've shown strength.";
        _prompts = new List<string>
        {
            "Think of a time when you stood up for someone.",
            "Think of a time when you overcame a fear.",
            "Think of a time when you helped someone in need."
        };
        _questions = new List<string>
        {
            "Why was this experience meaningful to you?",
            "What did you learn about yourself?",
            "How can you apply this in the future?",
            "What did you feel during this experience?",
            "What would you do differently next time?"
        };
    }

    public override void Run()
    {
        DisplayStartingMessage();
        DisplayPrompt();
        DisplayQuestions();
        DisplayEndingMessage();
    }

    private string GetRandomPrompt()
    {
        Random rand = new Random();
        return _prompts[rand.Next(_prompts.Count)];
    }

    private string GetRandomQuestion()
    {
        Random rand = new Random();
        return _questions[rand.Next(_questions.Count)];
    }

    private void DisplayPrompt()
    {
        string prompt = GetRandomPrompt();
        Console.WriteLine($"\nConsider the following prompt:");
        Console.WriteLine($"--- {prompt} ---");
        Console.WriteLine("When you have something in mind, press Enter to continue.");
        Console.ReadLine();
        Console.WriteLine("Now reflect on the following questions:");
        ShowSpinner(3);
    }

    private void DisplayQuestions()
    {
        int interval = _duration / 5;
        for (int i = 0; i < 5; i++)
        {
            Console.WriteLine($"> {GetRandomQuestion()}");
            ShowSpinner(interval);
        }
    }
}
