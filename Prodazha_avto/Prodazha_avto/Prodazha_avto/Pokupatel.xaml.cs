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
using System.Windows.Shapes;
using static Prodazha_avto.Prodavec;

namespace Prodazha_avto
{
    /// <summary>
    /// Логика взаимодействия для Pokupatel.xaml
    /// </summary>
    public partial class Pokupatel : Window
    {
        public Pokupatel()
        {
            InitializeComponent();
            LoadCars();
        }


        private void LoadCars()
        {
            using (var context = new user270_dbEntities())
            {
                var carsQuery = from car in context.Cars
                                select new CarViewModel // Используем CarViewModel вместо анонимного типа
                                {
                                    Id = car.id_car,
                                    Brand = car.CarModel.CarBrand.name,
                                    ModelName = car.CarModel.name,
                                    Color = car.color,
                                    Price = car.price,
                                    description = car.CarModel.description,
                                    ImagePath = car.CarImg.image // Убедитесь, что у вас есть это свойство
                                };

                CarsDataGrid.ItemsSource = carsQuery.ToList(); // Установка источника данных для DataGrid
            }
        }


        private void CarsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CarsDataGrid.SelectedItem is CarViewModel selectedCar)
            {
                CarDetailsTextBlock.Text = $"Марка: {selectedCar.Brand}\nМодель: {selectedCar.ModelName}\nЦвет: {selectedCar.Color}\nЦена: {selectedCar.Price} руб.";
            }
            else
            {
                CarDetailsTextBlock.Text = string.Empty; // Очистка деталей при отсутствии выбора
            }
        }

        private void PurchaseButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarsDataGrid.SelectedItem is CarViewModel selectedCar)
            {
                // Проверка наличия доступных автомобилей
                using (var context = new user270_dbEntities())
                {
                    var carToUpdate = context.Cars.Find(selectedCar.Id);
                    if (carToUpdate != null)
                    {
                        if (carToUpdate.quantity <= 0)
                        {
                            MessageBox.Show("Извините, данный автомобиль недоступен для покупки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        // Логика оформления покупки автомобиля
                        var result = MessageBox.Show($"Вы уверены, что хотите купить автомобиль {selectedCar.Brand} {selectedCar.ModelName} за {selectedCar.Price} руб.?", "Подтверждение", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            // Уменьшаем количество на 1
                            carToUpdate.quantity -= 1;

                            // Сохраняем изменения в базе данных
                            context.SaveChanges();

                            // Получаем отзыв из текстового поля
                            string review = ReviewTextBox.Text;

                            // Добавление записи о продаже
                            var sale = new Sales
                            {
                                id_car = carToUpdate.id_car,
                                id_user = App.CurrentUser.id_user, // Используем ID текущего пользователя
                                sale_date = DateTime.Now,
                                sale_price = selectedCar.Price,
                                reviews = string.IsNullOrWhiteSpace(review) ? null : review // Устанавливаем отзыв или null, если пусто
                            };

                            context.Sales.Add(sale);
                            context.SaveChanges(); // Сохраняем изменения в таблице Sales

                            MessageBox.Show($"Вы успешно купили автомобиль {carToUpdate.CarModel.CarBrand.name} {carToUpdate.CarModel.name} за {carToUpdate.price} руб.", "Успех", MessageBoxButton.OK);
                            LoadCars(); // Обновляем список автомобилей
                        }
                    }
                    else
                    {
                        MessageBox.Show("Автомобиль не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите автомобиль для оформления покупки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public class CarViewModel
        {
            public int Id { get; set; }
            public string Brand { get; set; }
            public string ModelName { get; set; }
            public string Color { get; set; }
            public decimal Price { get; set; }

            public string description { get; set; }
            public string ImagePath { get; set; } // Путь к изображению
        }
    }
}
