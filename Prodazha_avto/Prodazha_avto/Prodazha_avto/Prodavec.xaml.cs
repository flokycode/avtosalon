using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
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
using System.IO;
using System.Collections.ObjectModel;

namespace Prodazha_avto
{
    /// <summary>
    /// Логика взаимодействия для Prodavec.xaml
    /// </summary>
    public partial class Prodavec : Window
    {

        public ObservableCollection<Car> Cars { get; set; }

        public Prodavec()
        {
            InitializeComponent();
            LoadCars();
            DataContext = this;
        }

        private void LoadCars()
        {
            using (var context = new user270_dbEntities()) // Замените YourDbContext на ваш контекст базы данных
            {
                // Запрос к базе данных с использованием LINQ
                var carsQuery = from car in context.Cars
                                join model in context.CarModel on car.model equals model.id_model
                                join brand in context.CarBrand on model.brand equals brand.id_branda
                                join img in context.CarImg on car.image equals img.id_img into imgGroup // Объединение с CarImg
                                from img in imgGroup.DefaultIfEmpty() // Левое соединение для получения изображения
                                select new Car
                                {
                                    Id = car.id_car,
                                    Brand = brand.name,
                                    description = model.description,
                                    Color = car.color,
                                    Price = car.price,
                                    Quantity = car.quantity,
                                    ModelId = model.name,
                                    modelid = model.id_model,
                                    ImagePath = img != null ? img.image : @"C:\Users\Azim\Downloads\Prodazha_avto\Prodazha_avto\Prodazha_avto\Resources\volvo.jpg" // Путь к изображению или изображение по умолчанию
                                };

                Cars = new ObservableCollection<Car>(carsQuery.ToList()); // Преобразование результата в ObservableCollection

                CarsDataGrid.ItemsSource = Cars; // Установка источника данных для DataGrid
            }
        }

      

        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            AddCarWindow addCarWindow = new AddCarWindow();
            
            if (addCarWindow.ShowDialog() == true)
            {
                LoadCars();
            }
        }

        private void EditCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarsDataGrid.SelectedItem is Car selectedCar)
            {
                EditCarWindow editCarWindow = new EditCarWindow(selectedCar);

                if (editCarWindow.ShowDialog() == true) // Проверка результата диалога
                {
                    LoadCars(); // Перезагрузка списка автомобилей после редактирования
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите автомобиль для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (CarsDataGrid.SelectedItem is Car selectedCar)
            {
                // Подтверждение удаления
                var result = MessageBox.Show($"Вы уверены, что хотите удалить автомобиль {selectedCar.Brand} {selectedCar.ModelId}?", "Подтверждение", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    using (var context = new user270_dbEntities()) // Используйте ваш контекст базы данных
                    {
                        // Находим автомобиль по ID
                        var carToDelete = context.Cars.Find(selectedCar.Id);
                        if (carToDelete != null)
                        {
                            context.Cars.Remove(carToDelete); // Удаление автомобиля из контекста
                            context.SaveChanges(); // Сохранение изменений в базе данных
                        }
                        else
                        {
                            MessageBox.Show("Автомобиль не найден в базе данных.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }

                    LoadCars(); // Перезагрузка списка автомобилей после удаления
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите автомобиль для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

      
        public class Car
        {
            public int Id { get; set; }
            public string Brand { get; set; }
            public string description { get; set; }
          
            
            public int modelid { get; set; }
            public int TypeOfCuzovId { get; set; } // ID типа кузова
            public string ModelId { get; set; } // ID модели
            public string Color { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
            public string ImagePath { get; set; } // Путь к изображению
        }

    }
}
