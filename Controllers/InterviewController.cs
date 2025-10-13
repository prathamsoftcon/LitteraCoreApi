using LitteraCore.BLContext;
using LitteraCore.Common;
using LitteraCore.DBContext;
using LitteraCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.PowerBI.Api;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Tsp;
using System.Text.RegularExpressions;

namespace LitteraCore.Controllers
{
    public class InterviewController : Controller
    {
        private readonly ILogger<ApplicationConfigController> _logger;
        private readonly IConfiguration _configuration;

        public InterviewController(IConfiguration configuration, ILogger<ApplicationConfigController> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        [HttpGet]
        [Route("api/Interview_Questions")]
        public IActionResult Interview_Questions()
        {
           List<interviewQuestion> interviewquestion=new List<interviewQuestion>();
           InterviewBL ibl = new InterviewBL(_configuration);
           interviewquestion = ibl.Get_Questions();

            return Ok(interviewquestion);
        }
        [HttpPost]
        [Route("api/Interview_Questions")]
        public async Task<IActionResult> PostInterview_Questions(string activityid,string ttpai_id,string ttsam_id, [FromBody] interviewAnswers interviewQuestions,int is_ai_result_required,string? agencyid=null)
        {
            int tokenconsumed = 0;
            //List<interviewQuestion> interviewquestion = new List<interviewQuestion>();
            //InterviewBL ibl = new InterviewBL(_configuration);
            //interviewquestion = ibl.Get_Questions();

            //update user answer embeddings in json ans update json in database
            interviewQuestions.is_AI_enabled_required = is_ai_result_required;
            AIBL abl = new AIBL(_configuration);

            foreach (interviewQuestion ia in interviewQuestions.interviewQuestion)
            {
                if(ia.candidateanswer != null)
                {
                    if(ia.candidateanswer.ToString().Trim() != "")
                    {
                        var embeddingList = await abl.GetEmbeddingsAsync(ia.candidateanswer);
                        tokenconsumed = tokenconsumed + embeddingList.TotalTokens;
                        ia.candidateanswer_embeddings = embeddingList.Embedding.ToArray(); // Now it's a float[]

                    }
                }
                
            }
            string tpad_id = Guid.NewGuid().ToString();

            activity_data ad = new activity_data
            {
                tpad_id = tpad_id,
                tpad_activity_id = activityid,
                tpad_ttpai_id = ttpai_id,
                tpad_ttsam_id = ttsam_id,
                tpad_createdon = System.DateTime.Now,
                tpad_activity_data = JsonConvert.SerializeObject(interviewQuestions)

            };
            ContentBL cbl=new ContentBL(_configuration);
            bool issaved = cbl.Save_Activity_Data(ad);
            bool update_tokens = abl.Update_AI_BALANCE(agencyid, tokenconsumed);

            return Ok( new { tpad_id= tpad_id });
        }


        [HttpGet]
        [Route("api/Interview_Result")]
        public IActionResult Interview_Questions(string tpad_id)
        {
            interviewAnswers ans = new interviewAnswers();
            ContentDB cdb = new ContentDB(_configuration);
            List<activity_data> lad = new List<activity_data>();
            activity_data ad = new activity_data();
            lad = cdb.Get_Activity_Data_by_id(tpad_id);
            interviewAnswers questions=new interviewAnswers();
            if (lad.Count > 0)
            {
                ad = lad.FirstOrDefault();
                string jsonString = ad.tpad_activity_data;

                // Deserialize the JSON array into an array of interviewQuestion
                questions = System.Text.Json.JsonSerializer.Deserialize<interviewAnswers>(jsonString);

                //interviewAnswers result = new interviewAnswers
                //{
                //    interviewQuestion = questions
                //};
            }
            int ai_required = 0;
            if(questions.is_AI_enabled_required != null)
            {
                if (questions.is_AI_enabled_required == 1)
                {
                    ai_required = 1;
                }
                else
                {
                    ai_required = 0;
                }
            }

            if (ai_required == 1)
            {
                foreach (interviewQuestion q in questions.interviewQuestion)
                {
                    interviewResult r = new interviewResult();
                    if (q.candidateanswer_embeddings != null)
                    {
                        double sim = AIBL.CosineSimilarity(q.idealanswer_embedings, q.candidateanswer_embeddings);
                        r.Relevance = Math.Round(((sim + 1) / 2) * 100, 2);
                        r.Completeness = Math.Round(((AIBL.ComputeCompleteness(q.candidateanswer, q.idealanswer.Split(" ".ToArray()).ToList()) + 1) / 2) * 100, 2);
                        //r.Completeness = Math.Round(((AIBL.completeness(q.idealanswer_embedings, q.candidateanswer_embeddings) + 1) / 2) * 100, 2);
                        r.Accuracy = Math.Round(((AIBL.ComputeAccuracy(q.candidateanswer, q.idealanswer.Split(" ".ToArray()).ToList()) + 1) / 2) * 100, 2);
                        // r.Accuracy = AIBL.ComputeAccuracy(q., q.candidateanswer_embeddings);
                        r.Clarity = Math.Round(AIBL.ComputeClarity(q.candidateanswer), 2);
                        if (q.max_length != null)
                        {
                            r.Depth = AIBL.ComputeDepth(q.candidateanswer, q.idealanswer.Split(" ".ToCharArray()).ToList(), q.max_length);
                        }
                        else
                        {
                            r.Depth = AIBL.ComputeDepth(q.candidateanswer, q.idealanswer.Split(" ".ToCharArray()).ToList(), 1000);
                        }


                        q.result = r;
                    }

                }
            }
            
          
           
            return Ok(questions);
        }


        [HttpGet]
        [Route("api/get_embeddings")]
        public async Task<IActionResult> get_embeddings(string text)
        {

            //List<interviewQuestion> interviewquestion = new List<interviewQuestion>();
            //InterviewBL ibl = new InterviewBL(_configuration);
            //interviewquestion = ibl.Get_Questions();

            //update user answer embeddings in json ans update json in database
            AIBL abl = new AIBL(_configuration);
            var embeddingList = await abl.GetEmbeddingsAsync(text);
            return Ok(embeddingList.Embedding.ToArray());
        }


        [HttpGet]
        [Route("api/Next_Interview_Result")]
        public IActionResult Next_Interview_Result(string agencyid,string activityid, string? tpad_id=null,int isprevious=1)
        {
            ContentDB cdb = new ContentDB(_configuration);
            List<activity_data> ACTD = new List<activity_data>();
            ACTD = cdb.Get_Activity_Data(agencyid, activityid);
            ACTD = ACTD.OrderByDescending(x => x.tpad_createdon).ToList();
            string previous_toad_id = "";
            string next_toad_id = "";
            string final_ttpai_id = "";
           
            if (tpad_id == null)
            {
                final_ttpai_id = ACTD.FirstOrDefault().tpad_id;
            }
            else
            {
                int index = ACTD.FindIndex(x => x.tpad_id.ToString().ToUpper() == tpad_id.ToString().ToUpper());
                string? prevId = (index > 0) ? ACTD[index - 1].tpad_id : null;
                string? nextId = (index < ACTD.Count - 1) ? ACTD[index + 1].tpad_id : null;
                if (isprevious == 1)
                {
                    final_ttpai_id = prevId;
                }
                else
                {
                    final_ttpai_id = nextId;
                }
            }

          
           
            if (final_ttpai_id == null)
            {
                return Ok(null);
            }
           
            interviewAnswers ans = new interviewAnswers();
       
            List<activity_data> lad = new List<activity_data>();
            activity_data ad = new activity_data();
            lad = cdb.Get_Activity_Data_by_id(final_ttpai_id);
            interviewAnswers questions = new interviewAnswers();
            if (lad.Count > 0)
            {
                ad = lad.FirstOrDefault();
                string jsonString = ad.tpad_activity_data;

                // Deserialize the JSON array into an array of interviewQuestion
                questions = System.Text.Json.JsonSerializer.Deserialize<interviewAnswers>(jsonString);

               
            }

            foreach (interviewQuestion q in questions.interviewQuestion)
            {
                interviewResult r = new interviewResult();
                if (q.candidateanswer_embeddings != null)
                {
                    double sim = AIBL.CosineSimilarity(q.idealanswer_embedings, q.candidateanswer_embeddings);
                    r.Relevance = Math.Round(((sim + 1) / 2) * 100, 2);
                    r.Completeness = Math.Round(((AIBL.ComputeCompleteness(q.candidateanswer, q.idealanswer.Split(" ".ToArray()).ToList()) + 1) / 2) * 100, 2);
                    //r.Completeness = Math.Round(((AIBL.completeness(q.idealanswer_embedings, q.candidateanswer_embeddings) + 1) / 2) * 100, 2);
                    r.Accuracy = Math.Round(((AIBL.ComputeAccuracy(q.candidateanswer, q.idealanswer.Split(" ".ToArray()).ToList()) + 1) / 2) * 100, 2);
                    // r.Accuracy = AIBL.ComputeAccuracy(q., q.candidateanswer_embeddings);
                    r.Clarity = Math.Round(AIBL.ComputeClarity(q.candidateanswer), 2);
                    if (q.max_length != null)
                    {
                        r.Depth = AIBL.ComputeDepth(q.candidateanswer, q.idealanswer.Split(" ".ToCharArray()).ToList(), q.max_length);
                    }
                    else
                    {
                        r.Depth = AIBL.ComputeDepth(q.candidateanswer, q.idealanswer.Split(" ".ToCharArray()).ToList(), 1000);
                    }


                    q.result = r;
                }

            }

            questions.tpad_id = final_ttpai_id;
            return Ok(questions);
        }

        [HttpGet]
        [Route("api/Check_User_Balance")]
        public IActionResult Check_User_Balance(string agencyid, string activityid)
        {
           

            return Ok(new { is_balance_available = true });
        }

        [HttpGet]
        [Route("api/Enable_API_Interview_Result")]
        public async Task<IActionResult> Enable_API_Interview_Result(string tpad_id)
        {
            int tokenconsumed = 0;
            interviewAnswers ans = new interviewAnswers();
            ContentDB cdb = new ContentDB(_configuration);
            List<activity_data> lad = new List<activity_data>();
            activity_data ad = new activity_data();
            lad = cdb.Get_Activity_Data_by_id(tpad_id);
            interviewAnswers questions = new interviewAnswers();
            if (lad.Count > 0)
            {
                ad = lad.FirstOrDefault();
                string jsonString = ad.tpad_activity_data;

                // Deserialize the JSON array into an array of interviewQuestion
                questions = System.Text.Json.JsonSerializer.Deserialize<interviewAnswers>(jsonString);

                //interviewAnswers result = new interviewAnswers
                //{
                //    interviewQuestion = questions
                //};
            }

            AIBL abl = new AIBL(_configuration);
            foreach (interviewQuestion q in questions.interviewQuestion)
            {
                interviewResult r = new interviewResult();
                if (q.candidateanswer_embeddings != null)
                {
                    if (q.candidateanswer.ToString().Trim() != "")
                    {
                        var embeddingList = await abl.GetEmbeddingsAsync(q.candidateanswer);
                        q.candidateanswer_embeddings = embeddingList.Embedding.ToArray(); // Now it's a float[]
                        tokenconsumed = tokenconsumed + embeddingList.TotalTokens;
                    }
                }


                if (q.candidateanswer_embeddings != null)
                {
                    double sim = AIBL.CosineSimilarity(q.idealanswer_embedings, q.candidateanswer_embeddings);
                    r.Relevance = Math.Round(((sim + 1) / 2) * 100, 2);
                    r.Completeness = Math.Round(((AIBL.ComputeCompleteness(q.candidateanswer, q.idealanswer.Split(" ".ToArray()).ToList()) + 1) / 2) * 100, 2);
                    //r.Completeness = Math.Round(((AIBL.completeness(q.idealanswer_embedings, q.candidateanswer_embeddings) + 1) / 2) * 100, 2);
                    r.Accuracy = Math.Round(((AIBL.ComputeAccuracy(q.candidateanswer, q.idealanswer.Split(" ".ToArray()).ToList()) + 1) / 2) * 100, 2);
                    // r.Accuracy = AIBL.ComputeAccuracy(q., q.candidateanswer_embeddings);
                    r.Clarity = Math.Round(AIBL.ComputeClarity(q.candidateanswer), 2);
                    if (q.max_length != null)
                    {
                        r.Depth = AIBL.ComputeDepth(q.candidateanswer, q.idealanswer.Split(" ".ToCharArray()).ToList(), q.max_length);
                    }
                    else
                    {
                        r.Depth = AIBL.ComputeDepth(q.candidateanswer, q.idealanswer.Split(" ".ToCharArray()).ToList(), 1000);
                    }


                    q.result = r;
                }

            }



            return Ok(questions);
        }
    }
}


