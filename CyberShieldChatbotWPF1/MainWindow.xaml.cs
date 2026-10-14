using System;
using System.IO;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CyberShieldChatbotWPF1
{
    public partial class MainWindow : Window
    {
        private readonly ChatbotEngine _engine;

        public MainWindow()
        {
            InitializeComponent(); 
            _engine = new ChatbotEngine();

            DisplayAsciiArt();
            PlayVoiceGreeting();

            AppendBotMessage("Hello! I am CyberShield Bot. You can tell me your name, or ask me about passwords, scams, privacy, and phishing!");
        }

        private void DisplayAsciiArt()
        {
            AsciiBox.Text =
@"  ___ _| |_ ___ ___ ___ |_|___| | _| |___ ___ | |_ 
 |  _| . |  _| -_|  _| . | | -_| | . | | . | . ||  _|
 |_| |___|___|___|_| |___|_|___|_|___|_|___|___||_|  ";
        }

        private void PlayVoiceGreeting()
        {
            try
            {
                string soundPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "greeting.wav");
                if (File.Exists(soundPath))
                {
                    using (var player = new SoundPlayer(soundPath))
                    {
                        player.Play();
                    }
                }
            }
            catch
            {
                
            }
        }

        private void HandleSendMessage()
        {
            string userText = UserInputBox.Text;
            if (string.IsNullOrWhiteSpace(userText)) return;

            AppendUserMessage(userText);
            UserInputBox.Clear();

            string botResponse = _engine.ProcessInput(userText);
            AppendBotMessage(botResponse);
        }

        private void AppendUserMessage(string message)
        {
            ChatDisplay?.AppendText($"You: {message}\n\n");
            ChatScrollViewer?.ScrollToEnd();
        }

        private void AppendBotMessage(string message)
        {
            ChatDisplay?.AppendText($"CyberShield Bot: {message}\n");
            ChatDisplay?.AppendText("--------------------------------------------------\n\n");
            ChatScrollViewer?.ScrollToEnd();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            HandleSendMessage();
        }

        private void UserInputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                HandleSendMessage();
                e.Handled = true;
            }
        }
    }
}