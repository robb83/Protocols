using Protocol.Examples.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Examples.WebConsole
{
    public class BookStoreClientProxy : IBookStoreApi
    {
        String bookStoreRemoteBaseAddress;
        IWebProxy proxy;

        public BookStoreClientProxy(String bookStoreRemoteBaseAddress, IWebProxy proxy)
        {
            this.bookStoreRemoteBaseAddress = bookStoreRemoteBaseAddress;
            this.proxy = proxy;
        }

        public Book GetBookByISBN(string isbn)
        {
            return HttpRequestHelper.HttpPostRequestWithProtoContent<String, Book>(this.bookStoreRemoteBaseAddress + "/GetBookByISBN", this.proxy, isbn);
        }

        public Book[] SearchBook(string filter)
        {
            return HttpRequestHelper.HttpPostRequestWithProtoContent<String, Book[]>(this.bookStoreRemoteBaseAddress + "/SearchBook", this.proxy, filter);
        }

        public Response InsertBook(Book book)
        {
            return HttpRequestHelper.HttpPostRequestWithProtoContent<Book, Response>(this.bookStoreRemoteBaseAddress + "/InsertBook", this.proxy, book);
        }

        public Response DeleteBook(string isbn)
        {
            return HttpRequestHelper.HttpPostRequestWithProtoContent<String, Response>(this.bookStoreRemoteBaseAddress + "/DeleteBook", this.proxy, isbn);
        }
    }
}
