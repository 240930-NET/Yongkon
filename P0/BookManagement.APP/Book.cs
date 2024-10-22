using System.Dynamic;

public class Book {
    public int BookId {get; set;}  //Key Value for the Book (Book.txt)

    public string? BookName {get; set;} = "";

    public string? BookAuthor {get; set;} = "";

    public int BookQuantity {get; set;} // Total Number

    public int BookIn {get; set;} // Available books in the library

    public int BookOut {get; set;} // Check out books by member

    public List<int> Borrowers {get; set;} = new List<int>(); // List of Members who have checked out  

    public Book() {}

    public Book(int bookid, string? bookname, string? bookauthor, int bookquantity) {
        BookId = bookid;
        BookName = bookname;
        BookAuthor = bookauthor;
        BookQuantity = bookquantity;
        BookIn = bookquantity;
        BookOut = 0;
    }

    public void BookDetails() {
        Console.WriteLine($"Book ID : {BookId,10}  ||  Book Name : {BookName,20}  ||  Book Author : {BookAuthor,20}  ||  Book Quantity : {BookQuantity,3}  || In : {BookIn,3} || Out : {BookOut,3}");
    }

    public void BookDetails(List<Member> members) {
        Console.WriteLine($"\nBook ID : {BookId,10}  ||  Book Name : {BookName,20}  ||  Book Author : {BookAuthor,20}  ||  Book Quantity : {BookQuantity,3}  || In : {BookIn,3} || Out : {BookOut,3}");

        if(Borrowers.Count>0) {
            Console.WriteLine("***   Borrowed Member   ***");
            
            foreach(int memberId in Borrowers) {
                Member member = members.Find(member => member.MemberId == memberId)!;
                if (member != null) 
                {
                    Console.WriteLine($" Borrower Member ID: {member.MemberId,6}  ||  First Name: {member.FirstName,15}  ||  Last Name: {member.LastName,15}");             
                }
            }
        }
    }

}