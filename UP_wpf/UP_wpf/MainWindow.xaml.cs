using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using System.Xml.Linq;
using UP_wpf;

namespace UP_wpf
{
    public partial class MainWindow : Window
    {
        private Shop shop = new Shop();

        public MainWindow()
        {
            InitializeComponent();

            // Добавим товары
            shop.CreateProduct("Кола", 85, 200);
            shop.CreateProduct("Сок Добрый", 100, 50);
            shop.CreateProduct("Чипсы", 65, 30);
            shop.CreateProduct("Шоколад", 120, 15);
            shop.CreateProduct("Печенье", 45, 0); // Нет в наличии

            UpdateUI();
        }

        private void UpdateUI()
        {
            UpdateProductList();
            UpdateProfitDisplay();
            UpdateProductCombo();
        }

        private void UpdateProductList()
        {
            lstProducts.Items.Clear();

            foreach (var item in shop.GetAllProducts())
            {
                Brush stockColor;
                if (item.Value == 0)
                    stockColor = Brushes.Red;
                else if (item.Value < 10)
                    stockColor = Brushes.Orange;
                else
                    stockColor = Brushes.Green;

                var displayItem = new
                {
                    Name = item.Key.Name,
                    Price = item.Key.Price,
                    Count = item.Value,
                    StockColor = stockColor
                };
                lstProducts.Items.Add(displayItem);
            }
        }

        private void UpdateProfitDisplay()
        {
            txtTotalProfit.Text = $"{shop.GetTotalProfit():F2} руб.";
            txtTodayRevenue.Text = $"{shop.GetTodayRevenue():F2} руб.";
            txtTotalSold.Text = $"{shop.GetTotalSoldCount()} шт.";
        }

        private void UpdateProductCombo()
        {
            cmbProducts.Items.Clear();
            foreach (var product in shop.GetAllProducts())
            {
                if (product.Value > 0) // Показываем только товары в наличии
                {
                    cmbProducts.Items.Add($"{product.Key.Name} ({product.Value} шт.)");
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    ShowMessage("Введите название товара!", false);
                    return;
                }

                if (!decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
                {
                    ShowMessage("Введите корректную цену!", false);
                    return;
                }

                if (!int.TryParse(txtCount.Text, out int count) || count <= 0)
                {
                    ShowMessage("Введите корректное количество!", false);
                    return;
                }

                string productName = txtName.Text;
                shop.CreateProduct(productName, price, count);

                txtName.Clear();
                txtPrice.Clear();
                txtCount.Text = "1";

                UpdateUI();
                ShowMessage($"Товар '{productName}' добавлен! ({count} шт.)", true);
            }
            catch (Exception ex)
            {
                ShowMessage($"Ошибка: {ex.Message}", false);
            }
        }

        private void btnSell_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cmbProducts.Text))
                {
                    ShowMessage("Выберите товар для продажи!", false);
                    return;
                }

                string productName = cmbProducts.Text.Split('(')[0].Trim();

                if (!int.TryParse(txtSellCount.Text, out int count) || count <= 0)
                {
                    ShowMessage("Введите корректное количество!", false);
                    return;
                }

                var result = shop.Sell(productName, count);

                if (result.Success)
                {
                    UpdateUI();
                    ShowMessage($"Продано {count} шт. товара '{productName}'. " +
                              $"Сумма: {result.TotalAmount:F2} руб. " +
                              $"Прибыль: {result.Profit:F2} руб.", true);
                    txtSellCount.Text = "1";
                }
                else
                {
                    ShowMessage(result.Message, false);
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Ошибка: {ex.Message}", false);
            }
        }

        private void SellOne_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as System.Windows.Controls.Button;
            string productName = button.Tag.ToString();

            var result = shop.Sell(productName, 1);

            if (result.Success)
            {
                UpdateUI();
                ShowMessage($" Продан 1 шт. товара '{productName}'. " +
                          $"Прибыль: {result.Profit:F2} руб.", true);
            }
            else
            {
                ShowMessage(result.Message, false);
            }
        }

        private void ShowMessage(string message, bool isSuccess)
        {
            txtInfo.Text = message;
            txtInfo.Foreground = isSuccess ? Brushes.Green : Brushes.Red;
        }

        // Методы меню
        private void About_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Магазин\nВерсия какая нибудь\n\nФункции:\n" +
                          "• функция 1\n" +
                          "• Учет прибыли\n" +
                          "• Статистика продаж\n" +
                          "• Инвентаризация",
                          "че ниудб то ",
                          MessageBoxButton.OK,
                          MessageBoxImage.Information);
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ShowAllProducts_Click(object sender, RoutedEventArgs e)
        {
            var products = shop.GetAllProducts();
            string list = "Все товары:\n\n";
            foreach (var item in products)
            {
                list += $"• {item.Key.Name}: {item.Value} шт., {item.Key.Price} руб.\n";
            }
            MessageBox.Show(list, "Список товаров", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowLowStock_Click(object sender, RoutedEventArgs e)
        {
            var lowStock = shop.GetLowStockProducts(10);
            if (lowStock.Count == 0)
            {
                MessageBox.Show("Товаров с остатком меньше 10 нет!",
                              "Остатки", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                string list = "Товары с остатком меньше 10:\n\n";
                foreach (var item in lowStock)
                {
                    list += $"• {item.Key.Name}: {item.Value} шт.\n";
                }
                MessageBox.Show(list, "Критический остаток",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void ShowMostExpensive_Click(object sender, RoutedEventArgs e)
        {
            var product = shop.GetMostExpensiveProduct();
            if (product != null)
            {
                MessageBox.Show($"Самый дорогой товар:\n\n{product.Name}\nЦена: {product.Price} руб.",
                              "Максимальная цена", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ShowCheapest_Click(object sender, RoutedEventArgs e)
        {
            var product = shop.GetCheapestProduct();
            if (product != null)
            {
                MessageBox.Show($"Самый дешевый товар:\n\n{product.Name}\nЦена: {product.Price} руб.",
                              "Минимальная цена", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ShowTotalProfit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Общая прибыль магазина:\n\n{shop.GetTotalProfit():F2} руб.",
                          "Прибыль", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowTodayProfit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Прибыль за сегодня:\n\n{shop.GetProfitByPeriod(ProfitPeriod.Today):F2} руб.",
                          "Прибыль за сегодня", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowMonthProfit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Прибыль за месяц:\n\n{shop.GetProfitByPeriod(ProfitPeriod.Month):F2} руб.",
                          "Прибыль за месяц", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowTotalRevenue_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Общая выручка магазина:\n\n{shop.GetTotalRevenue():F2} руб.",
                          "Выручка", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowProductCount_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Количество уникальных товаров:\n\n{shop.GetProductCount()} шт.",
                          "Статистика", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowTotalUnits_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Общее количество товаров на складе:\n\n{shop.GetTotalUnits()} шт.",
                          "Статистика", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void ShowAveragePrice_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Средняя цена товара:\n\n{shop.GetAveragePrice():F2} руб.",
                          "Статистика", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Инструкция по использованию:\n\n" +
                          "1. Добавление товара: введите название, цену и количество\n" +
                          "2. Продажа: выберите товар, укажите количество\n" +
                          "3. Быстрая продажа: нажмите кнопку 'Продать 1 шт.' у товара\n" +
                          "4. Меню: просмотр статистики и отчетов\n\n" +
                          "Прибыль = Выручка - Себестоимость\n" +
                          "Себестоимость фиксированная: 70% от цены",
                          "Помощь", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}