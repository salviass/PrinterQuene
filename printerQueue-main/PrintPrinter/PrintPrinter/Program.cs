using System;
using System.Collections.Generic;
using System.Linq;


namespace PrintPrinter
{
    enum Priority
    {
        Highest,
        High,
        Normal,
        Low,
        Lowest
    }
    enum Status
    {
        Ok,
        Error,
        Waiting
    }
    class User
    {
        public string Name { get; set; }
        public Priority Priority { get; set; }
        public User(string name,Priority priority)
        {
            Name = name;
            Priority = priority;
        }
        public override string ToString()
        {
            return $"{Name}";
        }
    }
    class PrintJob
    {
        public User User { get; private set; }
        public DateTime Time { get; private set; }

        public PrintJob(User user,DateTime time)
        {
            User = user;
            Time = time;
        }
        public override string ToString()
        {
            return $"{User} at {Time.ToShortTimeString()}";
        }
    }
    class Printer
    {
        public string PrinterName { get; set; }
        public static Queue<PrintJob> ComplitedJobs { get; private set; }
        private Queue<PrintJob>[] PrintJobs;
        private int timer = 0;
        public Printer(string printerName)
        {
            PrinterName = printerName;
            PrintJobs = new Queue<PrintJob>[Enum.GetNames(typeof(Priority)).Length].Select(x=>x = new Queue<PrintJob>()).ToArray();
            ComplitedJobs = new Queue<PrintJob>();
        }
        public void AddJob(PrintJob newJob)
        {
            PrintJobs[(int)newJob.User.Priority].Enqueue(newJob);
        }
        public void DoPrint()
        {
            var curJob = PrintJobs.FirstOrDefault(x => Enumerable.Any<PrintJob>(x))?.Dequeue();
            if (curJob != null)
            {
                Console.WriteLine($"printed at {curJob.Time.AddMinutes(timer)} for {curJob.User.Name}");
                timer++;
                ComplitedJobs.Enqueue(curJob);
            }
        }
        public void DropPrinterQueue()
        {
            PrintJob printJob = null;
            foreach (var queue in PrintJobs)
            {
                while(queue.Any())
                {
                    printJob = queue.Dequeue();
                    ComplitedJobs.Enqueue(printJob);
                    Console.WriteLine($"{printJob} dropped");
                }
            }
            Console.WriteLine($"All jobs dropped");
        }
    }

    static class StudentPrint
    {
        static void Main(string[] args)
        {
            Printer printer = new Printer("hp lj");
            printer.DoPrint();
            printer.DoPrint();
            printer.DoPrint();
            printer.AddJob(new PrintJob(new User("111", Priority.High), new DateTime(1, 1, 1, 1, 1, 1)));
            printer.DoPrint();
            printer.DoPrint();
            printer.AddJob(new PrintJob(new User("222", Priority.Low), new DateTime(1, 1, 1, 1, 1, 1)));
            printer.AddJob(new PrintJob(new User("333", Priority.Normal), new DateTime(1, 1, 1, 1, 1, 1)));
            printer.DoPrint();
            printer.AddJob(new PrintJob(new User("444", Priority.Highest), new DateTime(1, 1, 1, 1, 1, 1)));
            printer.DoPrint();
            printer.DropPrinterQueue();
            printer.DoPrint();
        }
    }
}