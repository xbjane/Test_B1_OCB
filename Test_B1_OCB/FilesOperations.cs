using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Test_B1_OCB
{
    internal static class FilesOperations
    {
        static string path = "C:/Users/Lenovo/source/repos/Test_B1_OCB/Test_B1_OCB/bin/Debug/files.txt";
        public static List<File> fileInfos = new List<File>();
        public static void SaveFileInfo()
        {//записываем в файл данные об excel документе
            try
            {
                using (StreamWriter sW = new StreamWriter(path, false))
                {
                    foreach (var file in fileInfos)
                    {
                        sW.WriteLine(file.Name + "," + file.Period.PeriodId.ToString() + "," + file.Period.StartDate.ToString() + "," + file.Period.EndDate.ToString());

                    }
                }
            }
           catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public static void LoadFileInfo()
        {//перед загрузкой первого окна вызывается этот метод и происходит выгрузка данных из файла
            try
            {
                using (StreamReader sR = new StreamReader(path, true))
                {
                    string line;
                    while ((line = sR.ReadLine()) != null)
                    {
                        var lines = line.Split(',');
                        fileInfos.Add(new File(lines[0], new Period(Convert.ToInt32(lines[1]), DateTime.Parse(lines[2]), DateTime.Parse(lines[3]))));
                    }
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.ToString());
            }            
            }
        }
    }
