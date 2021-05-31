using System;
using System.Timers;
using System.IO;
using System.Xml.Serialization;
using Shared;

namespace OrderGenerator
{
    class Program : IDisposable
    {
        public const int INTERVAL = 5000;
        public const string PATH = @"C:\Orders\";
        public const string EXTENSION = ".xml";

        //Fields
        private Timer myTimer;
        private Order myOrder;

        static void Main(string[] args)
        {
            Program myProgram = new Program();
            myProgram.InitTimer();
            myProgram.TimerStart();

            myProgram.myOrder = new Order();

            Console.WriteLine(@"Press 'q' to quit application");
            while (Console.Read() != 'q') ;
        }
        private void InitTimer()
        {
            myTimer = new Timer();
            myTimer.Interval = INTERVAL; // in milliseconden uitgedrukt.
            myTimer.Elapsed += OnCurrentTimerElapsed;
            Console.WriteLine("Timer initiated");
        }
        private void TimerStart()
        {
            myTimer.Start();
            Console.WriteLine("Timer Start");
        }
        private void OnCurrentTimerElapsed(Object source, EventArgs e)
        {
            Console.WriteLine("Timer elapsed");
            SaveOrderObject();
            ToggleStatus();
        }
        private void SaveOrderObject()
        {
            string fileName = string.Empty;
            DirectoryInfo myDir = new DirectoryInfo(PATH);
            if (!myDir.Exists)
            {
                myDir.Create();
                Console.WriteLine($"Directory {PATH} created");
            }
            fileName = GenerateFileName();

            XmlSerializer myXML = new XmlSerializer(typeof(Shared.Order));

            using (Stream fStream = new FileStream(PATH + fileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                myXML.Serialize(fStream, this.myOrder);
            }
            Console.WriteLine("XML file created and saved");
        }
        private string GenerateFileName()
        {
            return $"{this.myOrder.Status.ToString()}-{DateTime.Now.ToString("yyyyMMdd")}-{DateTime.Now.ToString("HHmmss")}{EXTENSION}";
        }
        private void ToggleStatus()
        {
            myOrder.Status = !myOrder.Status;
        }
        public void Dispose()
        {
            myTimer?.Dispose();
            Console.WriteLine("Timer disposed");
        }
    }
}
