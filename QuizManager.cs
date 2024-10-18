// Klasser och metoder för att hantera quizet

using static System.Console;  // För att slippa skriva Console framför Write/WriteLine
using System.Text.Json;       // För att serialisering och deserialisering till/från JSON-fil.

namespace quiz
{

    public class QuizManager
    {
        private string filePath = @"quiz.json";     // Filväg till json-fil där topplistan lagras
        private List<Quiz> quizItems = new();             // Lista för frågor/svar 


        // Konstruktor
        public QuizManager()
        {
            if (File.Exists(filePath))              // Kontroll om json-fil finns
            {
                try
                {
                    string jsonString = File.ReadAllText(filePath);                       // Läs in text från filen
                    quizItems = JsonSerializer.Deserialize<List<Quiz>>(jsonString)!;      // Deserialiserar json till listan av quizItems
                }
                catch (Exception ex)
                {
                    WriteLine($"Fel vid inläsning av json-fil, {ex.Message}");
                }
            }
        }

        // Metod för att skapa ett nytt objekt av klasen Dog
        public Quiz AddToQuiz(string question, string breed)
        {
            Quiz obj = new Quiz(question, breed);

            quizItems.Add(obj);          // Lägg till i listan quizItems
            SaveToJsonFile();       // Spara till json-filen
            return obj;
        }

        // Metod för att rader en fråga
        public int DeleteQuiz(int index)
        {
            quizItems.RemoveAt(index);
            SaveToJsonFile();        // Spara på nytt till json-filen
            return index;
        }

        // Metod för att hämta listan
        public List<Quiz> GetQuiz()
        {
            return quizItems;
        }

        // Metod för att spara frågor/svar med serialize
        public void SaveToJsonFile()
        {
            try
            {
                var jsonString = JsonSerializer.Serialize(quizItems);
                File.WriteAllText(filePath, jsonString);
            }
            catch (Exception ex)
            {
                WriteLine($"Fel vid sparandet av json-filen: {ex.Message}");
            }
        }

        // Metod för att skriva ut alla frågor 
        public void ShowQuiz()
        {
            if (quizItems.Count == 0)
            {
                WriteLine("Det finns inga frågor i quizet just nu.");
            }
            else
            {
                WriteLine("Quizfrågor:\n");
                int i = 0;
                foreach (Quiz dog in GetQuiz())
                {

                    WriteLine($"[{i++}] {dog.Question} - {dog.Breed}");
                }
            }
        }
    }
}