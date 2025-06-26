namespace poe_part3
{
    public class QuizQuestionLoad
    {




        //ALL questions
        public void autoLoadQuiz(ref List<Question>? questions)
        {
            questions = new List<Question>() {

    new Question
    {
        Text = "what is the purpose of phishing?",
        correctAnswer = "To steal your personal details.",
        wrongAnswer = new List<string>{ "To protect your details", "To promote pop-ups", "To make password strong"}

    },
        new Question
    {
        Text = "what is password safety",
        correctAnswer = "Using a unique password",
        wrongAnswer = new List<string>{ "Sharing password", "Using password with less than 8 letters", "Using common password"}

    },
        new Question
    {
        Text = "what is safe browsing",
        correctAnswer = "using authorized sites",
        wrongAnswer = new List<string>{ "Illegal sites", "opening pop-ups", "opening every link"}

    },
        new Question
        {
            Text = "A sign of phishing email?",
            correctAnswer = "Suspicious links or urgent requests",
            wrongAnswer  = new List<string>{ "Personalized greeting from your friend", "Proper grammar and company logo", "An unsubscribe option at the bottom" }


        },
        new Question
        {
            Text = "Example of a strong password?",
            correctAnswer = "P@55w0rD!#987",
            wrongAnswer  = new List<string>{ "Password123", "qwerty2024", "123456789" }


        },
        new Question
        {
            Text = "How often to update passwords?",
            correctAnswer = "Every 3 to 6 months",
            wrongAnswer  = new List<string>{ "Once a year", "Never", "Only if hacked" }


        },
        new Question
        {
            Text = "Why is reusing passwords risky?",
            correctAnswer = "If one account is compromised, others are too",
            wrongAnswer  = new List<string>{ "It takes longer to type", "It confuses websites", "Websites don’t allow it" }


        },
        new Question
        {
            Text = " What shows a website is unsafe?",
            correctAnswer = "It contains spelling errors and pop-up ads",
            wrongAnswer  = new List<string>{ "It uses HTTPS", "It loads quickly", "It lacks a privacy policy" }


        },
        new Question
        {
            Text = "What’s safest on public Wi-Fi?",
            correctAnswer = "Use of VPN or avoid sensitive activity",
            wrongAnswer  = new List<string>{ "Share files with strangers", "Access your bank account", "Shop online" }


        },
        new Question
        {
            Text = "What to do if site is flagged?",
            correctAnswer = "Leave the site immediately",
            wrongAnswer  = new List<string>{ "Proceed anyway", "Refresh the page", "Ignore the warning" }


        },

    };
        }





    }
}