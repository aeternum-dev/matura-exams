using System;
using System.IO;
using System.Linq;

namespace Exam {


    public class Driver {
        public string _plate;
        public int[] _timestamps = new int[8];

        public Driver(string line) {
            string[] entryProperities = line.Split();
            this._plate =entryProperities[0];
            this._timestamps = entryProperities.Skip(1).Select(int.Parse).ToArray();
        }
    }
    class ExamMay2022 {

        public static void solve() {

            //read data from measurement.txt
            Console.WriteLine("Exercise 1");
            Console.WriteLine("Creating database from input data...");
            List<Driver> cars = ReadFile();
            //display number of records
            Console.WriteLine("Exercise 2");
            Console.WriteLine($"Number of records: {cars.Count}");
            
            //display number of vehicles that passed before 9am
            Console.WriteLine("Exercise 3");
            int earlyCars = NumberofCarsBefore9am(cars);
            Console.WriteLine($"Number of vehicles before 9: {earlyCars}");


            //ask the user of an hour and minute
            Console.WriteLine("Exercise 4");
            Console.WriteLine("Add your time in <hour> <minute> format");
            //a) display number of vehicles that entered the highway section in that minute
            try
            {
                string userTime = Console.ReadLine();
                int[] timearray = userTime.Split().Select(x => int.Parse(x)).ToArray();
                int numberOfCars = NumberofCarsBetweenUserTime(cars, timearray[0], timearray[1]);
                Console.WriteLine($"Number of cars in given minute: {numberOfCars}" );
            //b) density of the road section: (number of vehicles on the road / 10km)
                Console.WriteLine($"Density: {(double)numberOfCars/10}");
            
                Console.WriteLine("Exercise 5");

                Console.WriteLine("Exercise 6");
                Console.WriteLine("Exercise 7");
            
            }
            catch (System.Exception)
            {
                
                throw;
            }
            ;
            
            



        }
        static List<Driver> ReadFile() {
            
            IEnumerable<string> stuff = File.ReadLines("measurements.txt");
            List<Driver> drivers = new List<Driver>();
            foreach (var e in stuff)
            {
                drivers.Add(new Driver(e));
            }
            return drivers;
        }

        static int NumberofCarsBefore9am(List<Driver> Cars){
            int n = 0;
            for (int i = 0; i < Cars.Count; i++)
            {
                if (Cars[i]._timestamps[4] < 9)
                    n++;
            }
            return n;
        }

        static int NumberofCarsBetweenUserTime(List<Driver> Cars, int hour, int minute){
            int n = 0;
            for (int i = 0; i < Cars.Count; i++)
            {
                if (Cars[i]._timestamps[0] == hour && Cars[i]._timestamps[1] == minute)
                    n++;
            }
            return n;
        }
        static internal double calculateSpeed(int[] timestamps) {
            double firstMeasurement = timestamps[0] + 60/timestamps[1] + 3600/timestamps[3];
            double secondMeasurement = timestamps[4] + 60/timestamps[5] + 3600/timestamps[6];
            double time = firstMeasurement - secondMeasurement;
            int road = 10;
            return road/time;
        }

        string highestSpeed(List<Driver> Cars) {
            0
        }


    }


        //use streamreader next time

        //fastest vehicle
        //license plate, average speed as an int
        //number of vehicles overtaken by him

        //display number of vehicles that exceed speed limit ( 90km/h )

        //write to fines.txt; platenumber   average speed(int)  fine(ft)
}
