using System.Text.Json;
using static System.Console;  // För att slippa skriva Console framför Write/WriteLine

namespace quiz
{
    public class GameManager
    {
        private string fileName = @"topplist.json"; // Filväg till json-fil där topplistan lagras
        private List<TopList> topList = new();      // Lista för poängen

        // Konstruktor
        public GameManager()
        {
            if (File.Exists(fileName)) // Kontroll om json-fil finns
            {
                // Läs in text från filen
                string jsonString = File.ReadAllText(fileName);
                topList = JsonSerializer.Deserialize<List<TopList>>(jsonString)!; // Deserialiserar JSON till listan
            }
        }

        // Metod för att skapa ett nytt objekt av klassen TopList
        public TopList AddTopList(string name, int points, int maxPoints)
        {
            TopList obj = new TopList(name, points, maxPoints);     // Användning av konstruktorn
            topList.Add(obj);
            SaveToFile();
            return obj;
        }

        // Metod för att hämta listan
        public List<TopList> GetTopList()
        {
            return topList;
        }

        // Metod för att spara med serialize
        public void SaveToFile()
        {
            var jsonString = JsonSerializer.Serialize(topList);
            File.WriteAllText(fileName, jsonString);
        }

        // Metod för att skriva ut topplistan
        public void ShowTopList()
        {
            if (topList.Count == 0)
            {
                WriteLine("Det finns inga resultat i topplistan just nu.");
            }
            else
            {
                WriteLine("T O P P L I S T A N\n");

                int i = 1;
                // Sortera topplistan efter poäng i fallande ordning
                var sortedTopList = topList.OrderByDescending(t => t.Points);
                foreach (TopList score in sortedTopList)
                {
                    WriteLine($"{i++}. {score.PlayerName} - {score.Points} rätt av {score.MaxPoints}");
                }
            }
        }

        // Metod för att rensa hela topplistan
        public void DeleteTopList()
        {
            WriteLine("Är du säker på att du vill radera hela topplistan? y / n ");

            var keyInfo = ReadKey(intercept: true);  // Läs in tangent utan att visa den

            if (keyInfo.Key == ConsoleKey.Y)         // Om 'Y' trycks
            {
                topList.Clear();                     // Radera listan
                SaveToFile();                        // Spara raderingen
                WriteLine("\nTopplistan har raderats.");
            }
            else if (keyInfo.Key == ConsoleKey.N)    // Om 'N' trycks
            {
                WriteLine("\nInga ändringar gjordes.");
            }
            else
            {
                WriteLine("\nOgiltigt val.");
            }

            // Återgå till menyn
            WriteLine("Tryck på valfri tangent för att återgå till menyn.");
            ReadKey();
        }

        // Metod för att spela quizet med listan Dog som parameter
        public void PlayQuiz(List<Dog> questions)
        {
            string? name;

            Write("Skriv in ditt namn: ");

            while (string.IsNullOrWhiteSpace(name = ReadLine()))        // Kontroll om null/whitespace
            {
                Clear();
                WriteLine("Du måste skriva in ett giltigt namn!");
                Write("Skriv in ditt namn: ");
            }

            Clear();
            WriteLine($"Hej {name} och välkommen till hundquizet!\n");
            WriteLine("Tryck på valfri tangent för att böra quiza!");
            WriteLine("(Tryck 'Esc' om du vill avbryta spelet under quizets gång.)");
            ReadKey();


            int score = 0;                        // För att räkna poäng
            int questionNumber = 1;               // Numrera frågorna
            bool quizCancelled = false;           // Boolean för att hålla koll på om spelaren vill avbryta quizet

            // Loopa genom alla frågor
            foreach (var question in questions)
            {
                Clear();
                WriteLine($"Fråga {questionNumber} av {questions.Count}");              // Skriv ut vilken fråga det är
                WriteLine(question.Question);                                           // Skriv ut frågan
                Write("\nSvar: ");

                // Kontrollera om Esc trycktes för att svalsuta spelet utan att spara
                var keyInfo = ReadKey(intercept: true);

                if (keyInfo.Key == ConsoleKey.Escape)
                {
                    quizCancelled = true;
                    break;
                }
                // Det inskrivna svaret
                string? answer = ReadLine();

                while (string.IsNullOrWhiteSpace(answer))     // Kontroll null/whitespace
                {
                    WriteLine("\nDu måste skriva in ett giltigt svar!");
                    Write("Svar: ");
                    answer = ReadLine();
                }

                // Kontrollera svaret med String.Equals, oavsett stora/små bokstäver
                if (string.Equals(answer, question.Breed, StringComparison.OrdinalIgnoreCase))
                {
                    WriteLine("\nRätt svar!");
                    score++;                        // Lägg till poäng
                }
                else
                {
                    WriteLine($"\nTyvärr är det fel svar. Rätt svar är: {question.Breed}.");
                }

                WriteLine("Tryck på valfri tangent för att fortsätta...");
                ReadKey();

                questionNumber++;  // Öka frågenumret efter varje fråga
            }

            Clear();

            // Om quizet avbryts
            if (quizCancelled)
            {
                WriteLine("Quizet avbröts, ditt resultat sparas inte. Tryck på valfri tangent för att återgå till menyn.");
                ReadKey();
            }
            else
            {
                WriteLine($"Quizet är slut! Du fick {score} poäng av totalt {questions.Count} möjliga.\n");
                WriteLine("Tryck på valfri tangent för att återgå till meny.");
                ReadKey();

                // Lägger till namn och poäng på topplistan
                AddTopList(name, score, questions.Count);
            }
        }

    }
}
