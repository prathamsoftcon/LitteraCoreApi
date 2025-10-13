using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace LitteraCore.BLContext
{
    public class AIBL
    {
        private readonly IConfiguration _configuration;
        public AIBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }

      


        public bool Save_AI_Conversation(AITool ai)
        {
            AIDB cdb = new AIDB(_configuration);
            bool isSaved = cdb.Save_AI_RESPONSE(ai);
            return isSaved;
        }
        public bool Update_Conversation_Like(AITool ai)
        {
            AIDB cdb = new AIDB(_configuration);
            bool isSaved = cdb.Update_Like_Dislike(ai);
            return isSaved;
        }

        public PagedList<AITool> Get_AI_Tool_Conversations(PaginationParam param)
        {
            AIDB cdb = new AIDB(_configuration);
            PagedList<AITool> leveldata = cdb.Get_AI_Conversation(param);
            return leveldata;
        }
        public async Task<EmbeddingResponse> GetEmbeddingsAsync(string text)
        {
            try
            {
                string AI_KEY = _configuration.GetSection("AI_KEY").Value;
                string endpoint = "https://api.openai.com/v1/embeddings";
                string model = "text-embedding-3-small"; // or text-embedding-ada-002
                int promptTokens = 0;
                int totalTokens = 0;
                var requestData = new
                {
                    input = text,
                    model = model
                };

                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", AI_KEY);

                var response = await client.PostAsJsonAsync(endpoint, requestData);
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var json = JsonDocument.Parse(responseString);
                    var embedding = json.RootElement
                                        .GetProperty("data")[0]
                                        .GetProperty("embedding");

                    // Convert JsonElement to a list of floats
                    var floatList = new List<double>();
                    foreach (var number in embedding.EnumerateArray())
                    {
                        floatList.Add(number.GetSingle());
                    }

                    if (json.RootElement.TryGetProperty("usage", out JsonElement usage))
                    {
                         promptTokens = usage.GetProperty("prompt_tokens").GetInt32();
                         totalTokens = usage.GetProperty("total_tokens").GetInt32();

                       
                        Console.WriteLine($"Prompt Tokens: {promptTokens}, Total Tokens: {totalTokens}");
                    }


                    return new EmbeddingResponse
                    {
                        Embedding = floatList,
                        PromptTokens = promptTokens,
                        TotalTokens = totalTokens
                    };
                }
                else
                {
                    // Option 1: throw an exception
                    throw new Exception($"OpenAI API error: {response.StatusCode}, {responseString}");

                    // Option 2: return null or an empty list
                    // return new List<float>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Its taking too much time to fetch data.Try after sometime");
            }
            
        }

        public static double CosineSimilarity(double[] vectorA, double[] vectorB)
        {
            if (vectorA == null || vectorB == null)
                throw new ArgumentNullException("Vectors must not be null.");
            if (vectorA.Length != vectorB.Length)
                throw new ArgumentException("Vectors must be of same length.");

            double dotProduct = 0.0;
            double magnitudeA = 0.0;
            double magnitudeB = 0.0;

            for (int i = 0; i < vectorA.Length; i++)
            {
                dotProduct += vectorA[i] * vectorB[i];
                magnitudeA += Math.Pow(vectorA[i], 2);
                magnitudeB += Math.Pow(vectorB[i], 2);
            }

            magnitudeA = Math.Sqrt(magnitudeA);
            magnitudeB = Math.Sqrt(magnitudeB);

            if (magnitudeA == 0.0 || magnitudeB == 0.0)
                return 0.0; // Can't divide by zero, implies no similarity

            return dotProduct / (magnitudeA * magnitudeB);
        }


        public static double ComputeCompleteness(string response, List<string> expectedTopics)
        {
            var responseTokens = Tokenize(response);

            int matchedTopics = expectedTopics.Count(topic =>
                responseTokens.Contains(topic.ToLower()));

            return (double)matchedTopics / expectedTopics.Count;
        }
        private static HashSet<string> Tokenize(string text)
        {
            return new HashSet<string>(
                text.ToLower()
                    .Split(new char[] { ' ', '.', ',', '!', '?', ';', ':', '-', '\n', '\r' },
                           StringSplitOptions.RemoveEmptyEntries)
            );
        }

        public static double ComputeAccuracy(string response, List<string> expectedFacts)
        {
            var responseTokens = Tokenize(response);

            int matchedFacts = expectedFacts.Count(fact =>
                responseTokens.Contains(fact.ToLower()));

            return (double)matchedFacts / expectedFacts.Count;
        }
        public static double ComputeClarity(string text)
        {
            int sentenceCount = CountSentences(text);
            int wordCount = CountWords(text);
            int syllableCount = CountSyllables(text);
            if (sentenceCount == 0) { sentenceCount = 1; };
            if (sentenceCount == 0 || wordCount == 0)
                return 0;

            // Flesch-Kincaid Readability Score
            double score = 206.835 - 1.015 * ((double)wordCount / sentenceCount) - 84.6 * ((double)syllableCount / wordCount);

            // Clamp to [0, 100]
            return Math.Max(0, Math.Min(100, score));
        }
        private static int CountSentences(string text)
        {
            return Regex.Matches(text, @"[.!?]").Count;
        }

        private static int CountWords(string text)
        {
            return Regex.Matches(text, @"\b\w+\b").Count;
        }

        private static int CountSyllables(string text)
        {
            int syllables = 0;
            foreach (Match word in Regex.Matches(text.ToLower(), @"\b\w+\b"))
            {
                syllables += EstimateSyllables(word.Value);
            }
            return syllables;
        }

        private static int EstimateSyllables(string word)
        {
            word = word.ToLower();
            if (word.Length <= 3) return 1;

            word = Regex.Replace(word, @"e\b", ""); // remove trailing 'e'
            int count = Regex.Matches(word, @"[aeiouy]+").Count;

            return Math.Max(1, count);
        }

        public static double ComputeDepth(string text,List<string> depthWords,int maxlength)
        {
            var words = Tokenize(text);
            int wordCount = words.Count;
            int uniqueWords = words.Distinct().Count();

            // Conjunctions that imply depth (reasoning, comparison, etc.)
             //depthWords = new List<string> { "because", "however", "although", "therefore", "for example", "such as", "moreover", "furthermore" };
            int depthWordCount = words.Count(w => depthWords.Contains(w));

            // Scoring (heuristic based)
            double lengthScore = Normalize(wordCount, 50, maxlength);         // Prefer 50–300 words
            double vocabularyScore = Normalize((double)uniqueWords / wordCount, 0.3, 0.7); // Unique word ratio
            double logicScore = Normalize(depthWordCount, 0, 5);         // 0–5+ logic/depth cues

            // Combine scores with weights
            double depthScore = (lengthScore * 0.4) + (vocabularyScore * 0.3) + (logicScore * 0.3);

            return Math.Round(depthScore * 100, 2); // Return as % (0–100)
        }
        private static double Normalize(double value, double min, double max)
        {
            if (value <= min) return 0;
            if (value >= max) return 1;
            return (value - min) / (max - min);
        }




        public static double completeness(double[] vec1, double[] vec2)
        {
            double dot = 0.0;
            double normA = 0.0;
            double normB = 0.0;
            for (int i = 0; i < vec1.Length; i++)
            {
                dot += vec1[i] * vec2[i];
                normA += Math.Pow(vec1[i], 2);
                normB += Math.Pow(vec2[i], 2);
            }
            return dot / (Math.Sqrt(normA) * Math.Sqrt(normB));
        }

        public static double Accuracy(double[] actual, double[] predicted)
        {
            if (actual.Length != predicted.Length)
                throw new ArgumentException("Arrays must be the same length");

            int correct = 0;

            for (int i = 0; i < actual.Length; i++)
            {
                if (actual[i] == predicted[i]) // exact comparison
                {
                    correct++;
                }
            }

            double accuracy = (double)correct / actual.Length * 100;
            return Math.Round(accuracy, 2); // accuracy in percentage
        }


        public bool Update_AI_BALANCE(string agencyid,decimal consumed_oken)
        {
            
            return true;
        }
    }
}
