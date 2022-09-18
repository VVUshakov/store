using System.Text.RegularExpressions;

namespace Store;
public class Book
{
    public int Id { get; }

    public string Isbn { get; } //ISBN

    public string Author { get; }

    public string Title { get; }

    public Book(int id, string isbn, string author, string title)
    {
        Id = id;
        Isbn = isbn;
        Author = author;
        Title = title;        
    }

    internal static bool IsIsbn(string s)
    {
        string isbn = s;

        if (isbn is null)
            return false;

        isbn = isbn.Replace("-", "")
                    .Replace(" ", "")
                    .ToUpper();

        /*
         * Шаблон текста строки:
         * Строка должна начинаться с абревиатуры ISBN (^ISBN...)
         * далее содержать 10 или 13 цифр (...\d{10}(\d{3})?...),
         * и после цифр должен быть конец строки (...$)
        */
        return Regex.IsMatch(isbn, @"^ISBN\d{10}(\d{3})?$");
    }
}
