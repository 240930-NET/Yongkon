using System.Formats.Asn1;
using System.Text.Json;
using System.Text.RegularExpressions;


public class MemberManager {

    public List<Member> Members {get; set;}

    public string FilePath {get; set;} = "Member.txt";


    public MemberManager() {
        Members = new List<Member>();
        LoadMembers();
    }


    public Member AddNewMember() {

        //Get Last Name, First Name
        Console.WriteLine("Please enter First name of the new member?");
        string? firstName = Console.ReadLine();
        Console.WriteLine("Please enter Last name of the new member?");
        string? lastName = Console.ReadLine();

        if (firstName == null) firstName = "";
        if (lastName == null) lastName = "";

        //Get Max Member ID from Member.txt
         int NewMemberId = GetMaxMemberId(FilePath) + 1;

        //Creawte New Member
        Member NewMember = new Member(NewMemberId, firstName, lastName);

        //Add NewMeber to Memeber List
        Members.Add(NewMember);

        //Add New Member to File 
        _ = SaveMembers();

        //NewMember Details
        Console.WriteLine("** New Member Added!! **");
        NewMember.MemberDetails();
        Logic.Await(1100);
        
        return NewMember;
    }


    public int GetMaxMemberId(string FileName) {
        int MaxMemberId = 0;

        //if file don't exist, return 0
        if (!File.Exists(FilePath)) {
            return MaxMemberId; // return 0
        }

        // Read Json File to find a Max Member ID
        var JsonString = File.ReadAllText(FilePath);
        List<Member>? memberList = JsonSerializer.Deserialize<List<Member>>(JsonString);
        
        // If memberList is not empty, find Max Member ID
        if (memberList != null && memberList.Count > 0) {
            MaxMemberId = memberList.Max(m => m.MemberId); //LINQ, Lamda 
        }

        return MaxMemberId;
    }


    public Member EditMember() {
        //Get Member ID
        Console.WriteLine("Please enter Member Id that you want to edit");

        string? InputMember = Console.ReadLine();
        int MemberId;
        var SelectedMember = new Member();
        
        if (int.TryParse(InputMember, out MemberId))
        {
            SelectedMember = Members.Find(member => member.MemberId == MemberId);

            if (SelectedMember == null)  {
                Console.WriteLine("There is no such a Member");
            }
        }
        else
        {
            // Wrong Input
            Regex numberPattern = new Regex(@"^\d+$");  // Only allows numeric format
            
            while(SelectedMember == null || SelectedMember.MemberId == 0 || !numberPattern.IsMatch(InputMember ?? "")) {
                Console.WriteLine("Please enter numeric number only(bigger than 0) or there is no Such a member");
                
                InputMember = Console.ReadLine();
                MemberId = int.Parse(InputMember!);
                SelectedMember = Members.Find(member => member.MemberId == MemberId);
            }

            SelectedMember.MemberDetails();
        }

        // If Selectd Member is Not Null
        if (SelectedMember != null) {
            Console.WriteLine("**  Current Member Details  **");
            SelectedMember.MemberDetails();

            Console.WriteLine("Type First Name");
            string? FisrtName = Console.ReadLine();

            Console.WriteLine("Type Last Name");
            string? LastName = Console.ReadLine();

            SelectedMember.FirstName = FisrtName;
            SelectedMember.LastName = LastName;
            Console.WriteLine("Member data was changed");
            
            _ = SaveMembers();

        }
        return SelectedMember ?? new Member();
    }


    public void RemoveMember() {
        //Get Member ID
        Console.WriteLine("Please enter Member Id that you want to delete");

        string? InputMember = Console.ReadLine();
        int MemberId;
        var SelectedMember = new Member();
        
        if (int.TryParse(InputMember, out MemberId))
        {
            SelectedMember = Members.Find(member => member.MemberId == MemberId);
            if (SelectedMember != null) {
                // Confirm Delete Process
                Console.WriteLine("Do you want to delete below user?");
                SelectedMember.MemberDetails();

                Console.WriteLine("1. Yes");
                Console.WriteLine("2. No");
                
                string? inputString = Console.ReadLine();
                string pattern = "^[1-2]$";

                while(inputString == null || !Logic.ValidCheckWithRegex(inputString, pattern)) {
                    Console.WriteLine("Please enter valid input (1-2)");
                    inputString = Console.ReadLine()!;
                }
                
                // Delete Member
                if (inputString.Equals("1")) {
                    Members.Remove(SelectedMember);
                }     

                Console.WriteLine("**  Selected Member has been Removed  **");

                // Save Members to Member.txt
                _ = SaveMembers();
            } else {
                Console.WriteLine("There is no such a Member");
            }
        }
        else
        {
            // Wrong Input
            Console.WriteLine("Invalid Member ID. Please enter a valid number.");
        }        
    }


    public void LoadMembers() {
        if (File.Exists(FilePath)) {
            var JsonString = File.ReadAllText(FilePath);
            //Convert JsonString to List<Member>, if the result is NULL then initiate List<Member> as new
            Members = JsonSerializer.Deserialize<List<Member>>(JsonString) ?? new List<Member>();
        }
    }


    public async Task SaveMembers() {
        //Convert List<Member> to JsonString
        var JsonString = JsonSerializer.Serialize(Members);

        //Write JsonString(List<Member>) to Member.txt File
        using(StreamWriter writer = File.CreateText(FilePath)) {
            await writer.WriteAsync(JsonString);
        }
    }
}