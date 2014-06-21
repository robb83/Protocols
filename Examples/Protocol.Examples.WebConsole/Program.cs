using Protocol.Examples.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Examples.WebConsole
{
    class Program
    {
        static BookStoreClientProxy clientProxy;

        static void Main(string[] args)
        {
            clientProxy = new BookStoreClientProxy("http://localhost:3739/API.ashx", null);

            while(true) 
            {
                ShowMenu();

                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.D1)
                    GetAllBook();
                else if (key.Key == ConsoleKey.D2)
                    SearchBook();
                else if (key.Key == ConsoleKey.D3)
                    InsertBook();
                else if (key.Key == ConsoleKey.D4)
                    DeleteBook();
                else if (key.Key == ConsoleKey.Escape)
                    break;
                else
                    Console.WriteLine("Wrong choice!");
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("--- Main Menu");
            Console.WriteLine();
            Console.WriteLine("1. Get all book from external datastore ");
            Console.WriteLine("2. Search in external datasource ");
            Console.WriteLine("3. Insert new book");
            Console.WriteLine("4. Delete book");
            Console.WriteLine();
            Console.WriteLine("--- Choice Or Press ESC to exit");
        }

        static void SearchBook()
        {
            Console.WriteLine("Enter search filter expression: ");
            String filter = Console.ReadLine().Trim();

            DisplayBooks(clientProxy.SearchBook(filter));
        }

        static void InsertBook()
        {
            String isbn, author, title, publisher;
            int publishYear;

            Console.WriteLine("Insert new Book, please enter book data:");

            Console.WriteLine();
            Console.Write("ISBN: ");
            if (!ConsoleHelper.ReadString(out isbn))
                return;

            Console.WriteLine();
            Console.Write("Title: ");
            if (!ConsoleHelper.ReadString(out title))
                return;

            Console.WriteLine();
            Console.Write("Author: ");
            if (!ConsoleHelper.ReadString(out author))
                return;

            Console.WriteLine();
            Console.Write("Publisher: ");
            if (!ConsoleHelper.ReadString(out publisher))
                return;

            Console.WriteLine();
            Console.Write("Publish year: ");
            if (!ConsoleHelper.ReadDecimalNumber(out publishYear))
                return;

            Response response = clientProxy.InsertBook(new Book
            {
                Author = author,
                ISBN = isbn,
                Publisher = publisher,
                PublishYear = publishYear,
                Title = title
            });

            if (response == null)
            {
                Console.WriteLine("\tFatal error, nothing received");
            }
            else if (response.Success == false)
            {
                if (response.Error != null)
                {
                    Console.WriteLine("\tErrorCode: {0}, Message: {1}", response.Error.Code, response.Error.Message);
                }
                else
                {
                    Console.WriteLine("\tError received");
                }
            }
            else
            {
                Console.WriteLine("\tInsert successfully");
            }
        }

        static void DeleteBook()
        {
            clientProxy.DeleteBook(null);
        }

        static void GetAllBook()
        {
            DisplayBooks(clientProxy.SearchBook(null));
        }

        static void DisplayBooks(Book[] books)
        {
            Console.WriteLine("-- Books: ");

            if (books == null || books.Length == 0)
            {
                Console.WriteLine("\tNothing");
                return;
            }

            foreach(Book b in books)
            {
                Console.WriteLine();

                Console.WriteLine("\tISBN: {0} ({1}, {2})", b.ISBN, b.Publisher ,b.PublishYear);
                Console.WriteLine("\tTitle: {0}", b.Title);
                Console.WriteLine("\tAuthor: {0}", b.Author);
            }
        }
    }
}
