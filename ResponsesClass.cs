using System.Collections;

namespace poe_part3
{
    //code for the array responses
    public class ResponsesClass
    {
        //array list for responses
        private ArrayList answers = new ArrayList();
        private ArrayList ignore = new ArrayList();

        //create a method to store answers
        public void AnswerStorage()
        {
            // avoid duplicates if called multiple times
            if (answers.Count > 0) return;

            // phishing
            answers.Add("Phishing is when a user sends you a fake email so that when you open it, they can get your private information.");
            answers.Add("Phishing scams often create a sense of urgency—don’t fall for the pressure.");
            answers.Add("Avoid clicking on links in unsolicited emails. Instead, visit the official website directly.");

            // password 
            answers.Add("Try to avoid using the same password across multiple sites to ensure password safety.");
            answers.Add("Password safety is when you use a password that is comprised of capital letters, small letters and symbols.");
            answers.Add("Consider using a password manager to generate and store your passwords securely.");

            // safe browsing
            answers.Add("Safe browsing is when you pay attention to the sites you visit on the internet and avoid unauthorized internet sites.");
            answers.Add("Safe browsing is when you avoid clicking pop-ups or downloading files from unknown websites.");
            answers.Add("Use a browser that offers privacy features and ad-blocking.");
            answers.Add("Always look for HTTPS in the URL when entering sensitive information.");


            // Storage for ignored words (common filler and stop words)
            ignore.Add("what");
            ignore.Add("is");
            ignore.Add("tell");
            ignore.Add("me");
            ignore.Add("about");
            ignore.Add("and");
            ignore.Add("the");
            ignore.Add("you");
            ignore.Add("there");
            ignore.Add("do");
            ignore.Add("does");
            ignore.Add("are");
            ignore.Add("how");
            ignore.Add("it");
            ignore.Add("in");
            ignore.Add("to");
            ignore.Add("a");
            ignore.Add("on");
            ignore.Add("that");
            ignore.Add("this");
            ignore.Add("can");
            ignore.Add("of");
            ignore.Add("with");
            ignore.Add("your");
            ignore.Add("please");
            ignore.Add("i");
            ignore.Add("we");
            ignore.Add("they");
            ignore.Add("them");
            ignore.Add("us");
            ignore.Add("for");
            ignore.Add("be");
            ignore.Add("my");
            ignore.Add("just");
            ignore.Add("any");
            ignore.Add("as");
            ignore.Add("at");
            ignore.Add("why");
            ignore.Add("when");
            ignore.Add("who");
            ignore.Add("where");

        }

        //create a return response method
        public string ResponseReturn(string input)
        {
            if (answers.Count == 0) AnswerStorage();

            input = input.ToLower();
            string[] splitInput = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            ArrayList keywords = new ArrayList();
            foreach (string word in splitInput)
            {
                if (!ignore.Contains(word))
                {
                    // avoid duplicates
                    if (!keywords.Contains(word))
                        keywords.Add(word);
                }
            }

            if (keywords.Count == 0)
                return "Sorry, I couldn't find any useful keywords in your message.";

            Dictionary<string, List<string>> keywordAnswers = new Dictionary<string, List<string>>();

            foreach (string keyword in keywords)
            {
                List<string> matches = new List<string>();

                foreach (string answer in answers)
                {
                    string[] answerWords = answer.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);

                    foreach (string word in answerWords)
                    {
                        if (word.Equals(keyword, StringComparison.OrdinalIgnoreCase))
                        {
                            matches.Add(answer);
                            break;
                        }
                    }
                }

                if (matches.Count > 0)
                {
                    keywordAnswers[keyword] = matches;
                }
            }

            if (keywordAnswers.Count == 0)
                return "Sorry, I don't have information on that topic yet.";

            // Build final response — one random answer per keyword
            string finalResponse = "";
            Random rand = new Random();

            foreach (var pair in keywordAnswers)
            {
                string topic = pair.Key;
                List<string> topicAnswers = pair.Value;

                string selectedAnswer = topicAnswers[rand.Next(topicAnswers.Count)];

                finalResponse += $"- {selectedAnswer}\n";
            }

            return finalResponse.TrimEnd();
        }
    }
}