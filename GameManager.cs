// Klasser och metoder för att hantera spelets gång 

using System.Text.Json;       // För att serialisering och deserialisering till/från JSON-fil.
using static System.Console;  // För att slippa skriva Console framför Write/WriteLine
using System.Diagnostics;  // För att använda Stopwatch

/* Här stopp*/
namespace quiz
{
    public class GameManager
    {
        private string fileName = @"toplist.json"; // Filväg till json-fil där topplistan lagras
        private List<TopList> topList = new();      // Lista för poängen

        // Konstruktor
        public GameManager()
        {
            if (File.Exists(fileName)) // Kontroll om json-fil finns
            {
                try
                {
                    // Läs in text från filen
                    string jsonString = File.ReadAllText(fileName);                      // Läs in text från filen
                    topList = JsonSerializer.Deserialize<List<TopList>>(jsonString)!;    // Deserialiserar JSON till listan
                }
                catch (Exception ex)
                {
                    WriteLine($"Fel vid inläsning av json-fil, {ex.Message}");
                }
            }
        }

        // Metod för att skapa ett nytt objekt av klassen TopList
        public TopList AddTopList(string name, int points, int maxPoints, double time)
        {
            TopList obj = new TopList(name, points, maxPoints, time);     // Användning av konstruktorn
            topList.Add(obj);             // Lägg till objekt i listan
            SaveToFile();                 // Spara till json-fil
            return obj;
        }

        // Metod för att hämta listan publikt
        public List<TopList> GetTopList()
        {
            return topList;
        }

        // Metod för att spara topplistan med serialize
        public void SaveToFile()
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(topList);
                File.WriteAllText(fileName, jsonString);
            }
            catch (Exception ex)
            {
                WriteLine($"Fel vid sparandet av json-filen: {ex.Message}");
            }
        }

        // Metod för att skriva ut topplistan
        public void ShowTopList()
        {
            if (topList.Count == 0)        // Kontroll så att den inte är tom
            {
                WriteLine("Det finns inga resultat i topplistan just nu.");
            }
            else
            {
                WriteLine("T O P P  - T I O\n");

                // Sortera topplistan efter poäng och sedan efter tid om poängen är lika med LINQ
                var sortedTopList = topList
                    .OrderByDescending(t => t.Points)       // Sortera efter poäng i fallande ordning
                    .ThenByDescending(t => t.Time)          // Sortera efter tid i fallande ordning
                    .Take(10);                              // Visa endast de 10 bästa 

                int i = 1;
                foreach (TopList score in sortedTopList)     // Loopa genom topplistan och skriv ut
                {
                    WriteLine($"{i++}. {score.PlayerName} - {score.Points} rätt av {score.MaxPoints} (tid: {Math.Round(score.Time)} sek)");
                }
            }
        }


        // Metod för att rensa hela topplistan
        public void DeleteTopList()
        {
            WriteLine("Är du säker på att du vill radera hela topplistan? y / n ");

            var keyInfo = ReadKey(intercept: true);  // Läs in tangent utan att visa den

            if (keyInfo.Key == ConsoleKey.Y)         // Om tangent 'y' trycks
            {
                topList.Clear();                     // Radera listan
                SaveToFile();                        // Spara raderingen
                WriteLine("\nTopplistan har raderats.");
            }
            else if (keyInfo.Key == ConsoleKey.N)    // Om tangent 'n' trycks
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

        // Metod för att spela quizet med listan av frågor som parameter
        public void PlayQuiz(List<Quiz> questions)
        {
            if (questions == null || questions.Count == 0)      // Kontroll om listan är null/inga frågor
            {
                WriteLine("Det finns inga frågor i quizet just nu.");
            }
            else
            {
                string? name;
                CursorVisible = true;
                Write("Skriv in ditt namn: ");

                while (string.IsNullOrWhiteSpace(name = ReadLine()))        // Kontroll om null/whitespace
                {
                    Clear();
                    WriteLine("Du måste skriva in ett giltigt namn!");
                    Write("Skriv in ditt namn: ");
                }

                Clear();
                CursorVisible = false;
                WriteLine($"Hej {name} och välkommen till quizet!\n");
                WriteLine("Tryck på valfri tangent för att börja quiza!");
                WriteLine("(Svara 'x' om du vill avbryta quizet.)");
                ReadKey();

                int score = 0;                        // För att räkna poäng
                int questionNumber = 1;               // Numrera frågorna, start på 1
                bool quizCancelled = false;           // Hålla koll på om spelaren vill avbryta quizet

                Stopwatch timer = new();              
                timer.Start();                         // Starta tidtagning

                // Loopa genom alla frågor
                foreach (var question in questions)
                {
                    Clear();
                    CursorVisible = true;
                    WriteLine($"Fråga {questionNumber} av {questions.Count}");    // Skriv ut frågenumret
                    WriteLine(question.Question);                                 // Skriv ut frågan
                    Write("\nSvar: ");
                    string? answer = ReadLine();

                    // Kontroll om spelaren vill avbryta quizet
                    if (answer != null && answer.Trim().Equals("X", StringComparison.CurrentCultureIgnoreCase))
                    {
                        quizCancelled = true;
                        break;
                    }

                    // Kontroll null/whitespace och visa frågan igen tills giltigt svar eller avbryt
                    while (string.IsNullOrWhiteSpace(answer) || answer.Trim().Equals("X", StringComparison.CurrentCultureIgnoreCase))
                    {
                        if (answer != null && answer.Trim().Equals("X", StringComparison.CurrentCultureIgnoreCase))
                        {
                            quizCancelled = true;
                            break;
                        }

                        Clear();
                        WriteLine("Du måste skriva in ett giltigt svar! Eller tryck 'x' för att avbryta quizet.\n");
                        WriteLine($"Fråga {questionNumber} av {questions.Count}");
                        WriteLine(question.Question);
                        Write("\nSvar: ");
                        answer = ReadLine();
                    }

                    // Om quizet avbryts, gå ur loopen
                    if (quizCancelled)
                    {
                        break;
                    }

                    // Kontrollera om svaret är rätt
                    if (string.Equals(answer.Trim(), question.Breed, StringComparison.OrdinalIgnoreCase))
                    {
                        WriteLine("\nRätt svar!");
                        score++;                        // Lägg till poäng
                    }
                    else
                    {
                        WriteLine($"\nTyvärr är det fel svar. Rätt svar är: {question.Breed}.");
                    }

                    CursorVisible = false;
                    WriteLine("Tryck på valfri tangent för att fortsätta...");
                    ReadKey();

                    questionNumber++;  // Öka frågenumret efter varje fråga
                }

                timer.Stop();                                    // Stoppa tidtagningen
                double playerTime = timer.Elapsed.TotalSeconds;  // Tiden i sekunder
                Clear();
                CursorVisible = false;

                // Om quizet avbryts
                if (quizCancelled)
                {
                    WriteLine("Quizet avbröts, ditt resultat sparas inte. Tryck på valfri tangent för att återgå till menyn.");
                    ReadKey();
                }
                else
                {
                    WriteLine($"Quizet är slut! Du fick {score} poäng av totalt {questions.Count} möjliga, tid: {Math.Round(playerTime)} sekunder.\n");
                    WriteLine("Tryck på valfri tangent för att återgå till menyn.");
                    ReadKey();

                    // Lägger till namn och poäng på topplistan
                    AddTopList(name, score, questions.Count, playerTime);
                }
            }
        }
    }
}
