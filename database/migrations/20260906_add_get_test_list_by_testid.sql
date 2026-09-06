CREATE OR ALTER PROCEDURE [Eval].[GetTestListbytestid]
    @testid UNIQUEIDENTIFIER,
    @UserType VARCHAR(50),
    @UserID UNIQUEIDENTIFIER
AS
BEGIN
    BEGIN TRY
        SET NOCOUNT ON;

        ;WITH CTE_Test_Results AS
        (
            SELECT DISTINCT
                ts.*, tbd.TrainingCode, tbd.Training_SponsorType, tbd.trg_setting,
                t.TestName, t.AssesmentTime, t.NoOfQuestion, t.mark_per_question,
                t.QuestionDifficultyID, t.TestDescription, t.TrainingCategoryID,
                t.start_time, t.type, tttt.ttttt_content_desc, ttttt_session_dt,
                ttttt_session_time, ttttt_session_duration, ttttt_session_day,
                ttttt_session_no, ttttt_status, CONVERT(INT, tbd.trg_type) AS trg_type,
                CONVERT(INT, (
                    SELECT ds.tdds_status
                    FROM [DMS].[tbl_dms_document_status] ds
                    INNER JOIN Eval.Test status_test ON status_test.TestID = ds.tdds_doc_id
                    WHERE ds.tdds_tat_type_id = 115
                      AND ds.tdds_doc_id = ts.TestID
                      AND ds.tdds_process_id = (
                          SELECT MAX(tdds_process_id)
                          FROM [DMS].[tbl_dms_document_status]
                          WHERE tdds_doc_id = ts.TestID
                      )
                )) AS tdds_status
            FROM Eval.TestQuestions ts
            INNER JOIN trainingplan.TrainingBasicDetails tbd
                ON ts.[Training.TrainingID] = tbd.TrainingId
            INNER JOIN Eval.Test t ON t.TestID = ts.TestID
            INNER JOIN trainingplan.tbl_tp_trg_time_table tttt
                ON tttt.ttttt_session_id = ts.[Training.SessionID]
            WHERE t.TestID = @testid
        )
        SELECT
            ct.*, temp.*, participant_session_status.*,
            CASE WHEN @UserType = '5' THEN (
                SELECT DISTINCT result.TestPartcipantID
                FROM Eval.ParticipantTestResult result
                WHERE result.PartcipantID = @UserID
                  AND result.testid = ct.testid
            ) END AS ParticipantStatus,
            CASE WHEN @UserType = '5' THEN (
                SELECT ds.tdds_status
                FROM [DMS].[tbl_dms_document_status] ds
                INNER JOIN trainingplan.tbl_tp_participant_additional_info participant
                    ON ds.tdds_doc_id = participant.ttpai_id
                WHERE ds.tdds_tat_type_id = 122
                  AND participant.TrainingId = ct.[Training.TrainingID]
                  AND participant.Participantid = @UserID
                  AND ds.tdds_process_id = (
                      SELECT MAX(tdds_process_id)
                      FROM [DMS].[tbl_dms_document_status]
                      WHERE tdds_doc_id = participant.ttpai_id
                  )
            ) END AS ParticipantEnrollstatus,
            100 AS maxmarks
        FROM CTE_Test_Results ct
        INNER JOIN [TrainingPlan].[Ft_tp_get_trgid_for_usertype](@UserType, @UserID, NULL, NULL) temp
            ON temp.TrainingId = ct.[Training.TrainingID]
        LEFT JOIN [trainingplan].[tbl_tp_participant_session_status] participant_session_status
            ON participant_session_status.ttpss_session_id = ct.[Training.SessionID]
           AND participant_session_status.ttpss_participant_id = @UserID;
    END TRY
    BEGIN CATCH
        INSERT INTO db_Errors
        VALUES
        (
            SUSER_SNAME(), ERROR_NUMBER(), ERROR_STATE(), ERROR_SEVERITY(),
            ERROR_LINE(), ERROR_PROCEDURE(), ERROR_MESSAGE(), GETDATE()
        );
    END CATCH
END;
GO
