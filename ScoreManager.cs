using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MAZEGAME
{
    public class ScoreManager
    {
       private static string _fileName= "highscores.xml";
       public  List<Score> _highScores { get; private set; }

        public List<Score> _scores { get;  private set; }

        public  ScoreManager():this(new List<Score>()) // Constructor that initializes the ScoreManager with an empty list of scores
        {
            
        }

        public ScoreManager(List<Score> scores) //constructor that initialises ScoreManager with list of scores
        {
            _scores = scores;
            updateHighScores(); 
        }

        public void updateHighScores()
        {
            _highScores=_scores.Take(10).ToList(); //takes the top 10 scores
        }
       
        public void addScore(Score score)
        {
            _scores.Add(score);
            _scores = _scores.OrderByDescending(s => s.Value).ToList(); // Sort the scores in descending order
            updateHighScores();
        }

        public static ScoreManager loadScores()
        {
            if (!File.Exists(_fileName)) //check if file exists 
            {
                return new ScoreManager(); // If not, return a new ScoreManager with an empty list
            }
            using (var reader = new StreamReader(new FileStream(_fileName, FileMode.Open))) // Open the file for reading
            {
                var serializer=new XmlSerializer(typeof(List<Score>)); // Create a new XmlSerializer for the Score class
                var scores= (List<Score>)serializer.Deserialize(reader);// Deserialize the XML data into a list of Score objects
                return new ScoreManager(scores);// Return a new ScoreManager with the loaded scores
            }
        }

        public static void saveScores(ScoreManager scoreManager)
        {
            ScoreManager managerloading = loadScores(); // Load the scores from the file
            foreach (var score in managerloading._highScores) // Iterate through the loaded scores
            {
                scoreManager.addScore(score); // Add each score to the current ScoreManager
            }
            scoreManager.updateHighScores(); // Update the high scores

           using(var writer = new StreamWriter(new FileStream(_fileName, FileMode.Create))) // Open the file for writing
            {
                var serializer=new XmlSerializer(typeof(List<Score>)); // Create a new XmlSerializer for the Score class
                serializer.Serialize(writer, scoreManager._highScores); // Serialize the high scores to the file
            }
        }
        
    }
  
}