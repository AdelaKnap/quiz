// Klass för Dog med frågan och svaret

namespace quiz
{
    public class Dog
    {
        public string? Question { get; set; }
        public string? Breed { get; set; }

        // Konstruktor
        public Dog(string question, string breed)
        {
            Question = question;
            Breed = breed;
        }
    }
}

