using System;
using System.IO;
using System.Linq;

namespace Exam {


    public class Car {
        public string _plate;
        public TimeOnly _enterTime = new TimeOnly();
        public TimeOnly _leaveTime = new TimeOnly();

        public Car(string line) {
            string[] ep = line.Split();
            this._plate = ep[0];
            int[] tmp1 = new ArraySegment<string>(ep, 1, 4).Select(int.Parse).ToArray();
            int[] tmp2 = new ArraySegment<string>(ep, 5, 4).Select(int.Parse).ToArray();
            

            this._enterTime = new TimeOnly(tmp1[0], tmp1[1], tmp1[2], tmp1[3]);
            this._leaveTime = new TimeOnly( tmp2[0], tmp2[1], tmp2[2], tmp2[3]);
        }
    }
    class ExamMay2022 {

        public static void solve() {
            try {

                //read data from measurement.txt
                Console.WriteLine("Exercise 1");
                Console.WriteLine("Creating database from input data...");
                List<Car> cars = ReadFile();
                //display number of records
                Console.WriteLine("Exercise 2");
                Console.WriteLine($"Number of records: {cars.Count}");
                
                //display number of vehicles that passed before 9am
                Console.WriteLine("Exercise 3");
                int earlyCars = NumberofCarsBefore9am(cars);
                Console.WriteLine($"Number of vehicles before 9: {earlyCars}");

                //ask the user of an hour and minute
                Console.WriteLine("Exercise 4");
                Console.Write("Add your time in <hour> <minute> format: ");
                string userTime = Console.ReadLine();
                int[] timearray = userTime.Split().Select(x => int.Parse(x)).ToArray();
                //a) display number of vehicles that entered the highway section in that minute
                int justEntered = NumberofCarsEntered(cars, timearray[0], timearray[1]);
                Console.WriteLine($"Number of cars in given minute: {justEntered}" );
                //b) density of the road section: (number of vehicles on the road / 10km)
                int OnTheSection = NumberofCarsOntheSection(cars, timearray[0], timearray[1]);
                Console.WriteLine($"The trafic intensity: {((double)OnTheSection/10).ToString("0.0")}");

                //fastest vehicle
                //license plate, average speed as an int
                //number of vehicles overtaken by him
                Console.WriteLine("Exercise 5");
                Console.WriteLine("The data of the vehicle with the highest speed are");
                Car fastestCar = cars.OrderByDescending(x => (x._leaveTime - x._enterTime)).Last();
                Console.WriteLine("plate number: {0}",fastestCar._plate);
                Console.WriteLine("speed: {0:0} km/h",CalculateSpeed(fastestCar));
                Console.WriteLine("number of overtaken vehicles: {0}",OverTakenVehicles(cars, fastestCar));
                
                //display number of vehicles that exceed speed limit ( 90km/h )
                Console.WriteLine("Exercise 6");
                Console.WriteLine("Number of cars exceeding the speed limit: {0:0.00}%", ExceedSpeedLimit(cars));
                
                //write to fines.txt; platenumber   average speed(int)  fine(ft)
                Console.WriteLine("Exercise 7");
                Console.WriteLine("Saving fines to output file.");
                using ( StreamWriter reader = new StreamWriter("fines.txt")) {
                    foreach (var item in cars)
                    {
                    if (CalculateSpeed(item) > 104) {

                        reader.WriteLine("{0}\t{1:0.00} km/h\t{2} Ft",item._plate, CalculateSpeed(item), CalculateFine(item));
                        }
                    }    
                }
                
            
            }
            catch (System.Exception)
            {
                
                throw;
            };
            
            

        }
        static List<Car> ReadFile() {
            
            IEnumerable<string> stuff = File.ReadLines("measurements.txt");
            List<Car> cars = new List<Car>();
            foreach (var e in stuff)
            {
                cars.Add(new Car(e));
            }
            return cars;
        }

        static int NumberofCarsBefore9am(List<Car> cars){
            int n = 0;
            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i]._leaveTime.Hour < 9)
                    n++;
            }
            return n;
        }
        

        static int NumberofCarsEntered(List<Car> cars, int hour, int minute){
            int n = 0;
            for (int i = 0; i < cars.Count; i++)
            {
                if (cars[i]._enterTime.Hour == hour && cars[i]._enterTime.Minute == minute)
                    n++;
            }
            return n;
        }

        static int NumberofCarsOntheSection (List<Car> cars, int hour, int minute){
            int n = 0;
            TimeOnly userTime = new TimeOnly(hour, minute);
            for (int i = 0; i < cars.Count; i++)
            {
                if (userTime.IsBetween(cars[i]._enterTime, cars[i]._leaveTime))
                    n++;
            }
            return n;
        }

        static int OverTakenVehicles(List<Car> cars, Car fastestCar) {
            int n = 0;
            for (int i = 0; i < cars.Count; i++)
            {
                if (fastestCar._enterTime > cars[i]._enterTime && fastestCar._leaveTime < cars[i]._leaveTime)
                    n++;
            }
            return n;
        }
            
        static double CalculateSpeed(Car currentCar) {
            double time = (currentCar._leaveTime - currentCar._enterTime).TotalHours;
            double road = 10;
            return road/time;
        }


        static double ExceedSpeedLimit(List<Car> cars) {
            int n = 0;
            for (int i = 0; i < cars.Count; i++) {
                var a = CalculateSpeed(cars[i]);
                if (a >= 90) {
                    n++;
                }
            }
            return (double)n /cars.Count * 100;
        }

        static int CalculateFine(Car currentCar) {
            double speed = CalculateSpeed(currentCar);
            if ( speed <= 121) 
                return 30000;
            else if ( speed <= 136)
                return 45000;
            else if ( speed <= 151)
                return 60000;
            else return 200000;
        }

    }

    
}
