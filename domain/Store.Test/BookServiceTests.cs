/*Moq эта така€ библиотека, котора€, пользу€сь мощностью €зыка C#, 
 *позвол€ет делать заглушки дл€ »нтерфейсов и јбстрактных классов.*/
using Moq;

namespace Store.Tests
{
    public class BookServiceTests
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
        public void GetAllByQuery_WithAuthor_CallGetAllByTitleOrAuthor()
        {            
            string invalidIsbn = "12345-67890";
            BookService bookService = CreateStub(invalidIsbn);

            Book[] actual = bookService.GetAllByQuery(invalidIsbn);
            Assert.Collection(actual, book => Assert.Equal(2, book.Id));
        }

        public static BookService CreateStub(string isIsbn)
        {
            //ќборачиваем книжный репозиторий в класс Mock из библиотеки Moq.
            //Wrap the book repository in a Mock class from the Moq library.
            var bookRepositoryStub = new Mock<IBookRepository>();

            /*ѕерехватываем вызовы методов IBookRepository
             *с любыми строковыми параметрами (It.IsAny<string>())
             *и замен€ем их на "заглушки" с индификаторами 1 и 2.*/
            /* Intercept calls to IBookRepository methods
             *with any string parameters (It.IsAny<string>())
             *and replace them with "stubs" with indicators 1 and 2.*/
            if (Book.IsIsbn(isIsbn)) 
                bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
                    .Returns(new[] { new Book(1, "", "", "") });
            else
                bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>()))
                    .Returns(new[] { new Book(2, "", "", "") });

            //—оздаем "заглушку" к книжному репозиторию.
            //Create a "stub" for the book repository.
            var bookService = new BookService(bookRepositoryStub.Object);
            return bookService;                        
        }
    }
}