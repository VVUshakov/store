/*Moq эта такая библиотека, которая, пользуясь мощностью языка C#, 
 *позволяет делать заглушки для Интерфейсов и Абстрактных классов.*/
using Moq;

namespace Store.Tests
{
    public class BookServiceTests
    {
        [Fact]
        public void GetAllByQuery_WithIsbn_CallGetAllByIsbn()
        {
            //Оборачиваем книжный репозиторий в класс Mock из библиотеки Moq.
            var bookRepositoryStub = new Mock<IBookRepository>();

            /*
             *Перехватываем вызовы методов IBookRepository
             *с любыми строковыми параметрами (It.IsAny<string>())
             *и заменяем их на "заглушки" с индификаторами 1 и 2.
            */
            bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
                    .Returns(new[] { new Book(1, "", "", "") });

            bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>()))
                    .Returns(new[] { new Book(2, "", "", "") });

            //Создаем "заглушку" к книжному репозиторию.
            var bookService = new BookService(bookRepositoryStub.Object);
            string validIsbn = "ISBN 12345-67890";

            //Проводим тест
            var actual = bookService.GetAllByQuery(validIsbn);
            Assert.Collection(actual, book => Assert.Equal(1, book.Id));
        }

        [Fact]
        public void GetAllByQuery_WithAuthor_CallGetAllByTitleOrAuthor()
        {
            //Оборачиваем книжный репозиторий в класс Mock из библиотеки Moq.
            var bookRepositoryStub = new Mock<IBookRepository>();

            /*
             *Перехватываем вызовы методов IBookRepository
             *с любыми строковыми параметрами (It.IsAny<string>())
             *и заменяем их на "заглушки" с индификаторами 1 и 2.
            */
            bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
                    .Returns(new[] { new Book(1, "", "", "") });

            bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>()))
                    .Returns(new[] { new Book(2, "", "", "") });

            //Создаем "заглушку" к книжному репозиторию.
            var bookService = new BookService(bookRepositoryStub.Object);
            string invalidIsbn = "12345-67890";

            //Проводим тест
            var actual = bookService.GetAllByQuery(invalidIsbn);
            Assert.Collection(actual, book => Assert.Equal(2, book.Id));
        }
    }
}