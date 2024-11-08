/* DT071G Projektuppgift. En konsolapplikation där användaren kan spela ett quiz samt hantera quizets olika delar. Av Adela Knap */

using static System.Console;  // För att slippa skriva Console framför Write/WriteLine

namespace quiz
{
    class Program
    {
        static void Main(string[] args)
        {
            // För att det ska fungera med å, ä, ö
            OutputEncoding = System.Text.Encoding.Unicode;
            InputEncoding = System.Text.Encoding.Unicode;

            // Skapa nytt objekt av MenuManager, QuizManager och GameManager
            MenuManager menuManager = new();
            QuizManager quizManager = new();
            GameManager gameManager = new();

            // While-loop för att fortsätta köra applikationen tills den avslutas
            while (true)
            {
                // Visa huvudmeny
                menuManager.ShowMenu();

                // Användarens val med ReadKey
                ConsoleKeyInfo keyInfo = ReadKey(true);
                char choice = keyInfo.KeyChar;

                // Switch-sats för de olika valen
                switch (char.ToLower(choice))
                {
                    case '1':
                        Clear();
                        var questions = quizManager.GetQuiz();      // Hämta frågorna från quizManager
                        gameManager.PlayQuiz(questions);            // Spela quizet med frågorna som parameter
                        break;

                    case '2':
                        Clear();
                        gameManager.ShowTopList();    // Visa topplistan
                        WriteLine("\nTryck på valfri tangent för att återgå till menyn.");
                        ReadKey();
                        break;

                    // Undermeny för att hantera quizet
                    case '3':
                        Clear();
                        bool toMainMenu = false;   // Boolean för att hålla koll på när loopen för undermenyn ska brytas/återgå till huvudmeny

                        while (!toMainMenu)
                        {
                            menuManager.ShowSubMenu();   // Visa undermenyn

                            keyInfo = ReadKey(true);
                            char subChoice = keyInfo.KeyChar;

                            // Switch-sats för undermenyn
                            switch (char.ToLower(subChoice))
                            {
                                case '1':
                                    Clear();
                                    quizManager.ShowQuiz();         // Skriv ut alla frågor som finns
                                    WriteLine("\nTryck på valfri tangent för att återgå till undermenyn.");
                                    ReadKey();
                                    break;

                                case '2':
                                    Clear();

                                    string? inputQuestion;
                                    string? inputBreed;

                                    menuManager.AddQuestion(out inputQuestion, out inputBreed);       // Lägga till ny fråga i quizet
                                    quizManager.AddToQuiz(inputQuestion, inputBreed);                 // Kör metoden AddToQuiz och spara om allt stämmer

                                    WriteLine("\nFrågan har lagts till! Tryck på valfri tangent för att återgå till undermenyn.");
                                    ReadKey();
                                    break;

                                case '3':
                                    Clear();
                                    menuManager.DeleteQuestion(quizManager);         // Ta bort fråga, metod med objektet quizManager som parameter
                                    break;

                                case '4':
                                    Clear();
                                    gameManager.ShowTopList();          // Visa topplistan
                                    WriteLine("\nTryck på valfri tangent för att återgå till undermenyn.");
                                    ReadKey();
                                    break;

                                case '5':
                                    Clear();
                                    gameManager.DeleteTopList();        // Radera topplistan
                                    break;

                                case 'x':
                                    Clear();
                                    toMainMenu = true;   // Återgå till huvudmenyn, stoppa loop
                                    break;

                                default:
                                    menuManager.ErrorMessage();     // Felmeddelande 
                                    break;
                            }
                        }

                        break;

                    case 'x':
                        Clear();
                        Environment.Exit(0);  // Avsluta programmet
                        break;

                    default:
                        menuManager.ErrorMessage();
                        break;
                }
            }
        }
    }
}

