# CyberShield Awareness Chatbot

A desktop-based WPF (.NET 8.0) cybersecurity awareness chatbot designed to educate users on essential digital safety topics through interactive dialogue, sentiment-driven empathy, and custom audio/visual elements.

---

## Features

- **Knowledge Base & Generic Collections:** Utilizes generic `Dictionary<string, List<string>>` structures for fast, case-insensitive lookups across cybersecurity topics (Passwords, Scams, Privacy, and Phishing).
- **Sentiment Analysis & Custom Delegates:** Dynamically detects user emotions (`worried`, `frustrated`, `curious`) and uses custom delegates (`SentimentResponseHandler`) to route and format empathetic responses.
- **Conversation State & Memory:** Tracks user identity (`UserName`) and conversational context (`LastDiscussedTopic`) to deliver personalized responses and context-aware follow-ups (*"tell me more"*).
- **WPF Graphical User Interface:** Styled using a modern Catppuccin-inspired dark theme (`#1E1E2E`) with a custom ASCII art header, scrollable chat window, and responsive keyboard/button inputs.
- **Audio Integration:** Features an integrated `SoundPlayer` that plays an audio greeting on startup with robust error-handling.

---

## Project Structure

```text
CyberShieldChatbotWPF1/
│
├── ChatbotEngine.cs        # Core business logic, knowledge base, and sentiment engine
├── MainWindow.xaml         # XAML layout and UI control definitions
├── MainWindow.xaml.cs      # Code-behind handling user input, events, and audio playback
├── greeting.wav            # Startup audio greeting file
└── README.md               # Project documentation