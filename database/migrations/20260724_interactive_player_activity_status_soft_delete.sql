IF OBJECT_ID(N'trainingplan.tbl_tp_ip_acvtivity_master', N'U') IS NULL
BEGIN
    THROW 50001, 'Interactive player activity table not found. Update the table name in this migration before running it.', 1;
END;
GO

DECLARE @column_type sysname;
DECLARE @is_nullable bit;
DECLARE @default_name sysname;

SELECT
    @column_type = t.name,
    @is_nullable = c.is_nullable,
    @default_name = dc.name
FROM sys.columns c
INNER JOIN sys.types t
    ON t.user_type_id = c.user_type_id
LEFT JOIN sys.default_constraints dc
    ON dc.parent_object_id = c.object_id
   AND dc.parent_column_id = c.column_id
WHERE c.object_id = OBJECT_ID(N'trainingplan.tbl_tp_ip_acvtivity_master')
  AND c.name = N'ttiam_isactive';

IF @column_type IS NULL
BEGIN
    THROW 50001, 'ttiam_isactive column not found on trainingplan.tbl_tp_ip_acvtivity_master.', 1;
END;

IF @column_type = N'bit'
BEGIN
    IF @default_name IS NOT NULL
    BEGIN
        EXEC(N'ALTER TABLE trainingplan.tbl_tp_ip_acvtivity_master DROP CONSTRAINT ' + QUOTENAME(@default_name) + N';');
    END;

    EXEC(N'
        ALTER TABLE trainingplan.tbl_tp_ip_acvtivity_master
        ALTER COLUMN ttiam_isactive tinyint ' + CASE WHEN @is_nullable = 1 THEN N'NULL' ELSE N'NOT NULL' END + N';'
    );
END
ELSE IF @column_type NOT IN (N'tinyint', N'smallint', N'int')
BEGIN
    THROW 50001, 'ttiam_isactive must be numeric to support active/inactive/deleted statuses.', 1;
END;
GO

UPDATE trainingplan.tbl_tp_ip_acvtivity_master
SET ttiam_isactive = CASE
    WHEN ttiam_isactive = 0 THEN 2
    WHEN ttiam_isactive = 1 THEN 1
    ELSE 1
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.default_constraints dc
    INNER JOIN sys.columns c
        ON c.object_id = dc.parent_object_id
       AND c.column_id = dc.parent_column_id
    WHERE dc.parent_object_id = OBJECT_ID(N'trainingplan.tbl_tp_ip_acvtivity_master')
      AND c.name = N'ttiam_isactive'
)
BEGIN
    ALTER TABLE trainingplan.tbl_tp_ip_acvtivity_master
    ADD CONSTRAINT df_tp_ip_activity_master_isactive DEFAULT (1) FOR ttiam_isactive;
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.check_constraints
    WHERE name = N'ck_tp_ip_activity_master_isactive_status'
      AND parent_object_id = OBJECT_ID(N'trainingplan.tbl_tp_ip_acvtivity_master')
)
BEGIN
    ALTER TABLE trainingplan.tbl_tp_ip_acvtivity_master
    ADD CONSTRAINT ck_tp_ip_activity_master_isactive_status
        CHECK (ttiam_isactive IN (1, 2, 9));
END;
GO

CREATE OR ALTER PROCEDURE TrainingPlan.proc_tp_ip_ins_activity_response
    @activity_id bigint,
    @session_id nvarchar(100),
    @content_id nvarchar(100),
    @activity_type varchar(30),
    @user_id nvarchar(100),
    @user_type nvarchar(50),
    @status varchar(30),
    @response_json nvarchar(max),
    @result_json nvarchar(max) = null,
    @started_at datetime2(3) = null,
    @completed_at datetime2(3) = null
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (
        SELECT 1
        FROM trainingplan.tbl_tp_ip_acvtivity_master
        WHERE ttiam_activityid = @activity_id
          AND ttiam_isactive = 1
    )
    BEGIN
        THROW 50001, 'Interactive player activity not active.', 1;
    END;

    DECLARE @attempt_number int;

    SELECT @attempt_number = ISNULL(MAX(ttipr_attemptnumber), 0) + 1
    FROM trainingplan.tbl_tp_ip_response WITH (UPDLOCK, HOLDLOCK)
    WHERE ttipr_ttiam_activityid = @activity_id
      AND ttipr_sessionid = @session_id
      AND ttipr_contentid = @content_id
      AND ttipr_userid = @user_id;

    INSERT INTO trainingplan.tbl_tp_ip_response
    (
        ttipr_ttiam_activityid,
        ttipr_sessionid,
        ttipr_contentid,
        ttipr_activitytype,
        ttipr_userid,
        ttipr_usertype,
        ttipr_attemptnumber,
        ttipr_isresubmission,
        ttipr_status,
        ttipr_responsejson,
        ttipr_resultjson,
        ttipr_startedat,
        ttipr_completedat
    )
    VALUES
    (
        @activity_id,
        @session_id,
        @content_id,
        @activity_type,
        @user_id,
        @user_type,
        @attempt_number,
        CASE WHEN @attempt_number > 1 THEN 1 ELSE 0 END,
        @status,
        @response_json,
        @result_json,
        @started_at,
        @completed_at
    );

    DECLARE @response_id bigint = SCOPE_IDENTITY();

    SELECT
        CAST(r.ttipr_responseid AS nvarchar(50)) AS response_id,
        CAST(r.ttipr_ttiam_activityid AS nvarchar(50)) AS activity_id,
        r.ttipr_sessionid AS session_id,
        r.ttipr_contentid AS content_id,
        r.ttipr_activitytype AS activity_type,
        r.ttipr_userid AS user_id,
        r.ttipr_usertype AS user_type,
        r.ttipr_attemptnumber AS attempt_number,
        r.ttipr_isresubmission AS is_resubmission,
        r.ttipr_status AS status,
        r.ttipr_responsejson AS response_json,
        r.ttipr_resultjson AS result_json,
        CONVERT(nvarchar(33), r.ttipr_startedat, 127) AS started_at,
        CONVERT(nvarchar(33), r.ttipr_completedat, 127) AS completed_at,
        r.ttipr_createdon AS created_on
    FROM trainingplan.tbl_tp_ip_response r
    WHERE r.ttipr_responseid = @response_id;
END;
GO
