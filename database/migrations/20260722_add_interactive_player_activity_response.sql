IF OBJECT_ID(N'trainingplan.tbl_tp_ip_response', N'U') IS NULL
BEGIN
    CREATE TABLE trainingplan.tbl_tp_ip_response
    (
        ttipr_responseid bigint identity(1,1) not null,
        ttipr_ttiam_activityid bigint not null,
        ttipr_sessionid nvarchar(100) not null,
        ttipr_contentid nvarchar(100) not null,
        ttipr_activitytype varchar(30) not null,
        ttipr_userid nvarchar(100) not null,
        ttipr_usertype nvarchar(50) not null,
        ttipr_attemptnumber int not null,
        ttipr_isresubmission bit not null
            constraint df_tp_ip_response_isresubmission default (0),
        ttipr_status varchar(30) not null,
        ttipr_responsejson nvarchar(max) not null,
        ttipr_resultjson nvarchar(max) null,
        ttipr_startedat datetime2(3) null,
        ttipr_completedat datetime2(3) null,
        ttipr_createdon datetime2(3) not null
            constraint df_tp_ip_response_createdon default (sysutcdatetime()),
        constraint pk_tp_ip_response primary key (ttipr_responseid),
        constraint fk_tp_ip_response_activity
            foreign key (ttipr_ttiam_activityid)
            references trainingplan.tbl_tp_ip_acvtivity_master(ttiam_activityid),
        constraint ck_tp_ip_response_responsejson
            check (isjson(ttipr_responsejson) = 1),
        constraint ck_tp_ip_response_resultjson
            check (ttipr_resultjson is null or isjson(ttipr_resultjson) = 1),
        constraint uq_tp_ip_response_attempt
            unique (ttipr_ttiam_activityid, ttipr_sessionid, ttipr_contentid, ttipr_userid, ttipr_attemptnumber)
    );
END
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE name = N'ix_tp_ip_response_lookup'
      AND object_id = OBJECT_ID(N'trainingplan.tbl_tp_ip_response')
)
BEGIN
    CREATE INDEX ix_tp_ip_response_lookup
        ON trainingplan.tbl_tp_ip_response
        (
            ttipr_ttiam_activityid,
            ttipr_sessionid,
            ttipr_contentid,
            ttipr_userid,
            ttipr_createdon
        );
END
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
    )
    BEGIN
        THROW 50001, 'Interactive player activity not found.', 1;
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
END
GO

CREATE OR ALTER PROCEDURE TrainingPlan.proc_tp_ip_get_latest_activity_response
    @activity_id bigint,
    @session_id nvarchar(100),
    @content_id nvarchar(100),
    @user_id nvarchar(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1
        CAST(1 AS bit) AS has_prior_submission,
        r.ttipr_attemptnumber AS last_attempt_number,
        CONVERT(nvarchar(33), COALESCE(r.ttipr_completedat, r.ttipr_createdon), 127) AS last_submitted_at,
        r.ttipr_status AS last_status,
        r.ttipr_responsejson AS latest_response_json
    FROM trainingplan.tbl_tp_ip_response r
    WHERE r.ttipr_ttiam_activityid = @activity_id
      AND r.ttipr_sessionid = @session_id
      AND r.ttipr_contentid = @content_id
      AND r.ttipr_userid = @user_id
    ORDER BY r.ttipr_attemptnumber DESC, r.ttipr_createdon DESC;
END
GO
