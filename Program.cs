/*

*/

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

            // Skapa nytt objekt av QuizManager
            QuizManager quizManager = new();

            // Skapa nytt objekt av GameManager
            GameManager gameManager = new();

            // While-loop för att fortsätta köra applikationen tills den avslutas
            while (true)
            {
                Clear();  // Rensa konsolen

                CursorVisible = false;  // Dölj markör
                // Visa huvudmeny
                WriteLine("H U N D - Q U I Z \n");
                WriteLine("Välj vad du vill göra:");
                WriteLine("1. Spela quiz");
                WriteLine("2. Visa topplista");
                WriteLine("3. Hantera quizet och topplistan");
                WriteLine("X. Avsluta spelet");

                // Användarens val med ReadKey
                ConsoleKeyInfo keyInfo = ReadKey(true);
                char choice = keyInfo.KeyChar;

                // Switch-sats för de olika valen
                switch (char.ToLower(choice))
                {
                    case '1':
                        Clear();
                        var questions = quizManager.GetDogs();      // Hämta frågorna från quizManager
                        gameManager.PlayQuiz(questions);            // Spela quizet med frågorna
                        break;

                    case '2':
                        Clear();
                        gameManager.ShowTopList();
                        WriteLine("\nTryck på valfri tangent för att återgå till menyn.");
                        ReadKey();
                        break;



                    // Undermeny för att hantera frågorna (visa, lägga till/ta bort)
                    case '3':
                        Clear();

                        // Boolean för att hålla koll på när loopen för undermenyn ska brytas/återgå till huvudmeny
                        bool toMainMenu = false;

                        while (!toMainMenu)
                        {
                            Clear();  // Rensa konsolen

                            CursorVisible = false;  // Dölj markör
                            WriteLine("Välj vad du vill göra:\n");
                            WriteLine("1. Lägg till fråga");
                            WriteLine("2. Ta bort fråga");
                            WriteLine("3. Visa alla frågor");
                            WriteLine("4. Radera topplistan");
                            WriteLine("5. Visa topplistan\n");
                            WriteLine("X. Återgå till huvudmenyn");

                            // Användarens val för undermenyn
                            keyInfo = ReadKey(true);
                            char subChoice = keyInfo.KeyChar;

                            // Switch-sats för undermenyn
                            switch (char.ToLower(subChoice))
                            {
                                case '1':
                                    Clear();

                                    string? inputQuestion;
                                    string? inputBreed;

                                    CursorVisible = true;  // Visa markör
                                    Write("Skriv in en ny fråga: ");

                                    // Kör så länge inte användaren anger en "riktig" fråga och inte null/whitespace
                                    while (string.IsNullOrWhiteSpace(inputQuestion = ReadLine()))
                                    {
                                        Clear();
                                        WriteLine("Du måste skriva in en fråga, försök igen!");
                                        Write("Skriv in en fråga: ");
                                    }

                                    Write("Skriv in rätt ras på frågan: ");

                                    // Kör så länge inte användaren anger ett "riktigt" svar
                                    while (string.IsNullOrWhiteSpace(inputBreed = ReadLine()))
                                    {
                                        Clear();
                                        WriteLine("Du måste ange ett svar, försök igen!");
                                        Write("Skriv in rätt ras på frågan: ");
                                    }

                                    // Kör metoden AddDog och spara om allt stämmer
                                    quizManager.AddDog(inputQuestion, inputBreed);

                                    WriteLine("\nFrågan har lagts till! Tryck på valfri tangent för att återgå till undermenyn...");
                                    ReadKey();
                                    break;

                                case '2':
                                    Clear();
                                    // Skriv ut alla frågor som finns
                                    quizManager.ShowQuiz();

                                    // Kontroll om det finns några frågor i quizet
                                    if (quizManager.GetDogs().Count == 0)
                                    {
                                        WriteLine("Inga frågor finns i quizet. Tryck på valfri tangent för att återgå till undermenyn...");
                                        ReadKey();
                                        break;
                                    }

                                    // Fortsätt tills giltigt index anges
                                    while (true)
                                    {
                                        Write("\nAnge index på frågan som ska tas bort: ");

                                        // Konvertera till int samt kontrollera att index är rätt
                                        if (int.TryParse(ReadLine(), out int index) && index >= 0 && index < quizManager.GetDogs().Count)
                                        {
                                            try
                                            {
                                                quizManager.DeleteDog(index);   // Radera fråga
                                                WriteLine("Frågan har tagits bort! Tryck på valfri tangent för att återgå till undermenyn.");
                                                ReadKey();
                                                break;
                                            }
                                            catch (Exception)
                                            {
                                                WriteLine("Det blev något fel. Tryck på valfri tangent för att fortsätta.");
                                                ReadKey();
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            QuizManager.ErrorMessage();
                                        }
                                    }
                                    break;

                                case '3':
                                    Clear();
                                    // Skriv ut alla frågor som finns
                                    quizManager.ShowQuiz();
                                    WriteLine("\nTryck på valfri tangent för att återgåt till undermenyn.");
                                    ReadKey();
                                    break;

                                case '4':
                                    Clear();
                                    gameManager.DeleteTopList();
                                    WriteLine("Topplistan är raderad. Tryck på valfri tangent för att återgå till undermenyn.");
                                    ReadKey();
                                    break;

                                case '5':
                                    Clear();
                                    gameManager.ShowTopList();
                                    WriteLine("\nTryck på valfri tangent för att återgå till undermenyn.");
                                    ReadKey();
                                    break;

                                case 'x':
                                    Clear();
                                    toMainMenu = true;   // Återgå till huvudmenyn, stoppa loop
                                    break;

                                default:
                                    QuizManager.ErrorMessage();
                                    break;
                            }
                        }

                        break;

                    case 'x':
                        Clear();
                        Environment.Exit(0);  // Avsluta programmet
                        break;

                    default:
                        QuizManager.ErrorMessage();
                        break;
                }
            }
        }
    }
}


/*
Att göra: 

Fixa så att det inte går att skriva null/whitespace som svar
Lägg till index (från 1) på topplistan. 
Lägg till möjlighet att ta bort topplistan?
Fixa att visaren inte syns förräns man ska skriva in något
Fixa tidtagning


[0] Vilken hundras är känd för att vara en bra ledarhund för blinda? - Labrador
[1] Vilken hundras är den minsta i världen? - Chihuahua
[2] Vilken hundras har sitt ursprung i Sibirien och är känd för sin uthållighet i slädhundstävlingar? - Siberian husky
[3] Vilken hundras är känd för sin blå-svarta tunga? - Chow chow
[4] Vilken hundras är känd för sitt utseende med veckad hud och rynkor? - Shar pei
[5] Vilken hundras användes ursprungligen för att jaga uttrar och är känd för sitt vattenavstötande päls? - Irländsk vattenspaniel
[6] Vilken hundras är känd för sin snabba hastighet och användes traditionellt för jakt på harar? - Greyhound
[7] Vilken liten terrier är känd för sin modiga personlighet och har sitt ursprung i Skottland? - Skotsk terrier
[8] Vilken hundras, med sitt ursprung i Kina, är känd för sin "lejonlika" päls och sitt platta ansikte? - Pekinges
[9] Vilken hundras är känd för sin snabbhet och intelligens och används ofta i agility-tävlingar? - Border collie
[10] Vilken hundras, som är en av världens äldsta, har en lockig, ulliknande päls och sitt ursprung i Ungern? - Puli

*/
