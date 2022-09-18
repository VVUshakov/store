/*Moq ��� ����� ����������, �������, ��������� ��������� ����� C#, 
 *��������� ������ �������� ��� ����������� � ����������� �������.*/
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
            Assert.Collection(actual, book => Assert.Equal(2, book.Id));
        }

        private static BookService CreateStub(string isIsbn)
        {
            //����������� ������� ����������� � ����� Mock �� ���������� Moq.
            //Wrap the book repository in a Mock class from the Moq library.
            var bookRepositoryStub = new Mock<IBookRepository>();

            /*������������� ������ ������� IBookRepository
             *� ������ ���������� ����������� (It.IsAny<string>())
             *� �������� �� �� "��������" � �������������� 1 � 2.*/
            /* Intercept calls to IBookRepository methods
             *with any string parameters (It.IsAny<string>())
             *and replace them with "stubs" with indicators 1 and 2.*/
            if (Book.IsIsbn(isIsbn)) 
                bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
                    .Returns(new[] { new Book(1, "", "", "") });
            else
                bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>()))
                    .Returns(new[] { new Book(2, "", "", "") });

            //���������� "��������" � �������� �����������.
            //We return the "stub" to the book repository.
            return new BookService(bookRepositoryStub.Object);                        
        }


        ////////////////////////////////////////////////////
        //�� �� �����, �� ��� ������������� ���������� Moq//
        ////////////////////////////////////////////////////

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
            Assert.Collection(books, book => Assert.Equal(idOfTitleOrAuthorSearch, book.Id));
        }

        private static Book[] _CreateStub(string query)
        {
            const int idOfIsbnSearch = 1;
            const int idOfTitleOrAuthorSearch = 2;

            var bookRepository = new StubBookRepository
            {
                ResultOfGetAllByIsbn = new Book[]
                {
                    new Book(idOfIsbnSearch, "", "", ""),
                },

                ResultOfGetAllByTitleOrAuthor = new Book[]
                {
                    new Book(idOfTitleOrAuthorSearch, "", "", ""),
                }
            };

            var bookService = new BookService(bookRepository);
            return bookService.GetAllByQuery(query);
        }
    }
}