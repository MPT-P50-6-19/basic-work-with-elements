using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace basic_work_with_elements
{
    public class Book
    {
        public int id { get; set; }
        
        public string name { get; set; }
        
        public int authorID  { get; set; }
        
        public string content { get; set; }
        
        public int genreID  { get; set; }
        
        public int year_release  { get; set; }
        
    }
    
    public partial class MainWindow
    {
         List<Book> booksListViews = new List<Book>();
        private int bookID;
        private DB db = new DB("localhost","project","mysql","mysql");
        public MainWindow()
        {
            InitializeComponent();
            updateList("SELECT * FROM `book`");
        }    

        private void deleteClick(object sender, RoutedEventArgs routedEventArgs)
        {
            if (bookID == -1) return;
            db.execute($"DELETE FROM `book` WHERE `id`={bookID}");
            updateList("SELECT * FROM `book`");
            reset(all:true);
        }
        
        private void changeShow(object sender, RoutedEventArgs routedEventArgs)
        {
            if (bookID == -1) return;
            var element = getElement(id:bookID);
            nameBook.Text = element.name;
            string author = (string)db.execute($"SELECT `name` FROM `author` WHERE `id`={element.authorID}")[0]["name"];
            string genre = (string)db.execute($"SELECT `name` FROM `genre` WHERE `id`={element.genreID}")[0]["name"];
            nameAuthor.Text = author;
            nameGenre.Text = genre;
            yearRelease.Text = element.year_release.ToString();
            nameAuthorList.SelectedItem = author;
            nameGenreList.SelectedItem = genre;
            content.Text = element.content;
            showButtonSave();
            showInput();
        }
        
        private void createShow(object sender, RoutedEventArgs routedEventArgs)
        {
            reset(all:true);
            showButtonSave();
            showInput();
            createButton.IsEnabled = false;
        }
        
        private void saveClick(object sender, RoutedEventArgs routedEventArgs)
        {
            string AuthorName = (string) nameAuthorList.SelectedItem ?? "";
            AuthorName = nameAuthor.Text==""?AuthorName:nameAuthor.Text;
            string GenreName = (string) nameGenreList.SelectedItem ?? "";
            GenreName = nameGenre.Text == "" ? GenreName : nameGenre.Text;
            if (nameBook.Text == "" || AuthorName == "" || GenreName == "" || yearRelease.Text == "" || content.Text == "")
            {
                showError("Указаны не все параметры");
                return;
            }
            if (!Int32.TryParse(yearRelease.Text,out _))
            {
                showError("Год указан не числом");
                return;
            }
            bool isCreate = booksListViews.Count(vari => vari.id == bookID)==0;
            if (isCreate)
            {
                db.execute($"INSERT INTO `book`(`name`, `authorID`, `content`, `genreID`, `year_release`) VALUES ('{nameBook.Text}',{getAuthorID(AuthorName)},'{content.Text}',{getGenreID(GenreName)},{yearRelease.Text})");
            }
            else
            {
                db.execute($"UPDATE `book` SET `name`='{nameBook.Text}',`authorID`={getAuthorID(AuthorName)},`content`='{content.Text}',`genreID`={getGenreID(GenreName)},`year_release`={yearRelease.Text} WHERE `id`={bookID}");
            }
            updateList("SELECT * FROM `book`");
            reset(all: true);
        }
        
        private void searchClick(object sender, RoutedEventArgs routedEventArgs)
        {
            string nameBook = nameBookSearch.Text != "" ?nameBookSearch.Text:"";
            var yearRelease = yearReleaseSearch.SelectedItem ?? "";
            var nameAuthorList = nameAuthorListSearch.SelectedItem ?? "";
            var nameGenreList = nameGenreListSearch.SelectedItem ?? "";
            if (nameBook == "" && yearRelease == "" && nameAuthorList == "" && nameGenreList == "")
            {
                updateList("SELECT * FROM `book`");
                return;
            }
            string sql = "SELECT * FROM `book` WHERE ";
            sql+= nameBook!=""?$"`name` LIKE '%{nameBook}%' AND ":"";
            sql+= yearRelease!=""?$"`year_release`={yearRelease} AND ":"";
            sql+= nameAuthorList!=""?$"`authorID`={getAuthorID(nameAuthorList.ToString())} AND ":"";
            sql+= nameGenreList!=""?$"`genreID`={getGenreID(nameGenreList.ToString())} AND ":"";
            if (sql.Substring(sql.Length-5)==" AND ")
            {
                sql = sql.Substring(0,sql.Length-5);
            }
            updateList(sql, false);
        }
        
        private void choiceElement(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            if (ListElement.SelectedItem==null) return;
            reset();
            bookID = booksListViews[Int32.Parse(((string)ListElement.SelectedItem).Split('.')[0])-1].id;
            foreach (var book in booksListViews.Where(vari => vari.id==bookID))
            {
                string autor = (string)db.execute($"SELECT `name` FROM `author` WHERE `id`={book.authorID}")[0]["name"];
                string genre = (string)db.execute($"SELECT `name` FROM `genre` WHERE `id`={book.genreID}")[0]["name"];
                BlockInfo.Text = $"\nНазвание книги: {book.name}\nАвтор: {autor}\nЖанр: {genre}\nГод релиза: {book.year_release}\nТекст:\n{book.content}";
            }
        }

        private void reset(bool all=false)
        {
            nameBook.Text = "";
            nameAuthor.Text = "";
            nameGenre.Text = "";
            yearRelease.Text = "";
            content.Text = "";
            BlockInfo.Text = "";
            saveButton.Visibility = Visibility.Hidden;
            saveButton.IsEnabled = false;
            deleteButton.IsEnabled=true;
            changeButton.IsEnabled = true;
            createButton.IsEnabled = true;
            errorLabel.Content = "";
            hidenInput();
            if (all)
            {
                bookID = -1;
                ListElement.SelectedIndex = -1;
                deleteButton.IsEnabled=false;
                changeButton.IsEnabled = false;
            }
        }

        private void showButtonSave()
        {    
            saveButton.Visibility = Visibility.Visible;
            saveButton.IsEnabled = true;
        }

        private Book getElement(int id)
        {
            return booksListViews.Find(vari => vari.id == id);
        }

        private void showError(string error)
        {
            errorLabel.Content = error;
        }

        private void showInput()
        {
            nameBook.IsEnabled = true;
            nameAuthor.IsEnabled = true;
            nameGenre.IsEnabled = true;
            yearRelease.IsEnabled = true;
            content.IsEnabled = true;
            nameAuthorList.IsEnabled = true;
            nameGenreList.IsEnabled = true;
        }
        
        private void hidenInput()
        {
            nameBook.IsEnabled = false;
            nameAuthor.IsEnabled = false;
            nameGenre.IsEnabled = false;
            yearRelease.IsEnabled = false;
            content.IsEnabled = false;
            nameAuthorList.IsEnabled = false;
            nameGenreList.IsEnabled = false;
        }

        private int getAuthorID(string author)
        {
            var response = db.execute($"SELECT * FROM `author` WHERE `name`='{author}'");
            if (response.Count==0)
            {
                db.execute($"INSERT INTO `author`(`name`) VALUES ('{author}')");
                response = db.execute($"SELECT * FROM `author` WHERE `name`='{author}'");
            }
            return (int)response[0]["id"];
        }
            
        private int getGenreID(string genre)
        {
            var response = db.execute($"SELECT * FROM `genre` WHERE `name`='{genre}'");
            if (response.Count==0)
            {
                db.execute($"INSERT INTO `genre`(`name`) VALUES ('{genre}')");
                response = db.execute($"SELECT * FROM `genre` WHERE `name`='{genre}'");
            }
            return (int)response[0]["id"];
        }

        private void updateList(string searchElement, bool resetSearch = true)
        {
            ListElement.Items.Clear();
            booksListViews.Clear();
            nameAuthorList.Items.Clear();
            nameGenreList.Items.Clear();

            int number = 1;
            foreach (var vari in db.execute(searchElement))
            {
                booksListViews.Add(new Book
                {
                    id = (int)vari["id"], 
                    name = (string)vari["name"],
                    authorID = (int)vari["authorID"],
                    content = (string)vari["content"],
                    genreID = (int)vari["genreID"],
                    year_release = (int)vari["year_release"]
                });
                ListElement.Items.Add($"{number}. {(string)vari["name"]}");
                number += 1;
            }
            
            if (resetSearch)
            {
                nameAuthorListSearch.Items.Clear();
                nameGenreListSearch.Items.Clear();
                yearReleaseSearch.Items.Clear();
            
                nameAuthorListSearch.Items.Add("");
                nameGenreListSearch.Items.Add("");
                yearReleaseSearch.Items.Add("");
            
                db.execute("SELECT * FROM `author`").ForEach(vari =>
                {
                    nameAuthorList.Items.Add(vari["name"]);
                    nameAuthorListSearch.Items.Add(vari["name"]);
                });
                db.execute("SELECT * FROM `genre`").ForEach(vari =>
                {
                    nameGenreList.Items.Add(vari["name"]);
                    nameGenreListSearch.Items.Add(vari["name"]);
                });
                db.execute("SELECT DISTINCT `year_release` FROM `book`").ForEach(vari=>yearReleaseSearch.Items.Add(vari["year_release"]));

            }
        }
    }
}