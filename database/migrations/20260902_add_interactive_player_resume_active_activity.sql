IF COL_LENGTH('trainingplan.tbl_tp_ip_resume_checkpoint', 'ttiprc_activeactivityid') IS NULL
BEGIN
    ALTER TABLE trainingplan.tbl_tp_ip_resume_checkpoint
    ADD ttiprc_activeactivityid NVARCHAR(100) NULL;
END;
