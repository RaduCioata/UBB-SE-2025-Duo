CREATE OR ALTER PROCEDURE CreateUser
    @UserName NVARCHAR(100),
    @Email NVARCHAR(100),
    @Password NVARCHAR(255), -- Added Password Parameter
    @PrivacyStatus BIT,
    @OnlineStatus BIT,
    @DateJoined DATETIME,
    @ProfileImage NVARCHAR(MAX),
    @TotalPoints INT,
    @CoursesCompleted INT,
    @QuizzesCompleted INT,
    @Streak INT,
	@LastActivityDate DATETIME,
	@Accuracy DECIMAL(5,2)
AS
BEGIN
    INSERT INTO Users (
        UserName, Email, Password, PrivacyStatus, OnlineStatus, DateJoined,
        ProfileImage, TotalPoints, CoursesCompleted, QuizzesCompleted, Streak, LastActivityDate, Accuracy
    )
    VALUES (
        @UserName, @Email, @Password, @PrivacyStatus, @OnlineStatus, @DateJoined,
        @ProfileImage, @TotalPoints, @CoursesCompleted, @QuizzesCompleted, @Streak, @LastActivityDate, @Accuracy
    );

    SELECT SCOPE_IDENTITY() AS NewUserId;
END;
GO