# poe_part3

## Overview

**poe_part3** is a desktop application built using WPF (Windows Presentation Foundation) and C#. It serves as a personal AI-powered chatbot with additional features such as quizzes, reminders, and chat history. The project demonstrates integration with ML.NET for AI functionality and provides a modern, user-friendly interface.

This application was developed as a final project for a third-semester programming course.

---

## Features

### 1. Personalized Chatbot
- Greets users by name after collecting their username.
- Provides an interactive chat experience.
- Stores and displays chat history for each session.

### 2. Quiz Module
- Loads and presents multiple quiz questions to the user.
- Tracks user answers and scores.
- Provides feedback after quiz completion.

### 3. Reminders
- Allows users to set reminders.
- Displays all saved reminders.

### 4. AI & Sentiment Analysis
- Integrates ML.NET to analyze chat sentiment.
- Includes model training and prediction during runtime.

### 5. Modern WPF Interface
- Uses XAML to build a clean, intuitive GUI.
- Custom navigation between pages: Chat, Quiz, Reminders, Saved Conversations, and History.

---

## Getting Started

### Prerequisites

- Windows OS
- [.NET (Core/Framework) SDK compatible with WPF](https://dotnet.microsoft.com/download)
- Visual Studio or any C# IDE supporting WPF

### Installation & Running

1. **Clone the Repository:**
   ```sh
   git clone https://github.com/AmogelangMatlhaga/poe_part3.git
   cd poe_part3
   ```
2. **Open in Visual Studio:**
   - Open `poe_part3.sln` or the folder in Visual Studio.
   - Restore NuGet packages if prompted (especially `Microsoft.ML`).

3. **Build & Run:**
   - Build the solution.
   - Start debugging or run without debugging.

---

## Application Structure

### Main Components

- **MainWindow.xaml / MainWindow.xaml.cs**  
  The core UI and logic. Handles user authentication (username), navigation between main pages, and all feature logic (chat, quiz, reminders, history).

- **App.xaml / App.xaml.cs**  
  Entry point for the WPF application, manages resources and startup.

- **Images Folder**  
  Contains assets such as the application logo.

- **Data and Model Classes (not fully listed here):**  
  - `collectUsername`: Handles storing and retrieving the user’s name.
  - `ResponsesClass`: Stores possible responses for the chatbot.
  - `ReminderClass`: Manages reminders.
  - `TrainModelsclass`, `SentimentData`, `SentimentPrediction`: Used for AI model training and sentiment analysis.
  - `QuizQuestionLoad`, `Question`: Handles quiz loading and question logic.

---

## UI/UX Flow

1. **Username Page**  
   - Prompts the user for a username.
   - Username is stored and used to personalize all subsequent interactions.

2. **Main Page**
   - Divided into subpages accessible via the menu:
     - **Chats**: Main chatbot interface.
     - **Quiz**: Start and answer quiz questions.
     - **Reminders**: Create and view reminders.
     - **Saved Conversations / History**: Review past chats and quiz attempts.

3. **Navigation**
   - Menu buttons at the bottom of the main page allow switching between features.

---

## Core Logic Example

### Username Submission

Collects, validates, and stores the username. If a username is not entered, it shows an error message.

```csharp
private void usernameSubmit(object sender, RoutedEventArgs e)
{
    string collected_name = username.Text.ToString();
    if (collected_name != "")
    {
        collected_username.AssignUsername(collected_name);
        username_page.Visibility = Visibility.Hidden;
        main_page.Visibility = Visibility.Visible;
        username.Clear();
        chat_list.Items.Add("\nHey " + collected_username.UsernameCollection() + ", Welcome to Chatbot AI.");
    }
    else
    {
        MessageBox.Show("please enter the username");
    }
}
```

### Chat History

Stores each chat message in an array list for later retrieval and display.

### Quiz and Scoring

- Loads questions using `QuizQuestionLoad`.
- Tracks `currentQuestionIndex`, user answers, and score.
- Provides feedback at the end of the quiz.

### AI Integration

- Uses `MLContext`, `SentimentData`, and `SentimentPrediction` for ML.NET model training and prediction.
- Sentiment analysis or similar AI features can be triggered based on user input.

---

## Extending the Project

- **Add more AI/ML features:** Integrate additional ML.NET models for smarter chatbot responses.
- **Improve Quiz Module:** Add more question types, difficulty levels, or a database.
- **Persist Data:** Save chat history, reminders, and scores to disk or a database.
- **Enhance UI:** Introduce custom themes, animations, or responsive layouts.

---

## License

_No official license provided yet. Please add one if you wish to share or modify this project._

---

## Author

- GitHub: [AmogelangMatlhaga](https://github.com/AmogelangMatlhaga)

---

> **Note:** This README is based on available code and structure. For further details or updates, visit the [GitHub repository](https://github.com/AmogelangMatlhaga/poe_part3).
