/*Moq ��� ����� ����������, �������, ��������� ��������� ����� C#, 
 *��������� ������ �������� ��� ����������� � ����������� �������.*/
using Moq;

namespace Store.Tests
{
    public class BookServiceTests
    {
        [Fact]
        public void GetAllByQuery_WithIsbn_CallGetAllByIsbn()
        {
            //����������� ������� ����������� � ����� Mock �� ���������� Moq.
            var bookRepositoryStub = new Mock<IBookRepository>();

            /*
             *������������� ������ ������� IBookRepository
             *� ������ ���������� ����������� (It.IsAny<string>())
             *� �������� �� �� "��������" � �������������� 1 � 2.
            */
            bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
                    .Returns(new[] { new Book(1, "", "", "") });

            bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>()))
                    .Returns(new[] { new Book(2, "", "", "") });

            //������� "��������" � �������� �����������.
            var bookService = new BookService(bookRepositoryStub.Object);
            string validIsbn = "ISBN 12345-67890";

            //�������� ����
            var actual = bookService.GetAllByQuery(validIsbn);
            Assert.Collection(actual, book => Assert.Equal(1, book.Id));
        }

        [Fact]
        public void GetAllByQuery_WithAuthor_CallGetAllByTitleOrAuthor()
        {
            //����������� ������� ����������� � ����� Mock �� ���������� Moq.
            var bookRepositoryStub = new Mock<IBookRepository>();

            /*
             *������������� ������ ������� IBookRepository
             *� ������ ���������� ����������� (It.IsAny<string>())
             *� �������� �� �� "��������" � �������������� 1 � 2.
            */
            bookRepositoryStub.Setup(x => x.GetAllByIsbn(It.IsAny<string>()))
                    .Returns(new[] { new Book(1, "", "", "") });

            bookRepositoryStub.Setup(x => x.GetAllByTitleOrAuthor(It.IsAny<string>()))
                    .Returns(new[] { new Book(2, "", "", "") });

            //������� "��������" � �������� �����������.
            var bookService = new BookService(bookRepositoryStub.Object);
            string invalidIsbn = "12345-67890";

            //�������� ����
            var actual = bookService.GetAllByQuery(invalidIsbn);
            Assert.Collection(actual, book => Assert.Equal(2, book.Id));
        }
    }
}