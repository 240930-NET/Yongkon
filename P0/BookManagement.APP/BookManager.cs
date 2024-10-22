using System.Text.Json;
using System.Text.RegularExpressions;

public class BookManager {

    public List<Book> Books {get; set;}

    public string FilePath {get; set;} = "Book.txt";


    public BookManager() {
        Books = new List<Book>();

        LoadBooks();
    }


    public Book AddNewBook() {
        //Get Book ID, Book Name, Book Author, Book Quantity
        Console.WriteLine("Please enter Book ID");

        string? BookId = Console.ReadLine();
        int bookId = 0;
        
        Regex numberPattern = new Regex(@"^\d+$");  // Only allows numeric format
            
        while(!numberPattern.IsMatch(BookId ?? "")) {
            Console.WriteLine("Please enter numeric number only(bigger than 0)");
            
            BookId = Console.ReadLine();
        }
        if (BookId != null)  bookId = int.Parse(BookId);

        Console.WriteLine("Please enter Book Name");
        string? bookName = Console.ReadLine();

        Console.WriteLine("Please enter Book Author");
        string? bookAuthor = Console.ReadLine();

        Console.WriteLine("Please enter Book Quantity");

        string? BookQuantity = Console.ReadLine();
        int bookQuantity;
        
        while (!int.TryParse(BookQuantity, out bookQuantity))
        {
            // Wrong Input
            Console.WriteLine("Invalid Book Quntity. Please enter a valid number.");
            BookQuantity = Console.ReadLine();
        }


        Book newBook = new Book(bookId, bookName, bookAuthor, bookQuantity);

        Books.Add(newBook);

        SaveBooks();

        return newBook;
    }

    public void EditBook() {
        //Get Book ID, Book Name, Book Author, Book Quantity, In, Out
        Console.WriteLine("Please enter Book ID that you want to edit");
        string? BookId = Console.ReadLine();
        int SelectedBookId = 0;
        
        while (!int.TryParse(BookId, out SelectedBookId))
        {
            // Wrong Input
            Console.WriteLine("Invalid Book ID. Please enter a valid number.");
            BookId = Console.ReadLine();
        }


        var SelectedBook = Books.Find(book => book.BookId == SelectedBookId);
        
        if (SelectedBook == null) {
            Console.WriteLine("There is no such a book");
            return;
        }  


        Console.WriteLine("**  Current Book Details  **");
        SelectedBook.BookDetails();

        Console.WriteLine("Please enter Book Name");
        string? bookName = Console.ReadLine();

        Console.WriteLine("Please enter Book Author");
        string? bookAuthor = Console.ReadLine();

        Console.WriteLine("Please enter Book Quantity");

        string? BookQuantity = Console.ReadLine();
        int bookQuantity;
        
        while (!int.TryParse(BookQuantity, out bookQuantity))
        {
            // Wrong Input
            Console.WriteLine("Invalid Book Quntity. Please enter a valid number.");
            BookQuantity = Console.ReadLine();
        }

        Console.WriteLine("Please enter Book In");
        string? BookIn = Console.ReadLine();
        int bookIn;
        
        while (!int.TryParse(BookIn, out bookIn))
        {
            // Wrong Input
            Console.WriteLine("Invalid Book In. Please enter a valid number.");
            BookIn = Console.ReadLine();
        }

        Console.WriteLine("Please enter Book Out");
        string? BookOut = Console.ReadLine();
        int bookOut;
        
        while (!int.TryParse(BookOut, out bookOut))
        {
            // Wrong Input
            Console.WriteLine("Invalid Book In. Please enter a valid number.");
            BookOut = Console.ReadLine();
        }
        
        if (SelectedBook != null) {
            SelectedBook.BookName = bookName;
            SelectedBook.BookAuthor = bookAuthor;
            SelectedBook.BookQuantity = bookQuantity;
            SelectedBook.BookIn = bookIn;
            SelectedBook.BookOut = bookOut;

            SaveBooks();

            Console.WriteLine("Edited Successfully");
        }        
    }

    public void RemoveBook() {
        //Get Book ID, Book Name, Book Author, Book Quantity, In, Out
        Console.WriteLine("Please enter Book ID that you want to remove");
        string? BookId = Console.ReadLine();
        int SelectedBookId;
        
        while (!int.TryParse(BookId, out SelectedBookId))
        {
            // Wrong Input
            Console.WriteLine("Invalid Book ID. Please enter a valid number.");
            BookId = Console.ReadLine();
        }

        var SelectedBook = Books.Find(book => book.BookId == SelectedBookId);
        
        if (SelectedBook != null)  {
            Console.WriteLine("**   Do you want to delete below book?   **");
            SelectedBook.BookDetails();

            Console.WriteLine("1. Yes");
            Console.WriteLine("2. No");
            
            string? inputString = Console.ReadLine();
            string pattern = "^[1-2]$";

            while(inputString == null || !Logic.ValidCheckWithRegex(inputString, pattern)) {
                Console.WriteLine("Please enter valid input (1-2)");
                inputString = Console.ReadLine()!;
            }
            
            // Delete Book
            if (inputString.Equals("1")) {
                if (SelectedBook.BookOut ==0) {
                    Books.Remove(SelectedBook);                            
                    // Save Members to Member.txt
                    SaveBooks(); 

                    Console.WriteLine("Removed Successfully");
                }
                else Console.WriteLine("Cannot delete this book. There are books borrowed.");
            } 

        } else {
            Console.WriteLine("There is no such book");
        }
    }

    public void LoadBooks() {
        if (File.Exists(FilePath)) {
            var JsonString = File.ReadAllText(FilePath);

            Books = JsonSerializer.Deserialize<List<Book>>(JsonString) ?? new List<Book>();
        }
    }

    public async void SaveBooks() {
        // Convert List<Book> to JsonString
        string JsonString = JsonSerializer.Serialize(Books);

        // Write JsonString to Book.txt
        using (StreamWriter writer = File.CreateText(FilePath)) {
            await writer.WriteAsync(JsonString);
        }
    }

    public void BorrowBook(MemberManager memberManager) {
        //Get Member Info
        Console.WriteLine("Please enter Member ID who is borrowing book");
        string? InputMember = Console.ReadLine();
        int MemberId;
        var SelectedMember = new Member();
        
        if (int.TryParse(InputMember, out MemberId))
        {
            SelectedMember = memberManager.Members.Find(member => member.MemberId == MemberId);
            if (SelectedMember != null) SelectedMember.MemberDetails();
            else {
                Console.WriteLine("There was no such member");
                return;
            }
        }
        else
        {
            // Wrong Input
            Console.WriteLine("Invalid Member ID. Please enter a valid number.");
        }
        
        //Get Book Info
        Console.WriteLine("Please enter Book ID that the member want to borrow");

        String? inputBook = Console.ReadLine();
        int BookId;
        var SelectedBook = new Book();

        if (int.TryParse(inputBook, out BookId))
        {
            SelectedBook = Books.Find(book => book.BookId == BookId);
            if (SelectedBook != null && SelectedMember != null) {
                SelectedBook.BookDetails();

                //Check Availability of Borrowing
                if (SelectedBook.BookQuantity > SelectedBook.BookOut) {
                    SelectedBook.BookIn--;
                    SelectedBook.BookOut++;
                    SelectedBook.Borrowers.Add(SelectedMember.MemberId);
                    SelectedMember.BorrowedBooks.Add(SelectedBook.BookId);

                    SaveBooks();
                    _ = memberManager.SaveMembers();
                } else {
                    Console.WriteLine("I'm Sorry. There are no available books.");
                    return;
                }
            } else {
                Console.WriteLine("There is no such book");
                return;
            }
        }
        else
        {
            // Wrong Input
            Console.WriteLine("Invalid Book ID. Please enter a valid number.");
            return;
        }        

        Console.WriteLine("**  Borrow Process done successfully  **");
        if (SelectedMember != null)  SelectedMember.MemberDetails(Books);
    }

    public void ReturnBook(MemberManager memberManager) {
        //Get Member Info
        Console.WriteLine("Please enter Member ID who is returning book");

        String? inputMember = Console.ReadLine();
        int MemberId;
        var SelectedMember = new Member();
        
        if (int.TryParse(inputMember, out MemberId))
        {
            SelectedMember = memberManager.Members.Find(member => member.MemberId == MemberId);
            if (SelectedMember != null) SelectedMember.MemberDetails(Books);
            else {
                Console.WriteLine("There was no such Member");
                return;
            }
        }
        else
        {
            // Wrong Input
            Console.WriteLine("Invalid Member ID. Please enter a valid number.");
        }
        
        //Get Book Info
        Console.WriteLine("Please enter Book ID that the member want to return");

        String? inputBook = Console.ReadLine();
        int BookId;

        if (int.TryParse(inputBook, out BookId))
        {
            var SelectedBook = Books.Find(book => book.BookId == BookId);
            if (SelectedBook != null && SelectedMember != null) {
                SelectedBook.BookDetails();

                // Returning Process
                SelectedBook.BookIn++;
                SelectedBook.BookOut--;
                SelectedBook.Borrowers.Remove(SelectedMember.MemberId);
                SelectedMember.BorrowedBooks.Remove(SelectedBook.BookId);

                SaveBooks();
                _ = memberManager.SaveMembers();

                Console.WriteLine("**  Return Process done successfully  **");
                SelectedMember.MemberDetails(Books);
         
            } else {
                Console.WriteLine("There was no such a book");
                return;
            }
        }
        else
        {
            // Wrong Input
            Console.WriteLine("Invalid Book ID. Please enter a valid number.");
            return;
        }        

    }

}