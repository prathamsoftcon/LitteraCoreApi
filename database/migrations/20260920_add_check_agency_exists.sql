CREATE OR ALTER PROCEDURE [YUser].[proc_yuser_chk_agency_exists]
(
    @user_mobile_mail VARCHAR(4000)
)
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS
    (
        SELECT 1
        FROM YUser.AgencyMaster am
        WHERE ag_mobileno = @user_mobile_mail
           OR ag_email = @user_mobile_mail
    )
    BEGIN
        RAISERROR(
            'Mobile No. / Email is not registered. Contact admin.',
            16,
            2
        );
        RETURN;
    END

    -- User exists, return data
    SELECT am.*,au.tyuam_userid as userid
    FROM YUser.AgencyMaster am
	inner join  Yuser.tbl_yuser_user_agency_mapping  au on am.AgencyId=au.tyuam_agency_id
    WHERE ag_mobileno = @user_mobile_mail
       OR ag_email = @user_mobile_mail;
END
