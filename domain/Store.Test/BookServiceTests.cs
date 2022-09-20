/*Moq эта такая библиотека, которая, пользуясь мощностью языка C#, 
 *позволяет делать заглушки для Интерфейсов и Абстрактных классов.*/
/*Moq is a library that, using the power of the C# language,
 *allows you to stub Interfaces and Abstract Classes.*/
using Moq;

namespace Store.Tests
{
    public class StabBookRepository
    {

        [Fact]
        public void GetAllByQuery_WithIsbn_CallGetAllByIsbn()
        {           
            string validIsbn = "ISBN 12345-67890";
            BookService bookService = CreateStub(validIsbn);

            Book[] actual = bookService.GetAllByQuery(validIsbn);
            Assert.Collection(actual, book => Assert.Equal(1, book.Id));
        }


        [Fact]
        public void GetAllByQuery_WithTitle_CallGetAllByTitleOrAuthor()
        {            
            string invalidIsbn = "12345-67890";
            BookService bookService = CreateStub(invalidIsbn);

            Book[] actual = bookService.GetAllByQuery(invalidIsbn);
            Assert.Collection(collection: actual, elementInspectors: book => Assert.Equal(2, book.Id));
        }

        private static BookService CreateStub(string isIsbn)
        {
            //Оборачиваем книжный репозиторий в класс Mock из библиотеки Moq.
            //Wrap the book repository in a Mock class from the Moq library.
            var bookRepositoryStub = new Mock<IBookRepository>();

            /*Перехватываем вызовы методов IBookRepository
             *с любыми строковыми параметрами (It.IsAny<string>())
             *и заменяем их на "заглушки" с индификаторами 1 и 2.*/
            /* Intercept calls to IBookRepository methods
             *with any string parameters (It.IsAny<string>())
             *and replace them with "stubs" with indicators 1 and 2.*/
            if (Book.IsIsbn(isIsbn)) 
                bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
                    .Returns(new[] { new Book(1, "", "", "", "", 12.24m) });
            else
                bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>()))
                    .Returns(new[] { new Book(2, "", "", "", "", 65.17m) });

            //Возвращаем "заглушку" к книжному репозиторию.
            //We return the "stub" to the book repository.
            return new BookService(bookRepositoryStub.Object);                        
        }


        /////////////////////////////////////////////////////
        //Те же тесты, но без использования библиотеки Moq //
        //The same tests, but without using the Moq library//
        /////////////////////////////////////////////////////

        [Fact]
        public void _GetAllByQuery_WithIsbn_CallGetAllByIsbn()
        {
            const int idOfIsbnSearch = 1;
            string validIsbn = "ISBN 12345-67890";

            Book[] books = _CreateStub(validIsbn);
            Assert.Collection(books, book => Assert.Equal(idOfIsbnSearch, book.Id));
        }

        [Fact]
        public void _GetAllByQuery_WithTitle_CallGetAllByTitleOrAuthor()
        {            
            const int idOfTitleOrAuthorSearch = 2;
            string validTitleOrAuthor = "Programming";

            Book[] books = _CreateStub(validTitleOrAuthor);
            Assert.Collection(collection: books, elementInspectors: book => Assert.Equal(idOfTitleOrAuthorSearch, book.Id));
        }

        private static Book[] _CreateStub(string query)
        {
            const int idOfIsbnSearch = 1;
            const int idOfTitleOrAuthorSearch = 2;

            var bookRepository = new StubBookRepository
            {
                ResultOfGetAllByIsbn = new Book[]
                {
                    new Book(idOfIsbnSearch, "", "", "", "", 52.12m),
                },

                ResultOfGetAllByTitleOrAuthor = new Book[]
                {
                    new Book(idOfTitleOrAuthorSearch, "", "", "", "", 16.19m),
                }
            };

            var bookService = new BookService(bookRepository);
            return bookService.GetAllByQuery(query);
        }
    }
}