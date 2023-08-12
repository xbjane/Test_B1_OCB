using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace Test_B1_OCB
{
  
    /// <summary>
    /// Логика взаимодействия для ViewFilesWindow.xaml
    /// </summary>
    public partial class ViewFilesWindow : Window
    {
        public ObservableCollection<View> Views;//коллекция определяет событие CollectionChanged
     //т е может извещать внешние данные о своем изменении, удобно использовать в паре с ComboBox
        public ViewFilesWindow()
        {
            Views = new ObservableCollection<View>(); //выделяем место для коллекции перед загрузкой XAML
            InitializeComponent();
            DataContext = this;
            ListView1.ItemsSource = Views;//указываем источник данных для ListView
            foreach (var f in FilesOperations.fileInfos)//Запоняем Combobox значениями, сохраненными в прошлый раз в файле
                ComboBox.Items.Add(f.Name);
        }

        private void FileSelected(object sender, SelectionChangedEventArgs e)
        {
            FillViewCollection();//При выборе значения в ComboBox 
        }
       private void FillViewCollection()
        {
            try
            {
                Views.Clear();//очищаем коллекцию перед занесением новых данных
                Period period = FilesOperations.fileInfos[ComboBox.SelectedIndex].Period;//получаем период из файла,
                                                                                         //его используем, чтобы найти в бд все связанные записи за этот период
                using (AppContext appContext = new AppContext())
                {//работа с бд
                    var balances = (appContext.Balances
                        .Where((a) => a.Period.PeriodId == period.PeriodId)
                        .Select((f) => new { BalanceId = f.BalanceId, Period=f.Period, Account=f.Account, OpeningAmount = f.OpeningAmount ,ClosingAmount = f.ClosingAmount}))
                        .ToList() //используем анонимный тип, т к в тип модели проецировать нельзя
                        .Select(x => new Balance { BalanceId = x.BalanceId, Period = x.Period, Account = x.Account, OpeningAmount = x.OpeningAmount , ClosingAmount = x.ClosingAmount });                                       
                    foreach (var balance in balances)
                    {//с помощью Linq находим все необходимые записи и заполняем коллекцию экземплярами для вывода
                        var account = appContext.Accounts.Where((b) => b.AccountId == balance.Account.AccountId).FirstOrDefault();
                        var cashFlow = appContext.CashFlows.Where((c) => c.Account.AccountId == account.AccountId).FirstOrDefault();
                        Views.Add(new View(account.AccountNumber, balance.OpeningAmount, cashFlow.DebitAmount, cashFlow.CreditAmount, balance.ClosingAmount, account.ClassName, period.EndDate, period.StartDate));
                    }
                }           
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow=new MainWindow();
            mainWindow.Show();//переход в предыдущее окно
            Close();
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();//выход
        }
    }
}
