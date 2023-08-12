using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel=Microsoft.Office.Interop.Excel;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using System.Windows.Media.Media3D;

namespace Test_B1_OCB
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>   
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();//выход
        }
        private void LoadExcelFileButtonClick(object sender, RoutedEventArgs e)
        {
            ParseExcel();
        }
        private void ParseExcel()
        {//берём необходимую информацию из excel и переносим в тип модели
            OpenFile(out string fileName, out string path);//выбираем файл .xls
                try
                {
                    Excel.Application excel = new Excel.Application();//создаём экзмемпляр приложения excel
                    Excel.Workbook WB = excel.Workbooks.Open(path);//открываем книгу по указанному пути
                    Excel._Worksheet WS = WB.Sheets[1];
                    Excel.Range range = WS.UsedRange;//используем весь диапазон ячеек данного первого листа
                    int rowsNum = range.Rows.Count;//общее число строк
                    SearchAllFileClasses(range,out List<ClassInfo> classInfo);//находим все классы, определенные в ОВБС                                                                              //предполагается что могут появится новые классы в будущем
                    FormDateTime(range, out DateTime firstDate, out DateTime lastDate);//определяем за какой период
                    Period period = new Period(firstDate, lastDate);
                    FilesOperations.fileInfos.Add(new File(fileName, period));//заносим информайию о загруженном файле в тестовый файл для будущего использования
                    using (AppContext appContext = new AppContext())
                    {
                    appContext.Periods.Add(period);
                        for (int i = 0; i < classInfo.Count; i++) //сразу разделяем информацию по классам
                        {
                            for (int j = classInfo[i].Row + 1; j < ((i + 1) < classInfo.Count ? classInfo[i + 1].Row : rowsNum); j++)
                            {//переход между строками с определением класса, либо уже до конца листа
                                if (int.TryParse(range.Cells[j, 1].Value.ToString(), out int value) && value > 1000)
                                {//если в первом столбце лежит натуральное ЧИСЛО больше 1000, значит это Id,на него ориентируемся
                                    Account account = new Account(value, classInfo[i].Note, range.Cells[j, 3].Value == 0);                                     
                                    appContext.Accounts.Add(account);
                                    GetAmounts(range, j, out decimal debitAmount, out decimal creditAmount, out decimal balanceAmount, out decimal closingAmount);
                                   //получаем необходимые счета
                                    appContext.CashFlows.Add(new CashFlow(lastDate, debitAmount,creditAmount, account));
                                    appContext.Balances.Add(new Balance(period, account, balanceAmount, closingAmount));
                                }//записываем экземпляры модеои в бд
                            }
                        }
                        appContext.SaveChanges();//сохраняем изменения
                    }
                    WB.Close(false); //закрываем книгу, изменения не сохраняем
                    excel.Quit();//заверщается работа приложения
                    MessageBox.Show("Operation was successfully completed.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }          
        }
        private void OpenFile(out string fileName, out string path)
        {//открываем файл с проверками, действительно ли он был правильно выбран и открыт
            OpenFileDialog file= new OpenFileDialog();
            bool? success = file.ShowDialog();
            if (success == true)
            {
                path = file.FileName;
                fileName = file.SafeFileName;
                if (fileName.Split('.')[1] != "xls")
                {
                    throw new Exception("Wrong format file.");
                }
            }
            else
            {
                path = null;
                fileName = null;
                throw new Exception("File is not selected.");
            }
        }

        private void FormDateTime(Excel.Range range, out DateTime firstDate, out DateTime lastDate)
        {//формируем даты периода из строки 
            try
            {
                string[] s = range.Cells[3, 1].Value.ToString().Split(' ');
                firstDate = DateTime.Parse(s[3]);
                lastDate = DateTime.Parse(s[5]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                firstDate = new DateTime();
                lastDate = new DateTime();
                throw new Exception("Dates are not set. Use the correct format document.");
            }         
        }
        private void GetAmounts(Excel.Range range, int j, out decimal debitAmount, out decimal creditAmount, out decimal balanceAmount, out decimal closingBalance)
        {//высчитываем с приведением типов входящее сальдо и исходящее сальдо
            try
            {
                debitAmount = (decimal)(range.Cells[j, 4].Value);
                creditAmount = (decimal)((range.Cells[j, 5]).Value);
                decimal activeAmount = (decimal)range.Cells[j, 2].Value;
                balanceAmount = activeAmount != 0 ? activeAmount : (decimal)(range.Cells[j, 3].Value);
                activeAmount = (decimal)range.Cells[j, 6].Value;
                closingBalance = activeAmount != 0 ? activeAmount : (decimal)(range.Cells[j, 3].Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); 
                debitAmount=0;
                creditAmount = 0; 
                balanceAmount=0;
                throw new Exception("Amounts miscalculated. Use the correct format document.");
            }
        }
        private void SearchAllFileClasses(Excel.Range range,out List<ClassInfo> classInfo)
        {//Находим все определенные в файле классы
            classInfo = new List<ClassInfo>();
            try
            {
                Excel.Range searchRangeCurrent = range.Find("КЛАСС ", Type.Missing, XlFindLookIn.xlValues,
                Excel.XlLookAt.xlPart, Excel.XlSearchOrder.xlByRows,
                Excel.XlSearchDirection.xlNext, false);
                int firstRow = searchRangeCurrent.Row;
                do
                {
                    classInfo.Add(new ClassInfo(searchRangeCurrent.Row, searchRangeCurrent.Value));
                    Excel.Range range1 = range.FindNext(searchRangeCurrent);
                    searchRangeCurrent = range1;
                } while (searchRangeCurrent.Row != firstRow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw new Exception("Classes are not defined. Use the correct format document.");
            }           
        }
        private void ViewFileInformationButtonClick(object sender, RoutedEventArgs e)
        {
            ViewFilesWindow viewFilesWindow = new ViewFilesWindow();
            viewFilesWindow.Show();//открываем окно просмотра файлов
            Close();
        }
    }
}
