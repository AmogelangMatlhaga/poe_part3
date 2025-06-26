namespace poe_part3
{
    public class TrainModelsclass
    {

        private readonly MLContext mlContext;
        private List<SentimentData> trainingData;
        private PredictionEngine<SentimentData, SentimentPrediction> predEngine;

        public TrainModelsclass()
        {
            mlContext = new MLContext();
            trainingData = new List<SentimentData>();
        }

        public void testTrain()
        {

            trainingData = new List<SentimentData>
            {
                new SentimentData { Text = "I am happy", Label = true },
                new SentimentData { Text = "I hate this", Label = false },
                new SentimentData { Text = "I am sad", Label = false },
                new SentimentData { Text = "I am good", Label = true }
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

        public void check_sentiment(string input, System.Windows.Controls.ListView chat_list, System.Windows.Controls.ListView history_list)
        {
            var prediction = predEngine.Predict(new SentimentData { Text = input });

            float positiveScore = prediction.Probability * 100;
            float negativeScore = 100 - positiveScore;

            string emotionType = prediction.Prediction ? "Positive" : "Negative";

            string feedback = $"{emotionType} Emotion\n" +
                              $"Positive: {positiveScore:F1}%\n" +
                              $"Negative: {negativeScore:F1}%\n";

            string reply;
            if (positiveScore > 75)
            {
                reply = "You seem really upbeat! Keep shining ";
            }
            else if (positiveScore > 50)
            {
                reply = "You’re doing alright — keep your chin up!";
            }
            else if (positiveScore > 30)
            {
                reply = "I sense some heaviness — it’s okay to feel down sometimes.";
            }
            else
            {
                reply = "You seem quite low. Be kind to yourself — brighter days will come.";
            }

            chat_list.Items.Add("Chatbot AI : " + feedback + reply);
            history_list.Items.Add("Chatbot AI : " + feedback + reply);

            // Force learning
            trainingData.Add(new SentimentData { Text = input, Label = prediction.Prediction });
            TrainModel();

            chat_list.Items.Add("Chatbot AI : I've learned from this interaction.");
            history_list.Items.Add("Chatbot AI : I've learned from this interaction.");
        }
    }
}