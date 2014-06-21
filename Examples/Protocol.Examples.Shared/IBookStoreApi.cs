using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol.Examples.Shared
{
    public interface IBookStoreApi
    {
        Book GetBookByISBN(String isbn);

        Book[] SearchBook(String filter);

        Response InsertBook(Book info);

        Response DeleteBook(String isbn);
    }
}
