using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace my_app_connect
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class MainPage : Page
    {

        public class Student
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }


        private MobileServiceCollection<Student, Student> items;
        private IMobileServiceTable<Student> itemsTable = App.MobileService.GetTable<Student>();
        public MainPage()
        {
            this.InitializeComponent();
           
        }


        private async Task InsertTodoItem(Student student)
        {
            // This code inserts a new TodoItem into the database. When the operation completes
            // and Mobile App backend has assigned an Id, the item is added to the CollectionView.
            await itemsTable.InsertAsync(student);
            await new MessageDialog("Data Saved").ShowAsync();
        }

        private async void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var item = new Student { Name = TextInput.Text };
            await InsertTodoItem(item);
        }

        private async Task RefreshTodoItems()
        {
            MobileServiceInvalidOperationException exception = null;
            try
            {
                // This code refreshes the entries in the list view by querying the TodoItems table.
                // The query excludes completed TodoItems.
                items = await
                itemsTable.ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                ListItems.ItemsSource = items;
               
            }
        }

        private async void ButtonRefresh_Click(object sender, RoutedEventArgs e)
        {
           await RefreshTodoItems();
        }
    }
}
