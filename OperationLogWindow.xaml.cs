using System.IO;
using System.Text.Json;
using System.Windows;
using System.Collections.Generic;

namespace BankApp
{
    public partial class OperationLogWindow : Window
    {
        private static List<OperationLog> operationLogs = new List<OperationLog>();

        public OperationLogWindow()
        {
            InitializeComponent();
            LoadOperationLogs();
        }

        public static void AddOperationLog(OperationLog log)
        {
            operationLogs.Add(log);
            UpdateOperationLogs(); // Вызываем обновление списка логов после добавления новой операции
        }

        private static void UpdateOperationLogs()
        {
            // Обновляем список логов
            Application.Current.Dispatcher.Invoke(() =>
            {
                OperationLogWindow currentWindow = Application.Current.Windows.OfType<OperationLogWindow>().FirstOrDefault();
                if (currentWindow != null)
                {
                    currentWindow.OperationLogListBox.Items.Clear();
                    foreach (var log in operationLogs)
                    {
                        currentWindow.OperationLogListBox.Items.Add(log.ToString());
                    }
                }
            });
        }

        private void LoadOperationLogs()
        {
            OperationLogListBox.Items.Clear();
            foreach (var log in operationLogs)
            {
                OperationLogListBox.Items.Add(log.ToString());
            }
        }

        private void SaveLogButton_Click(object sender, RoutedEventArgs e)
        {
            SaveLogsToFile();
        }

        private void LoadLogButton_Click(object sender, RoutedEventArgs e)
        {
            LoadLogsFromFile();
            LoadOperationLogs();
        }

        private void SaveLogsToFile()
        {
            string jsonString = JsonSerializer.Serialize(operationLogs);
            File.WriteAllText("operation_logs.json", jsonString);
        }

        private void LoadLogsFromFile()
        {
            if (File.Exists("operation_logs.json"))
            {
                string jsonString = File.ReadAllText("operation_logs.json");
                operationLogs = JsonSerializer.Deserialize<List<OperationLog>>(jsonString);
            }
            else
            {
                MessageBox.Show("Файл логов не найден.");
            }
        }
    }
}
