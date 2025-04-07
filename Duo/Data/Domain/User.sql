
USE Duolingo;
GO


DROP TABLE IF EXISTS Users;
GO


CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Password NVARCHAR(255) NOT NULL, -- Added Password Column
    PrivacyStatus BIT NOT NULL,
    OnlineStatus BIT NOT NULL,
    DateJoined DATETIME NOT NULL DEFAULT GETDATE(),
    ProfileImage NVARCHAR(MAX),
    TotalPoints INT NOT NULL DEFAULT 0,
    CoursesCompleted INT NOT NULL DEFAULT 0,
    QuizzesCompleted INT NOT NULL DEFAULT 0,
    Streak INT NOT NULL DEFAULT 0,
	LastActivityDate DATETIME NULL,
	Accuracy DECIMAL(5,2) NOT NULL DEFAULT 0.00
);
GO



INSERT INTO Users (
    UserName,
    Email,
    Password,
    PrivacyStatus,
    OnlineStatus,
    DateJoined,
    ProfileImage,
    TotalPoints,
    CoursesCompleted,
    QuizzesCompleted,
    Streak,
	LastActivityDate,
	Accuracy
)
VALUES
('Alice', 'alice@example.com', 'hashedpassword123', 1, 1, GETDATE(), 'alice.jpg', 1200, 5, 10, 7,NULL,95.50),
('Bob', 'bob@example.com', 'securepassword456', 0, 0, GETDATE(), 'bob.jpg', 800, 3, 6, 2,'2024-12-31 14:30:00',95.50),
('Charlie', 'charlie@example.com', 'mypassword789', 1, 1, GETDATE(), 'charlie.jpg', 2000, 10, 15, 12,NULL,95.50),
('David', 'david@example.com', 'pass123', 1, 0, GETDATE(), 'david.jpg', 1500, 7, 12, 5,'2025-03-25 8:30:00',95.50),
('Emma', 'emma@example.com', 'emmaPass456', 1, 1, GETDATE(), 'emma.jpg', 1800, 9, 18, 10,NULL,95.50),
('Frank', 'frank@example.com', 'frankSecure', 0, 1, GETDATE(), 'frank.jpg', 2200, 12, 20, 15,NULL,95.50);



SELECT * FROM Users;
