using System;
using System.IO;
using System.Linq;

namespace Exam {
    class ExamOctober2022 {
        static public List<int[]> readSensorLog(string logfile)
            {
                List<int[]> SensorData = new List<int[]>();

                var stuff = File.ReadLines(logfile);
                foreach (var line in stuff)
                {
                    int[] entry = line.Split().Select(int.Parse).ToArray();
                    SensorData.Add(entry);
                }
                return SensorData;

            }
        //create interval fuction in seconds
        static int interval(int[] data1, int[] data2) {
        //          hours           minutes       seconds
        int time1 = data1[0]*3600 + data1[1]*60 + data1[2];
        int time2 = data2[0]*3600 + data2[1]*60 + data2[2];

        int difference = Math.Abs(time2 - time1);
        return difference;
        }

        //time interval between last and first transmisson "hour:minute:second"
        static void printWithStyle(int seconds) {
            int hours = seconds / 3600;
            int minutes = (seconds % 3600) / 60;
            seconds = (seconds % 3600) % 60;
            Console.WriteLine(string.Format("{0}:{1}:{2}", hours, minutes, seconds));
        }
        

        //cordinates of corners of limit of transponder entries
        static int[][] getBorders(List<int[]> logData) {
            int topX =
                logData.OrderByDescending(e => e[3]).First()[3];
            int bottomX =
                logData.OrderByDescending(e => e[3]).Last()[3];
            int topY =
                logData.OrderByDescending(e => e[4]).First()[4];
            int bottomY =
                logData.OrderByDescending(e => e[4]).Last()[4];
            
            return new int[2][] {new int[]
                {topX, topY}, new int[] {bottomX, bottomY}};
        }
        //create function caluclates accumulated travel distance
        
        static double distanceBetweenEntries(int x1, int x2, int y1, int y2) {
            double result = Math.Sqrt(Math.Pow((x1-x2),2) + Math.Pow((y1-y2),2));
            return result;
        }
        
        static double accumulatedDistance(List<int[]> logData) {
            double distance = 0;
            for (int i = 0; i<logData.Count-1; i++)
                distance  += distanceBetweenEntries(logData[i+1][3],logData[i][3], logData[i+1][4], logData[i][4]);

            return distance;
        }

        static void missingData(List<int[]> logData) {
            using (var missing = File.CreateText("missing.txt")){
                for (int i = 0; i < logData.Count-1; i++) {
                    double d = distanceBetweenEntries(logData[i+1][3],logData[i][3], logData[i+1][4], logData[i][4]);
                    if (d > 10) {
                        int potentiallyMissingEntries = (int)Math.Floor(d / 10);
                        missing.WriteLine($"{logData[i][0]} {logData[i][1]} {logData[i][2]} coordinate difference {potentiallyMissingEntries}");
                    } else if (interval(logData[i], logData[i+1])/60 > 5) {
                        int potentiallyMissingEntries = (int)Math.Floor(((double)interval(logData[i], logData[i+1])/60) / 5);
                        missing.WriteLine($"{logData[i][0]} {logData[i][1]} {logData[i][2]} time difference {potentiallyMissingEntries}");
                    }
                
                }
            }
        }

        public static void solve() {
            Console.WriteLine("Creating database from input file...");
            List<int[]> data = readSensorLog("signal.txt");


            Console.WriteLine("Calculating time interval between first and last record...");
            int operationTime = interval(data[0],data[data.Count-1]);
            Console.WriteLine($"The interval between first and last transmission is: {operationTime}");
            printWithStyle(operationTime);

            Console.WriteLine("Calculating global maximum and minimum coordinates...");
            int[][] cornerCords = getBorders(data);
            Console.WriteLine("Corners of container rectange: Topright: X " + cornerCords[0][0] + ", Y "
                + cornerCords[0][1] + "; Bottomleft: X " +  cornerCords[1][0] + ", Y " + cornerCords[1][1]);

            Console.WriteLine("Calculating accumulated distance...");
            Console.WriteLine("Accumulated distance: {0:0.000}",accumulatedDistance(data));

            Console.WriteLine("Printing recording anomalies to \"missing.txt\"...");
            missingData(data);

            Console.WriteLine("Exiting...");

        }


        //TODO: delete from file
        //private static void Main(string[] args)
        //{
        //    List<int[]> data = readSensorLog("signal.txt");
        //
        //    int operationTime = interval(data[0],data[data.Count-1]);
        //    Console.WriteLine($"The interval between first and last transmission is:{operationTime}");
        //    printWithStyle(operationTime);
        //    
        //    int[][] cornerCords = getBorders(data);
        //    Console.WriteLine("Corners of container rectange: Topright: X " + cornerCords[0][0] + ", Y "
        //        + cornerCords[0][1] + "; Bottomleft: X " +  cornerCords[1][0] + ", Y " + cornerCords[1][1]);
        //
        //    Console.WriteLine("Megtett távolság: {0:0.000}",accumulatedDistance(data));
        //
        //    missingData(data);
        //
        //    Console.ReadKey();
        //
        //}
    }
}