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
    /// Логика взаимодействия для EditCarWindow.xaml
    /// </summary>
    public partial class EditCarWindow : Window
    {

        private Car _car; // Хранение редактируемого автомобиля

        public EditCarWindow(Car car)
        {
            InitializeComponent();
            _car = car;

            // Заполнение полей данными автомобиля
            ColorTextBox.Text = _car.Color;
            PriceTextBox.Text = _car.Price.ToString();
            QuantityTextBox.Text = _car.Quantity.ToString();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на валидность ввода
            if (!decimal.TryParse(PriceTextBox.Text, out decimal price) ||
                !int.TryParse(QuantityTextBox.Text, out int quantity))
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.");
                return;
            }

            // Обновление свойств автомобиля
            _car.Color = ColorTextBox.Text;
            _car.Price = price;
            _car.Quantity = quantity;

            // Сохранение изменений в базе данных
            using (var context = new user270_dbEntities())
            {
                var carToUpdate = context.Cars.Find(_car.Id); // Находим автомобиль по ID
                if (carToUpdate != null)
                {
                    carToUpdate.color = _car.Color;
                    carToUpdate.price = _car.Price;
                    carToUpdate.quantity = _car.Quantity;

                    context.SaveChanges(); // Сохранение изменений в базе данных
                }
                else
                {
                    MessageBox.Show("Автомобиль не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            DialogResult = true; // Установка результата диалога
            Close(); // Закрытие окна
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(); // Закрытие окна без сохранения изменений
        }
    }
}
