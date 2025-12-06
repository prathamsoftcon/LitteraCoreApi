using LitteraCore.DBContext;
using LitteraCore.Models;

namespace LitteraCore.BLContext
{
    public class FeedbackBL
    {
        private readonly IConfiguration _configuration;
        public FeedbackBL(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public List<FeedbackReportSummery> Get_Feedback_360_Summery(string surveyid = null,string trainingid=null)
        {
            List<FeedbackReportSummery> LFS = new List<FeedbackReportSummery>();

            List<FeedbackReport> FR = new List<FeedbackReport>();
            FeedbackDB FDB = new FeedbackDB(_configuration);
            FR = FDB.Get_Feedback_Report_Data(surveyid);
            if(trainingid != null)
            {
                FR = FR.Where(o => o.trainingid.ToString().ToUpper() == trainingid.ToString().ToUpper()).ToList();
            }
            List<String> distinctSurvey = FR.Select(o => o.SurveyID).Distinct().ToList();

            foreach (string s in distinctSurvey)
            {
                FeedbackReportSummery FS = new FeedbackReportSummery();

                FeedbackReport filterreport = new FeedbackReport();
                filterreport = FR.Where(o => o.SurveyID.ToString().ToUpper() == s.ToString().ToUpper()).FirstOrDefault();
                if (filterreport != null)
                {
                    FS.Surveyid = filterreport.SurveyID;
                    FS.Surveyname = filterreport.SurveyName;
                    FS.no_of_respondent = FR.Where(o => o.SurveyID.ToString().ToUpper() == s.ToString().ToUpper()).Select(o => o.tssr_id).Distinct().Count();
                   
                    LFS.Add(FS);
                }


            }





            return LFS;
        }

        public SurveyResponseResult Get_Feedback_360_Survey_Result_Structured(string surveyid, string responsee_mobileno = null, string responsee_emailid = null)
        {
            SurveyResponseResult surveyResponses = new SurveyResponseResult();

            List<FeedbackReport> FR_Surveyresponse = new List<FeedbackReport>();
            FeedbackDB FDB = new FeedbackDB(_configuration);
            FR_Surveyresponse = FDB.Get_Feedback_Report_Data(surveyid);  //Survey API Table 1
                                                                         //Get survey name and id
            string surveyname = "";
            if (FR_Surveyresponse.Count > 0)
            {
                surveyname = FR_Surveyresponse.FirstOrDefault().SurveyName;
            }


            //Filter Data on basis of responsee
            if (responsee_mobileno != null)
            {
                if (responsee_mobileno != "")
                {
                    FR_Surveyresponse = FR_Surveyresponse.Where(o => o.tssr_responsee_mobile == responsee_mobileno).ToList();
                }
            }
            if (responsee_emailid != null)
            {
                if (responsee_emailid != "")
                {
                    FR_Surveyresponse = FR_Surveyresponse.Where(o => o.tssr_responsee_email.ToString().ToUpper() == responsee_emailid.ToString().ToUpper()).ToList();
                }
            }

            //************
            if (FR_Surveyresponse.Count > 0)
            {
                surveyResponses = Prepare_Feedback_Result(surveyid, FR_Surveyresponse);
            }
            else
            {
                surveyResponses.surveyid = surveyid;
                surveyResponses.surveyname = surveyname;
                List<categoryResponse> catres = new List<categoryResponse>();
                surveyResponses.category = catres.ToArray();
            }


            return surveyResponses;
        }
        public SurveyResponseResult Prepare_Feedback_Result(string surveyid, List<FeedbackReport> FR_Surveyresponse)
        {
            SurveyResponseResult surveyResponses = new SurveyResponseResult();
            surveyResponses = Get_Feedback_Result(FR_Surveyresponse, surveyid);



            return surveyResponses;

        }

        public SurveyResponseResult Get_Feedback_Result(List<FeedbackReport> FR_Surveyresponse, string surveyid)
        {

            SurveyResponseResult surveyResponses = new SurveyResponseResult();
            List<SurveyWiseResult> LSR = new List<SurveyWiseResult>();
            FeedbackDB FDB = new FeedbackDB(_configuration);
            List<TemplateQuestion> TQ_TemplateQuestions = new List<TemplateQuestion>();
            TQ_TemplateQuestions = FDB.Get_All_Template_Question_Data(); //All Template Question

            List<TemplateQuestion> GTQ_GroupTemplateQuestion = new List<TemplateQuestion>(); //Filtered Template Question by group

            List<string> distinctgroups = new List<string>();
            distinctgroups = FR_Surveyresponse.Select(o => o.GroupID).Distinct().ToList();
            //Code to get Rating Type
            List<RatingType> rt = new List<RatingType>();
            rt = FDB.Get_Rating_Type();

            List<categoryResponse> categoryresponses = new List<categoryResponse>();
            foreach (string grp in distinctgroups)
            {


                categoryResponse catresponse = new categoryResponse();
                catresponse.groupid = grp;


                SurveyWiseResult surveyResult = new SurveyWiseResult();


                List<sureveyRatingResult> surveyrationg = new List<sureveyRatingResult>();


                List<FeedbackReportSummery> LFS = new List<FeedbackReportSummery>();
                surveyResult.groupid = grp;

                List<FeedbackReport> FR_Surveyresponse_group = new List<FeedbackReport>();
                FR_Surveyresponse_group = FR_Surveyresponse.Where(o => o.GroupID.ToString().ToUpper() == grp.ToString().ToUpper()).ToList();




                catresponse.groupname = FR_Surveyresponse_group.FirstOrDefault().Groupname;

                List<string> distinctssharefeedbackid = new List<string>();
                distinctssharefeedbackid = FR_Surveyresponse_group.Select(o => o.sharefeedbackiD).Distinct().ToList();

                List<sharefeedbackResponse> sharefeedbacklist = new List<sharefeedbackResponse>();
                catresponse.totalresponse = distinctssharefeedbackid.Count();
                foreach (string shareid in distinctssharefeedbackid)
                {
                    sharefeedbackResponse sfr = new sharefeedbackResponse();
                    sfr.sharefeedbackid = shareid;


                    surveyResult.sharefeedbackid = shareid;
                    //**************Filter data by groupid
                    List<FeedbackReport> FR_Surveyresponse_group_shareid = new List<FeedbackReport>();
                    FR_Surveyresponse_group_shareid = FR_Surveyresponse_group.Where(o => o.sharefeedbackiD.ToString().ToUpper() == shareid.ToString().ToUpper()).ToList();

                    //Filter This Data on basis of all Group ids in /Survey API table1

                    GTQ_GroupTemplateQuestion = Get_Survey_Groups_Template_Quesions(TQ_TemplateQuestions, FR_Surveyresponse_group_shareid);

                    GTQ_GroupTemplateQuestion = TQ_TemplateQuestions.Where(o => o.groupid.ToString().ToUpper() == grp.ToString().ToUpper()).ToList();
                    //**************

                    //*********for Rating Result
                    List<FeedbackReport> FRRating = new List<FeedbackReport>();
                    FRRating = FR_Surveyresponse_group_shareid.Where(o => o.QuestionType == "3").ToList();  //Type 3 Result
                    List<TemplateQuestion> GTQ_GroupTemplate_type3 = new List<TemplateQuestion>();
                    GTQ_GroupTemplate_type3 = GTQ_GroupTemplateQuestion.Where(o => o.QuestionType.ToString() == "3").ToList();
                    int totalRatingResponse = FRRating.Count();

                    //********For MCQ
                    List<FeedbackReport> FRMCQ = new List<FeedbackReport>();
                    FRMCQ = FR_Surveyresponse_group_shareid.Where(o => o.QuestionType == "2").ToList();  //Type 3 Result
                    List<TemplateQuestion> GTQ_GroupTemplate_type2 = new List<TemplateQuestion>();
                    GTQ_GroupTemplate_type2 = GTQ_GroupTemplateQuestion.Where(o => o.QuestionType.ToString() == "2").ToList();
                    int totalMcqResponse = FRMCQ.Count();


                    //*********

                    //********For Desc
                    List<FeedbackReport> FRDESC = new List<FeedbackReport>();
                    FRDESC = FR_Surveyresponse_group_shareid.Where(o => o.QuestionType == "1").ToList();  //Type 1 Result for DESC
                    List<TemplateQuestion> GTQ_GroupTemplate_type1 = new List<TemplateQuestion>();
                    GTQ_GroupTemplate_type1 = GTQ_GroupTemplateQuestion.Where(o => o.QuestionType.ToString() == "1").ToList();
                    int totalDescResponse = FRDESC.Count();


                    //*********





                    //*****to get MCQ All answers / options
                    List<singleChoiceAnswers> scoptions = new List<singleChoiceAnswers>();
                    scoptions = FDB.Get_MCQ_Answer();
                    scoptions = scoptions.Where(o => o.QuestionGroupid.ToString().ToUpper() == grp.ToString().ToUpper()).ToList();
                    //*************
                    //*****to get Desc All answers / options
                    List<descriptiveAnswers> descoptions = new List<descriptiveAnswers>();
                    descoptions = FDB.Get_DESC_Answer();
                    descoptions = descoptions.Where(o => o.QuestionGroupid.ToString().ToUpper() == grp.ToString().ToUpper()).ToList();
                    //*************


                    List<TSOR_Results> tsqrResult = new List<TSOR_Results>();
                    List<TssrResponse> tssrResult = new List<TssrResponse>();
                    List<string> distinctTSQR = new List<string>();
                    distinctTSQR = FR_Surveyresponse_group_shareid.Select(o => o.tssr_id).Distinct().ToList();
                    sfr.totalresponse = distinctTSQR.Count();
                    foreach (string s in distinctTSQR)
                    {

                        List<FeedbackReport> FRRating_Filtered = new List<FeedbackReport>();
                        FRRating_Filtered = FRRating.Where(o => o.tssqr_id.ToString().ToUpper() == s.ToString().ToUpper()).ToList();
                        FRMCQ = FRMCQ.Where(o => o.tssqr_id.ToString().ToUpper() == s.ToString().ToUpper()).ToList();
                        FRDESC = FRDESC.Where(o => o.tssqr_id.ToString().ToUpper() == s.ToString().ToUpper()).ToList();
                        List<RatingQuesResult> rqr = new List<RatingQuesResult>();
                        List<MCQResult> mcqreslist = new List<MCQResult>();
                        List<DescResult> desreslist = new List<DescResult>();
                        TssrResponse tssrrobj = new TssrResponse();
                        tssrrobj.tssrid = s;




                        TSOR_Results tsqrobj = new TSOR_Results();
                        tsqrobj.tsqr_id = s;
                        List<FeedbackReport> Filtereddata = new List<FeedbackReport>();
                        Filtereddata = FR_Surveyresponse_group_shareid.Where(o => o.tssr_id.ToString().ToUpper() == s.ToUpper()).ToList();


                        int totalratingresponse = 0;
                        totalratingresponse = GTQ_GroupTemplate_type3.Count();

                        foreach (TemplateQuestion teq in GTQ_GroupTemplate_type3)
                        {

                            int totalquestionresponse = 0;
                            //Calculate total Question Response
                            totalquestionresponse = Filtereddata.Where(o => o.QuestionID.ToString().ToUpper() == teq.questionid.ToString().ToUpper()).Count();
                            //totalquestionresponse = GTQ_GroupTemplate_type3.Where(o => o.questionid.ToString().ToUpper() == teq.questionid.ToString().ToUpper()).Count();
                            //File.AppendAllText(HttpContext.Current.Server.MapPath("~/Log/Log.txt"), "FR11");
                            RatingQuesResult rqres = new RatingQuesResult();
                            rqres.questionid = teq.questionid;
                            rqres.questiontext = teq.QuestionText;
                            List<RatingResult> LRR = new List<RatingResult>();
                            //File.AppendAllText(HttpContext.Current.Server.MapPath("~/Log/Log.txt"), "Before Rating Result");
                            LRR = Get_Question_Rating_Options_result(rt, FRRating_Filtered, teq.questionid, totalquestionresponse);
                            //File.AppendAllText(HttpContext.Current.Server.MapPath("~/Log/Log.txt"), "After Rating Result");
                            rqres.ratingresult = LRR.ToArray();


                            rqr.Add(rqres);
                        }

                        //******* Loop For MCQ options
                        int totalmcqresponse = 0;
                        totalmcqresponse = GTQ_GroupTemplate_type2.Count();

                        foreach (TemplateQuestion teq in GTQ_GroupTemplate_type2)
                        {
                            int totalquestionresponse = 0;
                            //Calculate total Question Response
                            totalquestionresponse = Filtereddata.Where(o => o.QuestionID.ToString().ToUpper() == teq.questionid.ToString().ToUpper()).Count();
                            //totalquestionresponse = GTQ_GroupTemplate_type3.Where(o => o.questionid.ToString().ToUpper() == teq.questionid.ToString().ToUpper()).Count();

                            //MCQResult rqres = new MCQResult();
                            //rqres.questionid = teq.questionid;
                            //  rqres.questiontext = teq.QuestionText;

                            //Filter Question Answers
                            List<singleChoiceAnswers> Filteredans = scoptions.Where(o => o.QuestionID.ToString().ToUpper() == teq.questionid.ToString().ToUpper()).ToList();



                            MCQResult RR = new MCQResult();
                            RR.questionid = teq.questionid;
                            RR.questiontext = teq.QuestionText;
                            List<MCQResultOptions> mcqoptionsList = new List<MCQResultOptions>();
                            mcqoptionsList = Get_Question_MCQ_Options_result(Filteredans, Filtereddata, totalquestionresponse);

                            RR.mcqresult = mcqoptionsList.ToArray();
                            mcqreslist.Add(RR);

                        }
                        //******End MCQ

                        //*************DESC**********//

                        int totaldescresponse = 0;
                        totaldescresponse = GTQ_GroupTemplate_type1.Count();

                        foreach (TemplateQuestion teq in GTQ_GroupTemplate_type1)
                        {
                            int totalquestionresponse = 0;
                            //Calculate total Question Response
                            totalquestionresponse = Filtereddata.Where(o => o.QuestionID.ToString().ToUpper() == teq.questionid.ToString().ToUpper()).Count();


                            //Filter Question Answers
                            List<descriptiveAnswers> Filteredans = descoptions.Where(o => o.QuestionID.ToString().ToUpper() == teq.questionid.ToString().ToUpper()).ToList();



                            DescResult RR = new DescResult();
                            RR.questionid = teq.questionid;
                            RR.questiontext = teq.QuestionText;

                            List<DescAnsText> descoptionsList = new List<DescAnsText>();

                            foreach (descriptiveAnswers t in Filteredans)
                            {
                                DescAnsText descoptionobj = new DescAnsText();
                                descoptionobj.answerid = t.AnswerID;
                                descoptionobj.answertext = t.AnswerText;
                                string result = "";

                                if (Filtereddata.Where(o => o.QuestionID.ToString().ToUpper() == t.QuestionID.ToString().ToUpper()).FirstOrDefault() == null)
                                {
                                    result = "";
                                }
                                else
                                {
                                    string ResponseAnswerid = Filtereddata.Where(o => o.QuestionID.ToString().ToUpper() == t.QuestionID.ToString().ToUpper()).FirstOrDefault().tssqr_question_response;
                                    if (t.AnswerID.ToString().ToUpper() == ResponseAnswerid.ToString().ToUpper())
                                    {
                                        result = "";
                                    }
                                    else
                                    {
                                        result = ResponseAnswerid;
                                    }
                                }

                                descoptionobj.result = result;

                                descoptionsList.Add(descoptionobj);
                            }

                            RR.descresult = descoptionsList.ToArray();


                            desreslist.Add(RR);

                        }

                        //*****************


                        // surveyResult.ratingResults = rqr.ToArray();
                        tsqrobj.ratingResults = rqr.ToArray();
                        tsqrobj.mcqResult = mcqreslist.ToArray();
                        sureveyRatingResult SurRR = new sureveyRatingResult();
                        SurRR.totalquestions = totalratingresponse;
                        SurRR.result = rqr.ToArray();
                        tssrrobj.ratingResult = SurRR;

                        sureveyMCQResult SurMCQR = new sureveyMCQResult();
                        SurMCQR.totalquestions = totalmcqresponse;
                        SurMCQR.result = mcqreslist.ToArray();
                        tssrrobj.mcqResult = SurMCQR;


                        sureveyDescResult SurDESCR = new sureveyDescResult();
                        SurDESCR.totalquestions = totaldescresponse;
                        SurDESCR.result = desreslist.ToArray();
                        tssrrobj.descResult = SurDESCR;

                        tssrrobj.totalquestions = totalmcqresponse + totalratingresponse + totaldescresponse;

                        //***********Add extra columns for responsee details

                        tssrrobj.responsee_name = FR_Surveyresponse.FirstOrDefault().tssr_responsee_name;
                        tssrrobj.responsee_email = FR_Surveyresponse.FirstOrDefault().tssr_responsee_email;
                        tssrrobj.responsee_mobileno = FR_Surveyresponse.FirstOrDefault().tssr_responsee_mobile;

                        tsqrResult.Add(tsqrobj);
                        tssrResult.Add(tssrrobj);

                    }
                    surveyResult.tsqr_results = tsqrResult.ToArray();
                    sfr.tssrresult = tssrResult.ToArray();
                    try
                    {
                        sfr.overall_rating_impression = Calculate_Overall_Sharefeedback_Rating(sfr, rt, FR_Surveyresponse_group_shareid.Where(o => o.QuestionType == "3").Count(), shareid);
                    }
                    catch (Exception ex)
                    {
                        sfr.overall_rating_impression = 0;
                    }




                    sharefeedbacklist.Add(sfr);
                }
                catresponse.sharefeedback = sharefeedbacklist.ToArray();






                //*****************
                LSR.Add(surveyResult);
                try
                {
                    catresponse.overall_rating_impression = Calculate_Overall_Group_Rating(catresponse, rt, FR_Surveyresponse_group.Where(o => o.QuestionType == "3").Count());
                }
                catch (Exception ex)
                {
                    catresponse.overall_rating_impression = 0;
                }

                categoryresponses.Add(catresponse);

            }
            surveyResponses.surveyid = surveyid;
            surveyResponses.surveyname = FR_Surveyresponse.FirstOrDefault().SurveyName;

            surveyResponses.category = categoryresponses.ToArray();
            surveyResponses.totalresponse = FR_Surveyresponse.Select(o => o.GroupID).Distinct().Count();  //  111
            return surveyResponses;
        }
        public List<RatingResult> Get_Question_Rating_Options_result(List<RatingType> rationgoptions, List<FeedbackReport> survey_response_rating, string questionid, int totalquestionresponse)
        {
            //File.AppendAllText(HttpContext.Current.Server.MapPath("~/Log/Log.txt"), "In ratinf option result");
            List<RatingResult> question_rating_option_result = new List<RatingResult>();
            int maxMarks = rationgoptions.Max(o => Convert.ToInt32(o.ratingvalue));
            foreach (RatingType t in rationgoptions)
            {
                RatingResult RR = new RatingResult();
                RR.answerid = t.ratingvalue;
                RR.displaytext = t.ratingtext;
                //Calculate Rating Percent
                //Get Particular Question Data
                List<FeedbackReport> QFR = new List<FeedbackReport>();
                QFR = survey_response_rating.Where(o => o.QuestionID.ToString().ToUpper() == questionid.ToString().ToUpper()).ToList();

                //Filter Data for particular Rating 
                QFR = QFR.Where(o => o.tssqr_question_response == t.ratingvalue).ToList();
                //Filter by Ratingvalue  
                //Formula to calculate Rating Percentage 
                double ratingpercentage = 0;
                if (QFR.Count() == 0)
                {
                    ratingpercentage = 0;
                }
                else
                {
                    ratingpercentage = ((Convert.ToDouble(QFR.Count()) * Convert.ToDouble(t.ratingvalue)) / (Convert.ToDouble(totalquestionresponse) * Convert.ToDouble(maxMarks)));
                    //ratingpercentage = Convert.ToDouble(t.ratingvalue);
                }



                //******

                RR.result = ratingpercentage;
                //Get Count


                question_rating_option_result.Add(RR);

            }
            //File.AppendAllText(HttpContext.Current.Server.MapPath("~/Log/Log.txt"), "out ratinf option result");
            return question_rating_option_result;
        }


        public List<MCQResultOptions> Get_Question_MCQ_Options_result(List<singleChoiceAnswers> mcqoptions, List<FeedbackReport> survey_response_mcq, int totalquestionresponse)
        {
            List<MCQResultOptions> question_mcq_option_result = new List<MCQResultOptions>();

            foreach (singleChoiceAnswers t in mcqoptions)
            {
                MCQResultOptions mcqoptionobj = new MCQResultOptions();
                mcqoptionobj.answerid = t.AnswerID;
                mcqoptionobj.answertext = t.AnswerText;
                Double ratingValue = 0;

                if (survey_response_mcq.Where(o => o.QuestionID.ToString().ToUpper() == t.QuestionID.ToString().ToUpper()).FirstOrDefault() == null)
                {
                    ratingValue = 0;
                }
                else
                {
                    string ResponseAnswerid = survey_response_mcq.Where(o => o.QuestionID.ToString().ToUpper() == t.QuestionID.ToString().ToUpper()).FirstOrDefault().tssqr_answer_id;
                    if (t.AnswerID.ToString().ToUpper() == ResponseAnswerid.ToString().ToUpper())
                    {
                        ratingValue = 1;
                    }
                    else
                    {
                        ratingValue = 0;
                    }
                }



                //Filter by Ratingvalue  
                //Formula to calculate Rating Percentage 
                double resultpercentage = 0;
                if (totalquestionresponse == 0)
                {
                    resultpercentage = 0;
                }
                else
                {
                    resultpercentage = ((Convert.ToDouble(ratingValue)) / (totalquestionresponse));
                }




                //******

                mcqoptionobj.result = resultpercentage;


                question_mcq_option_result.Add(mcqoptionobj);
            }
            return question_mcq_option_result;
        }

        public decimal Calculate_Overall_Group_Rating(categoryResponse cr, List<RatingType> ratings, int totalgroupQuestionresponse)
        {
            decimal overall_Rating_Impression = 0;
            int ratingresultcount = ratings.Count();


            List<RatingType_val> rating_value_sum = new List<RatingType_val>();

            foreach (RatingType rt in ratings)
            {
                RatingType_val ratingtval = new RatingType_val();
                ratingtval.ratingid = rt.ratingid;
                ratingtval.ratingtext = rt.ratingtext;
                ratingtval.ratingvalue = rt.ratingvalue;
                ratingtval.total = calculate_total_question_value(cr.sharefeedback.ToList(), rt.ratingvalue);
                rating_value_sum.Add(ratingtval);

                overall_Rating_Impression = overall_Rating_Impression + ratingtval.total;
            }
            overall_Rating_Impression = overall_Rating_Impression / totalgroupQuestionresponse;

           


            return Math.Round(overall_Rating_Impression * 100, 2);
        }
        public decimal calculate_total_question_value(List<sharefeedbackResponse> sharefeedbacklist, string ratingval, string questionid = null)
        {
            decimal totalresponse = 0;
            foreach (sharefeedbackResponse sfr in sharefeedbacklist)
            {
                foreach (TssrResponse tssrres in sfr.tssrresult)
                {
                    sureveyRatingResult srr = tssrres.ratingResult;
                    foreach (RatingQuesResult rqr in srr.result)
                    {
                        if (questionid != null)
                        {
                            if (rqr.questionid.ToString().ToUpper() != questionid.ToString().ToUpper())
                            {
                                continue;
                            }
                        }
                        foreach (RatingResult rr in rqr.ratingresult)
                        {
                            if (rr.answerid == ratingval)
                            {
                                totalresponse = Convert.ToDecimal(totalresponse) + Convert.ToDecimal(rr.result);
                            }
                        }
                    }

                }
            }

            return totalresponse;
        }

        public List<TemplateQuestion> Get_Survey_Groups_Template_Quesions(List<TemplateQuestion> all_temp_question, List<FeedbackReport> group_sharefeedback_survey_response)
        {
            List<TemplateQuestion> templatequestions = new List<TemplateQuestion>();
            foreach (TemplateQuestion tqs in all_temp_question)
            {
                if (group_sharefeedback_survey_response.Where(o => o.GroupID.ToString().ToUpper() == tqs.groupid.ToString().ToUpper()).Count() > 0)
                {
                    //Condition to get Distinct  Questions
                    if (templatequestions.Where(o => o.questionid.ToString().ToUpper() == tqs.questionid.ToString().ToUpper()).Count() <= 0)
                    {
                        templatequestions.Add(tqs);
                    }

                }
            }
            return templatequestions;
        }
        public decimal Calculate_Overall_Sharefeedback_Rating(sharefeedbackResponse cr, List<RatingType> ratings, int totalsharefeedbackQuestionresponse, string sharefeedbackid)
        {
            List<sharefeedbackResponse> lsf = new List<sharefeedbackResponse>();
            lsf.Add(cr);
            decimal overall_Rating_Impression = 0;
            int ratingresultcount = ratings.Count();


            List<RatingType_val> rating_value_sum = new List<RatingType_val>();

            foreach (RatingType rt in ratings)
            {
                RatingType_val ratingtval = new RatingType_val();
                ratingtval.ratingid = rt.ratingid;
                ratingtval.ratingtext = rt.ratingtext;
                ratingtval.ratingvalue = rt.ratingvalue;
                ratingtval.total = calculate_total_question_value(lsf, rt.ratingvalue);
                rating_value_sum.Add(ratingtval);

                overall_Rating_Impression = overall_Rating_Impression + ratingtval.total;
            }
            overall_Rating_Impression = overall_Rating_Impression / totalsharefeedbackQuestionresponse;



            return Math.Round(overall_Rating_Impression * 100, 2);
        }

        public Question_Rating_Result Get_Rating_Result_Summary(string surveyid, string groupid, string sharefeedbackid = null, string responsee_mobileno = null, string responsee_emailid = null)
        {
            List<RatingQuesResult> QuestionratingResult = new List<RatingQuesResult>();
            List<Question_Rating_Result_Summary> ratingsummary = new List<Question_Rating_Result_Summary>();
            List<RatingType> ratings = new List<RatingType>();
            FeedbackDB FDB = new FeedbackDB(_configuration);
            ratings = FDB.Get_Rating_Type();

            SurveyResponseResult surveyResponses = new SurveyResponseResult();
            List<FeedbackReport> FR_Surveyresponse = new List<FeedbackReport>();

            FR_Surveyresponse = FDB.Get_Feedback_Report_Data(surveyid);
            if (groupid != null)
            {
                FR_Surveyresponse = FR_Surveyresponse.Where(o => o.GroupID.ToString().ToUpper() == groupid.ToString().ToUpper()).ToList();
            }
            if (sharefeedbackid != null)
            {
                FR_Surveyresponse = FR_Surveyresponse.Where(o => o.sharefeedbackiD.ToString().ToUpper() == sharefeedbackid.ToString().ToUpper()).ToList();
            }

            //string get survey and group name
            string surveyname = "";
            string groupname = "";
            if (FR_Surveyresponse.Count > 0)
            {
                surveyname = FR_Surveyresponse.FirstOrDefault().SurveyName;
                groupname = FR_Surveyresponse.FirstOrDefault().Groupname;
            }


            //*******Filter data according to responsee
            if (responsee_mobileno != null)
            {
                if (responsee_mobileno != "")
                {
                    FR_Surveyresponse = FR_Surveyresponse.Where(o => o.tssr_responsee_mobile == responsee_mobileno).ToList();
                }
            }
            if (responsee_emailid != null)
            {
                if (responsee_emailid != "")
                {
                    FR_Surveyresponse = FR_Surveyresponse.Where(o => o.tssr_responsee_email.ToString().ToUpper() == responsee_emailid.ToString().ToUpper()).ToList();
                }
            }


            //*************
            Question_Rating_Result QRR = new Question_Rating_Result();

            if (FR_Surveyresponse.Count > 0)
            {
                surveyResponses = Prepare_Feedback_Result(surveyid, FR_Surveyresponse);

                foreach (categoryResponse catres in surveyResponses.category)
                {
                    foreach (sharefeedbackResponse sf in catres.sharefeedback)
                    {
                        foreach (TssrResponse tssr in sf.tssrresult)
                        {
                            sureveyRatingResult SRR = tssr.ratingResult;
                            foreach (RatingQuesResult res in SRR.result)
                            {
                                RatingQuesResult RR = new RatingQuesResult();
                                RR.questionid = res.questionid;
                                RR.questiontext = res.questiontext;
                                RR.ratingresult = res.ratingresult;
                                QuestionratingResult.Add(RR);
                            }

                        }
                    }


                }

                //QuestionratingResult where all group questions with rating
                //now get distinct questions
                List<string> disQues = new List<string>();
                disQues = QuestionratingResult.Select(o => o.questionid).Distinct().ToList();
                //Loop to Get all distinct quesion rating summary
                foreach (string qid in disQues)
                {
                    Question_Rating_Result_Summary QRRS = new Question_Rating_Result_Summary();
                    List<RatingQuesResult> filterratingresult = QuestionratingResult.Where(o => o.questionid.ToString().ToUpper() == qid.ToString().ToUpper()).ToList();

                    List<RatingType_val> rating_value_sum = new List<RatingType_val>();
                    int totalquestionresponses = FR_Surveyresponse.Where(o => o.QuestionID.ToString().ToUpper() == qid.ToString().ToUpper()).Count();

                    foreach (RatingType rt in ratings)
                    {
                        RatingType_val ratingtval = new RatingType_val();
                        ratingtval.ratingid = rt.ratingid;
                        ratingtval.ratingtext = rt.ratingtext;
                        ratingtval.ratingvalue = rt.ratingvalue;
                        ratingtval.total = Math.Round((calculate_total_question_value(surveyResponses.category.FirstOrDefault().sharefeedback.ToList(), rt.ratingvalue, qid) / totalquestionresponses) * 100, 2);
                        rating_value_sum.Add(ratingtval);

                    }


                    QRRS.Questionid = filterratingresult.FirstOrDefault().questionid;
                    QRRS.Questiontext = filterratingresult.FirstOrDefault().questiontext;

                    QRRS.rating = rating_value_sum.ToArray();

                    decimal overallrating = 0;
                    foreach (RatingType_val ratval in rating_value_sum)
                    {
                        overallrating = Convert.ToDecimal(overallrating) + Convert.ToDecimal(ratval.total);
                    }
                    QRRS.overallrating = overallrating;




                    ratingsummary.Add(QRRS);
                }
                QRR.surveyid = FR_Surveyresponse.FirstOrDefault().SurveyID;
                QRR.surveyname = FR_Surveyresponse.FirstOrDefault().SurveyName;
                QRR.groupid = FR_Surveyresponse.FirstOrDefault().GroupID;
                QRR.groupname = FR_Surveyresponse.FirstOrDefault().SurveyCategory;
                QRR.Question_Rating_Result_Summary = ratingsummary.ToArray();

                if (responsee_mobileno != null || responsee_emailid != null)
                {
                    if (FR_Surveyresponse.Count > 0)
                    {
                        QRR.responsee_name = FR_Surveyresponse.FirstOrDefault().tssr_responsee_name;
                        QRR.responsee_mobile = FR_Surveyresponse.FirstOrDefault().tssr_responsee_mobile;
                        QRR.responsee_email = FR_Surveyresponse.FirstOrDefault().tssr_responsee_email;
                    }
                }



            }
            else
            {
                //Return in case of data not available
                QRR.surveyid = surveyid;
                QRR.surveyname = surveyname;
                QRR.groupid = groupid;
                QRR.groupname = groupname;
                List<Question_Rating_Result_Summary> qrres = new List<Question_Rating_Result_Summary>();
                QRR.Question_Rating_Result_Summary = qrres.ToArray();

            }



            return QRR;
        }

        public Question_MCQ_Result Get_MCQ_Result_Summary(string surveyid, string groupid, string sharefeedbackid = null, string responsee_mobileno = null, string responsee_emailid = null)
        {
            List<MCQResult> QuestionMCQResult = new List<MCQResult>();
            List<Question_MCQ_Result_Summary> mcqsummary = new List<Question_MCQ_Result_Summary>();
            FeedbackDB FDB = new FeedbackDB(_configuration);


            SurveyResponseResult surveyResponses = new SurveyResponseResult();
            List<FeedbackReport> FR_Surveyresponse = new List<FeedbackReport>();

            FR_Surveyresponse = FDB.Get_Feedback_Report_Data(surveyid);
            if (groupid != null)
            {
                FR_Surveyresponse = FR_Surveyresponse.Where(o => o.GroupID.ToString().ToUpper() == groupid.ToString().ToUpper()).ToList();
            }
            if (sharefeedbackid != null)
            {
                FR_Surveyresponse = FR_Surveyresponse.Where(o => o.sharefeedbackiD.ToString().ToUpper() == sharefeedbackid.ToString().ToUpper()).ToList();
            }

            //string get survey and group name
            string surveyname = "";
            string groupname = "";
            if (FR_Surveyresponse.Count > 0)
            {
                surveyname = FR_Surveyresponse.FirstOrDefault().SurveyName;
                groupname = FR_Surveyresponse.FirstOrDefault().Groupname;
            }


            //*******Filter data according to responsee
            if (responsee_mobileno != null)
            {
                if (responsee_mobileno != "")
                {
                    FR_Surveyresponse = FR_Surveyresponse.Where(o => o.tssr_responsee_mobile == responsee_mobileno).ToList();
                }
            }
            if (responsee_emailid != null)
            {
                if (responsee_emailid != "")
                {
                    FR_Surveyresponse = FR_Surveyresponse.Where(o => o.tssr_responsee_email.ToString().ToUpper() == responsee_emailid.ToString().ToUpper()).ToList();
                }
            }


            //*************
            Question_MCQ_Result QRR = new Question_MCQ_Result();

            if (FR_Surveyresponse.Count > 0)
            {
                surveyResponses = Prepare_Feedback_Result(surveyid, FR_Surveyresponse);

                foreach (categoryResponse catres in surveyResponses.category)
                {
                    foreach (sharefeedbackResponse sf in catres.sharefeedback)
                    {
                        foreach (TssrResponse tssr in sf.tssrresult)
                        {
                            sureveyMCQResult SRR = tssr.mcqResult;
                            foreach (MCQResult res in SRR.result)
                            {
                                MCQResult RR = new MCQResult();
                                RR.questionid = res.questionid;
                                RR.questiontext = res.questiontext;
                                RR.mcqresult = res.mcqresult;
                                QuestionMCQResult.Add(RR);
                            }

                        }
                    }


                }

                //QuestionratingResult where all group questions with rating
                //now get distinct questions
                List<string> disQues = new List<string>();
                disQues = QuestionMCQResult.Select(o => o.questionid).Distinct().ToList();
                //Loop to Get all distinct quesion rating summary
                foreach (string qid in disQues)
                {
                    Question_MCQ_Result_Summary QRRS = new Question_MCQ_Result_Summary();
                    List<MCQResult> filterratingresult = QuestionMCQResult.Where(o => o.questionid.ToString().ToUpper() == qid.ToString().ToUpper()).ToList();

                    List<MCQResultOptions> Qoptions = new List<MCQResultOptions>();
                    Qoptions = filterratingresult.FirstOrDefault().mcqresult.ToList();
                    List<MCQType_val> mcq_value_sum = new List<MCQType_val>();
                    // int totalquestionresponses = FR_Surveyresponse.Where(o => o.QuestionID.ToString().ToUpper() == qid.ToString().ToUpper()).Count();

                    foreach (MCQResultOptions rt in Qoptions)
                    {
                        MCQType_val mcqtval = new MCQType_val();
                        mcqtval.answerid = rt.answerid;
                        mcqtval.answertext = rt.answertext;
                        mcqtval.answervalue = "0";
                        mcqtval.total = Math.Round((calculate_total_mcq_question_value(surveyResponses.category.FirstOrDefault().sharefeedback.ToList(), rt.answerid, qid)), 2);
                        mcq_value_sum.Add(mcqtval);

                    }


                    QRRS.Questionid = filterratingresult.FirstOrDefault().questionid;
                    QRRS.Questiontext = filterratingresult.FirstOrDefault().questiontext;
                    QRRS.rating = mcq_value_sum.ToArray();

                    mcqsummary.Add(QRRS);
                }
                QRR.surveyid = FR_Surveyresponse.FirstOrDefault().SurveyID;
                QRR.surveyname = FR_Surveyresponse.FirstOrDefault().SurveyName;
                QRR.groupid = FR_Surveyresponse.FirstOrDefault().GroupID;
                QRR.groupname = FR_Surveyresponse.FirstOrDefault().SurveyCategory;
                QRR.Question_MCQ_Result_Summary = mcqsummary.ToArray();

                if (responsee_mobileno != null || responsee_emailid != null)
                {
                    if (FR_Surveyresponse.Count > 0)
                    {
                        QRR.responsee_name = FR_Surveyresponse.FirstOrDefault().tssr_responsee_name;
                        QRR.responsee_mobile = FR_Surveyresponse.FirstOrDefault().tssr_responsee_mobile;
                        QRR.responsee_email = FR_Surveyresponse.FirstOrDefault().tssr_responsee_email;
                    }
                }



            }
            else
            {
                //Return in case of data not available
                //QRR.surveyid = surveyid;
                //QRR.surveyname = surveyname;
                //QRR.groupid = groupid;
                //QRR.groupname = groupname;
                //List<Question_Rating_Result_Summary> qrres = new List<Question_Rating_Result_Summary>();
                //QRR.Question_Rating_Result_Summary = qrres.ToArray();

            }



            return QRR;
        }
        public decimal calculate_total_mcq_question_value(List<sharefeedbackResponse> sharefeedbacklist, string answerid, string questionid = null)
        {
            decimal totalresponse = 0;
            foreach (sharefeedbackResponse sfr in sharefeedbacklist)
            {
                foreach (TssrResponse tssrres in sfr.tssrresult)
                {
                    sureveyMCQResult srr = tssrres.mcqResult;
                    foreach (MCQResult rqr in srr.result)
                    {
                        if (questionid != null)
                        {
                            if (rqr.questionid.ToString().ToUpper() != questionid.ToString().ToUpper())
                            {
                                continue;
                            }
                        }
                        foreach (MCQResultOptions rr in rqr.mcqresult)
                        {
                            if (rr.answerid.ToString().ToUpper() == answerid.ToString().ToUpper())
                            {
                                totalresponse = Convert.ToDecimal(totalresponse) + Convert.ToDecimal(rr.result);
                            }
                        }
                    }

                }
            }

            return totalresponse;
        }

        public Question_MCQ_QUESTIONWISE_DETAIL Get_MCQ_DETAIL_QUESTIONWISE(string surveyid, string groupid, string sharefeedbackid = null, string responsee_mobileno = null, string responsee_emailid = null)
        {

            SurveyResponseResult surveyResponses = new SurveyResponseResult();

            List<FeedbackReport> FR_Surveyresponse = new List<FeedbackReport>();
            FeedbackDB FDB = new FeedbackDB(_configuration);
            FR_Surveyresponse = FDB.Get_Feedback_Report_Data(surveyid);  //Survey API Table 1
            FR_Surveyresponse = FR_Surveyresponse.Where(o => o.GroupID.ToString().ToUpper() == groupid.ToString().ToUpper()).ToList();                                                            //Get survey name and id
            string surveyname = "";
            if (FR_Surveyresponse.Count > 0)
            {
                surveyname = FR_Surveyresponse.FirstOrDefault().SurveyName;
            }


            //Filter Data on basis of responsee
            if (responsee_mobileno != null)
            {
                if (responsee_mobileno != "")
                {
                    FR_Surveyresponse = FR_Surveyresponse.Where(o => o.tssr_responsee_mobile == responsee_mobileno).ToList();
                }
            }
            if (responsee_emailid != null)
            {
                if (responsee_emailid != "")
                {
                    FR_Surveyresponse = FR_Surveyresponse.Where(o => o.tssr_responsee_email.ToString().ToUpper() == responsee_emailid.ToString().ToUpper()).ToList();
                }
            }

            List<TemplateQuestion> TQ_TemplateQuestions = new List<TemplateQuestion>();
            TQ_TemplateQuestions = FDB.Get_All_Template_Question_Data(); //All Template Question
            TQ_TemplateQuestions = TQ_TemplateQuestions.Where(o => o.groupid.ToString().ToUpper() == groupid.ToString().ToUpper()).ToList();
            TQ_TemplateQuestions = TQ_TemplateQuestions.Where(o => o.QuestionType == 2).ToList();

            List<singleChoiceAnswers> scoptions = new List<singleChoiceAnswers>();
            scoptions = FDB.Get_MCQ_Answer();
            scoptions = scoptions.Where(o => o.QuestionGroupid.ToString().ToUpper() == groupid.ToString().ToUpper()).ToList();


            List<Question_MCQ_Result_Detail> mcqresultdetails = new List<Question_MCQ_Result_Detail>();
            foreach (TemplateQuestion tq in TQ_TemplateQuestions)
            {
                List<FeedbackReport> FR_Surveyresponse_questions = new List<FeedbackReport>();
                FR_Surveyresponse_questions = FR_Surveyresponse.Where(o => o.QuestionID.ToString().ToUpper() == tq.questionid.ToString().ToUpper()).ToList();

                foreach (FeedbackReport FR in FR_Surveyresponse_questions)
                {
                    Question_MCQ_Result_Detail resultdetail = new Question_MCQ_Result_Detail();
                    resultdetail.Questionid = tq.questionid;
                    resultdetail.Questiontext = tq.QuestionText;
                    resultdetail.responsee_name = FR.tssr_responsee_name;
                    resultdetail.responsee_mobileno = FR.tssr_responsee_mobile;
                    resultdetail.responsee_email = FR.tssr_responsee_email;

                    List<singleChoiceAnswers> Question_scoptions = new List<singleChoiceAnswers>();
                    Question_scoptions = scoptions.Where(o => o.QuestionID.ToString().ToUpper() == tq.questionid.ToString().ToUpper()).ToList();
                    List<MCQType_val> optionlist = new List<MCQType_val>();
                    foreach (singleChoiceAnswers option in Question_scoptions)
                    {
                        MCQType_val mcqans = new MCQType_val();
                        mcqans.answerid = option.AnswerID;
                        mcqans.answertext = option.AnswerText;
                        if (FR.tssqr_answer_id.ToString().ToUpper() == option.AnswerID.ToString().ToUpper())
                        {
                            mcqans.answervalue = "1";
                        }
                        else
                        {
                            mcqans.answervalue = "0";
                        }
                        mcqans.total = FR_Surveyresponse_questions.Where(o => o.QuestionID.ToString().ToUpper() == tq.questionid.ToString().ToUpper() && o.tssr_responsee_mobile == FR.tssr_responsee_mobile && option.AnswerID.ToString().ToUpper() == FR.tssqr_answer_id.ToString().ToUpper()).Count();
                        optionlist.Add(mcqans);

                    }
                    resultdetail.results = optionlist.ToArray();
                    mcqresultdetails.Add(resultdetail);
                }

            }


            List<Question_MCQ_Result_Detail> mcqresultdetails_final = new List<Question_MCQ_Result_Detail>();
            foreach (Question_MCQ_Result_Detail mqf in mcqresultdetails)
            {
                if (mcqresultdetails_final.Where(o => o.Questionid.ToString().ToUpper() == mqf.Questionid.ToString().ToUpper() && o.responsee_mobileno == mqf.responsee_mobileno.ToString().ToUpper()).Count() <= 0)
                {
                    mcqresultdetails_final.Add(mqf);


                }
            }



            List<Question_Responsee_MCQ_Results> qrmc = new List<Question_Responsee_MCQ_Results>();

            foreach (Question_MCQ_Result_Detail qmrd in mcqresultdetails_final)
            {
                if (qrmc.Where(o => o.questionid.ToString().ToUpper() == qmrd.Questionid.ToString().ToString().ToUpper()).Count() == 0)
                {
                    Question_Responsee_MCQ_Results obj_qrmc = new Question_Responsee_MCQ_Results();
                    obj_qrmc.questionid = qmrd.Questionid;
                    obj_qrmc.questiontext = qmrd.Questiontext;

                    List<Responsee_MCQ_Results> rmr = new List<Responsee_MCQ_Results>();
                    List<Question_MCQ_Result_Detail> filtered_finaldata = new List<Question_MCQ_Result_Detail>();
                    filtered_finaldata = mcqresultdetails_final.Where(o => o.Questionid.ToString().ToUpper() == qmrd.Questionid.ToString().ToUpper()).ToList();
                    foreach (Question_MCQ_Result_Detail fqmrd in filtered_finaldata)
                    {
                        Responsee_MCQ_Results respres = new Responsee_MCQ_Results();
                        respres.responsee_name = fqmrd.responsee_name;
                        respres.responsee_mobileno = fqmrd.responsee_mobileno;
                        respres.results = fqmrd.results;
                        rmr.Add(respres);
                    }
                    obj_qrmc.Responsee_MCQ_Results = rmr.ToArray();
                    qrmc.Add(obj_qrmc);


                }
            }


            Question_MCQ_QUESTIONWISE_DETAIL QWD = new Question_MCQ_QUESTIONWISE_DETAIL();
            if (FR_Surveyresponse.Count() > 0)
            {
                QWD.surveyid = FR_Surveyresponse.FirstOrDefault().SurveyID;
                QWD.surveyname = FR_Surveyresponse.FirstOrDefault().SurveyName;
                QWD.groupid = FR_Surveyresponse.FirstOrDefault().GroupID;
                QWD.groupname = FR_Surveyresponse.FirstOrDefault().Groupname;
            }


            QWD.Question_MCQ_Result_Detail = qrmc.ToArray();




            return QWD;
        }
        public Question_DESC_QUESTIONWISE_DETAIL Get_DESC_DETAIL_QUESTIONWISE(string surveyid, string groupid, string sharefeedbackid = null, string responsee_mobileno = null, string responsee_emailid = null)
        {

            SurveyResponseResult surveyResponses = new SurveyResponseResult();

            List<FeedbackReport> FR_Surveyresponse = new List<FeedbackReport>();
            FeedbackDB FDB = new FeedbackDB(_configuration);
            FR_Surveyresponse = FDB.Get_Feedback_Report_Data(surveyid);  //Survey API Table 1
            FR_Surveyresponse = FR_Surveyresponse.Where(o => o.GroupID.ToString().ToUpper() == groupid.ToString().ToUpper()).ToList();                                                            //Get survey name and id
            string surveyname = "";
            if (FR_Surveyresponse.Count > 0)
            {
                surveyname = FR_Surveyresponse.FirstOrDefault().SurveyName;
            }


            //Filter Data on basis of responsee
            if (responsee_mobileno != null)
            {
                if (responsee_mobileno != "")
                {
                    FR_Surveyresponse = FR_Surveyresponse.Where(o => o.tssr_responsee_mobile == responsee_mobileno).ToList();
                }
            }
            if (responsee_emailid != null)
            {
                if (responsee_emailid != "")
                {
                    FR_Surveyresponse = FR_Surveyresponse.Where(o => o.tssr_responsee_email.ToString().ToUpper() == responsee_emailid.ToString().ToUpper()).ToList();
                }
            }

            List<TemplateQuestion> TQ_TemplateQuestions = new List<TemplateQuestion>();
            TQ_TemplateQuestions = FDB.Get_All_Template_Question_Data(); //All Template Question
            TQ_TemplateQuestions = TQ_TemplateQuestions.Where(o => o.groupid.ToString().ToUpper() == groupid.ToString().ToUpper()).ToList();
            TQ_TemplateQuestions = TQ_TemplateQuestions.Where(o => o.QuestionType == 1).ToList();

            //List<singleChoiceAnswers> scoptions = new List<singleChoiceAnswers>();
            //scoptions = FDB.Get_MCQ_Answer();
            //scoptions = scoptions.Where(o => o.QuestionGroupid.ToString().ToUpper() == groupid.ToString().ToUpper()).ToList();


            List<Question_DESC_Result_Detail> mcqresultdetails = new List<Question_DESC_Result_Detail>();
            foreach (TemplateQuestion tq in TQ_TemplateQuestions)
            {
                List<FeedbackReport> FR_Surveyresponse_questions = new List<FeedbackReport>();
                FR_Surveyresponse_questions = FR_Surveyresponse.Where(o => o.QuestionID.ToString().ToUpper() == tq.questionid.ToString().ToUpper()).ToList();

                foreach (FeedbackReport FR in FR_Surveyresponse_questions)
                {
                    Question_DESC_Result_Detail resultdetail = new Question_DESC_Result_Detail();
                    resultdetail.Questionid = tq.questionid;
                    resultdetail.Questiontext = tq.QuestionText;
                    resultdetail.responsee_name = FR.tssr_responsee_name;
                    resultdetail.responsee_mobileno = FR.tssr_responsee_mobile;
                    resultdetail.responsee_email = FR.tssr_responsee_email;

                    //List<singleChoiceAnswers> Question_scoptions = new List<singleChoiceAnswers>();
                    //Question_scoptions = scoptions.Where(o => o.QuestionID.ToString().ToUpper() == tq.questionid.ToString().ToUpper()).ToList();
                    List<DescAnsText> optionlist = new List<DescAnsText>();

                    optionlist.Add(new DescAnsText { answerid = FR.tssqr_answer_id, answertext = FR.tssqr_question_response, result = FR.tssqr_question_response });

                    resultdetail.results = optionlist.ToArray();
                    mcqresultdetails.Add(resultdetail);
                }

            }


            List<Question_DESC_Result_Detail> mcqresultdetails_final = new List<Question_DESC_Result_Detail>();
            foreach (Question_DESC_Result_Detail mqf in mcqresultdetails)
            {
                if (mcqresultdetails_final.Where(o => o.Questionid.ToString().ToUpper() == mqf.Questionid.ToString().ToUpper() && o.responsee_mobileno == mqf.responsee_mobileno.ToString().ToUpper()).Count() <= 0)
                {
                    mcqresultdetails_final.Add(mqf);


                }
            }



            List<Question_Responsee_DESC_Results> qrmc = new List<Question_Responsee_DESC_Results>();

            foreach (Question_DESC_Result_Detail qmrd in mcqresultdetails_final)
            {
                if (qrmc.Where(o => o.questionid.ToString().ToUpper() == qmrd.Questionid.ToString().ToString().ToUpper()).Count() == 0)
                {
                    Question_Responsee_DESC_Results obj_qrmc = new Question_Responsee_DESC_Results();
                    obj_qrmc.questionid = qmrd.Questionid;
                    obj_qrmc.questiontext = qmrd.Questiontext;

                    List<Responsee_DESC_Results> rmr = new List<Responsee_DESC_Results>();
                    List<Question_DESC_Result_Detail> filtered_finaldata = new List<Question_DESC_Result_Detail>();
                    filtered_finaldata = mcqresultdetails_final.Where(o => o.Questionid.ToString().ToUpper() == qmrd.Questionid.ToString().ToUpper()).ToList();
                    foreach (Question_DESC_Result_Detail fqmrd in filtered_finaldata)
                    {
                        Responsee_DESC_Results respres = new Responsee_DESC_Results();
                        respres.responsee_name = fqmrd.responsee_name;
                        respres.responsee_mobileno = fqmrd.responsee_mobileno;
                        respres.results = fqmrd.results;
                        rmr.Add(respres);
                    }
                    obj_qrmc.Responsee_DESC_Results = rmr.ToArray();
                    qrmc.Add(obj_qrmc);


                }
            }


            Question_DESC_QUESTIONWISE_DETAIL QWD = new Question_DESC_QUESTIONWISE_DETAIL();
            if (FR_Surveyresponse.Count() > 0)
            {
                QWD.surveyid = FR_Surveyresponse.FirstOrDefault().SurveyID;
                QWD.surveyname = FR_Surveyresponse.FirstOrDefault().SurveyName;
                QWD.groupid = FR_Surveyresponse.FirstOrDefault().GroupID;
                QWD.groupname = FR_Surveyresponse.FirstOrDefault().Groupname;
            }


            QWD.Question_DESC_Result_Detail = qrmc.ToArray();

            return QWD;
        }

        public List<FeedbackReportSummery_trainingwise> Get_Feedback_360_Summery_trainingwise(string fromdate, string todate, string trainingid = null)
        {
            List<FeedbackReportSummery_trainingwise> LFS = new List<FeedbackReportSummery_trainingwise>();
            List<FeedbackReportSummery_trainingwise> Final_LFS = new List<FeedbackReportSummery_trainingwise>();

            FeedbackDB FDB = new FeedbackDB(_configuration);
            LFS = FDB.Get_Survey_Summary_Data(fromdate,todate, trainingid);

            foreach (FeedbackReportSummery_trainingwise report in LFS)
            {
                if(Final_LFS.Where(o=>o.trainingid.ToString().ToUpper()== report.trainingid.ToString().ToUpper()).Count()<=0)
                {
                    Final_LFS.Add(new FeedbackReportSummery_trainingwise
                    {
                        trainingid = report.trainingid,
                        training_title = report.training_title,
                        trainingcode = report.trainingcode,
                        no_of_respondent = LFS.Sum(o => o.no_of_respondent),
                        training_rating = LFS.Sum(o => o.training_rating)/ LFS.Where(o=>o.trainingid.ToString().ToUpper()== report.trainingid).Count()

                    });
                }
            }
           





            return Final_LFS;
        }






        public List<Training_Feedback_Summary> Get_Trainingwise_Feedback_Summery(string surveyid = null, string trainingid = null)
        {
            List<Training_Feedback_Summary> LFS = new List<Training_Feedback_Summary>();

            List<FeedbackReport> FR = new List<FeedbackReport>();
            FeedbackDB FDB = new FeedbackDB(_configuration);
            FR = FDB.Get_Feedback_Report_Data(surveyid);
            FR = FR.Where(o => o.trainingid != null && o.trainingid.ToString() !="").ToList();
           
            List<String> distinctTrainings = FR.Select(o => o.trainingid).Distinct().ToList();
            int i = 0;
            foreach (string s in distinctTrainings)
            {
                i = i + 1;
               Training_Feedback_Summary FS = new Training_Feedback_Summary();

                FeedbackReport filterreport = new FeedbackReport();
                filterreport = FR.Where(o => o.trainingid.ToString().ToUpper() == s.ToString().ToUpper()).FirstOrDefault();
                if (filterreport != null)
                {
                    FS.trainingid = filterreport.trainingid;
                    FS.training_code = filterreport.SurveyName;
                    FS.t_title = filterreport.SurveyName;
                    FS.no_of_responses= FR.Where(o => o.trainingid.ToString().ToUpper() == s.ToString().ToUpper()).Select(o => o.tssr_id).Distinct().Count();
                    FS.trg_rating = i*10;

                    LFS.Add(FS);
                }


            }





            return LFS;
        }


        public List<Questionnaire_Wise_Responses> Get_Groupwise_Feedback_Summery(string trainingid)
        {
            List<Questionnaire_Wise_Responses> LFS = new List<Questionnaire_Wise_Responses>();

            List<FeedbackReport> FR = new List<FeedbackReport>();
            FeedbackDB FDB = new FeedbackDB(_configuration);
            FR = FDB.Get_Feedback_Report_Data(null);
            FR = FR.Where(o => o.trainingid.ToString().ToUpper() == trainingid.ToString().ToUpper()).ToList();

            List<String> distinctSurvey = FR.Select(o => o.GroupID).Distinct().ToList();
            int i = 0;
            foreach (string s in distinctSurvey)
            {
                i = i + 1;
                Questionnaire_Wise_Responses FS = new Questionnaire_Wise_Responses();

                FeedbackReport filterreport = new FeedbackReport();
                filterreport = FR.Where(o => o.GroupID.ToString().ToUpper() == s.ToString().ToUpper()).FirstOrDefault();
                if (filterreport != null)
                {
                    FS.groupid = filterreport.GroupID;
                    FS.group_title = filterreport.Groupname;
                    FS.no_of_responses = FR.Where(o => o.SurveyID.ToString().ToUpper() == s.ToString().ToUpper()).Select(o => o.tssr_id).Distinct().Count();
                    FS.mcq_responses = FR.Where(o => o.SurveyID.ToString().ToUpper() == s.ToString().ToUpper() && o.QuestionType=="2").Select(o => o.tssr_id).Distinct().Count();
                    FS.descriptive_responses = FR.Where(o => o.SurveyID.ToString().ToUpper() == s.ToString().ToUpper() && o.QuestionType == "1").Select(o => o.tssr_id).Distinct().Count();
                    FS.rating_percentage = i * 10;
                    LFS.Add(FS);
                }


            }





            return LFS;
        }




        public Question_Rating_Result Get_Rating_Result_Summary_New(string groupid,string trainingid=null, string sharefeedbackid = null, string responsee_mobileno = null, string responsee_emailid = null)
        {
            List<RatingQuesResult> QuestionratingResult = new List<RatingQuesResult>();
            List<Question_Rating_Result_Summary> ratingsummary = new List<Question_Rating_Result_Summary>();
            List<RatingType> ratings = new List<RatingType>();
            FeedbackDB FDB = new FeedbackDB(_configuration);
            ratings = FDB.Get_Rating_Type();

            SurveyResponseResult surveyResponses = new SurveyResponseResult();
            List<FeedbackReport> FR_Surveyresponse = new List<FeedbackReport>();

            FR_Surveyresponse = FDB.Get_Feedback_Report_Data(null);
            if(trainingid !=null && trainingid != "")
            {
                FR_Surveyresponse = FR_Surveyresponse.Where(o=>o.trainingid.ToString().ToUpper()==trainingid.ToString().ToUpper()).ToList();
            }
            if (groupid != null)
            {
                FR_Surveyresponse = FR_Surveyresponse.Where(o => o.GroupID.ToString().ToUpper() == groupid.ToString().ToUpper()).ToList();
            }
            if (sharefeedbackid != null)
            {
                FR_Surveyresponse = FR_Surveyresponse.Where(o => o.sharefeedbackiD.ToString().ToUpper() == sharefeedbackid.ToString().ToUpper()).ToList();
            }

            //string get survey and group name
            string surveyname = "";
            string groupname = "";
            if (FR_Surveyresponse.Count > 0)
            {
                surveyname = FR_Surveyresponse.FirstOrDefault().SurveyName;
                groupname = FR_Surveyresponse.FirstOrDefault().Groupname;
            }


            //*******Filter data according to responsee
            if (responsee_mobileno != null)
            {
                if (responsee_mobileno != "")
                {
                    FR_Surveyresponse = FR_Surveyresponse.Where(o => o.tssr_responsee_mobile == responsee_mobileno).ToList();
                }
            }
            if (responsee_emailid != null)
            {
                if (responsee_emailid != "")
                {
                    FR_Surveyresponse = FR_Surveyresponse.Where(o => o.tssr_responsee_email.ToString().ToUpper() == responsee_emailid.ToString().ToUpper()).ToList();
                }
            }


            //*************
            Question_Rating_Result QRR = new Question_Rating_Result();

            if (FR_Surveyresponse.Count > 0)
            {
                surveyResponses = Prepare_Feedback_Result(null, FR_Surveyresponse);

                foreach (categoryResponse catres in surveyResponses.category)
                {
                    foreach (sharefeedbackResponse sf in catres.sharefeedback)
                    {
                        foreach (TssrResponse tssr in sf.tssrresult)
                        {
                            sureveyRatingResult SRR = tssr.ratingResult;
                            foreach (RatingQuesResult res in SRR.result)
                            {
                                RatingQuesResult RR = new RatingQuesResult();
                                RR.questionid = res.questionid;
                                RR.questiontext = res.questiontext;
                                RR.ratingresult = res.ratingresult;
                                QuestionratingResult.Add(RR);
                            }

                        }
                    }


                }

                //QuestionratingResult where all group questions with rating
                //now get distinct questions
                List<string> disQues = new List<string>();
                disQues = QuestionratingResult.Select(o => o.questionid).Distinct().ToList();
                //Loop to Get all distinct quesion rating summary
                foreach (string qid in disQues)
                {
                    Question_Rating_Result_Summary QRRS = new Question_Rating_Result_Summary();
                    List<RatingQuesResult> filterratingresult = QuestionratingResult.Where(o => o.questionid.ToString().ToUpper() == qid.ToString().ToUpper()).ToList();

                    List<RatingType_val> rating_value_sum = new List<RatingType_val>();
                    int totalquestionresponses = FR_Surveyresponse.Where(o => o.QuestionID.ToString().ToUpper() == qid.ToString().ToUpper()).Count();

                    foreach (RatingType rt in ratings)
                    {
                        RatingType_val ratingtval = new RatingType_val();
                        ratingtval.ratingid = rt.ratingid;
                        ratingtval.ratingtext = rt.ratingtext;
                        ratingtval.ratingvalue = rt.ratingvalue;
                        ratingtval.total = Math.Round((calculate_total_question_value(surveyResponses.category.FirstOrDefault().sharefeedback.ToList(), rt.ratingvalue, qid) / totalquestionresponses) * 100, 2);
                        rating_value_sum.Add(ratingtval);

                    }


                    QRRS.Questionid = filterratingresult.FirstOrDefault().questionid;
                    QRRS.Questiontext = filterratingresult.FirstOrDefault().questiontext;

                    QRRS.rating = rating_value_sum.ToArray();

                    decimal overallrating = 0;
                    foreach (RatingType_val ratval in rating_value_sum)
                    {
                        overallrating = Convert.ToDecimal(overallrating) + Convert.ToDecimal(ratval.total);
                    }
                    QRRS.overallrating = overallrating;




                    ratingsummary.Add(QRRS);
                }
                QRR.surveyid = FR_Surveyresponse.FirstOrDefault().SurveyID;
                QRR.surveyname = FR_Surveyresponse.FirstOrDefault().SurveyName;
                QRR.groupid = FR_Surveyresponse.FirstOrDefault().GroupID;
                QRR.groupname = FR_Surveyresponse.FirstOrDefault().SurveyCategory;
                QRR.Question_Rating_Result_Summary = ratingsummary.ToArray();

                if (responsee_mobileno != null || responsee_emailid != null)
                {
                    if (FR_Surveyresponse.Count > 0)
                    {
                        QRR.responsee_name = FR_Surveyresponse.FirstOrDefault().tssr_responsee_name;
                        QRR.responsee_mobile = FR_Surveyresponse.FirstOrDefault().tssr_responsee_mobile;
                        QRR.responsee_email = FR_Surveyresponse.FirstOrDefault().tssr_responsee_email;
                    }
                }



            }
            else
            {
                //Return in case of data not available
               // QRR.surveyid = surveyid;
                QRR.surveyname = surveyname;
                QRR.groupid = groupid;
                QRR.groupname = groupname;
                List<Question_Rating_Result_Summary> qrres = new List<Question_Rating_Result_Summary>();
                QRR.Question_Rating_Result_Summary = qrres.ToArray();

            }



            return QRR;
        }



        public Question_MCQ_Result Get_MCQ_Result_Summary_New(string groupid,string trainingid, string sharefeedbackid = null, string responsee_mobileno = null, string responsee_emailid = null)
        {
            List<MCQResult> QuestionMCQResult = new List<MCQResult>();
            List<Question_MCQ_Result_Summary> mcqsummary = new List<Question_MCQ_Result_Summary>();
            FeedbackDB FDB = new FeedbackDB(_configuration);


            SurveyResponseResult surveyResponses = new SurveyResponseResult();
            List<FeedbackReport> FR_Surveyresponse = new List<FeedbackReport>();

            FR_Surveyresponse = FDB.Get_Feedback_Report_Data(null);
            if (trainingid != null && trainingid != "")
            {
                FR_Surveyresponse = FR_Surveyresponse.Where(o => o.trainingid.ToString().ToUpper() == trainingid.ToString().ToUpper()).ToList();
            }
        
            if (groupid != null)
            {
                FR_Surveyresponse = FR_Surveyresponse.Where(o => o.GroupID.ToString().ToUpper() == groupid.ToString().ToUpper()).ToList();
            }
            if (sharefeedbackid != null)
            {
                FR_Surveyresponse = FR_Surveyresponse.Where(o => o.sharefeedbackiD.ToString().ToUpper() == sharefeedbackid.ToString().ToUpper()).ToList();
            }

            //string get survey and group name
            string surveyname = "";
            string groupname = "";
            if (FR_Surveyresponse.Count > 0)
            {
                surveyname = FR_Surveyresponse.FirstOrDefault().SurveyName;
                groupname = FR_Surveyresponse.FirstOrDefault().Groupname;
            }


            //*******Filter data according to responsee
            if (responsee_mobileno != null)
            {
                if (responsee_mobileno != "")
                {
                    FR_Surveyresponse = FR_Surveyresponse.Where(o => o.tssr_responsee_mobile == responsee_mobileno).ToList();
                }
            }
            if (responsee_emailid != null)
            {
                if (responsee_emailid != "")
                {
                    FR_Surveyresponse = FR_Surveyresponse.Where(o => o.tssr_responsee_email.ToString().ToUpper() == responsee_emailid.ToString().ToUpper()).ToList();
                }
            }


            //*************
            Question_MCQ_Result QRR = new Question_MCQ_Result();

            if (FR_Surveyresponse.Count > 0)
            {
                surveyResponses = Prepare_Feedback_Result(null, FR_Surveyresponse);

                foreach (categoryResponse catres in surveyResponses.category)
                {
                    foreach (sharefeedbackResponse sf in catres.sharefeedback)
                    {
                        foreach (TssrResponse tssr in sf.tssrresult)
                        {
                            sureveyMCQResult SRR = tssr.mcqResult;
                            foreach (MCQResult res in SRR.result)
                            {
                                MCQResult RR = new MCQResult();
                                RR.questionid = res.questionid;
                                RR.questiontext = res.questiontext;
                                RR.mcqresult = res.mcqresult;
                                QuestionMCQResult.Add(RR);
                            }

                        }
                    }


                }

                //QuestionratingResult where all group questions with rating
                //now get distinct questions
                List<string> disQues = new List<string>();
                disQues = QuestionMCQResult.Select(o => o.questionid).Distinct().ToList();
                //Loop to Get all distinct quesion rating summary
                foreach (string qid in disQues)
                {
                    Question_MCQ_Result_Summary QRRS = new Question_MCQ_Result_Summary();
                    List<MCQResult> filterratingresult = QuestionMCQResult.Where(o => o.questionid.ToString().ToUpper() == qid.ToString().ToUpper()).ToList();

                    List<MCQResultOptions> Qoptions = new List<MCQResultOptions>();
                    Qoptions = filterratingresult.FirstOrDefault().mcqresult.ToList();
                    List<MCQType_val> mcq_value_sum = new List<MCQType_val>();
                    // int totalquestionresponses = FR_Surveyresponse.Where(o => o.QuestionID.ToString().ToUpper() == qid.ToString().ToUpper()).Count();

                    foreach (MCQResultOptions rt in Qoptions)
                    {
                        MCQType_val mcqtval = new MCQType_val();
                        mcqtval.answerid = rt.answerid;
                        mcqtval.answertext = rt.answertext;
                        mcqtval.answervalue = "0";
                        mcqtval.total = Math.Round((calculate_total_mcq_question_value(surveyResponses.category.FirstOrDefault().sharefeedback.ToList(), rt.answerid, qid)), 2);
                        mcq_value_sum.Add(mcqtval);

                    }


                    QRRS.Questionid = filterratingresult.FirstOrDefault().questionid;
                    QRRS.Questiontext = filterratingresult.FirstOrDefault().questiontext;
                    QRRS.rating = mcq_value_sum.ToArray();

                    mcqsummary.Add(QRRS);
                }
                QRR.surveyid = FR_Surveyresponse.FirstOrDefault().SurveyID;
                QRR.surveyname = FR_Surveyresponse.FirstOrDefault().SurveyName;
                QRR.groupid = FR_Surveyresponse.FirstOrDefault().GroupID;
                QRR.groupname = FR_Surveyresponse.FirstOrDefault().SurveyCategory;
                QRR.Question_MCQ_Result_Summary = mcqsummary.ToArray();

                if (responsee_mobileno != null || responsee_emailid != null)
                {
                    if (FR_Surveyresponse.Count > 0)
                    {
                        QRR.responsee_name = FR_Surveyresponse.FirstOrDefault().tssr_responsee_name;
                        QRR.responsee_mobile = FR_Surveyresponse.FirstOrDefault().tssr_responsee_mobile;
                        QRR.responsee_email = FR_Surveyresponse.FirstOrDefault().tssr_responsee_email;
                    }
                }



            }
            else
            {
                //Return in case of data not available
                //QRR.surveyid = surveyid;
                //QRR.surveyname = surveyname;
                //QRR.groupid = groupid;
                //QRR.groupname = groupname;
                //List<Question_Rating_Result_Summary> qrres = new List<Question_Rating_Result_Summary>();
                //QRR.Question_Rating_Result_Summary = qrres.ToArray();

            }



            return QRR;
        }



        public List<FeedbackReportSummery_trainingwise> Get_Feedback_360_Summery_Groupwise(string fromdate, string todate, string trainingid = null)
        {
            List<FeedbackReportSummery_trainingwise> LFS = new List<FeedbackReportSummery_trainingwise>();
            List<FeedbackReportSummery_trainingwise> Final_LFS = new List<FeedbackReportSummery_trainingwise>();

            FeedbackDB FDB = new FeedbackDB(_configuration);
            LFS = FDB.Get_Survey_Summary_Data(fromdate, todate, trainingid);

       






            return LFS;
        }


    }
}
