IF NOT EXISTS (
    SELECT 1
    FROM sys.tables AS t
    INNER JOIN sys.schemas AS s ON s.schema_id = t.schema_id
    WHERE s.name = 'trainingplan'
      AND t.name = 'tbl_tp_ip_resume_checkpoint'
)
BEGIN
    CREATE TABLE trainingplan.tbl_tp_ip_resume_checkpoint
    (
        ttiprc_id BIGINT IDENTITY(1, 1) NOT NULL,
        ttiprc_trainingid NVARCHAR(100) NOT NULL,
        ttiprc_sessionid NVARCHAR(100) NOT NULL,
        ttiprc_contentid NVARCHAR(100) NOT NULL,
        ttiprc_contentkind NVARCHAR(30) NOT NULL,
        ttiprc_userid NVARCHAR(100) NOT NULL,
        ttiprc_usertype NVARCHAR(50) NOT NULL,
        ttiprc_branchid NVARCHAR(100) NOT NULL,
        ttiprc_mediapositionseconds DECIMAL(18, 3) NULL,
        ttiprc_pagenumber INT NULL,
        ttiprc_activeactivityid NVARCHAR(100) NULL,
        ttiprc_createdon DATETIME2(3) NOT NULL
            CONSTRAINT DF_tbl_tp_ip_resume_checkpoint_createdon DEFAULT SYSUTCDATETIME(),
        ttiprc_updatedon DATETIME2(3) NOT NULL
            CONSTRAINT DF_tbl_tp_ip_resume_checkpoint_updatedon DEFAULT SYSUTCDATETIME(),
        CONSTRAINT PK_tbl_tp_ip_resume_checkpoint PRIMARY KEY CLUSTERED (ttiprc_id),
        CONSTRAINT UQ_tbl_tp_ip_resume_checkpoint_scope UNIQUE
        (
            ttiprc_trainingid,
            ttiprc_sessionid,
            ttiprc_userid,
            ttiprc_usertype,
            ttiprc_branchid
        ),
        CONSTRAINT CK_tbl_tp_ip_resume_checkpoint_media_position
            CHECK (ttiprc_mediapositionseconds IS NULL OR ttiprc_mediapositionseconds >= 0),
        CONSTRAINT CK_tbl_tp_ip_resume_checkpoint_page
            CHECK (ttiprc_pagenumber IS NULL OR ttiprc_pagenumber >= 1)
    );
END;
