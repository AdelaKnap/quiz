// Klasser och metoder för att hantera quizet

using System.Text;
using System.Text.Json;

namespace quiz
{

    public class QuizManager
    {
        private string filePath = @"quiz.json";     // Filväg till json-fil där topplistan lagras
        private List<Dog> dogs = new();             // Lista för frågor/svar 


        // Konstruktor
        public QuizManager()
        {
            if (File.Exists(filePath))                                          // Kontroll om json-fil finns
            {
                // Läs in text från filen med UTF-8-kodning
                string jsonString = File.ReadAllText(filePath);
                dogs = JsonSerializer.Deserialize<List<Dog>>(jsonString)!;      // Deserialiserar JSON till listan av dogs
            }
        }



        // Metod för att skapa ett nytt objekt av klasen Dog
        public Dog AddDog(string question, string breed)
        {
            Dog obj = new()
            {
                Question = question,
                Breed = breed
            };

            dogs.Add(obj);          // Lägg till i listan dogs
            SaveToJsonFile();       // Spara till json-filen
            return obj;
        }

        // Metod för att rader en fråga
        public int DeleteDog(int index)
        {
            dogs.RemoveAt(index);
            SaveToJsonFile();        // Spara på nytt till json-filen
            return index;
        }

        // Metod för att hämta listan
        public List<Dog> GetDogs()
        {
            return dogs;
        }

        // Metod för att spara frågor/svar med serialize
        public void SaveToJsonFile()
        {
            var jsonString = JsonSerializer.Serialize(dogs);
            File.WriteAllText(filePath, jsonString);
        }

        // Metod för att skriva ut alla frågor 
        public void ShowQuiz()
        {
            if (dogs.Count == 0)
            {
                Console.WriteLine("Det finns inga frågor i quizet just nu.");
            }
            else
            {
                int i = 0;
                foreach (Dog dog in GetDogs())
                {
                    Console.WriteLine($"[{i++}] {dog.Question} - {dog.Breed}");
                }
            }
        }

          // Metod för att skriva ut felmeddelande, för att slippa upprepning av detta in program.cs
        public static void ErrorMessage()
        {
            Console.WriteLine("\nFelaktigt val, tryck på valfri tangent för att testa igen!");
            Console.ReadKey();
        }

    }
}