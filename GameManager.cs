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
        public TopList AddTopList(string name, int points)
        {
            TopList obj = new TopList(name, points);     // Användning av konstruktorn
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
                Console.WriteLine("Det finns inga resultat i topplistan just nu.");
            }
            else
            {
                // Sortera topplistan efter poäng i fallande ordning
                var sortedTopList = topList.OrderByDescending(t => t.Points);
                foreach (TopList score in sortedTopList)
                {
                    Console.WriteLine($"{score.PlayerName} - {score.Points}");
                }
            }
        }

        // Metod för att spela quizet med listan Dog som parameter
        public void PlayQuiz(List<Dog> questions)
        {
            Clear();
            string? name = string.Empty; // Sätt namn till en tom stärng först

            while (string.IsNullOrEmpty(name))  // Kontroll om namnet är korrekt angivet
            {
                WriteLine("Skriv in ditt namn: ");
                name = ReadLine();
            }

            int score = 0;  // För att räkna poäng

            // Loopa genom alla frågor
            foreach (var question in questions)
            {
                WriteLine(question.Question);   // Skriv ut frågan
                WriteLine("Skriv vilken ras du tror att det är: ");

                string? answer = ReadLine();

                // Kontrollera av svar med String.Equals med en jämförelse oavsett stora/små bokstäver
                if (string.Equals(answer, question.Breed, StringComparison.OrdinalIgnoreCase))
                {
                    WriteLine("Rätt svar!");
                    score++;       // Lägg till poäng
                }
                else
                {
                    WriteLine($"Tyärr är det fel svar. Rätt svar är: {question.Breed}.");
                }

                WriteLine("Tryck på valfri tangent för att fortsätta...");
                ReadKey();
            }

            WriteLine($"Quizet är slut! Du fick: {score} poäng av totalt {questions.Count} möjliga.");

            // Lägger till namn och poäng på topplistan
            AddTopList(name, score);
        }

    }
}
