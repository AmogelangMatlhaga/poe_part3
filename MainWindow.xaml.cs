using Microsoft.ML.Data;
using System.Collections;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.ML;


namespace poe_part3;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{


    //declaring a global instance for the class collect_username
    private collectUsername collected_username = new collectUsername();
    private ResponsesClass array_responses = new ResponsesClass();
    private ReminderClass reminder = new ReminderClass();
    private readonly MLContext mlContext;
    private List<SentimentData> trainingData;
    private PredictionEngine<SentimentData, SentimentPrediction> predEngine;

    //array list to store the history chat
    ArrayList chat_history = new ArrayList();


    //declaring global variables for the currecnt indexscore selected answer
    int currentQuestionIndex = 0;
    int score = 0;
    string selectedAnswer = null;

    List<Button> answerButtons;
    List<Question> questions;
    TrainModelsclass AITrain = new TrainModelsclass();
    QuizQuestionLoad loadQuiz = new QuizQuestionLoad();


    //end of code


    public MainWindow()
    {
        InitializeComponent();

        //calling on the greeting method
        Greetings();

        //call storerage method
        array_responses.AnswerStorage();

        //calling the add all buttons method
        addAllButtons();

        //calling the load questions method
        loadQuiz.autoLoadQuiz(ref questions);

        loadQuestions();


        mlContext = new MLContext();

        testTrain();
        TrainModel();


    }













    //code for username collection ---------------------------------------------------
    private void usernameSubmit(object sender, RoutedEventArgs e)
    {
        //collecting the user's input
        string collected_name = username.Text.ToString();

        //validating the user's input
        if (collected_name != "")
        {
            //passing the username to be stored
            collected_username.AssignUsername(collected_name);


            //setting the username page to be hidden then calling the main page to be visible
            username_page.Visibility = Visibility.Hidden;
            main_page.Visibility = Visibility.Visible;




            //clearing the username textbox
            username.Clear();




            //greeting the user using their name
            chat_list.Items.Add("\nHey " + collected_username.UsernameCollection() + ", Welcome to Chatbot AI.");


        }
        else
        {
            //displaying error message
            MessageBox.Show("please enter the username");

        }
    }// end of username collection code 










    //code for the exit button--------------------------------------------------------------------
    private void exitButton(object sender, RoutedEventArgs e)
    {
        //exiting the environment
        System.Environment.Exit(0);


    }//end of exit button code






    //beginning of menu code
    private void chatsButton(object sender, RoutedEventArgs e)
    {
        //calling all the pages to hide them
        history_page.Visibility = Visibility.Hidden;
        reminders_page.Visibility = Visibility.Hidden;
        savedConversations_page.Visibility = Visibility.Hidden;
        quizLogo_page.Visibility = Visibility.Hidden;

        //calling on the chats page to open it
        chats_page.Visibility = Visibility.Visible;


    }

    private void historyButton(object sender, RoutedEventArgs e)
    {
        //calling all the pages to hide them
        chats_page.Visibility = Visibility.Hidden;
        reminders_page.Visibility = Visibility.Hidden;
        savedConversations_page.Visibility = Visibility.Hidden;
        quizLogo_page.Visibility = Visibility.Hidden;

        //calling on the history page to open it
        history_page.Visibility = Visibility.Visible;


    }

    private void favoritesButton(object sender, RoutedEventArgs e)
    {
        //calling all the pages to hide them
        chats_page.Visibility = Visibility.Hidden;
        history_page.Visibility = Visibility.Hidden;
        quizLogo_page.Visibility = Visibility.Hidden;

        //calling on the favorites page to open it
        reminders_page.Visibility = Visibility.Visible;
        savedConversations_page.Visibility = Visibility.Visible;

        //auto load reinders


    }

    private void quizButton(object sender, RoutedEventArgs e)
    {
        //calling all the pages to hide them
        chats_page.Visibility = Visibility.Hidden;
        history_page.Visibility = Visibility.Hidden;
        reminders_page.Visibility = Visibility.Hidden;
        savedConversations_page.Visibility = Visibility.Hidden;

        //calling on the quiz page to open it
        quizLogo_page.Visibility = Visibility.Visible;
        quiz_logo.Visibility = Visibility.Visible;
        quizGame_page.Visibility = Visibility.Hidden;


    }
    //the end of the menu code














    //--------------- NLP training code section---------------


    public void testTrain()
    {
        trainingData = new List<SentimentData>
    {
        // Emotion-based samples
        new SentimentData { Text = "I am happy", Label = true },
        new SentimentData { Text = "I hate this", Label = false },
        new SentimentData { Text = "I am sad", Label = false },
        new SentimentData { Text = "I am good", Label = true },

        // Greetings
        new SentimentData { Text = "hi", Label = true },
        new SentimentData { Text = "hello", Label = true },
        new SentimentData { Text = "hey", Label = true },
        new SentimentData { Text = "howdy", Label = true },
        new SentimentData { Text = "good morning", Label = true },
        new SentimentData { Text = "good afternoon", Label = true },
        new SentimentData { Text = "good evening", Label = true },

        // Thank-you expressions
        new SentimentData { Text = "thank you", Label = true },
        new SentimentData { Text = "thanks a lot", Label = true },
        new SentimentData { Text = "thank you very much", Label = true },
        new SentimentData { Text = "I appreciate it", Label = true },

        // Farewells 
        new SentimentData { Text = "bye", Label = true },
        new SentimentData { Text = "goodbye", Label = true },
        new SentimentData { Text = "see you later", Label = true },
        new SentimentData { Text = "talk to you soon", Label = true }
    };
    }


    public void TrainModel()
    {
        var trainDataView = mlContext.Data.LoadFromEnumerable(trainingData);

        var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(SentimentData.Text))
            .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(labelColumnName: "Label", featureColumnName: "Features"));

        var model = pipeline.Fit(trainDataView);

        predEngine = mlContext.Model.CreatePredictionEngine<SentimentData, SentimentPrediction>(model);
    }

    // learning binary 
    private int LevenshteinDistance(string s, string t)
    {
        int n = s.Length;
        int m = t.Length;
        int[,] d = new int[n + 1, m + 1];

        if (n == 0) return m;
        if (m == 0) return n;

        for (int i = 0; i <= n; i++) d[i, 0] = i;
        for (int j = 0; j <= m; j++) d[0, j] = j;

        for (int i = 1; i <= n; i++)
        {
            for (int j = 1; j <= m; j++)
            {
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1,
                                           d[i, j - 1] + 1),
                                           d[i - 1, j - 1] + cost);
            }
        }
        return d[n, m];
    }

    // Normalize similarity to [0..1]
    private double Similarity(string s, string t)
    {
        int dist = LevenshteinDistance(s.ToLower(), t.ToLower());
        int maxLen = Math.Max(s.Length, t.Length);
        if (maxLen == 0) return 1.0;
        return 1.0 - ((double)dist / maxLen);
    }

    public string check_sentiment(string input)
    {
        const double similarityThreshold = 0.8;

        if (IsNonsense(input))
        {
            return "";

        }

        // Try to find the closest match in training data
        SentimentData? bestMatch = null;
        double bestScore = 0;

        foreach (var td in trainingData)
        {
            double sim = Similarity(td.Text ?? "", input);
            if (sim > bestScore)
            {
                bestScore = sim;
                bestMatch = td;
            }
        }

        // If no good match found, ignore
        if (bestMatch == null || bestScore < similarityThreshold)
        {
            return "";
        }

        string matchedText = bestMatch.Text.ToLower();

        // Custom reply based on match category
        if (IsGreeting(matchedText))
        {
            return GetGreetingResponse();
        }

        if (IsThankYou(matchedText))
        {
            return GetThankYouResponse();
        }

        if (IsFarewell(matchedText))
        {
            return GetFarewellResponse();
        }

        // Otherwise, it's emotional
        if (bestMatch.Label == true)
        {
            return GetPositiveEmotionResponse();
        }
        else
        {
            return GetNegativeEmotionResponse();
        }
    }

    private bool IsGreeting(string text)
    {
        string[] greetings = { "hi", "hello", "hey", "howdy", "good morning", "good afternoon", "good evening" };
        return greetings.Any(g => text.StartsWith(g));
    }

    private bool IsThankYou(string text)
    {
        string[] thanks = { "thank you", "thanks", "thanks a lot", "thank you very much", "i appreciate it" };
        return thanks.Any(t => text.Contains(t));
    }

    private bool IsFarewell(string text)
    {
        string[] byes = { "bye", "goodbye", "see you later", "talk to you soon" };
        return byes.Any(b => text.Contains(b));
    }

    private string GetGreetingResponse()
    {
        string[] replies = {
        "Hello! How can I assist you today?",
        "Hi there! Need any help?",
        "Hey! I’m here if you need me.",
        "Good to see you! What would you like to do?"
    };
        return replies[new Random().Next(replies.Length)];
    }

    private string GetThankYouResponse()
    {
        string[] replies = {
        "You're very welcome!",
        "Glad I could help!",
        "Anytime!",
        "Always happy to assist!"
    };
        return replies[new Random().Next(replies.Length)];
    }

    private string GetFarewellResponse()
    {
        string[] replies = {
        "Goodbye! Take care.",
        "See you next time!",
        "Bye! Stay productive.",
        "Catch you later!"
    };
        return replies[new Random().Next(replies.Length)];
    }

    private string GetPositiveEmotionResponse()
    {
        string[] replies = {
        "That's great to hear!",
        "I’m glad you’re feeling good.",
        "Keep up the positivity!",
        "That's the spirit!"
    };
        return replies[new Random().Next(replies.Length)];
    }

    private string GetNegativeEmotionResponse()
    {
        string[] replies = {
        "I'm sorry you're feeling that way.",
        "It’s okay to have tough days — hang in there.",
        "Sending you strength. You got this.",
        "Hope things get better soon."
    };
        return replies[new Random().Next(replies.Length)];
    }

    private bool IsNonsense(string input)
    {
        string trimmed = input.Trim().ToLower();
        if (!Regex.IsMatch(trimmed, "[aeiou]")) return true;
        if (Regex.IsMatch(trimmed, @"(.)\1{3,}")) return true;
        string[] spam = { "asdf", "qwer", "zxcv", "lmao", "lololol", "huh", "hmmm" };
        return spam.Any(p => trimmed.Contains(p));
    }
    //end of sentiment and the NLP






    //start of the user questions code
    private void questionAskButton(object sender, RoutedEventArgs e)
    {
        //collecting the users input
        string collected_questions = question.Text.ToString();
        question.Text = "";
        DateTime date_time = DateTime.Now;





        //check which tasks are due today
        dueAllToday_task();





        //validating the questions on the listview
        if (collected_questions != "")
        {

            //then check for emotions and if the user is greeting
            string found_greet = check_sentiment(collected_questions);
            if (found_greet != "")
            {
                chat_list.Items.Add(collected_username.UsernameCollection() + ": " + collected_questions);
                chat_list.Items.Add("Chatbot AI: " + check_sentiment(collected_questions));
                history_list.Items.Add(collected_username.UsernameCollection() + ": " + collected_questions);
                history_list.Items.Add("Chatbot AI: " + check_sentiment(collected_questions));

                chat_list.ScrollIntoView(chat_list.Items[chat_list.Items.Count - 1]);
                history_list.ScrollIntoView(history_list.Items[history_list.Items.Count - 1]);
                return;
            }

            //check for add task
            if (collected_questions.ToLower().Contains("add task"))
            {
                //alerting the user of the reminder being added
                chat_list.Items.Add("Chatbot AI: " + reminder.UserTask(collected_questions) + ", " + "\n" + date_time);
                history_list.Items.Add("Chatbot AI: " + reminder.UserTask(collected_questions) + ", " + "\n" + "\n" + date_time);

                return;
            }




            //check for reminder
            if (collected_questions.Contains("remind"))
            {
                //check if the task was added
                if (reminder.TaskCheck("check") == "")
                {

                    chat_list.Items.Add("Chatbot AI : No task was added, please try  to add a task." + "\n" + date_time);
                    history_list.Items.Add("Chatbot AI : No task was added, please try  to add a task." + "\n" + date_time);

                    //clearing the textbox
                    question.Clear();
                    //then check how the user is feeling 
                    return;
                }

                //check if the user says today or zero day
                if (collected_questions.Contains("today") || collected_questions.Contains("0"))
                {
                    //return a message once the reminder is set
                    chat_list.Items.Add("Chatbot AI :" + reminder.TodayReminder());
                    history_list.Items.Add("Chatbot AI :" + reminder.TodayReminder());
                    //then check how the user is feeling 
                    //clearing the textbox
                    question.Clear();
                }
                else
                {
                    //collect the number of days
                    string collected_days = reminder.GetNumberOfDays(collected_questions);



                    chat_list.Items.Add("Chatbot AI : " + reminder.ReminderInDays(collected_days));
                    history_list.Items.Add("Chatbot AI : " + reminder.ReminderInDays(collected_days));


                    //clearing the textbox
                    question.Clear();
                    // MessageBox.Show("remind days are " + collected_days);


                }
                chat_list.ScrollIntoView(chat_list.Items[chat_list.Items.Count - 1]);
                history_list.ScrollIntoView(history_list.Items[history_list.Items.Count - 1]);
                //then check how the user is feeling 
                return;
            }




            //adding questions to the listview
            chat_list.Items.Add(collected_username.UsernameCollection() + " : " + collected_questions);
            history_list.Items.Add(collected_username.UsernameCollection() + " : " + collected_questions);


            //collecting user's answer 
            string foundAnswer = array_responses.ResponseReturn(collected_questions);

            //check if foundAnswer is not empty
            if (foundAnswer != "")
            {
                //if the answer is found
                chat_list.Items.Add("Chatbot AI : " + foundAnswer + " " + "" + "\n" + date_time);
                history_list.Items.Add("Chatbot AI : " + foundAnswer + " " + " " + "\n" + date_time);

                //auto scrollview for the listview
                chat_list.ScrollIntoView(chat_list.Items[chat_list.Items.Count - 1]);
                history_list.ScrollIntoView(history_list.Items[history_list.Items.Count - 1]);
            }
            else
            {
                //if the answer is not found
                chat_list.Items.Add("Chatbot AI: I did not quite understand that, could you please rephrase and ask something related to cybersecurity." + " " + "\n" + date_time);
                history_list.Items.Add("Chatbot AI : I did not quite understand that, could you please rephrase and ask something related to cybersecurity. " + " " + "\n" + date_time);

                //auto scrollview for the listview
                chat_list.ScrollIntoView(chat_list.Items[chat_list.Items.Count - 1]);
                history_list.ScrollIntoView(history_list.Items[history_list.Items.Count - 1]);

                //clearing the textbox
                question.Clear();
            }

        }
        else
        {
            //checking to see if the question textbox is empty
            MessageBox.Show("please ask something related to cybersecurity" + " " + "\n");
        }






        //clearing the textbox
        question.Clear();



    }//end of code






    //code for the history reload button
    private void reloadButton(object sender, RoutedEventArgs e)
    {
        //reloading the history conversation
        foreach (string conversation in chat_history)
        {
            //adding the reloaded conversation back into the list view
            history_list.Items.Add(conversation);
        }

        //successful message for when the conversation is reloaded
        MessageBox.Show("conversation successfully reloaded");

    }//end of code


    //code for the history save button
    private void saveButton(object sender, RoutedEventArgs e)
    {
        //adding each and every conversation in the historylist array
        foreach (string conversation in history_list.Items)
        {
            //storing the conversation in the array list
            chat_history.Add(conversation);
            favSaved_list.Items.Add(conversation);

            //auto scrollview
            history_list.ScrollIntoView(history_list.Items[history_list.Items.Count - 1]);
            favSaved_list.ScrollIntoView(favSaved_list.Items[favSaved_list.Items.Count - 1]);
        }

        //successful message for when the conversation is successfully store
        MessageBox.Show("conversation saved");


    }//end of code

    //button to clear the history listview
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        //clearing the history listview
        history_list.Items.Clear();


    }//end of code


    //code for the reminders page
    private void openReminder(object sender, RoutedEventArgs e)
    {
        //calling on the reminders page to hide it
        reminders_page.Visibility = Visibility.Hidden;
        chats_page.Visibility = Visibility.Hidden;
        history_page.Visibility = Visibility.Hidden;

        //calling on the conversations page to open it
        savedConversations_page.Visibility = Visibility.Visible;


    }//end of code

    //code for the saved history page

    private void openConversation(object sender, RoutedEventArgs e)
    {
        //calling on the conversations page to hide it
        savedConversations_page.Visibility = Visibility.Hidden;
        chats_page.Visibility = Visibility.Hidden;
        history_page.Visibility = Visibility.Hidden;

        //calling on the reminders page to open it
        reminders_page.Visibility = Visibility.Visible;
        load_reminders();
    }//end of code

    private void dueAllToday_task()
    {
        List<string> todaysTasks = reminder.getTasksDueToday();

        if (todaysTasks.Count != 0)
        {

            string dueTasks = "";
            foreach (string task in todaysTasks)
            {
                dueTasks += "\n" + task;
            }

            MessageBox.Show("Tasks due today\n" + dueTasks);
        }
    }
    //load all reminders
    private void load_reminders()
    {


        //check which tasks are due today
        dueAllToday_task();

        List<string> showRemind = reminder.allReminders();

        if (showRemind.Count == 0)
        {
            string msg = "No task added, add tasks first";
            MessageBox.Show(msg);
            history_list.Items.Add(msg);
            return;
        }

        int newItemsCount = 0;

        foreach (string reminder in showRemind)
        {
            //stripping [done] for comparison
            string normalizedNew = reminder.Replace(" [done]", "").Trim();

            bool alreadyExists = false;
            foreach (var item in favReminder_list.Items)
            {
                string normalizedExisting = item.ToString().Replace(" [done]", "").Trim();
                if (normalizedExisting == normalizedNew)
                {
                    alreadyExists = true;
                    break;
                }
            }

            if (!alreadyExists)
            {
                favReminder_list.Items.Add(reminder);
                newItemsCount++;
            }
        }

        if (newItemsCount == 0)
        {
            string msg = "No new task added...";
            MessageBox.Show(msg);
            history_list.Items.Add(msg);
        }
    }



    private void favReminder_list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        //check if a task is selected
        if (favReminder_list.SelectedItem != null)
        {
            //temporary variable to hold the selected
            string hold_selected = favReminder_list.SelectedItem.ToString();


            int get_index = favReminder_list.Items.IndexOf(hold_selected);

            if (!hold_selected.Contains(" [done]"))
            {
                //updating the selected
                favReminder_list.Items[get_index] = hold_selected + " [done]";
                reminder.mark_done(get_index);
                history_list.Items.Add(hold_selected + " was marked done.");

            }
            else
            {
                favReminder_list.Items.Remove(hold_selected);
                reminder.delete_marked(get_index);
                history_list.Items.Add(hold_selected + " was deleted.");


            }


            //  MessageBox.Show(hold_selected);


        }



    }



















    //beginning of the quiz game code

    private void startQuizButton(object sender, RoutedEventArgs e)
    {
        //calling on all pages to open them
        quizGame_page.Visibility = Visibility.Visible;

        //calling on the quiz page to hide it
        quiz_logo.Visibility = Visibility.Hidden;
    }

    //method to add all the buttons
    private void addAllButtons()
    {

        answerButtons = new List<Button> {
        optionButtonOne,
        optionButtonTwo,
        optionButtonThree,
        optionButtonFour

        };

    }//end of code

    //reset buttons background 
    private void buttonReset()
    {
        //reseting the background color of the button when clicked
        foreach (Button clickButton in answerButtons)
        {
            //resetting the background color of the button
            clickButton.ClearValue(Button.BackgroundProperty);
            clickButton.Background = System.Windows.Media.Brushes.SkyBlue;


        }


    }

    //creating a method to load the questions
    private void loadQuestions()
    {
        //checking if the user didnt complete the quiz
        if (currentQuestionIndex >= questions.Count)
        {
            //display message for when the quiz is not completed
            MessageBox.Show("quiz completed with the score " + score + "\n" + "We will reset the game for you , press OK");
            currentQuestionIndex = 0;
            score = 0;
            selectedAnswer = null;
            loadQuestions();
            return;
        }

        //get the question
        var foundQuestion = questions[currentQuestionIndex];
        question_asked.Text = foundQuestion.Text;

        //setting  up the score
        score_count.Text = "score.." + "\n" + score;

        //resetting the selected answer
        selectedAnswer = null;

        //resetting the buttons
        buttonReset();

        //randomizing answers
        List<string> allAnswers = new List<string>(foundQuestion.wrongAnswer);
        allAnswers.Add(foundQuestion.correctAnswer);

        //creating an instance for the randomize class
        Random getIndex = new Random();
        for (int count = 0; count < allAnswers.Count; count++)
        {
            int found_index = getIndex.Next(count, allAnswers.Count);

            (allAnswers[count], allAnswers[found_index]) = (allAnswers[found_index], allAnswers[count]);


        }

        //assigning answers to the buttons
        for (int count = 0; count < answerButtons.Count; count++)
        {
            answerButtons[count].Content = allAnswers[count];

        }



    }//end of code



    //quiz game code

    private void optionSelected(object sender, RoutedEventArgs e)
    {
        //calling the button reset method
        buttonReset();
        Button clickButton = sender as Button;
        clickButton.Background = System.Windows.Media.Brushes.Green;
        selectedAnswer = clickButton.Content.ToString();



    }

    private void answerButton(object sender, RoutedEventArgs e)
    {
        //checking if the user selected an option first
        if (selectedAnswer == string.Empty)
        {
            //display message
            MessageBox.Show("please select an answer!");

            return;
        }
        if (selectedAnswer == questions[currentQuestionIndex].correctAnswer)
        {
            score += 5;

        }
        currentQuestionIndex++;
        loadQuestions();





    }//end of quiz

    //greeting user method
    private void Greetings()
    {

        //creating an instance for the media class
        MediaPlayer Audio_greet = new MediaPlayer();


        string fullPath = AppDomain.CurrentDomain.BaseDirectory;


        string replaced = fullPath.Replace("\\bin\\Debug\\net8.0-windows", "");


        string combine_path = System.IO.Path.Combine(replaced, "VoiceGreeting.wav");

        //combine the url as uri
        Audio_greet.Open(new Uri(combine_path, UriKind.Relative));

        //play sound
        Audio_greet.Play();




    }//end of the greeting user method 
}