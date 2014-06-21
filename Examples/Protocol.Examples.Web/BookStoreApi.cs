using Protocol.Examples.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Examples.Web
{
    // Demonstration only!!

    public class BookStoreApi : IBookStoreApi
    {
        static Dictionary<String, Book> dataStore = new Dictionary<String, Book>();
        
        static BookStoreApi()
        {
            dataStore.Add("0-201-03801-3", new Book
            { 
                Author = "Donald Ervin Knuth", 
                ISBN = "0-201-03801-3", 
                PublishYear = 1968, 
                Title = "The Art of Computer Programming, Volume 1: Fundamental Algorithms",
                Publisher = "Addison-Wesley"
            });

            dataStore.Add("0-201-48567-2", new Book
            {
                Author = "Martin Fowler",
                ISBN = "0-201-48567-2",
                PublishYear = 1999,
                Title = "Refactoring: Improving the Design of Existing Code",
                Publisher = "Addison-Wesley"
            });
        }

        public Book GetBookByISBN(string isbn)
        {
            lock(dataStore)
            {
                Book result;
                if (dataStore.TryGetValue(isbn, out result))
                    return result;

                return null;
            }
        }

        public Book[] SearchBook(string filter)
        {
            // select all
            if (String.IsNullOrWhiteSpace(filter))
            {
                lock (dataStore)
                {
                    return dataStore.Values.ToArray();
                }
            }
            // filter
            else
            {
                List<Book> result = new List<Book>();

                lock (dataStore)
                {
                    foreach (var item in dataStore)
                    {
                        Book book = item.Value;

                        if (book.Author != null && book.Author.Contains(filter))
                        {
                            result.Add(book);

                            continue;
                        }

                        if (book.Title != null && book.Title.Contains(filter))
                        {
                            result.Add(book);

                            continue;
                        }

                        if (book.ISBN != null && book.ISBN.Contains(filter))
                        {
                            result.Add(book);

                            continue;
                        }

                        if (book.Publisher != null && book.Publisher.Contains(filter))
                        {
                            result.Add(book);

                            continue;
                        }
                    }
                }

                return result.ToArray();
            }
        }

        public Response InsertBook(Book info)
        {
            if (info == null)
                return Response.GenericError;
            if (String.IsNullOrWhiteSpace(info.Author))
                return Response.GenericError;
            if (String.IsNullOrWhiteSpace(info.ISBN))
                return Response.GenericError;
            if (String.IsNullOrWhiteSpace(info.Publisher))
                return Response.GenericError;
            if (String.IsNullOrWhiteSpace(info.Title))
                return Response.GenericError;
            
            lock (dataStore)
            {
                Book alreadyExists;
                if (dataStore.TryGetValue(info.ISBN, out alreadyExists))
                {
                    return Response.GenericError;
                }

                dataStore.Add(info.ISBN, info);
            }

            return Response.SuccessResponse;
        }
        
        public Response DeleteBook(string isbn)
        {
            throw new NotSupportedException();
        }
    }
}
