CREATE              or alter PROCEDURE [Eval].[GetTestListbytestid]     
 @testid  uniqueidentifier,
 @UserID  uniqueidentifier,--'7AA59FB2-9524-4A17-9AA2-51FDB9FD0603'                
 @UserType varchar(50) --'3'              
AS                
BEGIN                
Begin Try                
 SET NOCOUNT ON;                
 Begin                
 ;WITH CTE_Test_Results AS                   
 (                
  select distinct  ts.*,tbd.TrainingCode,tbd.Training_SponsorType,       
tbd.trg_setting, t.TestName,t.AssesmentTime,t.NoOfQuestion,t.mark_per_question,T.QuestionDifficultyID,T.TestDescription,t.TrainingCategoryID,    
T.start_time,T.type, tttt.ttttt_content_desc,ttttt_session_dt,        
        
              
  ttttt_session_time,ttttt_session_duration,ttttt_session_day,ttttt_session_no,ttttt_status                
  ,convert(int,tbd.trg_type) as trg_type,          
  convert(int,(select                 
ds.tdds_status from [DMS].[tbl_dms_document_status] as ds            
inner join Eval.Test t on t.TestID=ds.tdds_doc_id            
where ds.tdds_tat_type_id=115 and  ds.tdds_doc_id=ts.TestID and ds.tdds_process_id =          
(select max(tdds_process_id)   from [DMS].[tbl_dms_document_status] where [DMS].[tbl_dms_document_status].tdds_doc_id  =t.TestID ))) As tdds_status          
            
  from eval.TestQuestions ts                
  inner join trainingplan.TrainingBasicDetails tbd on ts.[Training.TrainingID]= tbd.TrainingId                
  inner join Eval.Test t on t.TestID=ts.TestID                
  inner join trainingplan.tbl_tp_trg_time_table tttt on tttt.ttttt_session_id=ts.[Training.SessionID]                 
  where t.TestID =@testid
 )   ,
 testsubmissionstatus as
(
    SELECT
        ss.tptss_participantid AS PartcipantID,
        ss.tptss_testid AS TestID,
		ss.tptss_status,
		tptss_lastactivityon,
		tptss_completedon,
		tptss_completionreason,
        CASE
            WHEN COUNT(ev.TestPartcipantID) = 0 THEN 3
            ELSE 1
        END AS testresultstatus,

        MAX(ev.TestPartcipantID) AS TestPartcipantID

    FROM eval.tbl_participat_test_start_status ss

    LEFT JOIN eval.ParticipantTestResult ev
        ON ev.TestID = ss.tptss_testid
        AND ev.PartcipantID = ss.tptss_participantid
		and ev.TestPartcipantID=ss.tptss_id
    WHERE ss.tptss_participantid = @UserID

    GROUP BY
        ss.tptss_participantid,
        ss.tptss_testid,		ss.tptss_status,
		tptss_lastactivityon,
		tptss_completedon,
		tptss_completionreason
)
 
 --select * from teststatus       -- where TestID='D0D5BB0D-9F34-4B08-AFD7-F19E84781BCE'
 
select                 
  ct.*,                
  temp.*,                
  ts.*,                
                
  Case                 
  When @UserType ='5' then t.TestPartcipantID  
     Else Null                 
 End 
  as ParticipantStatus ,
  CASE
    WHEN @UserType = '5'
    THEN ISNULL(t.testresultstatus, 0)
    ELSE NULL
END AS testresultstatus ,
  tptss_status,
		tptss_lastactivityon,
		tptss_completedon,
		tptss_completionreason,

  Case                 
    When  @UserType ='5'  then  (select                 
ds.tdds_status from [DMS].[tbl_dms_document_status] as ds   inner join trainingplan.tbl_tp_participant_additional_info as pt  on                
ds.tdds_doc_id =pt.ttpai_id                
where ds.tdds_tat_type_id=122 and pt.TrainingId=ct.[Training.TrainingID]                 
and Participantid= @UserID and  ds.tdds_process_id =          
(  select max(tdds_process_id)   from [DMS].[tbl_dms_document_status] where [DMS].[tbl_dms_document_status].tdds_doc_id  =pt.ttpai_id ))                
    else null                
    end as ParticipantEnrollstatus,100 as maxmarks--,1 as tdds_status                
    from CTE_Test_Results ct    
	LEFT JOIN testsubmissionstatus t
    ON ct.TestID = t.TestID
    AND t.PartcipantID = @UserID

inner join (Select TrainingId from [TrainingPlan].[Ft_tp_get_trgid_for_usertype] (@UserType ,@UserID,null,null )) temp                
on temp.TrainingId=ct.[Training.TrainingID]                 
left join [trainingplan].[tbl_tp_participant_session_status] ts on ts.[ttpss_session_id]=[Training.SessionID] and ts.ttpss_participant_id=@UserID                
--where mark_per_question is not null            
--where TestID='D0D5BB0D-9F34-4B08-AFD7-F19E84781BCE'
  End                
                
  END TRY                
                
---- Error Recording Starts here                
  BEGIN CATCH                
                 
  INSERT INTO db_Errors                
 VALUES                
  (SUSER_SNAME(),                
   ERROR_NUMBER(),                
   ERROR_STATE(),                
   ERROR_SEVERITY(),                
   ERROR_LINE(),                
   ERROR_PROCEDURE(),                
   ERROR_MESSAGE(),                
   GETDATE());                
  END CATCH                
END   
go
CREATE              or alter PROCEDURE [Eval].[GetTestListwithUSerType]                  
 @UserID  uniqueidentifier,--'7AA59FB2-9524-4A17-9AA2-51FDB9FD0603'                
 @UserType varchar(50), --'3'  ,              
 @testType varchar(50) =null               
AS                
BEGIN                
Begin Try                
 SET NOCOUNT ON;                
 Begin                
 ;WITH CTE_Test_Results AS                   
 (                
  select distinct  ts.*,tbd.TrainingCode,tbd.Training_SponsorType,       
tbd.trg_setting, t.TestName,t.AssesmentTime,t.NoOfQuestion,t.mark_per_question,T.QuestionDifficultyID,T.TestDescription,t.TrainingCategoryID,    
T.start_time,T.type, tttt.ttttt_content_desc,ttttt_session_dt,        
        
              
  ttttt_session_time,ttttt_session_duration,ttttt_session_day,ttttt_session_no,ttttt_status                
  ,convert(int,tbd.trg_type) as trg_type,          
  convert(int,(select                 
ds.tdds_status from [DMS].[tbl_dms_document_status] as ds            
inner join Eval.Test t on t.TestID=ds.tdds_doc_id            
where ds.tdds_tat_type_id=115 and  ds.tdds_doc_id=ts.TestID and ds.tdds_process_id =          
(select max(tdds_process_id)   from [DMS].[tbl_dms_document_status] where [DMS].[tbl_dms_document_status].tdds_doc_id  =t.TestID ))) As tdds_status          
            
  from eval.TestQuestions ts                
  inner join trainingplan.TrainingBasicDetails tbd on ts.[Training.TrainingID]= tbd.TrainingId                
  inner join Eval.Test t on t.TestID=ts.TestID                
  inner join trainingplan.tbl_tp_trg_time_table tttt on tttt.ttttt_session_id=ts.[Training.SessionID]                 
  where @testType is null or t.type =@testType  
  --and  ts.TestID='D0D5BB0D-9F34-4B08-AFD7-F19E84781BCE'
 )   ,
testsubmissionstatus as
(
    SELECT
        ss.tptss_participantid AS PartcipantID,
        ss.tptss_testid AS TestID,
		ss.tptss_status,
		tptss_lastactivityon,
		tptss_completedon,
		tptss_completionreason,
        CASE
            WHEN COUNT(ev.TestPartcipantID) = 0 THEN 3
            ELSE 1
        END AS testresultstatus,

        MAX(ev.TestPartcipantID) AS TestPartcipantID

    FROM eval.tbl_participat_test_start_status ss

    LEFT JOIN eval.ParticipantTestResult ev
        ON ev.TestID = ss.tptss_testid
        AND ev.PartcipantID = ss.tptss_participantid
		and ev.TestPartcipantID=ss.tptss_id
    WHERE ss.tptss_participantid = @UserID

    GROUP BY
        ss.tptss_participantid,
        ss.tptss_testid,		ss.tptss_status,
		tptss_lastactivityon,
		tptss_completedon,
		tptss_completionreason
)
  
 --select * from teststatus       -- where TestID='D0D5BB0D-9F34-4B08-AFD7-F19E84781BCE'
 
select                 
  ct.*,                
  temp.*,                
  ts.*,                
                
  Case                 
  When @UserType ='5' then t.TestPartcipantID  
     Else Null                 
 End 
  as ParticipantStatus ,
  CASE
    WHEN @UserType = '5'
    THEN ISNULL(t.testresultstatus, 0)
    ELSE NULL
END AS testresultstatus ,
  tptss_status,
		tptss_lastactivityon,
		tptss_completedon,
		tptss_completionreason
  ,Case                 
    When  @UserType ='5'  then  (select                 
ds.tdds_status from [DMS].[tbl_dms_document_status] as ds   inner join trainingplan.tbl_tp_participant_additional_info as pt  on                
ds.tdds_doc_id =pt.ttpai_id                
where ds.tdds_tat_type_id=122 and pt.TrainingId=ct.[Training.TrainingID]                 
and Participantid= @UserID and  ds.tdds_process_id =          
(  select max(tdds_process_id)   from [DMS].[tbl_dms_document_status] where [DMS].[tbl_dms_document_status].tdds_doc_id  =pt.ttpai_id ))                
    else null                
    end as ParticipantEnrollstatus,100 as maxmarks--,1 as tdds_status                
    from CTE_Test_Results ct    
	LEFT JOIN testsubmissionstatus t
    ON ct.TestID = t.TestID
    AND t.PartcipantID = @UserID
inner join (Select TrainingId from [TrainingPlan].[Ft_tp_get_trgid_for_usertype] (@UserType ,@UserID,null,null )) temp                
on temp.TrainingId=ct.[Training.TrainingID]                 
left join [trainingplan].[tbl_tp_participant_session_status] ts on ts.[ttpss_session_id]=[Training.SessionID] and ts.ttpss_participant_id=@UserID                
--where mark_per_question is not null            
--where TestID='D0D5BB0D-9F34-4B08-AFD7-F19E84781BCE'
  End                
                
  END TRY                
                
---- Error Recording Starts here                
  BEGIN CATCH                
                 
  INSERT INTO db_Errors                
 VALUES                
  (SUSER_SNAME(),                
   ERROR_NUMBER(),                
   ERROR_STATE(),                
   ERROR_SEVERITY(),                
   ERROR_LINE(),                
   ERROR_PROCEDURE(),                
   ERROR_MESSAGE(),                
   GETDATE());                
  END CATCH                
END   
