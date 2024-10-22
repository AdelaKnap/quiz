// Klasser och metoder för att hantera menyn som visas i program.cs

using static System.Console;

namespace quiz
{
    public class MenuManager
    {
        // Metod för att skriva ut huvudmenyn
        public void ShowMenu()
        {
            Clear();
            CursorVisible = false;
            WriteLine("H U N D - Q U I Z \n");
            WriteLine("Välj vad du vill göra:\n");
            WriteLine("1. Spela quiz");
            WriteLine("2. Visa topplista");
            WriteLine("3. Admin: Hantera quizet och topplistan\n");
            WriteLine("X. Avsluta quizet");
        }

        // Metod för att skriva ut undermenyn
        public void ShowSubMenu()
        {
            Clear();
            CursorVisible = false;
            WriteLine("A D M I N I S T R A T I O N \n");
            WriteLine("Välj vad du vill göra:\n");
            WriteLine("1. Visa alla frågor");
            WriteLine("2. Lägg till fråga");
            WriteLine("3. Ta bort fråga");
            WriteLine("4. Visa topplistan");
            WriteLine("5. Radera topplistan\n");
            WriteLine("X. Återgå till huvudmenyn");
        }

        // Menyval för att lägga till fråga i quizet med inputQuestion och breed som parametrar
        public void AddQuestion(out string inputQuestion, out string inputBreed)
        {
            Clear();
            CursorVisible = true;         // Visa markör
            Write("Skriv in en ny fråga: ");

            // Kör så länge inte användaren anger en "riktig" fråga och inte null/whitespace
            while (string.IsNullOrWhiteSpace(inputQuestion = ReadLine() ?? ""))
            {
                Clear();
                WriteLine("Du måste skriva in en fråga, försök igen!");
                Write("Skriv in en fråga: ");
            }

            Write("Skriv in rätt svar/ras på frågan: ");

            // Kör så länge inte användaren anger ett "riktigt" svar
            while (string.IsNullOrWhiteSpace(inputBreed = ReadLine() ?? ""))
            {
                WriteLine("\nDu måste ange ett svar, försök igen!");
                Write("Skriv in rätt ras på frågan: ");
            }
        }

        // Ta bort en fråga från quizet, objektet quizManager skickas med
        public void DeleteQuestion(QuizManager quizManager)
        {
            Clear();
            quizManager.ShowQuiz(); // Skriv ut alla frågor som finns

            // Kontroll om det finns några frågor i quizet
            if (quizManager.GetQuiz().Count == 0)
            {
                WriteLine("Inga frågor finns i quizet. Tryck på valfri tangent för att återgå till undermenyn.");
                ReadKey();
                return; // Avsluta om det inte finns några frågor
            }

            // Fortsätt tills giltigt index anges
            while (true)
            {
                CursorVisible = true;
                Write("\nAnge index på frågan som ska tas bort: ");

                // Konvertera till int samt kontrollera att index är rätt
                if (int.TryParse(ReadLine(), out int index) && index >= 0 && index < quizManager.GetQuiz().Count)
                {
                    try
                    {
                        quizManager.DeleteQuiz(index); // Radera fråga
                        WriteLine($"\nFråga {index} har tagits bort! Tryck på valfri tangent för att återgå till undermenyn.");
                        ReadKey();
                        break; // Bryt när frågan har tagits bort
                    }
                    catch (Exception)
                    {
                        WriteLine("Det blev något fel vid borttagning av frågan. Tryck på valfri tangent för att fortsätta.");
                        ReadKey();
                        break;
                    }
                }
                else
                {
                    ErrorMessage(); // Visa felmeddelande om index är ogiltigt
                    Clear();
                    quizManager.ShowQuiz(); // Visa frågorna igen
                }
            }
        }

        // Metod för att skriva ut felmeddelande
        public void ErrorMessage()
        {
            WriteLine("\nFelaktigt val, tryck på valfri tangent för att testa igen!");
            ReadKey();
        }
    }
}
