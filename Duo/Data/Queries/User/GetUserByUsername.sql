CREATE OR ALTER PROCEDURE GetUserByUsername
    @UserName NVARCHAR(100)
AS
BEGIN
    SELECT *
    FROM Users
    WHERE UserName = @UserName;
END;
GO