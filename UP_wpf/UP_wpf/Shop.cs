using System;
using System.Collections.Generic;
using System.Linq;
using UP_wpf;


namespace UP_wpf
{
    // Результат 
    public class SaleResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Profit { get; set; }
    }

    // Периоды для расчета прибыли
    public enum ProfitPeriod
    {
        Today,
        Week,
        Month,
        All
    }

    // Запись о продаже
    public class SaleRecord
    {
        public string ProductName { get; set; }
        public int Count { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Profit { get; set; }
        public DateTime SaleDate { get; set; }

        public SaleRecord(string productName, int count, decimal totalAmount, decimal profit)
        {
            ProductName = productName;
            Count = count;
            TotalAmount = totalAmount;
            Profit = profit;
            SaleDate = DateTime.Now;
        }
    }

    public class Shop
    {
        private Dictionary<Product, int> products = new Dictionary<Product, int>();
        private List<SaleRecord> saleHistory = new List<SaleRecord>();
        private decimal totalProfit = 0;
        private decimal costPercentage = 0.7m; // Себестоимость 70% от цены

        // Создать и добавить товар
        public void CreateProduct(string name, decimal price, int count)
        {
            var existingProduct = FindByName(name);

            if (existingProduct != null)
            {
                products[existingProduct] += count;
            }
            else
            {
                Product product = new Product(name, price);
                products.Add(product, count);
            }
        }

        // Продажа товара (с проверкой количества)
        public SaleResult Sell(string productName, int count)
        {
            Product product = FindByName(productName);

            if (product == null)
                return new SaleResult { Success = false, Message = $"Товар '{productName}' не найден!" };

            if (products[product] < count)
                return new SaleResult { Success = false, Message = $"Недостаточно товара! В наличии: {products[product]} шт." };

            if (count <= 0)
                return new SaleResult { Success = false, Message = "Количество должно быть больше 0!" };

            decimal totalAmount = product.Price * count;
            decimal cost = product.Price * costPercentage * count;
            decimal profit = totalAmount - cost;

      
            products[product] -= count;

            // Добавляем прибыль
            totalProfit += profit;

            // Записываем продажу в историю
            saleHistory.Add(new SaleRecord(productName, count, totalAmount, profit));

            return new SaleResult
            {
                Success = true,
                Message = "Успешно",
                TotalAmount = totalAmount,
                Profit = profit
            };
        }

        // Найти товар по имени 
        public Product FindByName(string name)
        {
            foreach (var product in products.Keys)
            {
                if (product.Name == name)
                    return product;
            }
            return null;
        }

        // Получить все товары 
        public Dictionary<Product, int> GetAllProducts()
        {
            return new Dictionary<Product, int>(products);
        }

        // Получить общую прибыль
        public decimal GetTotalProfit()
        {
            return totalProfit;
        }

        // прибыль за период 
        public decimal GetProfitByPeriod(ProfitPeriod period)
        {
            DateTime now = DateTime.Now;
            DateTime startDate;

            switch (period)
            {
                case ProfitPeriod.Today:
                    startDate = now.Date;
                    break;
                case ProfitPeriod.Week:
                    startDate = now.AddDays(-7);
                    break;
                case ProfitPeriod.Month:
                    startDate = now.AddMonths(-1);
                    break;
                default:
                    startDate = DateTime.MinValue;
                    break;
            }

            decimal total = 0;
            foreach (var sale in saleHistory)
            {
                if (sale.SaleDate >= startDate)
                    total += sale.Profit;
            }
            return total;
        }

        // получить прибыль за период с указанием даты
        public decimal GetProfitByPeriod(DateTime startDate, DateTime endDate)
        {
            decimal total = 0;
            foreach (var sale in saleHistory)
            {
                if (sale.SaleDate >= startDate && sale.SaleDate <= endDate)
                    total += sale.Profit;
            }
            return total;
        }

        // Получить общую выручку
        public decimal GetTotalRevenue()
        {
            decimal total = 0;
            foreach (var sale in saleHistory)
            {
                total += sale.TotalAmount;
            }
            return total;
        }

        // Получить выручку за сегодня
        public decimal GetTodayRevenue()
        {
            decimal total = 0;
            DateTime today = DateTime.Now.Date;
            foreach (var sale in saleHistory)
            {
                if (sale.SaleDate.Date == today)
                    total += sale.TotalAmount;
            }
            return total;
        }

        // Получить общее количество проданных товаров
        public int GetTotalSoldCount()
        {
            int total = 0;
            foreach (var sale in saleHistory)
            {
                total += sale.Count;
            }
            return total;
        }

        // Количество уникальных товаров
        public int GetProductCount()
        {
            return products.Count;
        }

        // Общее количество единиц на складе
        public int GetTotalUnits()
        {
            int total = 0;
            foreach (var count in products.Values)
            {
                total += count;
            }
            return total;
        }

        // Средняя цена товара
        public decimal GetAveragePrice()
        {
            if (products.Count == 0) return 0;

            decimal total = 0;
            foreach (var product in products.Keys)
            {
                total += product.Price;
            }
            return total / products.Count;
        }

        // Товары с остатком меньше указанного
        public Dictionary<Product, int> GetLowStockProducts(int threshold)
        {
            Dictionary<Product, int> result = new Dictionary<Product, int>();
            foreach (var item in products)
            {
                if (item.Value < threshold && item.Value > 0)
                {
                    result.Add(item.Key, item.Value);
                }
            }
            return result;
        }

        // Самый дорогой товар
        public Product GetMostExpensiveProduct()
        {
            if (products.Count == 0) return null;

            Product mostExpensive = null;
            decimal maxPrice = -1;

            foreach (var product in products.Keys)
            {
                if (product.Price > maxPrice)
                {
                    maxPrice = product.Price;
                    mostExpensive = product;
                }
            }
            return mostExpensive;
        }

        // Самый дешевый товар
        public Product GetCheapestProduct()
        {
            if (products.Count == 0) return null;

            Product cheapest = null;
            decimal minPrice = decimal.MaxValue;

            foreach (var product in products.Keys)
            {
                if (product.Price < minPrice)
                {
                    minPrice = product.Price;
                    cheapest = product;
                }
            }
            return cheapest;
        }

        // История продаж
        public List<SaleRecord> GetSaleHistory()
        {
            return new List<SaleRecord>(saleHistory);
        }

        // Изменить процент себестоимости
        public void SetCostPercentage(decimal percentage)
        {
            if (percentage >= 0 && percentage <= 1)
                costPercentage = percentage;
        }
    }
}