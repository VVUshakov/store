using System.Text.RegularExpressions;

namespace Store;
public class Book
{
    public int Id { get; }

    public string Isbn { get; } //Номер ISBN

    public string Author { get; }

    public string Title { get; }

    public string Description { get; }

    public decimal Price { get; }

    public Book(int id, string isbn, string author, string title, string description, decimal price)
    {
        Id = id;
        Isbn = isbn;
        Author = author;
        Title = title;
        Description = description;
        Price = price;
    }

    internal static bool IsIsbn(string s)
    {
        string isbn = s;

        if (isbn is null)
            return false;

        isbn = isbn.Replace("-", "")
                    .Replace(" ", "")
                    .ToUpper();

        /* Шаблон текста строки:
         * Строка должна начинаться с абревиатуры ISBN (^ISBN...)
         * далее содержать 10 или 13 цифр (...\d{10}(\d{3})?...),
         * и после цифр должен быть конец строки (...$)*/
        /* Line text template:
         * The line must begin with the ISBN abbreviation (^ISBN...)
         * then contain 10 or 13 digits (...\d{10}(\d{3})?...),
         * and after the numbers there must be the end of the line (... $) */
        return Regex.IsMatch(isbn, @"^ISBN\d{10}(\d{3})?$");
    }
}
