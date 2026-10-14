using System;
using System.Collections.Generic;

namespace CyberShieldChatbotWPF1
{
    public delegate string SentimentResponseHandler(string sentiment, string topicTip);

    public class ChatbotEngine
    {
        private Dictionary<string, List<string>> _knowledgeBase = null!;
        private Dictionary<string, List<string>> _sentimentPhrases = null!;
        private Random _random;

        public string UserName { get; private set; } = string.Empty;
        public string LastDiscussedTopic { get; private set; } = string.Empty;

        public SentimentResponseHandler? OnSentimentDetected;

        public ChatbotEngine()
        {
            _random = new Random();
            InitializeKnowledgeBase();
            InitializeSentimentPhrases();

            OnSentimentDetected = FormatSentimentResponse;
        }

        private void InitializeKnowledgeBase()
        {
            _knowledgeBase = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "password", new List<string>
                    {
                        "Always use a unique password with at least 12 characters combining letters, numbers, and symbols.",
                        "Consider using a password manager so you don't have to write down or repeat credentials.",
                        "Avoid using personal details like birthdays or pet names in your passwords."
                    }
                },
                {
                    "scam", new List<string>
                    {
                        "Be cautious of urgent requests for money or personal details—scammers create fake urgency.",
                        "Always double-check link URLs and email sender addresses before taking action.",
                        "If an investment or prize offer sounds too good to be true, it is almost certainly a scam."
                    }
                },
                {
                    "privacy", new List<string>
                    {
                        "Review app permissions on your phone to restrict access to your location and contacts.",
                        "Adjust social media privacy settings so only trusted friends can view your posts.",
                        "Avoid using public Wi-Fi networks for banking or sensitive transactions without a VPN."
                    }
                },
                {
                    "phishing", new List<string>
                    {
                        "Phishing emails often mimic official logos. Always inspect the actual sender email domain.",
                        "Never download attachments from unknown or suspicious emails.",
                        "If you receive a suspicious message from a known contact, verify with them through a separate channel."
                    }
                }
            };
        }

        private void InitializeSentimentPhrases()
        {
            _sentimentPhrases = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "worried", new List<string>
                    {
                        "It's completely normal to feel worried about online security. Scammers can be very convincing.",
                        "Don't worry! Taking small proactive steps makes a huge difference in keeping you safe."
                    }
                },
                {
                    "frustrated", new List<string>
                    {
                        "I understand how frustrating tech security can feel. Let's take it step by step.",
                        "Cybersecurity can feel overwhelming at times, but I am here to make it simple for you."
                    }
                },
                {
                    "curious", new List<string>
                    {
                        "That's a great mindset! Being curious is the best way to stay ahead of cyber threats.",
                        "I love your curiosity! Learning about security gives you control over your digital safety."
                    }
                }
            };
        }

        private string FormatSentimentResponse(string sentiment, string topicTip)
        {
            if (!_sentimentPhrases.TryGetValue(sentiment, out var empatheticList) || empatheticList.Count == 0)
            {
                empatheticList = new List<string> { "I completely understand how you feel." };
            }

            string empathyStatement = empatheticList[_random.Next(empatheticList.Count)];
            return $"{empathyStatement}\n\nHere is a tip to help: {topicTip}";
        }

        public string ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "Please type a message so I can assist you.";

            string cleanInput = input.Trim().ToLower();

            if (cleanInput.StartsWith("my name is "))
            {
                UserName = input.Substring(11).Trim();
                return $"Got it! Nice to meet you, {UserName}. I'll remember your name during our chat.";
            }

            string detectedSentiment = DetectSentiment(cleanInput);
            if (!string.IsNullOrEmpty(detectedSentiment))
            {
                string fallbackTopic = !string.IsNullOrEmpty(LastDiscussedTopic) ? LastDiscussedTopic : "scam";
                string tip = GetRandomTipForTopic(fallbackTopic);

                return OnSentimentDetected?.Invoke(detectedSentiment, tip) ?? string.Empty;
            }

            if (cleanInput.Contains("tell me more") || cleanInput.Contains("explain more") || cleanInput.Contains("another tip"))
            {
                if (!string.IsNullOrEmpty(LastDiscussedTopic))
                {
                    string personalizedPrefix = !string.IsNullOrEmpty(UserName)
                        ? $"{UserName}, as someone interested in {LastDiscussedTopic}, here is another key tip:\n"
                        : $"Here is another tip regarding {LastDiscussedTopic}:\n";

                    return personalizedPrefix + GetRandomTipForTopic(LastDiscussedTopic);
                }
                return "We haven't picked a specific topic yet! Ask me about passwords, scams, privacy, or phishing.";
            }

            foreach (var key in _knowledgeBase.Keys)
            {
                if (cleanInput.Contains(key))
                {
                    LastDiscussedTopic = key;
                    string personalizedIntro = !string.IsNullOrEmpty(UserName)
                        ? $"Thanks for asking, {UserName}! "
                        : "";

                    return $"{personalizedIntro}{GetRandomTipForTopic(key)}";
                }
            }

            if (cleanInput.Contains("what is my name") || cleanInput.Contains("who am i"))
            {
                return !string.IsNullOrEmpty(UserName)
                    ? $"Your name is {UserName}!"
                    : "You haven't told me your name yet! You can say 'My name is [your name]'.";
            }

            return "I'm not sure I quite understood that. You can ask me about passwords, scams, privacy, or phishing.";
        }

        private string GetRandomTipForTopic(string topic)
        {
            if (_knowledgeBase.TryGetValue(topic, out var tips) && tips.Count > 0)
            {
                return tips[_random.Next(tips.Count)];
            }
            return string.Empty;
        }

        private string DetectSentiment(string input)
        {
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("nervous") || input.Contains("anxious"))
                return "worried";
            if (input.Contains("frustrated") || input.Contains("confused") || input.Contains("annoyed"))
                return "frustrated"; 
            if (input.Contains("curious") || input.Contains("interested") || input.Contains("eager"))
                return "curious";

            return string.Empty;
        }
    }
}