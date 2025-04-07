CREATE TABLE UserAchievements (
    UserId INT,
    AchievementId INT,
    AwardedDate DATETIME,
    PRIMARY KEY (UserId, AchievementId)
);

select *from UserAchievements