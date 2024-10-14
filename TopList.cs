// Klass för topplistan med namn och poäng

namespace quiz
{
    public class TopList
    {
        public string PlayerName { get; set; }
        public int Points { get; set; }

        // Konstruktor
        public TopList(string playerName, int points)
        {
            PlayerName = playerName;
            Points = points;
        }
    }
}