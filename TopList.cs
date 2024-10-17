// Klass för topplistan med namn, poäng och max antalet rätt/poäng

namespace quiz
{
    public class TopList
    {
        public string PlayerName { get; set; }
        public int Points { get; set; }

        public int MaxPoints { get; set; }

        public double Time { get; set; }   

        // Konstruktor
        public TopList(string playerName, int points, int maxPoints, double time)
        {
            PlayerName = playerName;
            Points = points;
            MaxPoints = maxPoints;
            Time = time;
        }
    }
}