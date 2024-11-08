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
    /// Логика взаимодействия для AddCarWindow.xaml
    /// </summary>
    public partial class AddCarWindow : Window
    {

        public Car NewCar { get; private set; }

        public AddCarWindow()
        {
            InitializeComponent();
            LoadCarTypes();
            LoadCarmodel();
        }

        private void LoadCarmodel() 
        {
            var carModel = new List<CarModel>
            {
                new CarModel{ id_model = 1, name = "Lachetti"},
                new CarModel { id_model = 2, name = "XC90" },
                new CarModel {id_model = 3, name ="e200"},
                new CarModel {id_model=4, name ="e39"}
            };
            CarModelsAdd.ItemsSource = carModel;
        }

        private void LoadCarTypes()
        {
            using (var context = new user270_dbEntities())
            {
                var carTypes = context.CarTypes.ToList(); // Получение типов кузова из базы данных
                CarTypeComboBox.ItemsSource = carTypes; // Установка источника данных для ComboBox
            }
        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (
        string.IsNullOrWhiteSpace(colortxt.Text) ||
        !decimal.TryParse(pricetxt.Text, out decimal price) ||
        !int.TryParse(koltxt.Text, out int quantity) ||
        CarTypeComboBox.SelectedItem == null ||
        CarModelsAdd.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.");
                return;
            }

            // Создание нового объекта Car
            NewCar = new Car
            {
                modelid = ((CarModel)CarModelsAdd.SelectedItem).id_model,
                Color = colortxt.Text,
                Price = price,
                Quantity = quantity,
                ImagePath = imgtxt.Text // Путь к изображению из текстового поля
            };

            // Сохранение нового изображения в таблицу CarImg
            int? imageId = null;
            using (var context = new user270_dbEntities())
            {
                // Добавление изображения в таблицу CarImg
                var carImage = new CarImg
                {
                    image = NewCar.ImagePath // Здесь предполагается, что вы сохраняете путь к изображению
                };

                context.CarImg.Add(carImage);
                context.SaveChanges(); // Сохраняем изменения, чтобы получить ID

                // Получаем ID добавленного изображения
                imageId = carImage.id_img;

                // Теперь добавляем новый автомобиль в таблицу Cars
                var carToAdd = new Cars
                {
                    id_typeofcuzov = ((CarTypes)CarTypeComboBox.SelectedItem).id_type, // Получение ID типа кузова
                    model = NewCar.modelid,
                    color = NewCar.Color,
                    price = NewCar.Price,
                    quantity = NewCar.Quantity,
                    image = imageId // Устанавливаем ID изображения
                };

                context.Cars.Add(carToAdd);
                context.SaveChanges(); // Сохранение изменений в базе данных
            }

            DialogResult = true; // Установка результата диалога перед закрытием окна
            Close(); // Закрытие ок
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
