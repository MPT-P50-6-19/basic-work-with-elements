using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace basic_work_with_elements
{
    public class ListView
    {
        public string MarkaName { get; set; }
        public string ModelName { get; set; }
        public string price { get; set; }
        
    }
    
    public partial class MainWindow
    {
         List<ListView> menu = new List<ListView>();
        private string name = "";
        public MainWindow()
        {
            InitializeComponent();
            menu.Add(new ListView(){MarkaName = "Первая", ModelName = "это модель", price = "123"});
            ListElement.Items.Add("Первая");
            menu.Add(new ListView(){MarkaName = "Вторая", ModelName = "это модель", price = "574"});
            ListElement.Items.Add("Вторая");
            menu.Add(new ListView(){MarkaName = "Третья", ModelName = "это модель", price = "122353"});
            ListElement.Items.Add("Третья");
        }

        private void deleteClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (name == "") return;
            menu.Remove(getElement(marka:name));
            ListElement.Items.Remove(ListElement.SelectedItem);
            reset(all:true);
        }
        
        private void changeShow(object sender, RoutedEventArgs routedEventArgs)
        {
            if (name == "") return;
            var element = getElement(marka:name);
            mark.Text = element.MarkaName;
            modl.Text = element.ModelName;
            price.Text = element.price;
            showButtonSave();
            showInput();
        }
        
        private void createShow(object sender, RoutedEventArgs routedEventArgs)
        {
            reset(all:true);
            showButtonSave();
            showInput();
            add.IsEnabled = false;
        }
        
        private void saveClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (mark.Text == "" || modl.Text == "" || price.Text == "")
            {
                showError("Указаны не все параметры");
                return;
            }
            if (!Int32.TryParse(price.Text,out _))
            {
                showError("Цена указана не числом");
                return;
            }
            bool isCreate = menu.Count(vari => vari.MarkaName == name)==0;
            if (isCreate)
            {
                menu.Add(new ListView{MarkaName = mark.Text, ModelName = modl.Text, price = price.Text});
                ListElement.Items.Add(mark.Text);
            }
            else
            {
                var element = getElement(name);
                element.MarkaName = mark.Text;
                element.ModelName = mark.Text;
                element.price = price.Text;
            }
            reset(all: true);
        }
        
        private void choiceElement(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            reset();
            name = (string)ListElement.SelectedItem;
            foreach (var vari in menu.Where(vari => vari.MarkaName==name))
            {
                vivod.Content = "\nМарка: "+vari.MarkaName+"\nМодель: "+vari.ModelName+"\nЦена: "+vari.price;
            }
        }

        private void reset(bool all=false)
        {
            mark.Text = "";
            modl.Text = "";
            price.Text = "";
            vivod.Content = "";
            save.Visibility = Visibility.Hidden;
            save.IsEnabled = false;
            del.IsEnabled=true;
            red.IsEnabled = true;
            add.IsEnabled = true;
            errorLabel.Content = "";
            hidenInput();
            if (all)
            {
                name = "";
                ListElement.SelectedIndex = -1;
                del.IsEnabled=false;
                red.IsEnabled = false;
            }
        }

        private void showButtonSave()
        {
            save.Visibility = Visibility.Visible;
            save.IsEnabled = true;
        }

        private ListView getElement(string marka)
        {
            return menu.Find(vari => vari.MarkaName == marka);
        }

        private void showError(string error)
        {
            errorLabel.Content = error;
        }

        private void showInput()
        {
            modl.IsEnabled = true;
            mark.IsEnabled = true;
            price.IsEnabled = true;
        }
        
        private void hidenInput()
        {
            modl.IsEnabled = false;
            mark.IsEnabled = false;
            price.IsEnabled = false;
        }
    }
}