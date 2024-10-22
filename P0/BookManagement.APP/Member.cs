public class Member{
    public int MemberId {get; set;} //Auto generated Number & Key value for the Member (Member.txt)

    public string? FirstName {get; set;}

    public string? LastName {get; set;}

    public List<int> BorrowedBooks {get; set;} = new List<int>(); // list of Borrowed books by the Member 

    public Member() {        
    }

    public Member(int memberId, string firstName, string lastName) {
        MemberId = memberId;
        FirstName = firstName;
        LastName = lastName;        
    }   

    public void MemberDetails() {
        Console.WriteLine($"Member id :{MemberId,7} || First Name : {FirstName,15} || Last Name : {LastName,15}");
    }

    public void MemberDetails(List<Book> books) {
        Console.WriteLine($"\nMember id :{MemberId,7} || First Name : {FirstName,15} || Last Name : {LastName,15}");

        if (BorrowedBooks.Count > 0) {
            Console.WriteLine("***   Borrowed Books   ***");

            foreach(int bookId in BorrowedBooks) {
                //Find Book with BookId
                Book BorrowBook = books.Find(book => book.BookId == bookId)!;
                if (BorrowBook != null) {
                    Console.WriteLine($"Book ID : {BorrowBook.BookId,10}  ||  Book Name : {BorrowBook.BookName,20}  ||  Book Author : {BorrowBook.BookAuthor,20}");
                }
            }
        } else {
            Console.WriteLine("No borrowed books");
        }
    }
    
}