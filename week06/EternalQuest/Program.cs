using System;
using System.Collections.Generic;
using System.IO;

public abstract class Goal
{
    protected string _shortName;
    protected string _description;
    protected int _points;

    public Goal(string name, string description, int points)
    {
        _shortName = name;
        _description = description;
        _points = points;
    }

    public string GetName()
    {
        return _shortName;
    }

    public abstract void RecordEvent();

    public virtual bool IsCompleted()
    {
        return false;
    }

    public virtual string GetDetailsString()
    {
        return $"{_shortName} ({_description})";
    }

    public virtual string GetStringRepresentation()
    {
        return $"{GetType().Name}:{_shortName},{_description},{_points}";
    }

    public int GetPoints()
    {
        return _points;
    }
}

public class SimpleGoal : Goal
{
    private bool _isComplete;

    public SimpleGoal(string name, string description, int points) : base(name, description, points)
    {
        _isComplete = false;
    }

    public SimpleGoal(string name, string description, int points, bool isComplete)
        : base(name, description, points)
    {
        _isComplete = isComplete;
    }

    public override void RecordEvent()
    {
        if (!_isComplete)
        {
            _isComplete = true;
            Console.WriteLine($"Congratulations! You have earned {_points} points!");
        }
        else
        {
            Console.WriteLine("You have already completed this goal.");
        }
    }

    public override bool IsCompleted()
    {
        return _isComplete;
    }

    public override string GetStringRepresentation()
    {
        return $"{base.GetStringRepresentation()},{_isComplete}";
    }
}

public class EternalGoal : Goal
{
    public EternalGoal(string name, string description, int points) : base(name, description, points) { }

    public override void RecordEvent()
    {
        Console.WriteLine($"Congratulations! You have earned {_points} points!");
    }
}

public class ChecklistGoal : Goal
{
    private int _amountCompleted;
    private int _target;
    private int _bonus;

    public ChecklistGoal(string name, string description, int points, int target, int bonus)
        : base(name, description, points)
    {
        _amountCompleted = 0;
        _target = target;
        _bonus = bonus;
    }

    public ChecklistGoal(string name, string description, int points, int target, int bonus, int amountCompleted)
        : base(name, description, points)
    {
        _amountCompleted = amountCompleted;
        _target = target;
        _bonus = bonus;
    }

    public override void RecordEvent()
    {
        _amountCompleted++;

        if (_amountCompleted < _target)
        {
            Console.WriteLine($"Congratulations! You have earned {_points} points!");
            Console.WriteLine(GetProgressBar());
        }
        else if (_amountCompleted == _target)
        {
            int totalPoints = _points + _bonus;
            Console.WriteLine($"Congratulations! You have earned {_points} points plus a bonus of {_bonus} points!");
            Console.WriteLine($"Total: {totalPoints} points!");
            Console.WriteLine(GetProgressBar());
        }
        else
        {
            Console.WriteLine($"You have already completed this goal the required number of times.");
            _amountCompleted = _target;
        }
    }

    public override bool IsCompleted()
    {
        return _amountCompleted >= _target;
    }

    public override string GetDetailsString()
    {
        return $"{base.GetDetailsString()} -- Currently completed: {_amountCompleted}/{_target}";
    }

    public override string GetStringRepresentation()
    {
        return $"{base.GetStringRepresentation()},{_target},{_bonus},{_amountCompleted}";
    }

    private string GetProgressBar()
    {
        int width = 20;
        int progress = (int)Math.Round((double)_amountCompleted / _target * width);
        string bar = "[";

        for (int i = 0; i < width; i++)
        {
            bar += (i < progress) ? "=" : " ";
        }

        bar += $"] {_amountCompleted}/{_target}";
        return bar;
    }
}

public class GoalManager
{
    private List<Goal> _goals;
    private int _score;
    private string[] _motivationalQuotes;

    public GoalManager()
    {
        _goals = new List<Goal>();
        _score = 0;
        _motivationalQuotes = new string[]
        {
            "Success is not final, failure is not fatal: It is the courage to continue that counts.",
            "The secret of getting ahead is getting started.",
            "Don't watch the clock; do what it does. Keep going.",
            "Believe you can and you're halfway there.",
            "You don't have to be great to start, but you have to start to be great."
        };
    }

    public void Start()
    {
        bool quit = false;

        while (!quit)
        {
            DisplayInfo();
            DisplayMenu();
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateGoal();
                    break;
                case "2":
                    ListGoalNames();
                    break;
                case "3":
                    ListGoalDetails();
                    break;
                case "4":
                    SaveGoals();
                    break;
                case "5":
                    LoadGoals();
                    break;
                case "6":
                    RecordEvent();
                    break;
                case "7":
                    quit = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    public void DisplayInfo()
    {
        Console.WriteLine($"You have {_score} points.");
        Console.WriteLine($"You are currently at Level {GetUserLevel()}!\n");
    }

    private void DisplayMenu()
    {
        Console.WriteLine("Menu Options:");
        Console.WriteLine("1. Create New Goal");
        Console.WriteLine("2. List Goal Names");
        Console.WriteLine("3. List Goal Details");
        Console.WriteLine("4. Save Goals");
        Console.WriteLine("5. Load Goals");
        Console.WriteLine("6. Record Event");
        Console.WriteLine("7. Quit");
        Console.Write("Select a choice from the menu: ");
    }

    private int GetUserLevel()
    {
        return (_score / 500) + 1;
    }

    private string GetRandomQuote()
    {
        Random random = new Random();
        return _motivationalQuotes[random.Next(_motivationalQuotes.Length)];
    }

    public void ListGoalNames()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("You have no goals yet.");
            return;
        }

        Console.WriteLine("The goals are:");
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetName()}");
        }
        Console.WriteLine();
    }

    public void ListGoalDetails()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("You have no goals yet.");
            return;
        }

        Console.WriteLine("The goals are:");
        for (int i = 0; i < _goals.Count; i++)
        {
            string status = _goals[i].IsCompleted() ? "[X]" : "[ ]";
            Console.WriteLine($"{i + 1}. {status} {_goals[i].GetDetailsString()}");
        }
        Console.WriteLine();
    }

    public void CreateGoal()
    {
        Console.WriteLine("The types of Goals are:");
        Console.WriteLine("1. Simple Goal");
        Console.WriteLine("2. Eternal Goal");
        Console.WriteLine("3. Checklist Goal");
        Console.Write("Which type of goal would you like to create? ");
        string goalType = Console.ReadLine();

        Console.Write("What is the name of your goal? ");
        string name = Console.ReadLine();

        Console.Write("What is a short description of it? ");
        string description = Console.ReadLine();

        Console.Write("What is the amount of points associated with this goal? ");
        int points = int.Parse(Console.ReadLine());

        Goal newGoal;

        switch (goalType)
        {
            case "1":
                newGoal = new SimpleGoal(name, description, points);
                break;
            case "2":
                newGoal = new EternalGoal(name, description, points);
                break;
            case "3":
                Console.Write("How many times does this goal need to be accomplished for a bonus? ");
                int target = int.Parse(Console.ReadLine());

                Console.Write("What is the bonus for accomplishing it that many times? ");
                int bonus = int.Parse(Console.ReadLine());

                newGoal = new ChecklistGoal(name, description, points, target, bonus);
                break;
            default:
                Console.WriteLine("Invalid goal type.");
                return;
        }

        _goals.Add(newGoal);
        Console.WriteLine("Goal created successfully!\n");
    }

    public void RecordEvent()
    {
        if (_goals.Count == 0)
        {
            Console.WriteLine("No goals available to record. Please create a goal first.");
            return;
        }

        Console.WriteLine("Which goal did you accomplish?");
        for (int i = 0; i < _goals.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {_goals[i].GetName()}");
        }

        Console.Write("Enter the number of the goal: ");
        if (int.TryParse(Console.ReadLine(), out int index) && index >= 1 && index <= _goals.Count)
        {
            Goal goal = _goals[index - 1];
            goal.RecordEvent();
            _score += goal.GetPoints();
            Console.WriteLine(GetRandomQuote());
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }

        Console.WriteLine();
    }

    public void SaveGoals()
    {
        Console.Write("Enter filename to save to: ");
        string file = Console.ReadLine();

        using (StreamWriter outputFile = new StreamWriter(file))
        {
            outputFile.WriteLine(_score);
            foreach (Goal goal in _goals)
            {
                outputFile.WriteLine(goal.GetStringRepresentation());
            }
        }

        Console.WriteLine("Goals saved successfully!\n");
    }

    public void LoadGoals()
    {
        Console.Write("Enter filename to load from: ");
        string file = Console.ReadLine();

        if (File.Exists(file))
        {
            string[] lines = File.ReadAllLines(file);
            _score = int.Parse(lines[0]);
            _goals.Clear();

            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(':');
                string goalType = parts[0];
                string[] parameters = parts[1].Split(',');

                switch (goalType)
                {
                    case "SimpleGoal":
                        _goals.Add(new SimpleGoal(parameters[0], parameters[1], int.Parse(parameters[2]), bool.Parse(parameters[3])));
                        break;
                    case "EternalGoal":
                        _goals.Add(new EternalGoal(parameters[0], parameters[1], int.Parse(parameters[2])));
                        break;
                    case "ChecklistGoal":
                        _goals.Add(new ChecklistGoal(parameters[0], parameters[1], int.Parse(parameters[2]), int.Parse(parameters[3]), int.Parse(parameters[4]), int.Parse(parameters[5])));
                        break;
                }
            }

            Console.WriteLine("Goals loaded successfully!\n");
        }
        else
        {
            Console.WriteLine("File not found.\n");
        }
    }
}

// Entry point
public class Program
{
    public static void Main()
    {
        GoalManager manager = new GoalManager();
        manager.Start();
    }
}
