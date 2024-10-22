// Klass för Quiz med frågan och svaret

namespace quiz
{
    public class Quiz
    {
        public string? Question { get; set; }
        public string? Breed { get; set; }

        // Konstruktor
        public Quiz(string question, string breed)
        {
            Question = question;
            Breed = breed;
        }
    }
}

