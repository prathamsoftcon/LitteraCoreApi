/*
  TODO before execution:
  1. Replace [TrainingPlan].[tbl_tp_interactive_player_activity] with the actual interactive-player activity table name.
  2. Update these stored procedures to accept and return the new columns:
     - TrainingPlan.proc_tp_interactive_player_ins_activity
     - TrainingPlan.proc_tp_interactive_player_upd_activity
     - TrainingPlan.proc_tp_interactive_player_get_activities
*/

IF OBJECT_ID(N'[TrainingPlan].[tbl_tp_interactive_player_activity]', N'U') IS NULL
BEGIN
    THROW 50001, 'Interactive player activity table not found. Update the table name in this migration before running it.', 1;
END
GO

IF COL_LENGTH('TrainingPlan.tbl_tp_interactive_player_activity', 'image_upload_max_size_mb') IS NULL
BEGIN
    ALTER TABLE [TrainingPlan].[tbl_tp_interactive_player_activity]
    ADD [image_upload_max_size_mb] decimal(10, 2) NULL;
END
GO

IF COL_LENGTH('TrainingPlan.tbl_tp_interactive_player_activity', 'pdf_upload_max_size_mb') IS NULL
BEGIN
    ALTER TABLE [TrainingPlan].[tbl_tp_interactive_player_activity]
    ADD [pdf_upload_max_size_mb] decimal(10, 2) NULL;
END
GO

IF COL_LENGTH('TrainingPlan.tbl_tp_interactive_player_activity', 'upload_max_size_mb') IS NOT NULL
BEGIN
    ALTER TABLE [TrainingPlan].[tbl_tp_interactive_player_activity]
    ALTER COLUMN [upload_max_size_mb] decimal(10, 2) NULL;
END
GO
