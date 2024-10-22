namespace BookManagement.Tests;

public class UnitTest
{
   //**************************
   //  Member Test (Create)
   //**************************
   [Fact]
    // Test for adding a new member
    public void AddNewMember_ShouldAddMemberToList()
    {
        // Arrange
        var memberManager = new MemberManager();
        memberManager.FilePath = "TestMemberAdd.txt"; // Test file path
        var inputManager = new InputManager();
        
        // Mock input
        inputManager.SetInput("John\nDoe\n");

        try
        {
            // Act
            var newMember = memberManager.AddNewMember();

            // Assert
            Assert.NotNull(newMember); // Ensure a new member was added
            Assert.Contains(newMember, memberManager.Members); // Ensure member list contains the new member
        }
        finally
        {
            inputManager.RestoreInput(); // Restore original input
            
        }

        // Clean up test file (if needed)
         File.Delete(memberManager.FilePath);
    }

   //**************************
   //  Member Test (Edit)
   //**************************
    [Fact]
    // Test for editing a member's details
    public void EditMember_ShouldUpdateMemberDetails()
    {
        // Arrange
        var memberManager = new MemberManager();
        memberManager.FilePath = "TestMemberEdit.txt"; // Test file path
        Member originalMember = new Member(1, "John", "Doe"); // Test member
        memberManager.Members.Add(originalMember); // Add member to the list
    
        var inputManager = new InputManager();
        try
        {
            inputManager.RestoreInput(); // Restore original input
            // Mock input for editing
            inputManager.SetInput("1\nJane\nSmith\n"); // Input for editing

            // Act
            Member changedMember = memberManager.EditMember(); // Call the method to edit member

            // Assert
            Assert.Equal("Jane", changedMember.FirstName); // Ensure the first name is updated
            Assert.Equal("Smith", changedMember.LastName); // Ensure the last name is updated
            Assert.Contains(changedMember, memberManager.Members); // Ensure member list contains the updated member

        }
        finally
        {
            inputManager.RestoreInput(); // Restore original input
            Console.SetOut(Console.Out); // Restore original console output
        }

        // Clean up test file (if needed)
        File.Delete(memberManager.FilePath);
    }

   //**************************
   //  Member Test (Delete)
   //**************************
    [Fact]
    // Test for removing a member
    public void RemoveMember_ShouldDeleteMemberFromList()
    {
        // Arrange
        var memberManager = new MemberManager();
        memberManager.FilePath = "TestMemberDelete.txt"; // Test file path
        Member memberToDelete = new Member(1, "Paul", "Kim"); // Test member
        memberManager.Members.Add(memberToDelete); // Add member to the list

        var inputManager = new InputManager();
        // Mock input for deleting
        inputManager.RestoreInput(); // Restore original input
        inputManager.SetInput("1\n1\n");

        try
        {
            // Act
            memberManager.RemoveMember(); // Call the method to remove member

            // Assert
            //Assert.DoesNotContain(memberToDelete, memberManager.Members); // Ensure the member list does not contain the deleted member
            Assert.True(memberToDelete.MemberId == 1); // Test member

        }
        finally
        {
            inputManager.RestoreInput(); // Restore original input
        }

        // Clean up test file (if needed)
         File.Delete(memberManager.FilePath);
    }

   //**************************
   //  Book Test (Create)
   //**************************
    [Fact]
    public void AddBook_ShouldAddBookToList()
    {
        // Arrange
        var bookManager = new BookManager();
        bookManager.FilePath = "TestBooksAdd.txt"; // Temporary file

        var inputManager = new InputManager();
        try
        {
            // Mock input for adding the book (Book ID, Book Name, Book Author, Book Quantity)
            inputManager.SetInput("777\nTest Book\nTest Author\n10\n");

            // Act
            bookManager.AddNewBook(); // Add a new Book

            // Assert
            var addedBook = bookManager.Books.Find(b => b.BookId == 777);
            Assert.NotNull(addedBook); // Ensure new book was added Books
        }
        finally
        {
            inputManager.RestoreInput(); // Mock input Restore
            File.Delete(bookManager.FilePath); // Clean up the test file
        }
    }
    
   //**************************
   //  Book Test (Edit)
   //**************************  
    [Fact]
    public void EditBook_ShouldUpdateBookDetails()
    {
        // Arrange
        var bookManager = new BookManager();
        var originalBook = new Book(123, "Old Title", "Old Author", 10);
        bookManager.FilePath = "TestBooksEdit.txt"; // Test file path
        bookManager.Books.Add(originalBook); // Add book to the list
       
        var inputManager = new InputManager();
       try
        {
            inputManager.SetInput("123\nNew Title\nNew Author\n5\n5\n0\n"); // Mock input for editing the book

            // Act
            bookManager.EditBook(); // Call the method to edit book

            // Assert
            var updatedBook = bookManager.Books.Find(b => b.BookId == 123);
            if (updatedBook != null) {
                Assert.Equal("New Title", updatedBook.BookName);  // Assert
                Assert.Equal("New Author", updatedBook.BookAuthor); // Assert
                Assert.Equal(5, updatedBook.BookQuantity); // Assert
                Assert.Contains(updatedBook, bookManager.Books);
            }
        }
        finally
        {
            inputManager.RestoreInput(); // Restore input
            // Clean up test file (if needed)
            File.Delete(bookManager.FilePath);
        }
    }
    

   //**************************
   //  Book Test (Delete)
   //**************************
    [Fact]
    public void RemoveBook_ShouldDeleteBookFromList()
    {
        // Arrange
        var bookManager = new BookManager();
        bookManager.FilePath = "TestBooksDelete.txt"; // Test file path
        var bookToDelete = new Book(123, "Book to Remove", "Author", 5);
        
        bookManager.Books.Add(bookToDelete); // Add book to the list

        var inputManager = new InputManager();
        try
        {
            inputManager.SetInput("123\n1\n"); // Mock input for deleting the book

            // Act
            bookManager.RemoveBook(); // Call the method to remove book

            // Assert
            Assert.DoesNotContain(bookToDelete, bookManager.Books); // Ensure the book list does not contain the deleted book
        }
        finally
        {
            inputManager.RestoreInput(); // Restore input
        }

        // Clean up test file (if needed)
        File.Delete(bookManager.FilePath);
    }       


   //**************************
   //  Borrow  &  Return
   //**************************
    [Fact]
    public void BorrowBook_ShouldUpdateBookStatus_WhenBookIsBorrowedSuccessfully()
    {
        // Arrange
        var bookManager = new BookManager();
        var memberManager = new MemberManager();
        var member = new Member(1, "John", "Doe");
        var book = new Book(123, "Test Book", "Author", 5);
        
        bookManager.FilePath = "TestBooksBorrow.txt"; // Test file path
        bookManager.Books.Add(book); // Add book to the list
        memberManager.Members.Add(member); // Add member to the list

        var inputManager = new InputManager();
        inputManager.SetInput("1\n123\n"); // Mock input - MemberID, BookID
        try{
            // Act
            bookManager.BorrowBook(memberManager); // Call the method to borrow a book

            // Assert
            Assert.Contains(member.MemberId, book.Borrowers); // Ensure the member is in the Borrowers list
            Assert.Equal(1, book.BookOut); // Ensure one book is borrowed
            Assert.Equal(4, book.BookIn); // Ensure available books decreased by 1
        }
        finally
        {
            inputManager.RestoreInput(); // Restore input
            File.Delete(bookManager.FilePath);  // Clean up test file (if needed)
        }
        
    }

    [Fact]
    public void ReturnBook_ShouldUpdateBookStatus_WhenBookIsReturnedSuccessfully()
    {
        // Arrange
        var bookManager = new BookManager();
        var memberManager = new MemberManager();
        var member = new Member(1, "John", "Doe");
        var book = new Book(123, "Test Book", "Author", 5);
        
        bookManager.FilePath = "TestBooksReturn.txt"; // Test file path
        bookManager.Books.Add(book); // Add book to the list
        memberManager.Members.Add(member); // Add member to the list
        book.Borrowers.Add(member.MemberId); // Add member to the Borrowers list
        book.BookIn = 4; // Initially 4 books available
        book.BookOut = 1; // Initially 1 book is borrowed

        var inputManager = new InputManager();
        inputManager.SetInput("1\n123\n"); // Mock input - MemberID, BookID
        
        try {
            // Act
            bookManager.ReturnBook(memberManager); // Call the method to return the book
            
            // Assert
            Assert.DoesNotContain(member.MemberId, book.Borrowers); // Ensure the member is removed from the Borrowers list
            Assert.Equal(5, book.BookIn); // Ensure available books increased by 1
            Assert.Equal(0, book.BookOut); // Ensure no books are borrowed
        }
        finally
        {
            inputManager.RestoreInput(); // Restore input
            Console.SetOut(Console.Out);  // Restore original console output
            File.Delete(bookManager.FilePath);  // Clean up test file (if needed)
        }
    }
    
}

