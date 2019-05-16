using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Speech.Synthesis;

namespace newContactsProject
{
    class Program
    {
        static string pathOFile = "C:\\contactsProjects\\contactList.txt";//Global path name
        #region contactVariable
        struct contact
        {
            public string firstName;
            public string lastName;
            public string phoneNumber;
            public string emailAddress;
        }
        #endregion
        
        static void Main(string[] args)
        {
            if (!File.Exists(pathOFile))//Creates new contact file if one doesn't exist
            {
                Directory.CreateDirectory("C:\\contactsProjects");
                StreamWriter newFile = new StreamWriter(pathOFile);
                newFile.Close();
            } 
            
            #region mainVariables
            //Start variables
            string user;
            bool appOpen = true;
            #endregion//End variables

            do
            {
                Console.Clear();
                talkThat("Enter 0 to read the page");

                #region mainMenu
                //user options screen
                Console.WriteLine("1)\tAdd new record\n2)\tSearch for a contact\n3)\tModify a phone number\n4)\tModify an email\n5)\tDelete a record\n6)\tList all contacts\n7)\tAlphabetize contacts\n8)\tExit the program\n");
                user = Prompt("What would you like to do? (Choose Number):\t");
                #endregion

                #region ReadsPage (user enters 0)
                if (user == "0")
                {
                    talkThat("Enter 1 to Add a new contact. Enter 2 to Search for a contact. Enter 3 to modify a phone number. Enter 4 to modify an email address. Enter 5 to delete a contact. Enter 6 to list all contacts. Enter 7 to view contacts in ABC order. Enter 8 to EXIT the application.");
                }
                #endregion

                #region user chose 1
                if (user == "1")
                {
                    Console.Clear();
                    toUpdateContacts(addContact());
                }
                #endregion

                #region user chose 2
                else if (user == "2")
                {
                    Console.Clear();
                    viewContact();
                }
                #endregion

                #region user chose 3
                //Option to change contacts phone numnber
                else if (user == "3")
                {
                    Console.Clear();
                    contact[] newData = modifyContactsNumber(updatedList());
                    changesToContact(newData);
                }
                #endregion

                #region user chose 4
                //Option to change contacts email address
                else if (user == "4")
                {
                    Console.Clear();
                    contact[] newData = modifyContactsEmail(updatedList());
                    changesToContact(newData);
                }
                #endregion

                #region user chose 5
                else if (user == "5")
                {
                    Console.Clear();
                    deleteContact();
                }
                #endregion

                #region user chose 6
                //Display contact database
                else if (user == "6")
                {
                    Console.Clear();
                    showAllFunction(updatedList());
                }
                #endregion

                #region user chose 7
                else if (user == "7")
                {
                    Console.Clear();
                    abcOrder();
                }
                #endregion

                #region user chose 8
                else if (user == "8")
                {
                    appOpen = exitApp();
                }
                #endregion

                #region invalid input
                else
                {
                    Console.Clear();
                    Console.WriteLine("\nTRY AGAIN. CHOICE SHOULD BE EITHER 1, 2, 3, 4, 5, 6, 7 OR 8...\n");
                    appOpen = true;
                }
                #endregion//user didnt pick numbers 1 , 8 , or any one inbetween

               
            } while (appOpen);
        }//End Main

         //Start Functions
        #region userPromptFunction
            //Prompt for user interaction
        static string Prompt(string msg)
        {
            Console.Write(msg + "");
            return Console.ReadLine();
        }
        #endregion//userPrompt

        #region numberOfContactsFunction
        //Counting the number of contacts in the database / txt file
        static int numberOfContacts()
        {
            StreamReader contactFile = new StreamReader(pathOFile);
            int numberOfContacts = 0;
            while (!contactFile.EndOfStream)
            {
                    contactFile.ReadLine();
                    numberOfContacts += 1;

            }
            contactFile.Close();
            return numberOfContacts;
        }
        #endregion//Counts the number of contacts in the database / txt file

        #region updatedListFunction
        //Read from the latest revision of the text file
        static contact[] updatedList()
        {
            string contactLines;
            int looper = 0;
            contact[] savedContacts = new contact[numberOfContacts()];
            StreamReader inFile = new StreamReader(pathOFile);
            while (!inFile.EndOfStream)
            {
                contactLines = inFile.ReadLine();
                savedContacts[looper] =  populateContacts(contactLines);
                looper++;
            }

            inFile.Close();
            return savedContacts;
        }
        #endregion//Obtains data from the latest edition of the text file

        #region populateContactsFunction
        //Converts text data into contact
        static contact populateContacts(string data)
        {
            string[] contactData = data.Split(',');
            contact newContact;
            
            newContact.firstName    = contactData[0];
            newContact.lastName     = contactData[1];
            newContact.phoneNumber  = contactData[2];
            newContact.emailAddress = contactData[3];

            return newContact;
        }
        #endregion//Converts text data into contact

        #region addContactFunction (user option 1)
        //Allows user to add a contact to the database
        static contact addContact()
        {
            contact newContact;

            newContact.firstName = Prompt("Enter contact's first name:\t").Trim().ToUpper();
            newContact.lastName = Prompt("Enter contact's last name:\t").Trim().ToUpper();
            newContact.phoneNumber = Prompt("Enter contact's phone number:\t").Trim().ToUpper();
            newContact.emailAddress = Prompt("Enter contact's email address:\t").Trim().ToUpper();

            return newContact;
        }
        #endregion//Add a contact to the database

        #region writeToFileFunction (after adding a new contact)
        //updates text file after user inputs contacts info
        static void toUpdateContacts(contact newOne)
        {
            StreamReader updates = new StreamReader(pathOFile);
            contact[] database = new contact[numberOfContacts()];
            contact contactSplit;
            string lineData;
            int counter = 0;

            while(!updates.EndOfStream)
            {
                lineData = updates.ReadLine();
                contactSplit = populateContacts(lineData);
                database[counter] = contactSplit;
                counter++;
            }
            updates.Close();

            StreamWriter newFile = new StreamWriter(pathOFile);

            for (int i = 0; i < database.Length; i ++)
            {
                if (database[i].firstName != "")
                {
                    newFile.WriteLine("{0},{1},{2},{3}", database[i].firstName, database[i].lastName, database[i].phoneNumber, database[i].emailAddress);
                }
            }
                newFile.WriteLine("{0},{1},{2},{3}", newOne.firstName, newOne.lastName, newOne.phoneNumber, newOne.emailAddress);

            newFile.Close();

        }
        #endregion//Reads from the contact txt file and then writes back to the file including new contact

        #region searchFunction
        //searchFunction
        static contact searchFunction(string first, string last, contact[] group)
        {
            contact answer = new contact();
            for (int i = 0; i < group.Length; i += 1)
            {
                if (group[i].firstName == first && group[i].lastName == last)
                {
                    answer = group[i];
                    return answer;
                }
            }
            answer.lastName = "";
            answer.firstName = "";
            return answer;

        }
        //End searchFunction
        #endregion

        #region searchForAContact (user option 2)
        //Display contact and their information
        static void viewContact()
        {
            string first = Prompt("Enter First Name:\t").Trim().ToUpper();
            string last = Prompt("Enter Last Name:\t").Trim().ToUpper();
            contact Display = searchFunction(first, last, updatedList());

            if (Display.lastName == "" && Display.firstName == "")
            {
                Console.WriteLine("\nThere's no contact matching that input.\nPress any key to continue.");
                Console.ReadKey(true);
            }
            else
            {
                Console.Write("\n\n" + Display.lastName + ", " + Display.firstName + "\n" + Display.phoneNumber + "\n" + Display.emailAddress);
                Console.WriteLine("\n\nPress any key to continue");
                Console.ReadKey(true);
                Console.Clear();
            }
        }
        #endregion//Display contact lastname, firstname, number, email

        #region changesToContacts
        //Write changes to the txt file
        static void changesToContact(contact[] data)
        {
            int length = numberOfContacts();
            StreamWriter newFile = new StreamWriter(pathOFile);

            for (int i = 0; i < length; i++)
            {
                newFile.WriteLine("{0},{1},{2},{3}", data[i].firstName, data[i].lastName, data[i].phoneNumber, data[i].emailAddress);
            }
         
            newFile.Close();
        }
        #endregion

        #region modifyContact'sNumberFunction (user option 3)
        //Allows user to modify a contacts phone number
        static contact[] modifyContactsNumber(contact[] database)
        {
            int Ascend = 1;
            int NumSelect;
            if (database.Length == 0)
            {
                Console.WriteLine("No current contacts.\nPress any key to continue.");
                Console.ReadKey(true);
            }
            else
            {
                for (int y = 0; y < database.Length; y += 1)
                {
                    if (database[y].firstName != null)
                    {
                        Console.WriteLine(Ascend + ")\t" + database[y].lastName + ", " + database[y].firstName + "\n\t" + database[y].phoneNumber + "\n\t" + database[y].emailAddress + "\n");
                    }
                    Ascend++;
                }

                NumSelect = int.Parse(Prompt("Which number would you like to modify?\t"));
                NumSelect = NumSelect - 1;
                database[NumSelect].phoneNumber = Prompt("\nEnter the new number for " + database[NumSelect].firstName + ":\t").Trim();
            }

            contact[] newData = database;
            return newData;
        }
        #endregion//Modify a phone number

        #region modifyContact'sEmailFunction (user option 4)
        //Used to allow user to change a contacts E-mail address
        static contact[] modifyContactsEmail(contact[] database)
        {
            int Ascend = 1;
            int NumSelect = 0;
            if (database.Length == 0)
            {
                Console.WriteLine("No current contacts.\nPress any key to continue.");
                Console.ReadKey(true);
            }
            else
            {
                for (int y = 0; y < database.Length; y += 1)
                {
                    if (database[y].firstName != null)
                    {
                        Console.WriteLine(Ascend + ")\t" + database[y].lastName + ", " + database[y].firstName + "\n\t" + database[y].phoneNumber + "\n\t" + database[y].emailAddress + "\n");
                    }
                    Ascend++;
                }

                    NumSelect = int.Parse(Prompt("Which email address would you like to modify?\t"));
                    NumSelect = NumSelect - 1;
                    database[NumSelect].emailAddress = Prompt("\nEnter the new email for " + database[NumSelect].firstName + ":\t").Trim().ToUpper();
                
            }

            contact[] newData = database;
            return newData;
        }
        #endregion//Change the a contact's email address

        #region deleteContactFunction (user option 5)
        //Used to delete contact from the database
        static void deleteContact()
        {
            int Length = numberOfContacts();
            int Ascend = 1;
            int NumSelect = 0;
            contact[] database = updatedList();

            for (int a = 0; a < Length; a++)
            {
                if (database[a].firstName != null)
                {
                    Console.WriteLine(Ascend + ")\t" + database[a].lastName + ", " + database[a].firstName + "\n\t" + database[a].phoneNumber + "\n\t" + database[a].emailAddress + "\n");
                }
                Ascend++;
            }

            NumSelect = int.Parse(Prompt("Which one would you like to delete?\t"));
            NumSelect = NumSelect - 1;
            database[NumSelect].firstName = "";
            database[NumSelect].lastName = "";
            database[NumSelect].phoneNumber = "";
            database[NumSelect].emailAddress = "";

            StreamWriter newFile = new StreamWriter(pathOFile);

            for (int i = 0; i < database.Length; i++)
            {
                if (database[i].firstName != "")
                {
                    newFile.WriteLine("{0},{1},{2},{3}", database[i].firstName, database[i].lastName, database[i].phoneNumber, database[i].emailAddress);
                }
            }

            newFile.Close();
        }
        #endregion//Delete contact from database

        #region showAllFunction (user option 6)
        //Show all contacts in text file 
        static void showAllFunction(contact[] database)
        {
            Console.Clear();
            int Ascend = 1;
            if (database.Length == 0)
            {
                Console.Write("No current entries.\n");
                Console.Write("Press any key to continue.\t");
                Console.ReadKey();
                Console.Clear();
            }
            else
            {
                for (int b = 0; b < database.Length; b++)
                {
                    Console.WriteLine(Ascend + ")\t" + database[b].lastName + ", " + database[b].firstName + "\n\t" + database[b].phoneNumber + "\n\t" + database[b].emailAddress + "\n");
                    Ascend++;
                }

                Console.WriteLine();
                Console.Write("Press any key to continue.\t");
                Console.ReadKey();
                Console.Clear();
            }
        }
        #endregion//Show contacts currently within database

        #region comparesNamesOfContacts
        static int comparison(string contactI, string iMinusOne)
        {
            if (contactI == iMinusOne)
            { return  0; }

            int size = Math.Min(contactI.Length, iMinusOne.Length);

            for (int index = 0; index < size; index += 1)
            {
                char contactIChars = contactI[index];
                char iMinusOneChars = iMinusOne[index];

                if (contactIChars != iMinusOneChars)
                {
                    if (contactIChars < iMinusOneChars) { return -1; }
                    if (contactIChars > iMinusOneChars) { return  1; }
                }
            }//end for

            if (contactI.Length < iMinusOne.Length)
            { return -1; }
            else if (contactI.Length > iMinusOne.Length)
            { return  1; }
            else
            { return  0; }
        }
        #endregion

        #region abcOrderFunction (user option 7)
        //Organizes contacts in ABC order
        static void abcOrder()

        {
            contact[] currentContacts = updatedList();
            int order;
            bool swapped;

            do
            {
                order = 0;
                swapped = false;

                for (int i = 1; i < currentContacts.Length; i ++)
                {
                    order = comparison(currentContacts[i].lastName, currentContacts[i - 1].lastName);
                    if (order == -1)
                    {
                        contact temp = currentContacts[i - 1];
                        currentContacts[i - 1] = currentContacts[i];
                        currentContacts[i] = temp;
                        swapped = true;
                    }
                    else if (order == 0)
                    {
                        order = comparison(currentContacts[i].firstName, currentContacts[i - 1].firstName);
                        if (order == -1)
                        {
                            contact temp = currentContacts[i - 1];
                            currentContacts[i - 1] = currentContacts[i];
                            currentContacts[i] = temp;
                            swapped = true;
                        }
                    }

                }

            } while (swapped);

            #region writeNewText
            StreamWriter outFile = new StreamWriter("C:\\temp\\contactList.txt");
            for (int i = 0; i < currentContacts.Length; i += 1)
            {
                contact Display = currentContacts[i];
                Console.Write(Display.lastName + ", " + Display.firstName + "\n" + Display.phoneNumber + "\n" + Display.emailAddress + "\n\n");
                outFile.WriteLine("{0},{1},{2},{3}", currentContacts[i].firstName, currentContacts[i].lastName, currentContacts[i].phoneNumber, currentContacts[i].emailAddress);
            }
            outFile.Close();

            Console.Write("Press any key to go to the main menu.\t");
            Console.ReadKey(true);
            #endregion
        }
        #endregion//Lexicographical Order

        #region exitAppFunction (user option 8)
        //Exit the program
        static bool exitApp()
        {
            Console.Clear();
            Console.Write("Press any key to close\t");
            Console.ReadKey(true);
            return false;
        }
        #endregion//Exit Program
        //End Contact Functions

        #region Read Page (user enters 0)
        //Voice Function
        static void talkThat(string words)
        {
            SpeechSynthesizer voiceBox = new SpeechSynthesizer();

            voiceBox.SelectVoiceByHints(VoiceGender.Female);
            voiceBox.Volume = 100;
            voiceBox.Rate = 0;
            voiceBox.Speak(words);
        }
        //End Voice Function
        #endregion

    }//End Class
}//End Name
