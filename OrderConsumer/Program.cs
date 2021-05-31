using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Timers;
using System.Configuration;
using Shared;

namespace OrderConsumer
{
    class Program
    {
        // Constants
        private int INTERVAL = 17000;
        private string PATH = @"C:\Orders\";

        //Fields
        DBTools myDBTools;
        private Timer myTimer;
        //ctor
        public Program()
        {
            myDBTools = new DBTools();
        }

        static void Main(string[] args)
        {
            Program myProgram = new Program();
            myProgram.InitTimer();
            myProgram.TimerStart();

            //Fill in the database creation parameters
            myProgram.myDBTools.DatabaseName = "myDatabase";
            myProgram.myDBTools.LogName = "myDatabaseLog";


            //Create database and table in SQL server
            myProgram.myDBTools.CreateDatabase();
            myProgram.myDBTools.CreateTable();
            
            Console.WriteLine("Wachten op nieuwe orders om te verwerken...");
            Console.WriteLine(@"Press 'q' to quit application");
            while (Console.Read() != 'q')
            {
                //Do nothing here...
            };
        }
        
        private void InitTimer()
        {
            myTimer = new Timer
            {
                Interval = INTERVAL // in milliseconden uitgedrukt.
            };
            myTimer.Elapsed += OnCurrentTimerElapsed;
            Console.WriteLine("Timer initiated");
        }
        private void TimerStart()
        {
            myTimer.Start();
            Console.WriteLine("Timer Started");
        }
        private void OnCurrentTimerElapsed(Object source, EventArgs e)
        {
            var aantal = ScanFolder(PATH);
            Console.WriteLine($"Timer elapsed : {aantal} orders verwerkt om {DateTime.Now.ToString()}"); 
        }

        public int ScanFolder(string fileName)
        {
            DirectoryInfo di = new DirectoryInfo(fileName);
            Order myOrder;
            OrderData myOrderData = new OrderData();
            DBTools myDBTools = new DBTools();
            int counter = 0;
            foreach (FileInfo fi in di.GetFiles("*.xml"))
            {
                if (!myDBTools.CheckIfRecordExists(fi.Name))
                {
                    Console.WriteLine(fi.Name);
                    XmlSerializer serializer = new XmlSerializer(typeof(Order));
                    using (Stream fstream = new FileStream(fi.FullName, FileMode.Open))
                    {
                        // Call the Deserialize method to restore the object's state.
                        myOrder = (Order)serializer.Deserialize(fstream);
                        myOrderData.Status = myOrder.Status;
                        myOrderData.FileName = fi.Name;
                        myOrderData.FileDate = File.GetLastWriteTime(fi.FullName);
                    }

                    // Read the XML contents of the file
                    var xmlContent = File.ReadAllText(fi.FullName);
                    
                    //Insert record in the DB.
                    myDBTools.InsertRecord(myOrderData.Status, myOrderData.FileName, myOrderData.FileDate, xmlContent);
                    counter++;
                }
            }
            return counter;
        }
    }
}
