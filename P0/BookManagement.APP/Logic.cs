using System.Text.RegularExpressions;
using BookManagement.APP;

public class Logic { // List (Book & Member), Common Functions
    public static void MemberList() {
        Console.WriteLine("\n**  This is Member List  **");
        foreach(Member member in Program.Members) member.MemberDetails();
    }

    public static void BookList() {
        Console.WriteLine("\n**  This is Book List  **");
        foreach(Book book in Program.Books) book.BookDetails();   
    }

    public static void MemberListWithBorrowInfo() {
        Console.WriteLine("\n**  This is Member List With Borrowing Information **");
        foreach(Member member in Program.Members) {
            member.MemberDetails(Program.Books);
        }
    }

    public static void BookListWithBorrowInfo() {
        Console.WriteLine("\n**  This is Book List with Borrowing Information **");
        foreach(Book book in Program.Books) {
            book.BookDetails(Program.Members);
        }        
    }

    public static bool ValidCheckWithRegex(string input, string pattern) {
        
        Regex regex = new Regex(pattern);

        return regex.IsMatch(input);
    }

    public static void Await(int sec) {
        Thread.Sleep(sec);
    }
}