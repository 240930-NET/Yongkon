public class Menu {

    public static void Greeting() {
        Console.WriteLine("****************************************");
        Console.WriteLine("* Welcome! This is Book Management App *");
        Console.WriteLine("****************************************");
        Logic.Await(1100);
    }

    public static void MenuDisplay() {
        Console.WriteLine("\n1. Add Member");
        Console.WriteLine("2. Edit Member");
        Console.WriteLine("3. Remove Member");
        Console.WriteLine("4. Add Book");
        Console.WriteLine("5. Edit Book");
        Console.WriteLine("6. Remove Book");
        Console.WriteLine("7. Borrow Book");
        Console.WriteLine("8. Return Book");
        Console.WriteLine("9. List (Member/Book)");
        Console.WriteLine("0. Exit\n");
    }

    public static int GetUserInput() {
        int selectedOption = 0;

        Console.WriteLine("Select the menu");

        try {
            string? inputString = Console.ReadLine();
            string pattern = "^[0-9]$";

            while(inputString ==null || !Logic.ValidCheckWithRegex(inputString, pattern)) {
                Console.WriteLine("Please enter valid input (0-9)");
                inputString = Console.ReadLine()!;
            }

            selectedOption = int.Parse(inputString);
            return selectedOption;
        }
        catch(Exception err) {
            Console.WriteLine(err.Message);
            return 0;
        }
        
    }

    public static void SelectList() {
        Console.WriteLine("1. Member List");
        Console.WriteLine("2. Book List");
        Console.WriteLine("3. Member List (with borrowing info)");
        Console.WriteLine("4. Book List(with borrowing info)");

        int SelectedList = 0;

        try {

            string? inputString = Console.ReadLine();
            string pattern = "^[1-4]$";

            while(inputString == null || !Logic.ValidCheckWithRegex(inputString, pattern)) {
                Console.WriteLine("Please enter valid input (1-4)");
                inputString = Console.ReadLine()!;
            }

            SelectedList = int.Parse(inputString);
        }
        catch(Exception err) {
            Console.WriteLine(err.Message);
        }

        switch(SelectedList) {
            case 1:
                Logic.MemberList();
                break;
            case 2:
                Logic.BookList();
                break;
            case 3:
                Logic.MemberListWithBorrowInfo();
                break;
            case 4:
                Logic.BookListWithBorrowInfo();
                break;
        }
    }
}